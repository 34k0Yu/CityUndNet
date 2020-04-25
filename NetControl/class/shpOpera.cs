using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo;

using System.IO;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;
using System.Collections;

class MLayer
{
    public List<MFeature> mFeatures;

    public MLayer()
    {
        mFeatures = new List<MFeature>();
    }
}

class MFeature
{
    public List<MField> fields;
    public MGeometry mGeometry { get; set; }

    public MFeature()
    {
        fields = new List<MField>();
    }
}

class MField
{
    public string fieldName { get; set; }
    public string fieldValue { get; set; }
}

class MGeometry
{
    public string mGeometryType { get; set; }
    public List<MCoordinate> coordinates;

    public MGeometry()
    {
        coordinates = new List<MCoordinate>();
    }
}

class MCoordinate
{
    public string Longitude { get; set; }
    public string Latitude { get; set; }
}

namespace NetControl
{

    class shpOpera
    {
        public OSGeo.OGR.Driver oDriver;
        public List<string> mFiledList;
        public Layer oLayer;
        public string sCoordinates;

        public shpOpera()
        {
            mFiledList = new List<string>();
            oLayer = null;
            sCoordinates = null;
        }


        public void InitGDAL()
        {
            Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            Gdal.SetConfigOption("SHAPE_ENCODING", "");
            Gdal.AllRegister();
            Ogr.RegisterAll();

            oDriver = Ogr.GetDriverByName("ESRI Shapefile");
            if (oDriver == null)
            {
               // MessageBox.Show("文件不能打开，请检查");
            }

        }

        public string getSHPLayer(string filename)
        {
            if (null == filename || filename.Length <= 3)
            {
                oLayer = null;
                return null;
            }
            if (oDriver == null)
            {
               // MessageBox.Show("文件不能打开，请检查");
            }
            DataSource ds = oDriver.Open(filename, 1);
            if (null == ds)
            {
                oLayer = null;
                return null;
            }
            int position = filename.LastIndexOf("\\");
            string tempName = filename.Substring(position + 1, filename.Length - position - 4 - 1);
            //获取图层数目
            int layerCount = ds.GetLayerCount();
            if (layerCount > 1)
            {
               // MessageBox.Show("该SHP文件不止一个图层");
            }
            //根据名字索引图层
            oLayer = ds.GetLayerByName(tempName);
            if (oLayer == null)
            {
                ds.Dispose();
                return null;
            }
            return tempName;
        }

        public long getFeatureCount()
        {
            return oLayer.GetFeatureCount(1);
        }

        public bool getFields()
        {
            if (oLayer == null)
            {
                return false;
            }

            mFiledList.Clear();

            //获取图层的属性表结构
            FeatureDefn oDefn = oLayer.GetLayerDefn();

            int filedCount = oDefn.GetFieldCount();
            for (int i = 0; i < filedCount; i++)
            {
                //获取指定序号的属性列
                FieldDefn oField = oDefn.GetFieldDefn(i);
                if (oField != null)
                {
                    //获取属性列名字
                    mFiledList.Add(oField.GetNameRef());
                }
            }
            return true;
        }

        public bool GetFieldContent(int index, List<MField> fieldList)
        {

            Feature oFeature = null;

            if ((oFeature = oLayer.GetFeature(index)) != null)
            {

                FeatureDefn oDefn = oLayer.GetLayerDefn();
                int iFieldCount = oDefn.GetFieldCount();
                // 查找字段属性  
                for (int i = 0; i < iFieldCount; i++)
                {
                    FieldDefn oField = oDefn.GetFieldDefn(i);
                    string sFeildName = oField.GetNameRef();

                    #region 获取属性字段
                    FieldType Ftype = oFeature.GetFieldType(sFeildName);
                    string sTempType = "";
                    MField mField = new MField();
                    switch (Ftype)
                    {
                        case FieldType.OFTString:
                            string sFValue = oFeature.GetFieldAsString(sFeildName);
                            sTempType = "string";
                            mField.fieldValue = sFValue;
                            mField.fieldName = mFiledList[i];
                            fieldList.Add(mField);
                            break;
                        case FieldType.OFTReal:
                            double dFValue = oFeature.GetFieldAsDouble(sFeildName);
                            sTempType = "float";
                            mField.fieldValue = dFValue.ToString();
                            mField.fieldName = mFiledList[i];
                            fieldList.Add(mField);
                            break;
                        case FieldType.OFTInteger:
                            int iFValue = oFeature.GetFieldAsInteger(sFeildName);
                            sTempType = "int";
                            mField.fieldValue = iFValue.ToString();
                            mField.fieldName = mFiledList[i];
                            fieldList.Add(mField);
                            break;
                        case FieldType.OFTInteger64:
                            long lFValue = oFeature.GetFieldAsInteger64(sFeildName);
                            sTempType = "long";
                            mField.fieldValue = lFValue.ToString();
                            mField.fieldName = mFiledList[i];
                            fieldList.Add(mField);
                            break;
                        case FieldType.OFTDate:
                            int year = 0, month = 0, day = 0, hour = 0, minute = 0, flag = 0;
                            float second = 0;
                            oFeature.GetFieldAsDateTime(sFeildName, out year, out month, out day, out hour, out minute, out second, out flag);
                            DateTime dt = new DateTime(year, month, day, hour, minute, (int)second);
                            sTempType = "datetime";
                            mField.fieldValue = dt.ToString();
                            mField.fieldName = mFiledList[i];
                            fieldList.Add(mField);
                            break;
                        default:
                            //System.Console.WriteLine("error!");
                            break;
                    }
                    #endregion
                }
            }
            return true;
        }

        public void setCoordinates(string oCoordinates, List<MCoordinate> nCoordinates, string type)
        {
            int first = 0, last = 0;
            if (type == "POLYGON")
            {
                first = oCoordinates.IndexOf("((") + "((".Length;
                last = oCoordinates.LastIndexOf("))");

            }
            else if (type == "POINT" || type == "LINE")
            {
                first = oCoordinates.IndexOf("(") + "(".Length;
                last = oCoordinates.LastIndexOf(")");
            }

            string coordinates = oCoordinates.Substring(first, last - first);
            string[] allcoordinate = coordinates.Split(',');
            for (int i = 0; i < allcoordinate.Length; i++)
            {
                string[] coordinate = allcoordinate[i].Split(' ');
                MCoordinate mCoordinate = new MCoordinate();
                mCoordinate.Longitude = coordinate[0];
                mCoordinate.Latitude = coordinate[1];

                nCoordinates.Add(mCoordinate);
            }

        }

        public bool GetGeometry(int iIndex, MGeometry mGeometry)
        {
            if (null == oLayer)
            {
                return false;
            }
            int iFeatureCout = (int)oLayer.GetFeatureCount(0);
            Feature oFeature = null;
            oFeature = oLayer.GetFeature(iIndex);
            //  Geometry  
            Geometry oGeometry = oFeature.GetGeometryRef();

            wkbGeometryType oGeometryType = oGeometry.GetGeometryType();
            switch (oGeometryType)
            {
                case wkbGeometryType.wkbPoint:
                    mGeometry.mGeometryType = "POINT";
                    oGeometry.ExportToWkt(out sCoordinates);
                    setCoordinates(sCoordinates, mGeometry.coordinates, mGeometry.mGeometryType);
                    break;
                case wkbGeometryType.wkbLineString:
                    mGeometry.mGeometryType = "LINE";
                    oGeometry.ExportToWkt(out sCoordinates);
                    setCoordinates(sCoordinates, mGeometry.coordinates, mGeometry.mGeometryType);
                    break;
                case wkbGeometryType.wkbPolygon:
                    mGeometry.mGeometryType = "POLYGON";
                    oGeometry.ExportToWkt(out sCoordinates);
                    setCoordinates(sCoordinates, mGeometry.coordinates, mGeometry.mGeometryType);
                    break;

                default:
                    break;
            }
            return false;
        }


    }
}
