
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.WindowsPresentation;
using GMap.NET;
using GMap.NET.MapProviders;
using System.Windows;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;

namespace NetControl
{
    public partial class Gmap : GMapControl
    {
        public PointLatLng mapCenter = new PointLatLng(39.8, 116.5); //new PointLatLng(31.175209828310848, 121.46209716796875); (39.8, 116.5)

        //地图平移设置
        public delegate void MapMoveProcDelegate(Thickness margin);
        private static void mapMoved(Thickness margin) { }
        public MapMoveProcDelegate mapMovedProc = new MapMoveProcDelegate(mapMoved);

        //地图缩放设置
        public delegate Point MapZoomProcDelegate(double scale);
        private static Point mapZoomed(double scale) { return new Point(0, 0); }
        public MapZoomProcDelegate mapZoomedProc = new MapZoomProcDelegate(mapZoomed);
        //比例尺
        public delegate void repainProcDelegate();
        private static void repained() { }
        public repainProcDelegate repainedProc = new repainProcDelegate(repained);

        public int mapZoom = 18;   //缩放等级
        Thickness marginCoor = new Thickness(0, 0, 0, 0);   //偏移坐标
        Point offset = new Point(0, 0);
        double scale = 1;

        public Gmap()
        {
            /*地图初始化*/
            try
            {
                System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("map.baidu.com");
            }
            catch
            {
                Manager.Mode = AccessMode.ServerAndCache;
            }
            MapProvider = GMapProviders.GoogleChinaMap; // GMapProviders.ArcGIS_World_Physical_Map; //地图选择 EmptyProvider
            MinZoom = 10;  //最小缩放
            MaxZoom = 24; //最大缩放
            Zoom = mapZoom;     //当前缩放
            ShowCenter = false; //不显示中心十字点
            DragButton = System.Windows.Input.MouseButton.Right; //右键拖拽地图
            Position = mapCenter; //初始化地图   
            MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            //地图变换设置
            OnPositionChanged += mapMoved;  //平移
            OnMapZoomChanged += mapZoomed;  //缩放
        }

        public void mapMoved(PointLatLng point)
        {
            Point previous = setLatLng(point.Lat, point.Lng);
            Point current = setLatLng(mapCenter.Lat, mapCenter.Lng);

            //偏移计算
            double dx = current.X - previous.X;
            double dy = current.Y - previous.Y;

            //缩放计算
            dx *= scale;
            dy *= scale;

            Thickness margin = new Thickness(dx, dy, -dx, -dy);
            marginCoor = margin;
            mapMovedProc(margin);   //外部地图平移处理

        }

        void mapZoomed()
        {
            //缩放
            double sc = Zoom - mapZoom;
            sc = Math.Pow(2, sc);
            offset = mapZoomedProc(sc);  //外部地图缩放控制
            scale = sc;

            //定位
            mapMoved(Position);
            repainedProc();
            //offset = mapZoomedProc(sc);
        }

        public Point setLatLng(double lat, double lng)
        {
            GPoint gpoint = FromLatLngToLocal(new PointLatLng(lat, lng));
            Point ret = new Point((double)gpoint.X, (double)gpoint.Y);

            //绘图坐标系偏移处理
            ret.X -= marginCoor.Left;
            ret.Y -= marginCoor.Top;

            //缩放处理
            ret.X += offset.X;
            ret.Y += offset.Y;
            ret.X /= scale;
            ret.Y /= scale;

            return ret;
        }

        public Point setLatLng1(double lat, double lng)
        {
            GPoint gpoint = FromLatLngToLocal(new PointLatLng(lat, lng));
            Point ret = new Point((double)gpoint.X, (double)gpoint.Y);
            return ret;
        }

        public Point setXY(double x, double y)
        {
            PointLatLng pointll = FromLocalToLatLng((int)x, (int)y);
            return new Point(pointll.Lat, pointll.Lng);
        }

        public void setCenter(double lat, double lng)
        {
            PointLatLng pointll = new PointLatLng(lat, lng);
            //mapCenter = pointll;
            Position = pointll;
        }

        [Description("图层编号"), Category("图层号码")]
        public GMapProvider laid
        {
            get { return (GMapProvider)GetValue(laidProperty); }
            set { SetValue(laidProperty, value); }
        }
        public static readonly DependencyProperty laidProperty =
           DependencyProperty.Register("laid", typeof(GMapProvider), typeof(Gmap));

        public PointLatLng Mousemovepoint
        {
            get { return (PointLatLng)GetValue(MousemovepointProperty); }
            set { SetValue(MousemovepointProperty, value); }
        }
        public static readonly DependencyProperty MousemovepointProperty =
            DependencyProperty.Register("Mousemovepoint", typeof(PointLatLng), typeof(Gmap), new PropertyMetadata(new PointLatLng(0, 0)));


        public void changeLayer(int num)
        {
            switch (num)
            {
                case 1:
                    MapProvider = GMapProviders.GoogleChinaHybridMap; //形状图
                    break;
                case 2:
                    MapProvider = GMapProviders.GoogleChinaTerrainMap; //地形图
                    break;
                case 3:
                    MapProvider = GMapProviders.OpenStreetMap; //行政图
                    break;
                case 4:
                    MapProvider = GMapProviders.BingSatelliteMap; //卫星图
                    break;
                case 5:
                    MapProvider = GMapProviders.EmptyProvider;//空白图层
                    break;
                case 6:
                    MapProvider = GMapProviders.GoogleChinaMap;
                    break;
            }
        }
    }

    public class StringtoInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var revalue = value;
            switch (value + string.Empty)
            {
                case "形状图层":
                    revalue = GMapProviders.GoogleChinaHybridMap;
                    break;
                case "行政图层":
                    revalue = GMapProviders.OpenStreetMap;
                    break;
                case "卫星图层":
                    revalue = GMapProviders.BingSatelliteMap;
                    break;
                case "空白图层":
                    revalue = GMapProviders.EmptyProvider;
                    break;
                case "普通图层":
                    revalue = GMapProviders.GoogleChinaMap;
                    break;
            }
            return revalue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class PointToStringConverter : IValueConverter
    {
        public string FormatString { get; set; }
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is PointLatLng))
                return null;
            PointLatLng pos = (PointLatLng)value;
            return String.Format(FormatString ?? "lat:{0}  lng:{1}", pos.Lat, pos.Lng);
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class indicator : IValueConverter
    {
        public string FormatString { get; set; }
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string command = "";
            int a = Convert.ToInt32(value);
            switch (a)
            {
                case 0:
                    command = "无指令";
                    break;
                case 1:
                    command = "添加两通";
                    break;
                case 2:
                    command = "添加三通";
                    break;
                case 3:
                    command = "添加管线";
                    break;
                case 4:
                    command = "添加四通";
                    break;
                case 5:
                    command = "添加五通";
                    break;
                case 6:
                    command = "添加供气站";
                    break;
                case 7:
                    command = "添加调压站";
                    break;
                case 8:
                    command = "添加阀门";
                    break;
                case 9:
                    command = "添加终端用户";
                    break;
                case 10:
                    command = "添加scada测点";
                    break;
                case 11:
                    command = "添加燃气";
                    break;
            }

            //PointLatLng pos = g.FromLocalToLatLng((int)p.X, (int)p.Y);
            return String.Format(FormatString ?? "目前指令:{0}", command);
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PointToZoomConverter : IValueConverter
    {
        public string FormatString { get; set; }
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double pos = (double)value;
            string warn = " ";
            if(pos<18)
            {
                warn = "当前地图等级，无法添加设备";
            }
            return String.Format(FormatString ?? "当前地图等级:{0}", pos) +"  " + warn;
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Pathshowconvert : IValueConverter
    {
        public string FormatString { get; set; }
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;
            if (text == null)
            {
                return null;
            }
            return String.Format(FormatString ?? "当前保存路径:{0}", text);
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
