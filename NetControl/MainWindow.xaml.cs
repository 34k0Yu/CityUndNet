using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using System.Threading.Tasks;
using GMap.NET.WindowsPresentation;
using GMap.NET;
using System.Data;
using System.IO;
using System.Collections.ObjectModel;
using DevExpress.LookAndFeel;
using System.Threading;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.DataAnnotations;
using System.Windows.Markup;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using DevExpress.Xpf.Charts;
using System.Runtime.CompilerServices;
using DevExpress.Xpf.Docking;
using GisCalculate;
using System.Reflection;

namespace NetControl
{
    #region 图层样式
    public class layerid
    {
        public string name { get; set; }
        public int id { get; set; }
    }
    public class layeridArr : ObservableCollection<layerid>
    {
        public layeridArr()
        {
            //Add(new layerid { name = "形状图层", id = 1 });
            //Add(new layerid { name = "行政图层", id = 3 });
            //Add(new layerid { name = "卫星图层", id = 4 });
            Add(new layerid { name = "空白图层", id = 5 });
            Add(new layerid { name = "普通图层", id = 6 });
        }
    }
    #endregion

    public partial class MainWindow : Window
    {
        #region 全局变量
        public static int command = 0;
        double[] zoomscale = new double[21] { 10, 20, 50, 100, 200, 500, 1000, 1500, 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 1500, 2000, 2000 };
        bool[] isMorNMi = new bool[21] { true, true, true, true, true, true, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false };
        const double pi = 3.1415926;
        const double earth_R = 6371393;
        const double a = 6378137;
        const double b = 6356752.3142;
        const double f = 1 / 298.2572236;
        const double g = 9.8;
        string pa; string fl; double Scale = 1;//scale要有初值，后面要计算
        static MLayer mLayer = new MLayer();
        static long featureCount = 0;
        static shpOpera m_Shp;
        public static twowayvalve_data da = new twowayvalve_data();
        public static threewayvalve_data th = new threewayvalve_data();
        public static line_attribute_t lin = new line_attribute_t();
        public static fourwayvalve_data fo = new fourwayvalve_data();
        public static fivewayvalve_data fi = new fivewayvalve_data();
        public static scadatest_data sc = new scadatest_data();
        public static user_data us = new user_data();
        public static pel_data pe = new pel_data();
        public static valve_data va = new valve_data();
        public static regulatestation_data re = new regulatestation_data();
        public static gasstation_data ga = new gasstation_data();
        public int nodeID = 0;//节点ID分配
        public static List<Point> pline = new List<Point>();
        public static List<string> tablename = new List<string>();
        public static List<string> markname = new List<string>();
        public static List<string> marksign = new List<string>();
        public static DataTable lineda = new DataTable();
        int i = 0;
        public static MapMark mapMark = new MapMark();
        //page
        LineProperty LineProperty = new LineProperty();
        Device deviceproperty = new Device();
        ultimate ultimateproperty = new ultimate();
        Regulation regulationpro = new Regulation();
        Firegas firegas = new Firegas();
        Valvepro valvepros = new Valvepro();
        Line segment = new Line();
        private List<Rectangle> mapMarkBorders = new List<Rectangle>();//框选中集合
        //多播事件
        public List<LineColorModel> itemSourceLists = new List<LineColorModel>();
        public List<LineColorModel> lineColors = new List<LineColorModel>();//排序列表
        public List<LinePMModel> linePMs = new List<LinePMModel>();//显示管线计算后的压强和流量
        private delegate void DrawrectangleDelegate(object sender, MouseButtonEventArgs e);
        private event DrawrectangleDelegate DrawrectangleLeftMouseEvent;
        private delegate void InsertLineDelegate(IList<excelInNode> node);
        InsertLineDelegate insertline;
        IAsyncResult iAsyncResult;
        #region 全局图片
        BitmapImage towiImg = new BitmapImage(new Uri("pack://application:,,,/image/2.png"));
        BitmapImage towbiImg = new BitmapImage(new Uri("pack://application:,,,/image/2_blue.png"));
        BitmapImage towpiImg = new BitmapImage(new Uri("pack://application:,,,/image/2_purple.png"));
        BitmapImage towriImg = new BitmapImage(new Uri("pack://application:,,,/image/2_red.png"));

        BitmapImage threeiImg = new BitmapImage(new Uri("pack://application:,,,/image/3.png"));
        BitmapImage threbiImg = new BitmapImage(new Uri("pack://application:,,,/image/3_blue.png"));
        BitmapImage threpiImg = new BitmapImage(new Uri("pack://application:,,,/image/3_purple.png"));
        BitmapImage threriImg = new BitmapImage(new Uri("pack://application:,,,/image/3_red.png"));

        BitmapImage fourImg = new BitmapImage(new Uri("pack://application:,,,/image/4.png"));
        BitmapImage fourbImg = new BitmapImage(new Uri("pack://application:,,,/image/4_blue.png"));
        BitmapImage fourpImg = new BitmapImage(new Uri("pack://application:,,,/image/4_purple.png"));
        BitmapImage fourrImg = new BitmapImage(new Uri("pack://application:,,,/image/4_red.png"));

        BitmapImage fiveImg = new BitmapImage(new Uri("pack://application:,,,/image/5.png"));
        BitmapImage fivebImg = new BitmapImage(new Uri("pack://application:,,,/image/5_blue.png"));
        BitmapImage fivepImg = new BitmapImage(new Uri("pack://application:,,,/image/5_purple.png"));
        BitmapImage fiverImg = new BitmapImage(new Uri("pack://application:,,,/image/5_red.png"));

        BitmapImage gasImg = new BitmapImage(new Uri("pack://application:,,,/image/gsta.png"));
        BitmapImage gasbImg = new BitmapImage(new Uri("pack://application:,,,/image/gsta_blue.png"));
        BitmapImage gaspImg = new BitmapImage(new Uri("pack://application:,,,/image/gsta_purple.png"));
        BitmapImage gasrImg = new BitmapImage(new Uri("pack://application:,,,/image/gsta_red.png"));

        public BitmapImage valveImg = new BitmapImage(new Uri("pack://application:,,,/image/fm.png"));
        BitmapImage valvebImg = new BitmapImage(new Uri("pack://application:,,,/image/fm_blue.png"));
        BitmapImage valvepImg = new BitmapImage(new Uri("pack://application:,,,/image/fm_purple.png"));
        BitmapImage valverImg = new BitmapImage(new Uri("pack://application:,,,/image/fm_red.png"));

        BitmapImage relguImg = new BitmapImage(new Uri("pack://application:,,,/image/res.png"));
        BitmapImage relgubImg = new BitmapImage(new Uri("pack://application:,,,/image/res_blue.png"));
        BitmapImage relgupImg = new BitmapImage(new Uri("pack://application:,,,/image/res_purple.png"));
        BitmapImage relgurImg = new BitmapImage(new Uri("pack://application:,,,/image/res_red.png"));

        BitmapImage pelImg = new BitmapImage(new Uri("pack://application:,,,/image/pel.png"));
        BitmapImage pelbImg = new BitmapImage(new Uri("pack://application:,,,/image/pel_blue.png"));
        BitmapImage pelpImg = new BitmapImage(new Uri("pack://application:,,,/image/pel_purple.png"));
        BitmapImage pelrImg = new BitmapImage(new Uri("pack://application:,,,/image/pel_red.png"));

        BitmapImage scadaImg = new BitmapImage(new Uri("pack://application:,,,/image/scada.png"));
        BitmapImage scadabImg = new BitmapImage(new Uri("pack://application:,,,/image/scada_blue.png"));
        BitmapImage scadapImg = new BitmapImage(new Uri("pack://application:,,,/image/scada_purple.png"));
        BitmapImage scadarImg = new BitmapImage(new Uri("pack://application:,,,/image/scada_red.png"));

        BitmapImage userImg = new BitmapImage(new Uri("pack://application:,,,/image/user.png"));
        BitmapImage userbImg = new BitmapImage(new Uri("pack://application:,,,/image/user_blue.png"));
        BitmapImage userpImg = new BitmapImage(new Uri("pack://application:,,,/image/user_purple.png"));
        BitmapImage userrImg = new BitmapImage(new Uri("pack://application:,,,/image/user_red.png"));

        public BitmapImage vabreImg = new BitmapImage(new Uri("pack://application:,,,/image/vabre.png"));
        public BitmapImage vabrebImg = new BitmapImage(new Uri("pack://application:,,,/image/vabre_blue.png"));
        BitmapImage vabrepImg = new BitmapImage(new Uri("pack://application:,,,/image/vabre_purple.png"));
        BitmapImage vabrerImg = new BitmapImage(new Uri("pack://application:,,,/image/vabre_red.png"));

        BitmapImage circul = new BitmapImage(new Uri("pack://application:,,,/image/circul.png"));
        ImageBrush lineImg = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/image/linestyle.png")));
        BitmapImage selectmark = new BitmapImage(new Uri("pack://application:,,,/image/selectmark.png"));
        #endregion
        private bool flag;
        //全局一级节点,二级节点
        public ObservableCollection<Sence> sencesList { get; set; } = new ObservableCollection<Sence>();
        public ObservableCollection<ChartData> paDatasList { get; set; } = new ObservableCollection<ChartData>();
        public ObservableCollection<ChartData> tempDatasList { get; set; } = new ObservableCollection<ChartData>();
        public int tabflag = 0;
        public MapMark scadaNow;
        public static string tempModifyStr = string.Empty;
        public static Sence tempSence;
        public Action<string> AddModify;
        public static List<ShowDatas> rectDatas;
        SelectLinePipProp LinePipListProp = new SelectLinePipProp();
        Warming warming = new Warming();
        public enum optState_Type
        {
            pointmode, linemode
        };//加点和连线
        public static int count;
        public static optState_Type optState = optState_Type.pointmode;
        Point location = new Point();
        Rectangle rect = null;
        Rectangle logicrect = null;
        string resultfilename;
        elemt_data elemt_Datas = new elemt_data();
        devicedata devicedatas = new devicedata();
        BitmapImage image = new BitmapImage();
        List<Rectangle> rec = new List<Rectangle>();
        [Description("命令"), Category("自定义属性")]
        public int directive
        {
            get { return (int)GetValue(directiveProperty); }
            set { SetValue(directiveProperty, value); }
        }
        public static readonly DependencyProperty directiveProperty =
            DependencyProperty.Register("directive", typeof(int), typeof(MainWindow));
        bool closed;
        public static List<Line> linelist = new List<Line>();
        public static List<MapMark> marklist = new List<MapMark>();
        [Description("显示命令"), Category("自定义属性")]
        public bool showcomd
        {
            get { return (bool)GetValue(showcomdProperty); }
            set { SetValue(showcomdProperty, value); }
        }
        public static readonly DependencyProperty showcomdProperty =
            DependencyProperty.Register("showcomd", typeof(bool), typeof(MainWindow));
        bool selectall = false;
        string filepath = "C:\\";//打开和保存文件路径
        [Description("显示路径"), Category("自定义属性")]
        public string savepath//保存文件路径
        {
            get { return (string)GetValue(savepathProperty); }
            set { SetValue(savepathProperty, value); }
        }
        public static readonly DependencyProperty savepathProperty =
            DependencyProperty.Register("savepath", typeof(string), typeof(MainWindow));
        string workpath = "C:\\work\\";
        private delegate void ThreadDelegate();//声明线程
        string timename;//时间文件夹
        public string scenename;//场景名称
        bool prescansal = false;//选中事件在计算后不可用
        List<excelInNode> excelNodes = new List<excelInNode>();//存储经纬度相同的点
        public string impath;
        Point markp;//框选坐标
        int mousecount = 0;//鼠标点击次数
        List<MapMark> selcetnode = new List<MapMark>();//框选中的设备
        List<MapMark> singglenode = new List<MapMark>();//单选中的设备
        IList<string> sqlStringList = new List<string>();
        //清空数据库
        string deletext = "delete from twowayvalve_data;delete from threewayvalve_data;delete from station_data;" +
                "delete from regulatestation_data;delete from line_attribute_t;delete from fourwayvalve_data;delete from fivewayvalve_data;delete from firegas_data";
        //颜色层级
        double[] color = new double[10];
        #endregion

        #region 主函数
        public MainWindow()
        {
            InitializeComponent();
            dbOpera db = dbOpera.Instance;
            mapcontrol.mapMovedProc = mapMovedProc;
            mapcontrol.mapZoomedProc = mapZoomedProc;
            mapcontrol.repainedProc = repainedProc;
            Device.ParentWindow = this;
            LineProperty.ParentWindow = this;
            SelectLinePipProp.ParentWindow = this;
            Warming.ParentWindow = this;
            ultimate.ParentWindow = this;
            Regulation.ParentWindow = this;
            Firegas.ParentWindow = this;
            Newcreat.ParentWindow = this;
            Valvepro.ParentWindow = this;
            Sight.ParentWindow = this;
            showcomd = true;

            AddModify = (string msg) =>
            {
                if (tempSence == null || string.IsNullOrEmpty(msg)) return;
                tempModifyStr += msg;
            };
            insertline = new InsertLineDelegate(InserDatas);
        }

        #endregion

        #region 地图比例尺
        //比例尺canvas处理
        private void Scanvas_Loaded(object sender, RoutedEventArgs e)
        {
            CalcScale(zoomscale[24 - (int)mapcontrol.Zoom], !isMorNMi[24 - (int)mapcontrol.Zoom]);
        }

        public void CalcScale(double dist, bool isNMi)
        {
            Point latlon1 = new Point();
            Point latlon2 = new Point();
            latlon1 = mapcontrol.setXY(Canvas.GetLeft(scale), Canvas.GetTop(scale));
            if (isNMi)
            {
                latlon2 = computerThatLatLon(latlon1.Y, latlon1.X, 90, NauticaltoMeter(dist));
                scale_text.Text = dist.ToString() + "公里";
            }
            else
            {
                latlon2 = computerThatLatLon(latlon1.Y, latlon1.X, 90, dist);
                scale_text.Text = dist.ToString() + "米";
            }
            //Point XY = mapcontrol.setLatLng(latlon2.X, latlon2.Y);
            //if ((XY.X - Canvas.GetLeft(scale)) > 0)//xy.x变成负的导致直接调到else里面了,比例尺就显示不出来
            //    scale.Width = XY.X - Canvas.GetLeft(scale);
            //else
            //    scale.Width = 0;
        }

        public void repainedProc()
        {
            CalcScale(zoomscale[24 - (int)mapcontrol.Zoom], !isMorNMi[24 - (int)mapcontrol.Zoom]);
            //GraphLayer.Clear();
            //Thickness mg = new Thickness(0, 0, 0, 0);
            //GraphLayer.Margin = mg;
            //gdipainlayer1();
        }
        #endregion

        #region 两点的距离和方位角计算

        double toRadians(double d)
        {
            return d / 180 * pi;
        }

        double toDegrees(double d)
        {
            return d / pi * 180;
        }

        double HaverSin(double theta)
        {
            double v = Math.Sin(theta / 2);
            return v * v;
        }

        double distance(double Aj, double Aw, double Bj, double Bw)
        {
            double A_Lat = toRadians(Aw);
            double B_Lat = toRadians(Bw);
            double A_Long = toRadians(Aj);
            double B_Long = toRadians(Bj);
            double Lat_Difference = Math.Abs(A_Lat - B_Lat);
            double Long_Difference = Math.Abs(A_Long - B_Long);
            double h = HaverSin(Lat_Difference) + Math.Cos(A_Lat) * Math.Cos(B_Lat) * HaverSin(Long_Difference);
            double dis = 2 * earth_R * Math.Asin(Math.Sqrt(h));
            return dis;
        }

        double GetAzimuth(double Aj, double Aw, double Bj, double Bw)
        {

            double A_Lat = toRadians(Aw);
            double B_Lat = toRadians(Bw);
            double A_Long = toRadians(Aj);
            double B_Long = toRadians(Bj);

            double y = Math.Sin(B_Long - A_Long) * Math.Cos(B_Lat);
            double x = Math.Cos(A_Lat) * Math.Sin(B_Lat) - Math.Sin(A_Lat) * Math.Cos(B_Lat) * Math.Cos(B_Long - A_Long);
            double Azimuth_rad = Math.Atan2(y, x);

            double Azimuth = toDegrees(Azimuth_rad);
            if (Azimuth < 0)
                Azimuth += 360;
            return Azimuth;
        }

        public Point computerThatLatLon(double lon, double lat, double brng, double dist)
        {
            double alpha1 = toRadians(brng);
            double sinAlpha1 = Math.Sin(alpha1);
            double cosAlpha1 = Math.Cos(alpha1);

            double tanU1 = (1 - f) * Math.Tan(toRadians(lat));
            double cosU1 = 1 / Math.Sqrt((1 + tanU1 * tanU1));
            double sinU1 = tanU1 * cosU1;
            double sigma1 = Math.Atan2(tanU1, cosAlpha1);
            double sinAlpha = cosU1 * sinAlpha1;
            double cosSqAlpha = 1 - sinAlpha * sinAlpha;
            double uSq = cosSqAlpha * (a * a - b * b) / (b * b);
            double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));

            double cos2SigmaM = 0;
            double sinSigma = 0;
            double cosSigma = 0;
            double sigma = dist / (b * A), sigmaP = 2 * Math.PI;
            while (Math.Abs(sigma - sigmaP) > 1e-12)
            {
                cos2SigmaM = Math.Cos(2 * sigma1 + sigma);
                sinSigma = Math.Sin(sigma);
                cosSigma = Math.Cos(sigma);
                double deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)
                        - B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));
                sigmaP = sigma;
                sigma = dist / (b * A) + deltaSigma;
            }

            double tmp = sinU1 * sinSigma - cosU1 * cosSigma * cosAlpha1;
            double lat2 = Math.Atan2(sinU1 * cosSigma + cosU1 * sinSigma * cosAlpha1,
                    (1 - f) * Math.Sqrt(sinAlpha * sinAlpha + tmp * tmp));
            double lambda = Math.Atan2(sinSigma * sinAlpha1, cosU1 * cosSigma - sinU1 * sinSigma * cosAlpha1);
            double C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));
            double L = lambda - (1 - C) * f * sinAlpha
                    * (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));

            double revAz = Math.Atan2(sinAlpha, -tmp); // final bearing

            return new Point(toDegrees(lat2), lon + toDegrees(L));
        }

        public double NauticaltoMeter(double nm)
        {
            return nm * 1852;
        }
        //根据经纬度算管线长度
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radlat1 = Rad(lat1);
            double radlng1 = Rad(lng1);
            double radlat2 = Rad(lat2);
            double radlng2 = Rad(lng2);
            double a = radlat1 - radlat2;
            double b = radlng1 - radlng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radlat1) * Math.Cos(radlat2) * Math.Pow(Math.Sin(b / 2), 2))) * MainWindow.a;
            return result;
        }

        private static double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }
        #endregion

        #region <!--canvas平移缩放处理-->
        void mapMovedProc(Thickness margin)
        {
            Thickness mg = new Thickness(margin.Left, margin.Top, margin.Right, margin.Bottom);
            layer.Margin = mg;
            marklayer.Margin = mg;
            linelayer.Margin = mg;
            selectcav.Margin = mg;
            imageContainer.Margin = mg;
            newselectr.Margin = mg;
            //drawrectangle.Margin = mg;
        }


        /*canvas缩放处理*/
        Point mapZoomedProc(double scale)
        {
            Scale = scale;
            if (mapcontrol.Zoom < 18)
            {
                foreach (MapMark node in marklayer.Children)//设备与线保存不变
                {
                    node.nodeimage.Source = circul;
                    node.Width = node.Height = node.imgheight = node.imgwidth = 10;
                }
                if (prescansal == false)
                {
                    foreach (Line li in linelayer.Children)
                    {
                        li.StrokeThickness = 1 / scale;
                        li.Stroke = Brushes.Black;
                    }
                }
                else
                {
                    foreach (Line li in linelayer.Children)
                    {
                        li.StrokeThickness = 1 / scale;
                    }
                }
            }
            else
            {
                imageback();
                if (prescansal == false)
                {
                    foreach (Line li in linelayer.Children)
                    {
                        li.StrokeThickness = 6 / scale;
                        li.Stroke = Brushes.Black;
                    }
                }
                else
                {
                    foreach (Line li in linelayer.Children)
                    {
                        li.StrokeThickness = 6 / scale;
                    }
                }
            }
            foreach (MapMark node in marklayer.Children)//设备与线保存不变
            {
                node.Scale = 1 / scale;
            }
            foreach (Rectangle rect in selectcav.Children)
            {
                try
                {
                    TransformGroup tg = rect.RenderTransform as TransformGroup;
                    ScaleTransform st = tg.Children[0] as ScaleTransform;
                    rect.RenderTransformOrigin = new Point(0.5, 0.5);
                    st.ScaleX = st.ScaleY = 1 / scale;
                }
                catch { }
            }
            //foreach (MapMark node in selectcav.Children)//设备与线保存不变
            //{
            //    node.Scale = 1 / scale;
            //}

            if (drawrectangle.Background != null)
            {
                mapcontrol.Zoom = 18;
                return new Point(0, 0);
            }
            //repainedProc();
            zoomProc(layer, scale);  //绘制图层
            zoomProc(marklayer, scale);  //节点图层
            zoomProc(linelayer, scale);  //线图层
            zoomProc(selectcav, scale);  //高亮图层
            zoomProc(imageContainer, scale);    //镜像图层
            zoomProc(newselectr, scale);
            //zoomProc(drawrectangle, scale);
            //RenderTransformOrigin采用中心缩放，丢失的位移信息需计算处理
            Point offset = new Point(layer.Width * (scale - 1) / 2, layer.Height * (scale - 1) / 2);
            return offset;
        }

        void zoomProc(UIElement uie, double scale)
        {
            TransformGroup tg = uie.RenderTransform as TransformGroup;
            ScaleTransform st = tg.Children[0] as ScaleTransform;
            uie.RenderTransformOrigin = new Point(0.5, 0.5);
            st.ScaleX = st.ScaleY = scale;//设置缩放比率

            tg = uie.RenderTransform as TransformGroup;
            st = tg.Children[0] as ScaleTransform;
            uie.RenderTransformOrigin = new Point(0.5, 0.5);
            st.ScaleX = st.ScaleY = scale;//设置缩放比率
        }

        private void imageback()
        {
            foreach (MapMark node in marklayer.Children)
            {
                node.Width = node.Height = node.imgheight = node.imgwidth = 24;
                switch (node.sign)
                {
                    case "两通":
                        node.nodeimage.Source = towiImg;
                        break;
                    case "三通":
                        node.nodeimage.Source = threeiImg;
                        break;
                    case "四通":
                        node.nodeimage.Source = fourImg;
                        break;
                    case "五通":
                        node.nodeimage.Source = fiveImg;
                        break;
                    case "供气站":
                        node.nodeimage.Source = gasImg;
                        break;
                    case "调压站":
                        node.nodeimage.Source = relguImg;
                        break;
                    case "阀门":
                        node.nodeimage.Source = valveImg;
                        break;
                    case "终端用户":
                        node.nodeimage.Source = userImg;
                        break;
                    case "scada测点":
                        node.nodeimage.Source = scadaImg;
                        break;
                    case "燃气":
                        node.nodeimage.Source = pelImg;
                        break;
                }
            }
        }

        #endregion

        #region  更换图层
        private void Changelayer_EditValueChanged(object sender, RoutedEventArgs e)
        {
            int id = 6;
            if (changelayer.EditValue.ToString() == "空白图层")
            {
                id = 5;
            }
            mapcontrol.changeLayer(id);
        }
        #endregion

        #region 添加图标
        private void Two_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//两通
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 1;
            showcomd = true;
            directive = command;
            optState = optState_Type.pointmode;
        }

        private void Three_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//三通
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 2;
            showcomd = true;
            directive = command;
            optState = optState_Type.pointmode;
        }

        private void Four_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//四通
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 4;
            showcomd = true;
            directive = command;
            optState = optState_Type.pointmode;
        }

        private void Five_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//五通
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 5;
            showcomd = true;
            directive = command;
            optState = optState_Type.pointmode;
        }

        private void Valve_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//阀门
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 8;
            showcomd = true;
            directive = command;
            optState = optState_Type.pointmode;
        }

        private void Line_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//管线
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 3;
            showcomd = true;
            directive = command;
            optState = optState_Type.linemode;
        }

        private void Gasstation_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//供气站
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 6;
            showcomd = true;
            directive = command;
            optState = optState_Type.pointmode;
        }

        private void Regulate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//调压站
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 7;
            showcomd = true;
            directive = command;
            optState = optState_Type.pointmode;
        }

        private void User_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//终端用户
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 9;
            showcomd = true;
            directive = command;
            optState = optState_Type.pointmode;
        }

        private void Scada_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//scada测点
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 10;
            showcomd = true;
            directive = command;
            optState = optState_Type.pointmode;
        }

        private void Pel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)//燃气
        {
            if (mapcontrol.Zoom < 18)
            {
                return;
            }
            command = 11;
            showcomd = true;
            directive = command;
            optState = optState_Type.pointmode;
        }

        /// <summary>
        /// 添加选中效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mark_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (optState == optState_Type.pointmode)
            {
                MapMark Mark = (MapMark)sender;
                if (selectall == false)
                {
                    //Seletedrect.Visibility = Visibility.Visible;
                    //Seletedrect.nodeimage.Source = selectmark;
                    //double x = Mark.GetLeft();
                    //double y = Mark.GetTop();
                    //Seletedrect.SetLeft(Mark.Movie.X);
                    //Seletedrect.SetTop(Mark.Movie.Y);
                    // Point p = new Point(Mark.Movie.X + 12, Mark.Movie.Y + 12);
                    //Seletedrect.Movie = p;
                    for (int i = 0; i < singglenode.Count; i++)
                    {
                        singglenode[i].alteration.Visibility = Visibility.Collapsed;
                    }
                    Mark.alteration.Visibility = Visibility.Visible;
                    singglenode.Add(Mark);
                }
                else
                {
                    alldrect.Visibility = Visibility.Visible;
                    alldrect.SetLeft(Mark.Movie.X);
                    alldrect.SetTop(Mark.Movie.Y);
                }
            }
        }

        private void Gmapgrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(marklayer);

            PointLatLng pointLatLng = mapcontrol.FromLocalToLatLng((int)point.X, (int)point.Y);
            MapMark Mark = new MapMark();
            DataTable dt = new DataTable();
            Mark.imgheight = Mark.Height = 24;
            Mark.imgwidth = Mark.Width = 24;
            switch (command)
            {

                case 1://两通
                    Mark.nodeimage.Source = towiImg;
                    Mark.Movie = point;
                    Mark.Scale = 1 / Scale;//必须在这，保存设备不受缩放影响
                    Mark.click_proc = Node_Click;
                    Mark.GetRefreshpage = repage;
                    Mark.deletdevice = delete;
                    Mark.sign = "两通";
                    Mark.Alter = true;
                    Mark.ultisign = 0;
                    Mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                    marklayer.Children.Add(Mark);
                    da.name = Mark.Name;
                    da.lat = pointLatLng.Lat;
                    da.lng = pointLatLng.Lng;
                    dbOpera.Instance.insetdata("twowayvalve_data", "名称,纬度,经度", "'" + da.name + "'," + da.lat + "," + da.lng);
                    dt = dbOpera.Instance.getData("twowayvalve_data", "编号", "名称='" + Mark.Name + "'");
                    Mark.Name = "tw" + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("twowayvalve_data", "编号=" + dt.Rows[0]["编号"] + "", "名称='" + Mark.Name + "'");
                    break;
                case 2://三通
                    Mark.nodeimage.Source = threeiImg;
                    Mark.Movie = point;
                    Mark.Scale = 1 / Scale;
                    Mark.click_proc = Node_Click;
                    Mark.GetRefreshpage = repage;
                    Mark.deletdevice = delete;
                    Mark.sign = "三通";
                    Mark.ultisign = 0;
                    Mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                    marklayer.Children.Add(Mark);
                    th.name = Mark.Name;
                    th.lat = pointLatLng.Lat;
                    th.lng = pointLatLng.Lng;
                    dbOpera.Instance.insetdata("threewayvalve_data", "名称,纬度,经度", "'" + th.name + "'," + th.lat + "," + th.lng);
                    dt = dbOpera.Instance.getData("threewayvalve_data", "编号", "名称='" + Mark.Name + "'");
                    Mark.Name = "th" + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("threewayvalve_data", "编号=" + dt.Rows[0]["编号"] + "", "名称='" + Mark.Name + "'");
                    break;
                case 4://四通
                    Mark.nodeimage.Source = fourImg;
                    Mark.Movie = point;
                    Mark.Scale = 1 / Scale;
                    Mark.click_proc = Node_Click;
                    Mark.GetRefreshpage = repage;
                    Mark.deletdevice = delete;
                    Mark.sign = "四通";
                    Mark.ultisign = 0;
                    Mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                    marklayer.Children.Add(Mark);
                    fo.name = Mark.Name;
                    fo.lat = pointLatLng.Lat;
                    fo.lng = pointLatLng.Lng;
                    dbOpera.Instance.insetdata("fourwayvalve_data", "名称,纬度,经度", "'" + fo.name + "'," + fo.lat + "," + fo.lng);
                    dt = dbOpera.Instance.getData("fourwayvalve_data", "编号", "名称='" + Mark.Name + "'");
                    Mark.Name = "fo" + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("fourwayvalve_data", "编号=" + dt.Rows[0]["编号"] + "", "名称='" + Mark.Name + "'");
                    break;
                case 5://五通
                    Mark.nodeimage.Source = fiveImg;
                    Mark.Movie = point;
                    Mark.Scale = 1 / Scale;
                    Mark.click_proc = Node_Click;
                    Mark.GetRefreshpage = repage;
                    Mark.deletdevice = delete;
                    Mark.sign = "五通";
                    Mark.ultisign = 0;
                    Mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                    marklayer.Children.Add(Mark);
                    fi.name = Mark.Name;
                    fi.lat = pointLatLng.Lat;
                    fi.lng = pointLatLng.Lng;
                    dbOpera.Instance.insetdata("fivewayvalve_data", "名称,纬度,经度", "'" + fi.name + "'," + fi.lat + "," + fi.lng);
                    dt = dbOpera.Instance.getData("fivewayvalve_data", "编号", "名称='" + Mark.Name + "'");
                    Mark.Name = "fi" + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("fivewayvalve_data", "编号=" + dt.Rows[0]["编号"] + "", "名称='" + Mark.Name + "'");
                    break;
                case 6://供气站
                    Mark.nodeimage.Source = gasImg;
                    Mark.Movie = point;
                    Mark.Scale = 1 / Scale;
                    Mark.click_proc = Node_Click;
                    Mark.GetRefreshpage = repage;
                    Mark.deletdevice = delete;
                    Mark.sign = "供气站";
                    Mark.ultisign = 1;
                    Mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                    marklayer.Children.Add(Mark);
                    ga.name = Mark.Name;
                    ga.lat = pointLatLng.Lat;
                    ga.lng = pointLatLng.Lng;
                    dbOpera.Instance.insetdata("station_data", "名称,纬度,经度,类型", "'" + ga.name + "'," + ga.lat + "," + ga.lng + ",'" + Mark.ultisign + "'");
                    dt = dbOpera.Instance.getData("station_data", "编号", "名称='" + Mark.Name + "'");
                    Mark.Name = "ga" + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("station_data", "编号=" + dt.Rows[0]["编号"] + "", "名称='" + Mark.Name + "'");
                    break;
                case 7://调压站
                    Mark.nodeimage.Source = relguImg;
                    Mark.Movie = point;
                    Mark.Scale = 1 / Scale;
                    Mark.click_proc = Node_Click;
                    Mark.GetRefreshpage = repage;
                    Mark.deletdevice = delete;
                    Mark.sign = "调压站";
                    Mark.ultisign = 3;
                    Mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                    marklayer.Children.Add(Mark);
                    re.name = Mark.Name;
                    re.lat = pointLatLng.Lat;
                    re.lng = pointLatLng.Lng;
                    dbOpera.Instance.insetdata("regulatestation_data", "名称,纬度,经度,类型", "'" + re.name + "'," + re.lat + "," + re.lng + ",'" + Mark.ultisign + "'");
                    dt = dbOpera.Instance.getData("regulatestation_data", "编号", "名称='" + Mark.Name + "'");
                    Mark.Name = "re" + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("regulatestation_data", "编号=" + dt.Rows[0]["编号"] + "", "名称='" + Mark.Name + "'");
                    break;
                case 8://阀门
                    Mark.nodeimage.Source = valveImg;
                    Mark.Movie = point;
                    Mark.Scale = 1 / Scale;
                    Mark.click_proc = Node_Click;
                    Mark.GetRefreshpage = repage;
                    Mark.deletdevice = delete;
                    Mark.sign = "阀门";
                    Mark.ultisign = 0;
                    Mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                    marklayer.Children.Add(Mark);
                    va.name = Mark.Name;
                    va.lat = pointLatLng.Lat;
                    va.lng = pointLatLng.Lng;
                    dbOpera.Instance.insetdata("twowayvalve_data", "名称,纬度,经度", "'" + va.name + "'," + va.lat + "," + va.lng);
                    dt = dbOpera.Instance.getData("twowayvalve_data", "编号", "名称='" + Mark.Name + "'");
                    Mark.Name = "va" + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("twowayvalve_data", "编号=" + dt.Rows[0]["编号"] + "", "名称='" + Mark.Name + "'");
                    break;
                case 9://终端用户
                    Mark.nodeimage.Source = userImg;
                    Mark.Movie = point;
                    Mark.Scale = 1 / Scale;
                    Mark.click_proc = Node_Click;
                    Mark.GetRefreshpage = repage;
                    Mark.deletdevice = delete;
                    Mark.sign = "终端用户";
                    Mark.ultisign = 3;
                    Mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                    marklayer.Children.Add(Mark);
                    us.name = Mark.Name;
                    us.lat = pointLatLng.Lat;
                    us.lng = pointLatLng.Lng;
                    dbOpera.Instance.insetdata("station_data", "名称,纬度,经度,类型", "'" + us.name + "'," + us.lat + "," + us.lng + ",'" + Mark.ultisign + "'");
                    dt = dbOpera.Instance.getData("station_data", "编号", "名称='" + Mark.Name + "'");
                    Mark.Name = "us" + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("station_data", "编号=" + dt.Rows[0]["编号"] + "", "名称='" + Mark.Name + "'");
                    break;
                case 10://scada测点
                    Mark.nodeimage.Source = scadaImg;
                    Mark.Movie = point;
                    Mark.Scale = 1 / Scale;
                    Mark.click_proc = Node_Click;
                    Mark.GetRefreshpage = repage;
                    Mark.deletdevice = delete;
                    Mark.sign = "scada测点";
                    Mark.ultisign = 3;
                    Mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                    Mark.MouseLeftButtonDown += Scada_ShowDataChart;
                    marklayer.Children.Add(Mark);
                    sc.name = Mark.Name;
                    sc.lat = pointLatLng.Lat;
                    sc.lng = pointLatLng.Lng;
                    dbOpera.Instance.insetdata("station_data", "名称,纬度,经度,类型", "'" + sc.name + "'," + sc.lat + "," + sc.lng + ",'" + Mark.ultisign + "'");
                    dt = dbOpera.Instance.getData("station_data", "编号", "名称='" + Mark.Name + "'");
                    Mark.Name = "sc" + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("station_data", "编号=" + dt.Rows[0]["编号"] + "", "名称='" + Mark.Name + "'");
                    addchartdata(Mark);
                    break;
                case 11://燃气
                    Mark.nodeimage.Source = pelImg;
                    Mark.Movie = point;
                    Mark.Scale = 1 / Scale;
                    Mark.click_proc = Node_Click;
                    Mark.GetRefreshpage = repage;
                    Mark.deletdevice = delete;
                    Mark.sign = "燃气";
                    Mark.ultisign = 3;
                    Mark.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                    marklayer.Children.Add(Mark);
                    pe.name = Mark.Name;
                    pe.lat = pointLatLng.Lat;
                    pe.lng = pointLatLng.Lng;
                    dbOpera.Instance.insetdata("firegas_data", "名称,纬度,经度", "'" + pe.name + "'," + pe.lat + "," + pe.lng);
                    dt = dbOpera.Instance.getData("firegas_data", "编号", "名称='" + Mark.Name + "'");
                    Mark.Name = "pe" + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("firegas_data", "编号=" + dt.Rows[0]["编号"] + "", "名称='" + Mark.Name + "'");
                    break;
            }
        }

        //取消选中效果
        private void Mapcontrol_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Seletedrect.Visibility = Visibility.Collapsed;
            foreach (MapMark node in singglenode)
            {
                node.alteration.Visibility = Visibility.Collapsed;
            }
            singglenode.Clear();
            if (prescansal)
            {
                return;
            }
            foreach (Line myline in linelayer.Children)
            {
                myline.Stroke = Brushes.Black;
            }
        }

        #endregion

        #region 线段操作
        void Node_Click(Point pos)
        {
            directive = command;
            if (optState == optState_Type.linemode && checkonepipe() == true)//checkonepipe()确保两个设备之间不会重复添加
            {
                if (pline.Count > 0)
                {
                    if (markname.Count >= 2)//不能自己对自己添加
                    {
                        if (markname[0] == markname[1])
                        {
                            tablename.Clear();
                            markname.Clear();
                            marksign.Clear();
                            pline.Clear();
                            return;
                        }
                    }
                    Point pre_point = pline[0];
                    DataTable dt = dbOpera.Instance.getData("line_attribute_t", "编号");
                    i = dt.Rows.Count;
                    addLine("line", pre_point.X, pre_point.Y, pos.X, pos.Y, Scale);
                    double lat1 = mapcontrol.FromLocalToLatLng((int)pre_point.X, (int)pre_point.Y).Lat;
                    double lat2 = mapcontrol.FromLocalToLatLng((int)pos.X, (int)pos.Y).Lat;
                    double lng1 = mapcontrol.FromLocalToLatLng((int)pre_point.X, (int)pre_point.Y).Lng;
                    double lng2 = mapcontrol.FromLocalToLatLng((int)pos.X, (int)pos.Y).Lng;
                    lin.length = GetDistance(lat1, lng1, lat2, lng2);
                    //计算线段中心点坐标
                    double x = (lat1 + lat2) / 2;
                    double y = (lng1 + lng2) / 2;
                    string length = lin.length.ToString("f2");//保留后两位
                    pline = new List<Point>();
                    dbOpera.Instance.insetdata("line_attribute_t", "名称,长度,纬度,经度", "'" + lin.name + "'," + length + "," + x + "," + y);
                    changlname1(lin.name);
                    updatetableid();
                }
                else
                {
                    pline.Add(pos);
                }
            }
        }

        public void changlname1(string name)
        {
            DataTable dt = dbOpera.Instance.getData("line_attribute_t", "编号", "名称='" + name + "'");
            foreach (Line ml in linelayer.Children)
            {
                if (ml.Name == name)
                {
                    ml.Name = ml.Name + dt.Rows[0]["编号"];
                    dbOpera.Instance.updata("line_attribute_t", "编号 = " + dt.Rows[0]["编号"], "名称='" + ml.Name + "'");
                    lin.name = ml.Name;
                    segment = ml;
                }
            }
        }

        public bool checkonepipe()
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            if (markname.Count >= 2)
            {
                foreach (MapMark node in marklayer.Children)
                {
                    if (node.Name == markname[0])
                    {
                        dt = eqidcheck(node.sign, node.Name);
                    }
                }
                foreach (MapMark node in marklayer.Children)
                {
                    if (node.Name == markname[1])
                    {
                        dt1 = eqidcheck(node.sign, node.Name);
                    }
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Rows[0][i].ToString() != "")
                    {
                        for (int j = 0; j < dt1.Columns.Count; j++)
                        {
                            if (dt.Rows[0][i].ToString() == dt1.Rows[0][j].ToString())
                            {
                                MessageBox.Show("不能重复添加管线！");
                                pline = new List<Point>();
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        //画线
        public void addLine(string name, double x1, double y1, double x2, double y2, double width)
        {
            Line myline = new Line();
            myline.Name = name;
            lin.name = myline.Name;
            myline.X1 = x1;
            myline.Y1 = y1;
            myline.X2 = x2;
            myline.Y2 = y2;
            //double l = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            //myline.X2 = x1;
            //myline.Y2 = l;
            //double radian = Math.Atan2(y2 - y1, x2 - x1);
            //double angle = radian * (180 / Math.PI);
            //RotateTransform rt = new RotateTransform();
            //rt.CenterX = 1;
            //rt.CenterY = 1;
            //rt.Angle = angle;
            myline.StrokeThickness = 6 / width;
            myline.StrokeDashCap = PenLineCap.Round;
            myline.Cursor = System.Windows.Input.Cursors.Hand;
            //ImageBrush b = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/image/linestyle.png")));
            myline.Stroke = Brushes.Black;
            //b.Stretch = Stretch.Uniform;
            //myline.Stroke.RelativeTransform = rt;
            linelayer.Children.Add(myline);
            myline.MouseLeftButtonUp += new MouseButtonEventHandler(line_mouseLeftButtonUp);
            myline.PreviewMouseRightButtonUp += new MouseButtonEventHandler(Myline_MouseRightButtonUp);
            myline.MouseEnter += Myline_MouseEnter;
        }

        //显示管线压强
        private void Myline_MouseEnter(object sender, MouseEventArgs e)
        {
            Line line = (Line)sender;
            if (prescansal == false || resultfilename == null)
            {
                DataTable dt = dbOpera.Instance.getData("line_attribute_t", "压降Pa,质量流量", "名称='" + line.Name + "'");
                try
                {
                    pa = dt.Rows[0]["压降Pa"].ToString();
                    fl = dt.Rows[0]["质量流量"].ToString();
                }
                catch (Exception ex)
                { MessageBox.Show(ex.ToString()); }
                line.ToolTip = pa + " Pa\t\n" + fl + " Kg/s";
            }
            else
            {
                DataTable csvtable = CsvHelper.CsvToDataTable(resultfilename, 1);//显示计算后的压强
                linePMs = new List<LinePMModel>();
                foreach (DataRow rows in csvtable.Rows)
                {
                    string name = rows["名称"].ToString();
                    if (line.Name == name)
                    {
                        double press1 = checkdouble(Convert.ToString(rows["口1压力"]));
                        double press2 = checkdouble(Convert.ToString(rows["口2压力"]));
                        double press = Math.Max(press1, press2);
                        linePMs.Add(new LinePMModel { ID = Convert.ToInt32(rows["编号"]), Name = name, Flu = Convert.ToDouble(rows["质量流量kg/s"]), Pa = press });
                        pa = linePMs[0].Pa.ToString();
                        fl = linePMs[0].Flu.ToString();
                    }
                }
                line.ToolTip = pa + " Pa\t\n" + fl + " Kg/s";
            }
        }

        //删除线段并删除数据库
        private void Myline_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Line line = (Line)sender;
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = new MenuItem();
            menuItem.Header = "删除线段";
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Add(menuItem);
            line.ContextMenu = contextMenu;
            segment = line;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            linelayer.Children.Remove(segment);
            dbOpera.Instance.deleteData("line_attribute_t", "名称='" + segment.Name + "'");
            lineprop.Visibility = Visibility.Collapsed;
            pline.Clear();
        }

        //线段属性窗口
        public void line_mouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataTable dt = new DataTable();
            lineprop.Visibility = Visibility.Visible;
            Line myline = (Line)sender;
            segment = myline;
            //LineSeletrect.Visibility = Visibility.Visible;
            //LineSeletrect.SetLeft(myline.X1-1);
            //LineSeletrect.SetTop(myline.Y1-1);
            //LineSeletrect.Width = myline.StrokeThickness;
            //LineSeletrect.Height = Math.Sqrt(Math.Pow((myline.X1-myline.X2),2)+ Math.Pow((myline.Y1 - myline.Y2), 2));
            //LineSeletrect.Stroke = Brushes.Red;
            //LineSeletrect.StrokeThickness = 1;
            //TransformGroup rectt1 = rect.RenderTransform as TransformGroup;
            //RotateTransform rt = new RotateTransform();
            //rt.Angle = (Math.Atan2((myline.Y2 - myline.Y1), (myline.X2 - myline.X1)) * 180 / Math.PI) + 270;
            //LineSeletrect.RenderTransform = rt;
            if (!prescansal)
            {
                foreach (Line mline in linelayer.Children)
                {
                    mline.Stroke = Brushes.Black;
                }
                if (selectall == false)//框选颜色
                {
                    myline.Stroke = Brushes.Purple;
                }
                else
                    myline.Stroke = Brushes.MediumPurple;
            }
            //lineframe.Refresh();
            lineda = dbOpera.Instance.getData("line_attribute_t", "*", "名称='" + myline.Name + "'");
            if (lineda.Rows.Count != 0) { }
            else
            {
                lineda = dbOpera.Instance.getData("line_attribute_t", "*", "名称='" + lin.name + "'");
            }
            lineframe.Content = LineProperty;
            LineProperty.TextSearch();
        }

        #endregion

        #region 添加线和设备关系
        private void updatetableid()
        {
            DataTable dt = new DataTable();
            bool stop = false;
            try
            {
                dt = dbOpera.Instance.getData("line_attribute_t", "编号", "名称='" + lin.name + "'");
                string lineid = dt.Rows[0]["编号"].ToString();
                lin.id = System.Convert.ToInt16(lineid);//线段id
                string table1 = tablename[0];
                string name1 = markname[0];
                string sign1 = marksign[0];
                dt = dbOpera.Instance.getData(table1, "编号", "名称='" + name1 + "'");
                string equipid = dt.Rows[0]["编号"].ToString();//设备1id
                dt = eqidcheck(sign1, name1);
                for (int i = 1; i <= dt.Columns.Count && stop != true; i++)
                {
                    if (dt.Rows[0]["口" + i + "连接元件编号"].ToString() == "")
                    {
                        if (table1 == "station_data" || table1 == "firegas_data" || table1 == "regulatestation_data")
                        {
                            dbOpera.Instance.updata(table1, "名称='" + name1 + "'", "口" + i + "连接元件编号=" + lin.id + ",口" + i + "连接元件接口编号=1,端口数=端口数 - 1");
                            dbOpera.Instance.updata("line_attribute_t", "名称='" + lin.name + "'", "口1连接元件编号=" + equipid + ",口1连接元件接口编号=0");
                            stop = true;
                        }
                        else
                        {
                            dbOpera.Instance.updata(table1, "名称='" + name1 + "'", "口" + i + "连接元件编号=" + lin.id + ",口" + i + "连接元件接口编号=1,端口数=端口数 - 1" + ",口" + i + "管径=0");
                            dbOpera.Instance.updata("line_attribute_t", "名称='" + lin.name + "'", "口1连接元件编号=" + equipid + ",口1连接元件接口编号=" + i);
                            stop = true;
                        }
                    }
                }
                bool stop1 = false;
                string table2 = tablename[1];
                string name2 = markname[1];
                string sign2 = marksign[1];
                dt = dbOpera.Instance.getData(table2, "编号", "名称='" + name2 + "'");
                string equipid1 = dt.Rows[0]["编号"].ToString();//设备2id
                dt = eqidcheck(sign2, name2);
                for (int i = 1; i <= dt.Columns.Count && stop1 != true; i++)
                {
                    if (dt.Rows[0]["口" + i + "连接元件编号"].ToString() == "")
                    {
                        if (table2 == "station_data" || table2 == "firegas_data" || table2 == "regulatestation_data")
                        {
                            dbOpera.Instance.updata(table2, "名称='" + name2 + "'", "口" + i + "连接元件编号=" + lin.id + ",口" + i + "连接元件接口编号=2,端口数=端口数 - 1");
                            dbOpera.Instance.updata("line_attribute_t", "名称='" + lin.name + "'", "口2连接元件编号=" + equipid1 + ",口2连接元件接口编号=0");
                            stop1 = true;
                        }
                        else
                        {
                            dbOpera.Instance.updata(table2, "名称='" + name2 + "'", "口" + i + "连接元件编号=" + lin.id + ",口" + i + "连接元件接口编号=2,端口数=端口数 - 1" + ",口" + i + "管径=0");
                            dbOpera.Instance.updata("line_attribute_t", "名称='" + lin.name + "'", "口2连接元件编号=" + equipid1 + ",口2连接元件接口编号=" + i);
                            stop1 = true;
                        }
                    }
                }
                tablename = new List<string>();
                markname = new List<string>();
                marksign = new List<string>();
            }
            catch { }
        }

        public DataTable eqidcheck(string sign, string name)
        {
            DataTable table = new DataTable();
            switch (sign)
            {
                case "两通":
                    table = dbOpera.Instance.getData("twowayvalve_data", "口1连接元件编号,口2连接元件编号", "名称='" + name + "'");
                    break;
                case "三通":
                    table = dbOpera.Instance.getData("threewayvalve_data", "口1连接元件编号,口2连接元件编号,口3连接元件编号", "名称='" + name + "'");
                    break;
                case "四通":
                    table = dbOpera.Instance.getData("fourwayvalve_data", "口1连接元件编号,口2连接元件编号,口3连接元件编号,口4连接元件编号", "名称='" + name + "'");
                    break;
                case "五通":
                    table = dbOpera.Instance.getData("fivewayvalve_data", "口1连接元件编号,口2连接元件编号,口3连接元件编号,口4连接元件编号,口5连接元件编号", "名称='" + name + "'");
                    break;
                case "供气站":
                    table = dbOpera.Instance.getData("station_data", "口1连接元件编号,口2连接元件编号", "名称='" + name + "'");
                    break;
                case "调压站":
                    table = dbOpera.Instance.getData("regulatestation_data", "口1连接元件编号,口2连接元件编号", "名称='" + name + "'");
                    break;
                case "阀门":
                    table = dbOpera.Instance.getData("twowayvalve_data", "口1连接元件编号,口2连接元件编号", "名称='" + name + "'");
                    break;
                case "终端用户":
                    table = dbOpera.Instance.getData("station_data", "口1连接元件编号,口2连接元件编号", "名称='" + name + "'");
                    break;
                case "scada测点":
                    table = dbOpera.Instance.getData("station_data", "口1连接元件编号,口2连接元件编号", "名称 ='" + name + "'");
                    break;
                case "燃气":
                    table = dbOpera.Instance.getData("firegas_data", "口1连接元件编号,口2连接元件编号", "名称='" + name + "'");
                    break;
            }
            return table;
        }
        #endregion

        #region 删除设备
        private void delete()
        {
            string table = Device.checktable(mapMark.sign);
            DataTable dt = dbOpera.Instance.getData(table, "*", "名称='" + mapMark.Name + "'");
            if (dt.Rows.Count == 0)//数据库里没有就只删除图层上的
            {
                marklayer.Children.Remove(mapMark);
                return;
            }
            string id = dt.Rows[0]["编号"].ToString();
            Seletedrect.Visibility = Visibility.Collapsed;
            if (determinecount(mapMark.sign, id) != false)
            {
                for (int i = 1; i <= 2; i++)
                {
                    //更新管线表中所有与删除设备id相同的字段
                    dbOpera.Instance.updata("line_attribute_t", "口" + i + "连接元件编号=" + id, "口" + i + "连接元件编号=0,口" + i + "连接元件接口编号=0");
                }
                dbOpera.Instance.deleteData(table, "名称='" + mapMark.Name + "'");
                marklayer.Children.Remove(mapMark);
                deprop.Visibility = Visibility.Collapsed;
                try
                {
                    foreach (Rectangle re in selectcav.Children)
                    {
                        for (int i = 0; i < mapMark.chil.Count; i++)
                        {
                            if (mapMark.chil[i].ToString() == re.Name)
                            {
                                re.Visibility = Visibility.Collapsed;
                                mapMark.chil.RemoveAt(i);
                            }
                        }
                    }
                }
                catch { }
                finally
                {
                    if (rectDatas != null)
                    {
                        rectDatas.RemoveAll(o => o.Id == Convert.ToInt32(id));//更新datagrid
                        warming.itemSourceList = new ObservableCollection<ShowDatas>(rectDatas);
                        warming.init();
                        warmingframe.Refresh();
                        warmingframe.Content = warming;
                        if (rectDatas.Count == 0)
                        {
                            checkpanel.Visibility = Visibility.Collapsed;
                        }
                    }
                    if (mapMark.sign == "scada测点")
                    {
                        chartGroup.Visibility = Visibility.Collapsed;
                        chartpage.Visibility = Visibility.Collapsed;
                        modepage.Visibility = Visibility.Visible;
                    }
                    tablename.Clear();
                    markname.Clear();
                    marksign.Clear();
                    Seletedrect.Visibility = Visibility.Collapsed;
                    alldrect.Visibility = Visibility.Collapsed;
                }
            }
        }

        //判断设备上是否有管线
        public bool determinecount(string sign, string id)
        {
            DataTable dt = new DataTable();
            string c;
            switch (sign)
            {
                case "两通":
                    dt = dbOpera.Instance.getData("twowayvalve_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "2")
                    {
                        MessageBox.Show("请先删除与之连接的线段！");
                        return false;
                    }
                    break;
                case "三通":
                    dt = dbOpera.Instance.getData("threewayvalve_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "3")
                    {
                        MessageBox.Show("请先删除与之连接的线段！");
                        return false;
                    }
                    break;
                case "四通":
                    dt = dbOpera.Instance.getData("fourwayvalve_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "4")
                    {
                        MessageBox.Show("请先删除与之连接的线段！");
                        return false;
                    }
                    break;
                case "五通":
                    dt = dbOpera.Instance.getData("fivewayvalve_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "5")
                    {
                        MessageBox.Show("请先删除与之连接的线段！");
                        return false;
                    }
                    break;
                case "供气站":
                    dt = dbOpera.Instance.getData("station_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "2")
                    {
                        MessageBox.Show("请先删除与之连接的线段！");
                        return false;
                    }
                    break;
                case "调压站":
                    dt = dbOpera.Instance.getData("regulatestation_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "2")
                    {
                        MessageBox.Show("请先删除与之连接的线段！");
                        return false;
                    }
                    break;
                case "阀门":
                    dt = dbOpera.Instance.getData("twowayvalve_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "2")
                    {
                        MessageBox.Show("请先删除与之连接的线段！");
                        return false;
                    }
                    break;
                case "终端用户":
                    dt = dbOpera.Instance.getData("station_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "2")
                    {
                        MessageBox.Show("请先删除与之连接的线段！");
                        return false;
                    }
                    break;
                case "scada测点":
                    dt = dbOpera.Instance.getData("station_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "2")
                    {
                        MessageBox.Show("请先删除与之连接的线段！");
                        return false;
                    }
                    break;
                case "燃气":
                    dt = dbOpera.Instance.getData("firegas_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "1")
                    {
                        MessageBox.Show("请先删除与之连接的线段！");
                        return false;
                    }
                    break;
            }
            return true;
        }
        #endregion

        #region 将数据存到内存表
        public void Plusdatatolist()
        {
            devicedatas = new devicedata();
            DataTable dt = new DataTable();
            try
            {
                foreach (MapMark node in marklayer.Children)
                {
                    dt = node.signcheck(node.sign, node.Name);
                    switch (node.sign)
                    {
                        case "两通":
                            devicedatas.twdata.Add(new twowayvalve_data
                            {
                                name = dt.Rows[0]["名称"].ToString(),
                                id = Convert.ToInt32(dt.Rows[0]["编号"].ToString()),
                                equip_1_id = nulltoint(dt.Rows[0]["口1连接元件编号"].ToString()),
                                equip_2_id = nulltoint(dt.Rows[0]["口2连接元件编号"].ToString())
                            });
                            break;
                        case "三通":
                            devicedatas.thdata.Add(new threewayvalve_data
                            {
                                name = dt.Rows[0]["名称"].ToString(),
                                id = Convert.ToInt32(dt.Rows[0]["编号"].ToString()),
                                equip_1_id = nulltoint(dt.Rows[0]["口1连接元件编号"].ToString()),
                                equip_2_id = nulltoint(dt.Rows[0]["口2连接元件编号"].ToString()),
                                equip_3_id = nulltoint(dt.Rows[0]["口3连接元件编号"].ToString())
                            });
                            break;
                        case "四通":
                            devicedatas.fodata.Add(new fourwayvalve_data
                            {
                                name = dt.Rows[0]["名称"].ToString(),
                                id = Convert.ToInt32(dt.Rows[0]["编号"].ToString()),
                                equip_1_id = nulltoint(dt.Rows[0]["口1连接元件编号"].ToString()),
                                equip_2_id = nulltoint(dt.Rows[0]["口2连接元件编号"].ToString()),
                                equip_3_id = nulltoint(dt.Rows[0]["口3连接元件编号"].ToString()),
                                equip_4_id = nulltoint(dt.Rows[0]["口4连接元件编号"].ToString())
                            });
                            break;
                        case "五通":
                            devicedatas.fidata.Add(new fivewayvalve_data
                            {
                                name = dt.Rows[0]["名称"].ToString(),
                                id = Convert.ToInt32(dt.Rows[0]["编号"].ToString()),
                                equip_1_id = nulltoint(dt.Rows[0]["口1连接元件编号"].ToString()),
                                equip_2_id = nulltoint(dt.Rows[0]["口2连接元件编号"].ToString()),
                                equip_3_id = nulltoint(dt.Rows[0]["口3连接元件编号"].ToString()),
                                equip_4_id = nulltoint(dt.Rows[0]["口4连接元件编号"].ToString()),
                                equip_5_id = nulltoint(dt.Rows[0]["口5连接元件编号"].ToString())
                            });
                            break;
                        case "供气站":
                            devicedatas.gadata.Add(new gasstation_data
                            {
                                name = dt.Rows[0]["名称"].ToString(),
                                id = Convert.ToInt32(dt.Rows[0]["编号"].ToString()),
                                equip_1_id = nulltoint(dt.Rows[0]["口1连接元件编号"].ToString()),
                                equip_2_id = nulltoint(dt.Rows[0]["口2连接元件编号"].ToString())
                            });
                            break;
                        case "调压站":
                            devicedatas.redata.Add(new regulatestation_data
                            {
                                name = dt.Rows[0]["名称"].ToString(),
                                id = Convert.ToInt32(dt.Rows[0]["编号"].ToString()),
                                equip_1_id = nulltoint(dt.Rows[0]["口1连接元件编号"].ToString()),
                                equip_2_id = nulltoint(dt.Rows[0]["口2连接元件编号"].ToString())
                            });
                            break;
                        case "阀门":
                            devicedatas.vadata.Add(new valve_data
                            {
                                name = dt.Rows[0]["名称"].ToString(),
                                id = Convert.ToInt32(dt.Rows[0]["编号"].ToString()),
                                equip_1_id = nulltoint(dt.Rows[0]["口1连接元件编号"].ToString()),
                                equip_2_id = nulltoint(dt.Rows[0]["口2连接元件编号"].ToString())
                            });
                            break;
                        case "终端用户":
                            devicedatas.usdata.Add(new user_data
                            {
                                name = dt.Rows[0]["名称"].ToString(),
                                id = Convert.ToInt32(dt.Rows[0]["编号"].ToString()),
                                equip_1_id = nulltoint(dt.Rows[0]["口1连接元件编号"].ToString()),
                                equip_2_id = nulltoint(dt.Rows[0]["口2连接元件编号"].ToString())
                            });
                            break;
                        case "scada测点":
                            devicedatas.scdata.Add(new scadatest_data
                            {
                                name = dt.Rows[0]["名称"].ToString(),
                                id = Convert.ToInt32(dt.Rows[0]["编号"].ToString()),
                                equip_1_id = nulltoint(dt.Rows[0]["口1连接元件编号"].ToString()),
                                equip_2_id = nulltoint(dt.Rows[0]["口2连接元件编号"].ToString())
                            });
                            break;
                        case "燃气":
                            devicedatas.pedata.Add(new pel_data
                            {
                                name = dt.Rows[0]["名称"].ToString(),
                                id = Convert.ToInt32(dt.Rows[0]["编号"].ToString()),
                                equip_1_id = nulltoint(dt.Rows[0]["口1连接元件编号"].ToString()),
                                equip_2_id = nulltoint(dt.Rows[0]["口2连接元件编号"].ToString())
                            });
                            break;
                    }
                }
                foreach (Line myline in linelayer.Children)
                {
                    dt = dbOpera.Instance.getData("line_attribute_t", "*", "名称='" + myline.Name + "'");
                    lin.name = dt.Rows[0]["名称"].ToString();
                    lin.id = System.Convert.ToInt32(dt.Rows[0]["编号"].ToString());
                    devicedatas.linedata.Add(lin);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        /// <summary>
        /// 将值为空的列，赋值为0
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int nulltoint(string number)
        {
            int i = 0;
            if (number != "")
            {
                i = Convert.ToInt32(number);
            }
            return i;
        }
        #endregion

        #region shp文件操作

        class ImageObject
        {
            public string name = null;  //名称
            public string type = null;  //类型
            public List<Point> points = new List<Point>(); //位置信息
            public Color color = Colors.Red;   //颜色
            public double size = 0;    //线宽
            public void addPoint(Point p)
            {
                points.Add(p);
            }
        }
        static List<ImageObject> Image_objects = new List<ImageObject>();  //存储
        /*
        * 函数功能：shpLoad
        * 函数功能介绍：加载地图文件至mLayer（MLayer类型）
        * 形参列表：path（string，地图文件路径），c（string，颜色），parm（string，自定义显示样式）
        * 返回值：void
        */
        public static void shpLoad(string path, string c, string parm)
        {
            string sShpFileName = path;

            // 初始化GDAL
            m_Shp = new shpOpera();
            m_Shp.InitGDAL();

            string layerName = m_Shp.getSHPLayer(sShpFileName);

            // 获取所有属性字段名称,存放在m_FeildList中  
            m_Shp.getFields();

            featureCount = m_Shp.getFeatureCount();
            for (int i = 0; i < featureCount; i++)
            {
                MFeature mFeature = new MFeature();
                m_Shp.GetFieldContent(i, mFeature.fields);

                MGeometry mGeometry = new MGeometry();
                m_Shp.GetGeometry(i, mGeometry);

                /* ---- 数据拷贝 ---- */
                ImageObject obj = new ImageObject();
                //类型
                obj.type = mGeometry.mGeometryType;
                //属性
                //mFeature.fieldName.Equals("color")     
                //位置
                for (int j = 0; j < mGeometry.coordinates.Count; j++)
                {
                    Point point = new Point();
                    point.X = Convert.ToDouble(mGeometry.coordinates[j].Latitude);
                    point.Y = Convert.ToDouble(mGeometry.coordinates[j].Longitude);
                    obj.addPoint(point);
                }
                if (obj.points.Count > 0)
                    Image_objects.Add(obj);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Point> points = new List<Point>();
            List<System.Drawing.PointF> pts = new List<System.Drawing.PointF>();
            //int cnt = 0;
            //DataTable dt = dbOpera.Instance.getData("select count('*') from line_attribute_t");
            //nodeID = Convert.ToInt32(dt.Rows[0][0]);
            bitmap.Visibility = Visibility.Hidden;
        }

        static int cnt = 0;
        private void draw_layer()
        {
            foreach (ImageObject obj in Image_objects)
            {
                if (obj.type != "LINE") continue;
                List<Point> points = new List<Point>();
                foreach (Point point in obj.points)
                {
                    Point p = mapcontrol.setLatLng(point.X, point.Y);
                    points.Add(p);
                }
                this.Dispatcher.Invoke(new Action(() => { layer.DrawLine(points, "LINE_" + cnt.ToString(), Brushes.Red, 1); }));

                cnt++;
            }
        }
        BackgroundWorker bgw = null;
        private void NewItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            sqlStringList = new List<string>();
            MessageBoxResult result = MessageBox.Show("加载shp文件前，是否保存当前数据？", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (savepath == null)
                {
                    saveAllfile();
                }
                else
                {
                    Savethis_ItemClick(null, null);
                }
                ClearLayer(null, null);
                Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
                openFile.Filter = "SHP文件|*.shp";
                openFile.InitialDirectory = filepath;
                openFile.RestoreDirectory = false;
                openFile.Title = "选择shp文件";
                bool? opem = openFile.ShowDialog();
                if (!(bool)opem) { return; }
                string filename = openFile.FileName;
                savepath = filename;
                filepath = filename.Substring(0, filename.LastIndexOf("\\"));//获取文件路径，不带文件名
                load.Visibility = Visibility.Visible;
                sqlStringList.Add(deletext);
                dbOpera.Instance.ExcuteQuery(sqlStringList);
            }
            else if (result == MessageBoxResult.No)
            {
                ClearLayer(null, null);
                //backWorkDel.BeginInvoke(null, null);
                Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
                openFile.Filter = "SHP文件|*.shp";
                openFile.InitialDirectory = filepath;
                openFile.RestoreDirectory = false;
                openFile.Title = "选择shp文件";
                bool? opem = openFile.ShowDialog();
                if (!(bool)opem) { return; }
                string filename = openFile.FileName;
                savepath = filename;
                filepath = filename.Substring(0, filename.LastIndexOf("\\"));//获取文件路径，不带文件名
                sqlStringList.Add(deletext);
                dbOpera.Instance.ExcuteQuery(sqlStringList);
                load.Visibility = Visibility.Visible;
            }
            else
            {
                return;
            }
            bgw = new BackgroundWorker();
            bgw.DoWork += Bgw_DoWork;
            bgw.RunWorkerAsync();
            //thread = new Thread(shapeloadThread);
            //thread.Start();

            //shpLoad(AppDomain.CurrentDomain.BaseDirectory + "road\\centerline.shp", "Indigo", "");
            //shpLoad(AppDomain.CurrentDomain.BaseDirectory + "road\\surface.shp", "Red", "");
            //shpLoad(AppDomain.CurrentDomain.BaseDirectory + "Shanghai\\cityway.shp", "Red", "");


            //BitmapImage img = bitmap.getImage();
            //imageContainer.Source = img;
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            shapeloading();
        }

        private void shapeloadThread()
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new ThreadDelegate(shapeloading));
        }

        //线程里的方法
        private void shapeloading()
        {
            this.Dispatcher.BeginInvoke(new Action(() => { mapcontrol.changeLayer(5); }));

            shpLoad(savepath, "MistyRose", "");
            //shpLoad(AppDomain.CurrentDomain.BaseDirectory + "Shanghai\\way2.shp", "Pink", "");
            //shpLoad(AppDomain.CurrentDomain.BaseDirectory + "Shanghai\\way3.shp", "Salmon", "");

            //gdipainlayer1();
            draw_layer();
            this.Dispatcher.BeginInvoke(new Action(() => { load.Visibility = Visibility.Collapsed; }));

            //Thread.Sleep(3000);
        }

        #endregion

        #region 键盘事件
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                pline.Clear();
                optState = optState_Type.pointmode;
                command = 0;
                directive = 0;
            }
        }
        #endregion

        #region 导出csv文件
        private void ExportCSV_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            DataTable dt = new DataTable();
            string filename;
            marklist = new List<MapMark>();
            linelist = new List<Line>();
            if (linelayer.Children.Count == 0 || marklayer.Children.Count == 0)
            {
                MessageBox.Show("页面没有管线或设备,导出失败");
                return;
            }
            foreach (Line mline in linelayer.Children)
            {
                linelist.Add(mline);
            }
            foreach (MapMark node in marklayer.Children)
            {
                marklist.Add(node);
            }
            //DataFile dataFile = new DataFile();
            //dataFile.Show();
            string strdir = @"C:\work\" + DateTime.Now.ToString("yyyyMMddHHmmss");
            timename = DateTime.Now.ToString("yyyyMMddHHmmss");
            string strpath = strdir + "\\input\\"; //("yyyyMMddfff");
            string outpath = strdir + "\\output\\";
            if (!Directory.Exists(strdir))
            {
                Directory.CreateDirectory(strdir);
                Directory.CreateDirectory(outpath);
            }
            if (!File.Exists(strpath))
            {
                Directory.CreateDirectory(strpath);
                for (int i = 0; i < 5; i++)
                {
                    switch (i)
                    {
                        case 0:
                            filename = strpath + "SourcesAndConvergencesData" + ".csv";
                            File.Create(filename).Close();//关闭流不占用内存
                            dt = exportdata(i);
                            dbOpera.Instance.SaveCSV(dt, filename);
                            break;
                        case 1:
                            filename = strpath + "PipesData" + ".csv";
                            File.Create(filename).Close();
                            dt = exportdata(i);
                            dbOpera.Instance.SaveCSV(dt, filename);
                            break;
                        case 2:
                            filename = strpath + "TwowaysData" + ".csv";
                            File.Create(filename).Close();
                            dt = exportdata(i);
                            dbOpera.Instance.SaveCSV(dt, filename);
                            break;
                        case 3:
                            filename = strpath + "TeesData" + ".csv";
                            File.Create(filename).Close();
                            dt = exportdata(i);
                            dbOpera.Instance.SaveCSV(dt, filename);
                            break;
                        case 4:
                            filename = strpath + "CrossesData" + ".csv";
                            File.Create(filename).Close();
                            dt = exportdata(i);
                            dbOpera.Instance.SaveCSV(dt, filename);
                            break;
                    }

                }
            }
            workpath = outpath;
            MessageBox.Show("CSV文件保存成功！");
        }

        private DataTable exportdata(int i)
        {
            StringBuilder str = new StringBuilder();
            DataTable dt = new DataTable();
            try
            {
                switch (i)
                {
                    case 1:
                        foreach (Line mline in MainWindow.linelist)
                        {
                            str.Append("'" + mline.Name + "',");
                        }
                        string resultStrs = str.ToString();
                        resultStrs = resultStrs.Substring(0, resultStrs.Length - 1);
                        dt = dbOpera.Instance.getData("view_lineView", "*", "名称 in (" + resultStrs + ")");
                        break;
                    case 2:
                        foreach (MapMark node in MainWindow.marklist)
                        {
                            str.Append("'" + node.Name + "',");
                        }
                        string two = str.ToString();
                        two = two.Substring(0, two.Length - 1);
                        dt = dbOpera.Instance.getData("view_two", "*", "名称 in (" + two + ")");
                        dt = removenullrow(dt);
                        break;
                    case 3:
                        foreach (MapMark node in MainWindow.marklist)
                        {
                            str.Append("'" + node.Name + "',");
                        }
                        string three = str.ToString();
                        three = three.Substring(0, three.Length - 1);
                        dt = dbOpera.Instance.getData("view_three", "*", "名称 in (" + three + ")");
                        dt = removenullrow(dt);
                        break;
                    case 4:
                        foreach (MapMark node in MainWindow.marklist)
                        {
                            str.Append("'" + node.Name + "',");
                        }
                        string four = str.ToString();
                        four = four.Substring(0, four.Length - 1);
                        dt = dbOpera.Instance.getData("four", "*", "名称 in (" + four + ")");
                        dt = removenullrow(dt);
                        break;
                    case 5:
                        foreach (MapMark node in MainWindow.marklist)
                        {
                            str.Append("'" + node.Name + "',");
                        }
                        string five = str.ToString();
                        five = five.Substring(0, five.Length - 1);
                        dt = dbOpera.Instance.getData("five", "*", "名称 in (" + five + ")");
                        dt = removenullrow(dt);
                        break;
                    case 0:
                        foreach (MapMark node in MainWindow.marklist)
                        {
                            if (node.sign == "燃气") { }
                            else
                            {
                                str.Append("'" + node.Name + "',");
                            }

                        }
                        string station = str.ToString();
                        station = station.Substring(0, station.Length - 1);
                        dt = dbOpera.Instance.getData("station_data", "*", "名称 in (" + station + ")");
                        dt.Columns.Remove("端口数");
                        dt.Columns.Remove("经度");
                        dt.Columns.Remove("纬度");
                        dt = stationdatachange(dt);
                        dt.Columns.Remove("口2连接元件编号");
                        dt.Columns.Remove("口2连接元件接口编号");
                        dt.Columns["口1连接元件编号"].ColumnName = "连接元件编号";
                        break;
                }
            }
            catch (Exception ex)
            { MessageBox.Show("数据的不完整", "警告", MessageBoxButton.OK); }//(ex.ToString()); }
            return dt;
        }

        /// <summary>
        ///把datatable中不符合条件的行去掉 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DataTable removenullrow(DataTable data)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    if (data.Rows[i][j].ToString() == "")
                    {
                        removelist.Add(data.Rows[i]);
                        break;
                    }
                }
            }
            for (int i = 0; i < removelist.Count; i++)
            {
                data.Rows.Remove(removelist[i]);
            }
            return data;
        }

        private DataTable stationdatachange(DataTable data)
        {
            foreach (DataRow row in data.Rows)
            {
                if (row["口1连接元件编号"].ToString() == "")
                {
                    row["口1连接元件编号"] = row["口2连接元件编号"];
                    row["口1连接元件接口编号"] = row["口2连接元件接口编号"];
                }
            }
            return data;
        }
        #endregion

        #region 属性编辑框弹出
        void repage()
        {
            directive = 0;
            deprop.Visibility = Visibility.Visible;
            switch (MapMark.mark.sign)
            {
                case "供气站":
                    deframe.Refresh();
                    ultimateproperty.TextSearch();
                    deframe.Content = ultimateproperty;
                    break;
                case "终端用户":
                    deframe.Refresh();
                    ultimateproperty.TextSearch();
                    deframe.Content = ultimateproperty;
                    break;
                case "调压站":
                    deframe.Refresh();
                    regulationpro.TextSearch();
                    deframe.Content = regulationpro;
                    break;
                case "燃气":
                    deframe.Refresh();
                    firegas.TextSearch();
                    deframe.Content = firegas;
                    break;
                case "阀门":
                    deframe.Refresh();
                    valvepros.TextSearch();
                    deframe.Content = valvepros;
                    break;
                case "scada测点":
                    deframe.Refresh();
                    ultimateproperty.TextSearch();
                    deframe.Content = ultimateproperty;
                    break;
                default:
                    deframe.Refresh();
                    deviceproperty.TextSearch();
                    deframe.Content = deviceproperty;
                    break;

            }
        }

        public void addchartdata(MapMark mark)
        {
            paDatasList.Clear();
            tempDatasList.Clear();
            DataTable dt = dbOpera.Instance.getData("station_data", "*", "名称='" + mark.Name + "'");
            double fact = Convert.ToDouble(dt.Rows[0]["压力"]);
            double tem = Convert.ToDouble(dt.Rows[0]["温度"]);
            paDatasList.Add(new ChartData { Time = "12:00", Num = 700, Count = fact });
            paDatasList.Add(new ChartData { Time = "13:00", Num = 900, Count = fact });
            paDatasList.Add(new ChartData { Time = "14:00", Num = 720, Count = fact });
            paDatasList.Add(new ChartData { Time = "15:00", Num = 750, Count = fact });
            paDatasList.Add(new ChartData { Time = "16:00", Num = 760, Count = fact });
            tempDatasList.Add(new ChartData { Time = "12:00", Num = 70, Count = tem });
            tempDatasList.Add(new ChartData { Time = "13:00", Num = 70, Count = tem });
            tempDatasList.Add(new ChartData { Time = "14:00", Num = 50, Count = tem });
            tempDatasList.Add(new ChartData { Time = "15:00", Num = 60, Count = tem });
            tempDatasList.Add(new ChartData { Time = "16:00", Num = 20, Count = tem });
        }
        #endregion

        #region 状态栏坐标显示
        public void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            Point p = new Point();
            p = e.GetPosition(scanvas);
            mapcontrol.Mousemovepoint = mapcontrol.FromLocalToLatLng((int)p.X, (int)p.Y);
        }
        #endregion

        #region 加载和保存图层

        //另存为
        private void Savelayer_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            saveAllfile();
        }
        //直接保存
        private void Savethis_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            if (savepath == null)
            {
                saveAllfile();
            }
            else
            {
                addlayermessage();
                BinaryFormatter b = new BinaryFormatter();
                using (FileStream fs = new FileStream(savepath, FileMode.Create, FileAccess.Write))
                {
                    b.Serialize(fs, elemt_Datas);
                    MessageBox.Show("保存成功");
                }
            }
        }

        private void saveAllfile()
        {
            addlayermessage();
            BinaryFormatter b = new BinaryFormatter();
            Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
            saveFile.Filter = "DAT文件|*.dat";
            saveFile.InitialDirectory = filepath;
            saveFile.RestoreDirectory = false;
            bool? result = saveFile.ShowDialog();
            if (result == true)
            {
                string filename = saveFile.FileName;
                savepath = filename;
                filepath = filename.Substring(0, filename.LastIndexOf("\\"));//获取文件路径，不带文件名
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    b.Serialize(fs, elemt_Datas);
                }
                JsonHelper.InsertNewSences(filename, new string[] { resultfilename, string.Empty });
                MessageBox.Show("保存成功");
            }
            else
            {
                closed = false;
            }
        }

        private void Loadlayer_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            pressure.IsChecked = false;
            temperature.IsChecked = false;
            checkpanel.Visibility = Visibility.Collapsed;
            prescansal = false;
            sympton.Visibility = Visibility.Collapsed;
            linepiplistprop.Visibility = Visibility.Collapsed;
            deprop.Visibility = Visibility.Collapsed;
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            openFile.Filter = "DAT文件|*.dat";
            openFile.InitialDirectory = filepath;
            openFile.RestoreDirectory = false;
            bool? result = openFile.ShowDialog();
            if (result == true)
            {
                string filename = openFile.FileName;
                savepath = filename;
                filepath = filename.Substring(0, filename.LastIndexOf("\\"));//获取文件路径，不带文件名
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryFormatter b = new BinaryFormatter();
                    elemt_Datas = b.Deserialize(fs) as elemt_data;
                    //把文件中的数据反序列化到数据库中
                    addDeletedInDB<twowayvalve_data>("twowayvalve_data", elemt_Datas.two);
                    addDeletedInDB<threewayvalve_data>("threewayvalve_data", elemt_Datas.three);
                    addDeletedInDB<fourwayvalve_data>("fourwayvalve_data", elemt_Datas.four);
                    addDeletedInDB<fivewayvalve_data>("fivewayvalve_data", elemt_Datas.five);
                    addDeletedInDB<gasstation_data>("station_data", elemt_Datas.gas);
                    addDeletedInDB<regulatestation_data>("regulatestation_data", elemt_Datas.regul);
                    addDeletedInDB<scadatest_data>("station_data", elemt_Datas.scada);
                    addDeletedInDB<user_data>("station_data", elemt_Datas.user);
                    addDeletedInDB<valve_data>("twowayvalve_data", elemt_Datas.valve);
                    addDeletedInDB<pel_data>("firegas_data", elemt_Datas.pel);
                    //线
                    try
                    {
                        IList<string> linedatas = new List<string>();
                        foreach (var line in elemt_Datas.Lines)
                        {
                            string lientext = "insert into line_attribute_t" + " values (" +
                                line.id + ",'" + line.name + "'," + line.diameter + "," + line.roughness + "," +
                                line.thickness + ",'" + line.damage + "'," + line.length + "," + line.equip_1_id + "," + line.eqinter_1_id + "," + line.equip_1_h
                                + "," + line.equip_2_id + "," + line.eqinter_2_id + "," + line.equip_2_h + ","
                                + line.pressure_1 + "," + line.speed_1 + "," + line.tempera_1 + ","
                                + line.pressure_2 + "," + line.speed_2 + "," + line.tempera_2 + "," + line.pa + "," + line.flow + "," + line.coefficient + "," + line.compress + "," + line.frictional + ","
                                + line.lat + "," + line.lng + ",'" + line.material + "'," + line.startX + "," + line.startY + "," + line.endX + "," + line.endY + ")";
                            linedatas.Add(lientext);
                        }
                        dbOpera.Instance.ExcuteQuery(linedatas);
                    }
                    catch (Exception ex) {/*MessageBox.Show(ex.ToString()); */}
                    //finally
                    //{
                    chartGroup.Visibility = Visibility.Hidden;
                    modepage.Visibility = Visibility.Visible;
                    RefreshUI();
                    //通过文件名查询数据文件
                    //string[] details=JsonHelper.getFilesLocationByConfig(filename);
                    resultfilename = elemt_Datas.filepath;
                    string s1 = savepath.Substring(savepath.LastIndexOf("\\") + 1, savepath.LastIndexOf(".") - savepath.LastIndexOf("\\") - 1);
                    Sence senceOnce = null;
                    if (File.Exists(resultfilename))
                    {

                        string s2 = resultfilename; //savepath.Substring(0, savepath.LastIndexOf("\\")) + "\\output\\PipesResults.csv";
                        senceOnce = new Sence { Name = s1, Modify = "计算成功", Result = s2, FileName = savepath };
                        //resultfilename = senceOnce.Result;
                        if (resultfilename == "") return;
                        DataTable csvtable = CsvHelper.CsvToDataTable(resultfilename, 1);
                        itemSourceLists.Clear();
                        foreach (DataRow rows in csvtable.Rows)
                        {
                            string name = rows["名称"].ToString();
                            double press1 = checkdouble(Convert.ToString(rows["口1压力"]));
                            double press2 = checkdouble(Convert.ToString(rows["口2压力"]));
                            double press = Math.Max(press1, press2);
                            itemSourceLists.Add(new LineColorModel { ID = Convert.ToInt32(rows["编号"]), Name = name, Temp = Convert.ToDouble(rows["口1温度"]), Pa = press });
                        }
                        calculate.ItemsSource = null;
                        calculate.ItemsSource = itemSourceLists;
                        pressure.IsEnabled = true;
                        temperature.IsEnabled = true;
                    }
                    else
                    {
                        pressure.IsEnabled = false;
                        temperature.IsEnabled = false;
                        senceOnce = new Sence { Name = s1, Modify = "未计算或结果文件被删除", Result = string.Empty, FileName = savepath };
                    }
                    if (sencesList.Where(o => o.FileName == filename).Count() == 0) { sencesList.Add(senceOnce); tempSence = senceOnce; tempModifyStr = string.Empty; }
                    else { tempSence = sencesList.Where(o => o.FileName == filename).First(); tempModifyStr = string.Empty; }
                    sence.ItemsSource = null;
                    sence.ItemsSource = sencesList;
                    //}
                }
            }
        }

        private double checkdouble(string press)
        {
            double a = 0;
            if (press == "NaN")
            {
                a = 0;
            }
            else
            {
                a = Convert.ToDouble(press);
            }
            return a;
        }



        public void addDeletedInDB<T>(string dbName, List<T> mapNodes)
        {
            IList<string> resulist = new List<string>();
            if (mapNodes.Count <= 0) return;
            IDictionary<string, string> keyValues = new Dictionary<string, string>();
            initDictionary(keyValues);
            DataColumnCollection dcs = dbOpera.Instance.getData(dbName, "*").Columns;
            mapNodes.ForEach(o =>
            {
                if (dbOpera.Instance.getData(dbName, "*", "编号='" + typeof(T).GetField("id").GetValue(o) + "'").Rows.Count == 0)
                {
                    IList<string> propertyName = new List<string>();
                    for (int i = 0; i < dcs.Count; i++)
                    {
                        propertyName.Add(keyValues.Where(v => v.Key.Equals(dcs[i].ColumnName, StringComparison.OrdinalIgnoreCase)).First().Value);
                    }
                    StringBuilder sb = new StringBuilder();
                    foreach (var str in propertyName)
                    {
                        object obj = typeof(T).GetField(str).GetValue(o);
                        if (typeof(T).GetField(str).FieldType.Equals(typeof(double)) || typeof(T).GetField(str).FieldType.Equals(typeof(int)))//要判断要插入的数据的类型
                        {
                            sb.Append(obj + ",");
                        }
                        else if (typeof(T).GetField(str).FieldType.Equals(typeof(string)))
                        {
                            if (obj.ToString() == string.Empty)
                            {
                                obj = "NULL";
                                sb.Append(obj + ",");
                            }
                            else
                            {
                                sb.Append("'" + obj + "',");
                            }
                        }
                        else
                        {
                            sb.Append(obj + ",");
                        }

                    }
                    string excu = sb.ToString().Substring(0, sb.Length - 1);
                    string result = "insert into " + dbName + " values (" + excu + ")";
                    resulist.Add(result);
                }
            });
            dbOpera.Instance.ExcuteQuery(resulist);
        }
        private delegate void DelegateUpdataUI1();
        void RefreshUI()
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new DelegateUpdataUI1(updateUI_func1));
        }
        void updateUI_func1()
        {
            //界面元素操作

            ClearLayer(null, null);
            mapcontrol.Zoom = elemt_Datas.myGmap[0].mzoom;
            PointLatLng p = elemt_Datas.myGmap[0].center;
            mapcontrol.setCenter(p.Lat, p.Lng);
            timename = elemt_Datas.resultname;
            for (int i = 0; i < elemt_Datas.mapnode.Count; i++)
            {
                MapMark eui = new MapMark();
                eui.Name = elemt_Datas.mapnode[i].name;
                eui.sign = elemt_Datas.mapnode[i].sign;
                eui.Width = elemt_Datas.mapnode[i].width;
                eui.Height = elemt_Datas.mapnode[i].height;
                eui.node.Width = 0;
                eui.node.Height = 0;
                eui.Movie = elemt_Datas.mapnode[i].move;
                eui.imgheight = elemt_Datas.mapnode[i].imgheight;
                eui.imgwidth = elemt_Datas.mapnode[i].imgwidth;
                eui.Scale = elemt_Datas.mapnode[i].Scale;
                eui.ultisign = elemt_Datas.mapnode[i].ultisign;
                eui.nodeimage.Source = new BitmapImage(new Uri(elemt_Datas.mapnode[i].path));
                eui.click_proc = Node_Click;
                eui.GetRefreshpage = repage;
                eui.deletdevice = delete;
                eui.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                if (eui.sign == "scada测点")
                {
                    eui.MouseLeftButtonDown += Scada_ShowDataChart;
                }
                if (eui.sign == "阀门")
                {
                    DataTable dt = dbOpera.Instance.getData("select 设备状态 from twowayvalve_data where 名称='" + eui.Name + "'");
                    if (dt.Rows[0][0].ToString() == "闭合")
                    {
                        eui.nodeimage.Source = vabreImg;
                    }
                }
                marklayer.Children.Add(eui);
            }
            for (int i = 0; i < elemt_Datas.myline.Count; i++)
            {
                Line wire = new Line();
                wire.Name = elemt_Datas.myline[i].name;
                wire.X1 = elemt_Datas.myline[i].X1;
                wire.Y1 = elemt_Datas.myline[i].Y1;
                wire.X2 = elemt_Datas.myline[i].X2;
                wire.Y2 = elemt_Datas.myline[i].Y2;
                wire.StrokeThickness = 6;
                wire.StrokeDashCap = PenLineCap.Round;
                wire.Cursor = System.Windows.Input.Cursors.Hand;
                wire.Stroke = Brushes.Black;
                linelayer.Children.Add(wire);
                wire.MouseLeftButtonUp += line_mouseLeftButtonUp;
                wire.MouseEnter += Myline_MouseEnter;
                wire.MouseRightButtonUp += Myline_MouseRightButtonUp;
            }
        }
        //加载时，先清理目前的场景
        void ClearLayer(object sender, MouseEventArgs e)
        {
            marklayer.Children.RemoveRange(0, marklayer.Children.Count);
            linelayer.Children.RemoveRange(0, linelayer.Children.Count);
            //fistalayer.Children.Clear();
            //thforlayer.Children.Clear();
            rec.ForEach(p => selectcav.Children.Remove(p));
        }
        //添加界面元素信息到内存中
        public void addlayermessage()
        {
            elemt_Datas = new elemt_data();
            myMap map = new myMap();
            map.center = mapcontrol.Position;
            map.mzoom = Convert.ToInt32(mapcontrol.Zoom);
            elemt_Datas.myGmap.Add(map);
            elemt_Datas.filepath = workpath + "PipesResults.csv";
            elemt_Datas.resultname = timename;
            foreach (MapMark node in marklayer.Children)
            {
                Mapnode mapnode = new Mapnode();
                mapnode.imgheight = node.imgheight;
                mapnode.imgwidth = node.imgwidth;
                mapnode.width = node.Width;
                mapnode.height = node.Width;
                mapnode.move = node.Movie;
                mapnode.name = node.Name;
                mapnode.Scale = node.Scale;
                mapnode.ultisign = node.ultisign;
                mapnode.sign = node.sign;
                mapnode.path = checkpath(node.sign);
                mapnode.chilname = node.chil;
                elemt_Datas.mapnode.Add(mapnode);
                //序列化数据库中设备和管线的数据到文件中，作为历史数据
                switch (mapnode.sign)
                {
                    case "两通":
                        twowayvalve_data data = InsertValue<twowayvalve_data>("twowayvalve_data", mapnode.name);
                        elemt_Datas.two.Add(data);
                        break;
                    case "三通":
                        threewayvalve_data data1 = InsertValue<threewayvalve_data>("threewayvalve_data", mapnode.name);
                        elemt_Datas.three.Add(data1);
                        break;
                    case "四通":
                        fourwayvalve_data data2 = InsertValue<fourwayvalve_data>("fourwayvalve_data", mapnode.name);
                        elemt_Datas.four.Add(data2);
                        break;
                    case "五通":
                        fivewayvalve_data data3 = InsertValue<fivewayvalve_data>("fivewayvalve_data", mapnode.name);
                        elemt_Datas.five.Add(data3);
                        break;
                    case "供气站":
                        gasstation_data data4 = InsertValue<gasstation_data>("station_data", mapnode.name);
                        elemt_Datas.gas.Add(data4);
                        break;
                    case "调压站":
                        regulatestation_data data5 = InsertValue<regulatestation_data>("regulatestation_data", mapnode.name);
                        elemt_Datas.regul.Add(data5);
                        break;
                    case "阀门":
                        valve_data data6 = InsertValue<valve_data>("twowayvalve_data", mapnode.name);
                        elemt_Datas.valve.Add(data6);
                        break;
                    case "终端用户":
                        user_data data7 = InsertValue<user_data>("station_data", mapnode.name);
                        elemt_Datas.user.Add(data7);
                        break;
                    case "scada测点":
                        scadatest_data data8 = InsertValue<scadatest_data>("station_data", mapnode.name);
                        elemt_Datas.scada.Add(data8);
                        break;
                    case "燃气":
                        pel_data data9 = InsertValue<pel_data>("firegas_data", mapnode.name);
                        elemt_Datas.pel.Add(data9);
                        break;
                }
            }
            foreach (Line mline in linelayer.Children)
            {
                myline myline = new myline();
                myline.name = mline.Name;
                myline.Thickness = mline.StrokeThickness;
                myline.X1 = mline.X1;
                myline.X2 = mline.X2;
                myline.Y1 = mline.Y1;
                myline.Y2 = mline.Y2;
                DataRow dr = dbOpera.Instance.getData("line_attribute_t", "*", "名称='" + mline.Name + "'").Rows[0];
                lin = new line_attribute_t
                {
                    id = Convert.ToInt32(dr["编号"]),
                    name = dr["名称"].ToString(),
                    roughness = Convert.ToDouble(dr["粗糙度"]),//粗糙度
                    thickness = Convert.ToDouble(dr["壁厚"]),//壁厚
                    damage = dr["设备状态"].ToString(),//设备状态
                    length = Convert.ToDouble(dr["长度"]),//长度
                    equip_1_id = Convert.ToInt32(dr["口1连接元件编号"]),//口1连接元件编号,即端口1连接的设备id号
                    eqinter_1_id = Convert.ToInt32(dr["口1连接元件接口编号"]),//口1连接元件接口编号
                    equip_1_h = Convert.ToInt32(dr["口1高度"]),//口1高度
                    equip_2_id = Convert.ToInt32(dr["口2连接元件编号"]),//口2连接元件编号,即端口2连接的设备id号
                    eqinter_2_id = Convert.ToInt32(dr["口2连接元件接口编号"]),//口2连接元件接口编号
                    equip_2_h = Convert.ToInt32(dr["口2高度"]),//口2高度
                    pressure_1 = Convert.ToDouble(dr["口1压力"]),//口1压力
                    speed_1 = Convert.ToDouble(dr["口1速度"]),//口1速度    
                    tempera_1 = Convert.ToDouble(dr["口1温度"]),//口1温度
                    pressure_2 = Convert.ToDouble(dr["口2压力"]),//口2压力
                    speed_2 = Convert.ToDouble(dr["口2速度"]),//口2速度
                    tempera_2 = Convert.ToDouble(dr["口2温度"]),//口2温度
                    pa = Convert.ToDouble(dr["压降pa"]),//压降pa
                    frictional = Convert.ToDouble(dr["比摩阻"]),//比摩阻
                    flow = Convert.ToDouble(dr["质量流量"]),//质量流量
                    coefficient = Convert.ToDouble(dr["平均摩擦系数"]),//平均摩擦系数
                    compress = Convert.ToDouble(dr["压缩因子"]),//压缩因子
                    diameter = Convert.ToDouble(dr["内径"]),//内径
                    lat = Convert.ToDouble(dr["纬度"]),//压缩因子
                    lng = Convert.ToDouble(dr["经度"]),//内径
                    material = dr["材质"].ToString()//材质
                };
                elemt_Datas.Lines.Add(lin);
                elemt_Datas.myline.Add(myline);
            }
        }
        public void initDictionary(IDictionary<string, string> keyValues)
        {
            keyValues.Add("编号", "id");
            keyValues.Add("名称", "name");
            keyValues.Add("端口数", "count");
            keyValues.Add("经度", "lng");
            keyValues.Add("纬度", "lat");
            keyValues.Add("设备状态", "damage");
            keyValues.Add("口1连接元件编号", "equip_1_id");
            keyValues.Add("口1连接元件接口编号", "eqinter_1_id");
            keyValues.Add("口1管径", "diameter_1");
            keyValues.Add("口2连接元件编号", "equip_2_id");
            keyValues.Add("口2连接元件接口编号", "eqinter_2_id");
            keyValues.Add("口2管径", "diameter_2");
            keyValues.Add("口3连接元件编号", "equip_3_id");
            keyValues.Add("口3连接元件接口编号", "eqinter_3_id");
            keyValues.Add("口3管径", "diameter_3");
            keyValues.Add("口4连接元件编号", "equip_4_id");
            keyValues.Add("口4连接元件接口编号", "eqinter_4_id");
            keyValues.Add("口4管径", "diameter_4");
            keyValues.Add("口5连接元件编号", "equip_5_id");
            keyValues.Add("口5连接元件接口编号", "eqinter_5_id");
            keyValues.Add("口5管径", "diameter_5");
            keyValues.Add("类型", "sign");
            keyValues.Add("压力", "pressure");
            keyValues.Add("温度", "speed");
            keyValues.Add("速度", "tempera");
            keyValues.Add("进口压强", "inpressure");
            keyValues.Add("进口流量", "influ");
            keyValues.Add("进口温度", "intempera");
            keyValues.Add("出口压强", "outpressure");
            keyValues.Add("出口流量", "outflu");
            keyValues.Add("出口温度", "outtempera");
            keyValues.Add("甲烷", "methane");
            keyValues.Add("乙烷", "ethane");
            keyValues.Add("丙烷", "propane");
            keyValues.Add("氮气", "nitrogen");
            keyValues.Add("氢气", "hydrogen");
            keyValues.Add("二氧化碳", "carbon");
        }
        public T InsertValue<T>(string dbName, string name) where T : new()
        {
            T t = new T();
            IDictionary<string, string> keyValues = new Dictionary<string, string>();
            initDictionary(keyValues);
            DataRow dr = dbOpera.Instance.getData(dbName, "*", "名称='" + name + "'").Rows[0];
            foreach (var propertty in typeof(T).GetFields())
            {
                string colnumName = string.Empty;
                if ((colnumName = keyValues.Where(p => p.Value.Equals(propertty.Name, StringComparison.OrdinalIgnoreCase)).First().Key) != null)
                {
                    object obj = dr[colnumName];
                    if (obj.GetType().Equals(typeof(DBNull)))
                    {
                        //Console.WriteLine(propertty.GetType());
                        if (propertty.FieldType.Equals(typeof(double)) || propertty.FieldType.Equals(typeof(int)))
                        {
                            obj = 0;
                        }
                        else if (propertty.FieldType.Equals(typeof(string)))
                        {
                            obj = string.Empty;
                        }
                    }
                    if (obj.GetType().Equals(typeof(long)))
                    {
                        obj = Convert.ToInt32(obj);
                    }
                    propertty.SetValue(t, obj);
                    continue;
                }
            }
            return t;
        }
        public string checkpath(string sign)
        {
            string path = "";
            switch (sign)
            {
                case "两通":
                    path = "pack://application:,,,/image/2.png";
                    break;
                case "三通":
                    path = "pack://application:,,,/image/3.png";
                    break;
                case "四通":
                    path = "pack://application:,,,/image/4.png";
                    break;
                case "五通":
                    path = "pack://application:,,,/image/5.png";
                    break;
                case "供气站":
                    path = "pack://application:,,,/image/gsta.png";
                    break;
                case "调压站":
                    path = "pack://application:,,,/image/res.png";
                    break;
                case "阀门":
                    path = "pack://application:,,,/image/fm.png";
                    break;
                case "终端用户":
                    path = "pack://application:,,,/image/user.png";
                    break;
                case "scada测点":
                    path = "pack://application:,,,/image/scada.png";
                    break;
                case "燃气":
                    path = "pack://application:,,,/image/pel.png";
                    break;
            }
            return path;
        }
        #endregion

        #region 更改控件名称
        public void changmname(string ob)
        {
            foreach (MapMark node in marklayer.Children)
            {
                if (MapMark.mark.Name == node.Name)
                {
                    node.Name = ob;
                }
            }
        }

        public void changlname(string ob)
        {
            foreach (Line line in linelayer.Children)
            {
                if (line.Name == segment.Name)
                {
                    line.Name = ob;
                }
            }
        }
        #endregion

        #region 框选功能
        private void Rectsel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            mapcontrol.Zoom = 18;
            drawrectangle.Background = Brushes.Transparent;
            foreach (MapMark node in singglenode)
            {
                node.alteration.Visibility = Visibility.Collapsed;
            }
            singglenode.Clear();
        }

        private void Drawrectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            location = e.GetPosition(drawrectangle);
            markp = e.GetPosition(marklayer);
        }

        private void Drawrectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DrawrectangleLeftMouseEvent != null)
            {
                DrawrectangleLeftMouseEvent.Invoke(sender, e);
                DrawrectangleLeftMouseEvent -= this.Drawrectangle_MouseLeftButtonUpEvent;
                flag = false;
            }
        }
        //画刷
        //public LinearGradientBrush getLineBrush()
        //{

        //    var myLinearGradientBrush = new LinearGradientBrush();
        //    myLinearGradientBrush.StartPoint = new Point(0, 0);
        //    myLinearGradientBrush.EndPoint = new Point(1, 1);
        //    myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
        //    myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
        //    myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
        //    myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));
        //    return myLinearGradientBrush;
        //}
        private void Drawrectangle_MouseLeftButtonUpEvent(object sender, MouseButtonEventArgs e)
        {
            linepiplistprop.Visibility = Visibility.Visible;
            rectDatas = new List<ShowDatas>();
            if (selcetnode.Count > 0)//恢复上次的设备图片
            {
                foreach (MapMark n in selcetnode)
                {
                    selectImg(n);
                }
                selcetnode = new List<MapMark>();
            }
            foreach (MapMark node in marklayer.Children)
            {
                Point p = node.Movie;
                if (IsInside(p))
                {
                    selcetnode.Add(node);
                    //Rectangle rect = new Rectangle();
                    //rect.Width = 24;
                    //rect.Height = 24;
                    //rect.StrokeThickness = 5;
                    //rect.SetLeft(node.Movie.X);
                    //rect.SetTop(node.Movie.Y);
                    //rect.Stroke = new SolidColorBrush(Colors.BlueViolet);
                    //mapMarkBorders.Add(rect);
                    //selectcav.Children.Add(rect);
                    DataRow markDB = node.signcheck(node.sign, node.Name).Rows[0];
                    rectDatas.Add(new ShowDatas { Id = Convert.ToInt32(markDB["编号"]), Name = markDB["名称"].ToString(), Type = node.sign });
                    selectall = true;
                }
            }
            foreach (MapMark n in selcetnode)
            {
                selectnormalImg(n);
            }
            foreach (Line node in linelayer.Children)
            {
                node.Stroke = Brushes.Black;
                if (IsInside(new Point(node.X1, node.Y1)) && IsInside(new Point(node.X2, node.Y2)))
                {
                    node.Stroke = Brushes.Purple; //getLineBrush();
                    DataRow LineTable = dbOpera.Instance.getData("line_attribute_t", "*", "名称 = '" + node.Name + "'").Rows[0];
                    rectDatas.Add(new ShowDatas { Id = Convert.ToInt32(LineTable["编号"]), Name = LineTable["名称"].ToString(), Type = "管线" });
                    selectall = true;
                }
            }
            if (rectDatas.Count <= 0) { rectDatas.Clear(); MessageBox.Show("区域内没有设备"); };
            LinePipListProp.itemSourceList = new ObservableCollection<ShowDatas>(rectDatas);
            LinePipListProp.init();
            linepipframe.Content = LinePipListProp;
        }

        private bool IsInside(Point p)
        {
            return p.X >= markp.X && p.X <= markp.X + logicrect.Width && p.Y >= markp.Y && p.Y <= markp.Y + logicrect.Height;
        }

        private void Drawrectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(drawrectangle);
                if (p != location && rect == null)
                {
                    rect = new Rectangle() { Fill = Brushes.Transparent };
                    drawrectangle.Children.Add(rect);
                }
                rect.Width = Math.Abs(p.X - location.X);
                rect.Height = Math.Abs(p.Y - location.Y);
                rect.StrokeThickness = 1;
                rect.Stroke = Brushes.Black;
                rect.MouseRightButtonUp += Rect_MouseRightButtonUp;
                //绑定鼠标弹起事件
                if (!flag)
                {
                    DrawrectangleLeftMouseEvent += this.Drawrectangle_MouseLeftButtonUpEvent;
                    flag = true;
                }
                Canvas.SetLeft(rect, Math.Min(p.X, location.X));
                Canvas.SetTop(rect, Math.Min(p.Y, location.Y));

                Point mp = e.GetPosition(marklayer);
                if (mp != markp && logicrect == null)
                {
                    logicrect = new Rectangle() { Fill = Brushes.Transparent };

                }
                logicrect.Width = Math.Abs(mp.X - markp.X);
                logicrect.Height = Math.Abs(mp.Y - markp.Y);
                Canvas.SetLeft(logicrect, Math.Min(mp.X, markp.X));
                Canvas.SetTop(logicrect, Math.Min(mp.Y, markp.Y));
            }
        }

        private void Rect_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            rect = (Rectangle)sender;
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = new MenuItem();
            menuItem.Header = "退出框选";
            menuItem.Click += MenuItem_Click1;
            contextMenu.Items.Add(menuItem);
            rect.ContextMenu = contextMenu;
        }


        private void MenuItem_Click1(object sender, RoutedEventArgs e)
        {
            foreach (MapMark n in selcetnode)
            {
                selectImg(n);
            }
            foreach (Line node in linelayer.Children)
            {
                node.Stroke = Brushes.Black;
            }
            alldrect.Visibility = Visibility.Collapsed;
            selectall = false;
            drawrectangle.Children.Remove(rect);
            drawrectangle.Background = null;
            rect = null;
            logicrect = null;
            linepiplistprop.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region scada测点
        private void Scada_ShowDataChart(object sender, MouseButtonEventArgs e)
        {
            if (optState == optState_Type.linemode) return;
            MapMark scada = (MapMark)sender;
            scadaNow = scada;
            DataRow dr = dbOpera.Instance.getData("station_data", "*", "名称='" + scada.Name + "'").Rows[0];
            LayoutTemp.Caption = "温度(℃) 测点编号:" + dr["编号"];
            LayoutPa.Caption = "压强(Pa) 测点编号:" + dr["编号"];
            IDictionary<string, IDictionary<string, double>> dics = new Dictionary<string, IDictionary<string, double>>();
            IDictionary<string, double> dic = new Dictionary<string, double>();
            IDictionary<string, double> dic1 = new Dictionary<string, double>();
            foreach (var item in paDatasList)
            {
                dic.Add(item.Time, item.Count);
            }
            foreach (var item in paDatasList)
            {
                dic1.Add(item.Time, item.Num);
            }
            dics.Add("测点压强", dic);
            dics.Add("模拟压强", dic1);

            IDictionary<string, IDictionary<string, double>> tempDics = new Dictionary<string, IDictionary<string, double>>();
            IDictionary<string, double> dicss1 = new Dictionary<string, double>();

            IDictionary<string, double> dics1 = new Dictionary<string, double>();
            foreach (var item in tempDatasList)
            {
                dics1.Add(item.Time, item.Num);
            }
            tempDics.Add("测点温度", dicss1);
            tempDics.Add("模拟温度", dics1);

            DrawCharts(diagram1, dics, false, true);
            DrawCharts(diagram2, tempDics, false, true);
            DrawCharts(diagram3, dics, true, true);
            DrawCharts(diagram4, tempDics, true, true);
            modepage.Visibility = Visibility.Collapsed;
            chartpage.Visibility = Visibility.Visible;
            simuchart.ItemsSource = paDatasList;
        }
        void DrawCharts(XYDiagram2D diagram, IDictionary<string, IDictionary<string, double>> datas, bool isLine, bool isShowLabel)
        {
            diagram.Series.Clear();
            int index = 0;
            foreach (var item in datas)
            {
                Series series = null;
                if (isLine)
                {
                    series = new LineSeries2D();
                }
                else
                {
                    series = new BarSideBySideSeries2D();
                    SeriesLabel label = new SeriesLabel();
                    label.Indent = 20;
                    series.Label = label;
                    if (isShowLabel)
                    {
                        BarSideBySideSeries2D.SetLabelPosition(series.Label, Bar2DLabelPosition.Outside);
                    }
                }
                series.Name = "series" + index;
                series.DisplayName = item.Key;
                //设置series的数据源       
                series.DataSource = item.Value;
                series.LabelsVisibility = isShowLabel;
                series.ArgumentDataMember = "Key";
                series.ValueDataMember = "Value";
                //向XYDiagram中添加series
                diagram.Series.Add(series);
            }
            chartGroup.Visibility = Visibility.Visible;
        }
        #endregion

        #region 打印管道图
        private void Print_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            //MessageBox.Show(marklayer.Margin.ToString() + "\n" + linelayer.Margin.ToString());
            //4个图层的缩放和移动是一起的，所以只要知道其中一个图层的坐标即可
            Point a = new Point();
            a.X = layer.Margin.Left;
            a.Y = layer.Margin.Top;
            Point b = new Point();
            b.X = layer.Margin.Left + layer.Width;
            b.Y = layer.Margin.Top + layer.Height;
            PointLatLng point = mapcontrol.FromLocalToLatLng((int)a.X, (int)a.Y);
            PointLatLng point1 = mapcontrol.FromLocalToLatLng((int)b.X, (int)b.Y);
            Point m = new Point(Gmap.ActualWidth / 2, Gmap.ActualHeight / 2);
            PointLatLng center = mapcontrol.FromLocalToLatLng((int)m.X, (int)m.Y);
            try
            {
                //ImageSource img = mapcontrol.ToImageSource();
                //PngBitmapEncoder en = new PngBitmapEncoder();
                //en.Frames.Add(BitmapFrame.Create(img as BitmapSource));
                if (linelayer.Children.Count > 10)
                {
                    mapcontrol.Zoom = 17;
                    mapcontrol.Position = center;
                    marklayer.Visibility = Visibility.Collapsed;
                }
                else if (linelayer.Children.Count > 50)
                {
                    mapcontrol.Zoom = 16;
                    mapcontrol.Position = center;
                    marklayer.Visibility = Visibility.Collapsed;
                }
                else if (linelayer.Children.Count > 100)
                {
                    mapcontrol.Zoom = 15;
                    mapcontrol.Position = center;
                    marklayer.Visibility = Visibility.Collapsed;
                }
                else if (linelayer.Children.Count > 200)
                {
                    mapcontrol.Zoom = 13;
                    mapcontrol.Position = center;
                    marklayer.Visibility = Visibility.Collapsed;
                }

                RenderTargetBitmap rtb = new RenderTargetBitmap((int)linelayer.RenderSize.Width,
               (int)linelayer.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);

                rtb.Render(layer);
                rtb.Render(linelayer);
                rtb.Render(scanvas);
                rtb.Render(marklayer);


                var crop = new CroppedBitmap(rtb, new Int32Rect(0, 0, (int)linelayer.RenderSize.Width, (int)linelayer.RenderSize.Height));

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Pipe Image"; // Default file name
                dlg.DefaultExt = ".png"; // Default file extension
                dlg.Filter = "Image (.png)|*.png"; // Filter files by extension
                dlg.AddExtension = true;
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                // Show save file dialog box
                bool? result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;

                    using (System.IO.Stream st = System.IO.File.OpenWrite(filename))
                    {
                        pngEncoder.Save(st);
                        //en.Save(st);
                    }
                }
                marklayer.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 查询设备和管线
        private void Idselect_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            Select window = new Select();
            window.Owner = this;
            window.TransfEvent += Window_TransfEvent;
            window.clearesult += Window_clearesult;
            mapcontrol.Zoom = 18;
            window.ShowDialog();
        }

        private void Window_TransfEvent(string value)
        {
            string result = value;
            foreach (Line li in linelayer.Children)
            {
                li.Stroke = Brushes.Black;
            }
            foreach (MapMark node in marklayer.Children)
            {
                if (node.Name == result)
                {
                    Seletedrect.SetLeft(node.Movie.X);
                    Seletedrect.SetTop(node.Movie.Y);
                    Seletedrect.Visibility = Visibility.Visible;
                    DataTable dt = node.getposition(node.sign, node.Name);
                    double lat = Convert.ToDouble(dt.Rows[0]["纬度"]);//地图中心点移动到设备上
                    double lng = Convert.ToDouble(dt.Rows[0]["经度"]);

                    mapcontrol.setCenter(lat, lng);
                }
            }
            foreach (Line mline in linelayer.Children)
            {
                if (mline.Name == result)
                {
                    mline.Stroke = Brushes.Purple;
                    DataTable dt = dbOpera.Instance.getData("line_attribute_t", "纬度,经度", "名称='" + mline.Name + "'");
                    mapcontrol.setCenter(Convert.ToDouble(dt.Rows[0]["纬度"]), Convert.ToDouble(dt.Rows[0]["经度"]));
                    /* PointLatLng p = mapcontrol.FromLocalToLatLng((int)x, (int)y);*///地图中心点移动到管线中心位置
                                                                                      //mapcontrol.setCenter(x, y);
                }
            }
        }

        private void Window_clearesult()
        {
            Seletedrect.Visibility = Visibility.Collapsed;
            foreach (Line myline in linelayer.Children)
            {
                myline.Stroke = Brushes.Black;
            }
        }

        #endregion

        #region 检查设备通道
        private void Lackchk_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            mapcontrol.Zoom = 18;
            rec.ForEach(p => selectcav.Children.Remove(p));
            List<MapMark> check = new List<MapMark>();
            rectDatas = new List<ShowDatas>();
            foreach (MapMark node in marklayer.Children)
            {
                string name = node.Name;
                string sign = node.sign;
                int usign = node.ultisign;
                DataTable dt = node.signcheck(sign, name);
                string id = dt.Rows[0]["编号"].ToString();
                if (determineline(sign, id) == true && usign != 3)
                {
                    check.Add(node);
                }
            }
            if (check.Count != 0)
            {
                MessageBox.Show("设备通道有缺失！");
                foreach (MapMark mark in check)
                {
                    Rectangle rect = new Rectangle();
                    rect.Name = "selre" + i; i++;
                    rect.Width = 24;
                    rect.Height = 24;
                    rect.StrokeThickness = 5;
                    rect.SetLeft(mark.Movie.X);
                    rect.SetTop(mark.Movie.Y);
                    rect.Stroke = new SolidColorBrush(Colors.Red);
                    double sc = mapcontrol.Zoom - mapcontrol.mapZoom;
                    sc = Math.Pow(2, sc);
                    rect.RenderTransform = new TransformGroup()//缩放问题
                    {
                        Children = new TransformCollection()
                        {
                            new ScaleTransform(){ScaleX=1 / sc,ScaleY=1 / sc}
                        }
                    };
                    rect.RenderTransformOrigin = new Point(0.5, 0.5);
                    mapMarkBorders.Add(rect);
                    selectcav.Children.Add(rect);
                    rec.Add(rect);
                    mark.chil.Add(rect.Name);
                    DataRow markDB = mark.signcheck(mark.sign, mark.Name).Rows[0];
                    rectDatas.Add(new ShowDatas { Id = Convert.ToInt32(markDB["编号"]), Name = markDB["名称"].ToString(), Type = mark.sign });

                }
                checkpanel.Visibility = Visibility.Visible;
                warming.itemSourceList = new ObservableCollection<ShowDatas>(rectDatas);
                warming.init();
                warmingframe.Content = warming;
            }
            else
            {
                checkpanel.Visibility = Visibility.Collapsed;
                MessageBox.Show("设备通道完整");
            }
        }

        private void Result_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            MapMark ma = dg.SelectedItem as MapMark;
            DataTable dt = ma.getposition(ma.sign, ma.Name);
            double lat = Convert.ToDouble(dt.Rows[0]["纬度"]);//地图中心点移动到设备上
            double lng = Convert.ToDouble(dt.Rows[0]["经度"]);
            mapcontrol.setCenter(lat, lng);
        }


        public bool determineline(string sign, string id)
        {
            DataTable dt = new DataTable();
            string c = "";
            switch (sign)
            {
                case "两通":
                    dt = dbOpera.Instance.getData("twowayvalve_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "0")
                    {
                        return true;
                    }
                    break;
                case "三通":
                    dt = dbOpera.Instance.getData("threewayvalve_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "0")
                    {
                        return true;
                    }
                    break;
                case "四通":
                    dt = dbOpera.Instance.getData("fourwayvalve_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "0")
                    {
                        return true;
                    }
                    break;
                case "五通":
                    dt = dbOpera.Instance.getData("fivewayvalve_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "0")
                    {
                        return true;
                    }
                    break;
                case "供气站":
                    dt = dbOpera.Instance.getData("station_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    int a = Convert.ToInt32(c);
                    if (a > 1)
                    {
                        return true;
                    }
                    break;
                case "调压站":
                    dt = dbOpera.Instance.getData("regulatestation_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "0")
                    {
                        return true;
                    }
                    break;
                case "阀门":
                    dt = dbOpera.Instance.getData("twowayvalve_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "0")
                    {
                        return true;
                    }
                    break;
                case "终端用户":
                    dt = dbOpera.Instance.getData("station_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "0")
                    {
                        return true;
                    }
                    break;
                case "scada测点":
                    dt = dbOpera.Instance.getData("station_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "0")
                    {
                        return true;
                    }
                    break;
                case "燃气":
                    dt = dbOpera.Instance.getData("firegas_data", "端口数", "编号=" + id);
                    c = dt.Rows[0]["端口数"].ToString();
                    if (c != "0")
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        #endregion

        #region 检查设备之间的联通性

        class disConDev
        {
            public int id = 0;  //设备节点
            public string notype;
            public List<int> unReached = new List<int>(); //不能到达的设备节点
        }

        class disConDevList : Collection<disConDev>
        {
            public void addDevUnreached(int id, int unId, string type)
            {
                foreach (disConDev dev in this)
                {
                    if (dev.id == id)
                    {
                        dev.unReached.Add(unId);
                        return;
                    }
                }
                disConDev new_dev = new disConDev();
                new_dev.id = id;
                new_dev.unReached.Add(unId);
                new_dev.notype = type;
                this.Add(new_dev);
            }
        }
        disConDevList disConDevs = new disConDevList();  //不连通的设备列表

        private void Connectchk_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            //rec.ForEach(p => selectcav.Children.Remove(p));
            optState = optState_Type.pointmode;
            Dijkstra dijk = new Dijkstra();
            disConDevs = new disConDevList();
            rectDatas = new List<ShowDatas>();

            Plusdatatolist();

            //处理所有设备节点
            foreach (twowayvalve_data twdata in devicedatas.twdata)
            {
                List<int> lines = new List<int>();
                lines.Add(twdata.equip_1_id);
                lines.Add(twdata.equip_2_id);
                dijk.nodes.Add(new Dijkstra.Node(twdata.id, twdata.lat, twdata.lng, lines, "两通"));
            }
            foreach (threewayvalve_data thdata in devicedatas.thdata)
            {
                List<int> lines = new List<int>();
                lines.Add(thdata.equip_1_id);
                lines.Add(thdata.equip_2_id);
                lines.Add(thdata.equip_3_id);
                dijk.nodes.Add(new Dijkstra.Node(thdata.id, thdata.lat, thdata.lng, lines, "三通"));
            }
            foreach (fourwayvalve_data fodata in devicedatas.fodata)
            {
                List<int> lines = new List<int>();
                lines.Add(fodata.equip_1_id);
                lines.Add(fodata.equip_2_id);
                lines.Add(fodata.equip_3_id);
                lines.Add(fodata.equip_4_id);
                dijk.nodes.Add(new Dijkstra.Node(fodata.id, fodata.lat, fodata.lng, lines, "四通"));
            }
            foreach (fivewayvalve_data fidata in devicedatas.fidata)
            {
                List<int> lines = new List<int>();
                lines.Add(fidata.equip_1_id);
                lines.Add(fidata.equip_2_id);
                lines.Add(fidata.equip_3_id);
                lines.Add(fidata.equip_4_id);
                lines.Add(fidata.equip_5_id);
                dijk.nodes.Add(new Dijkstra.Node(fidata.id, fidata.lat, fidata.lng, lines, "五通"));
            }
            foreach (scadatest_data scdata in devicedatas.scdata)
            {
                List<int> lines = new List<int>();
                lines.Add(scdata.equip_1_id);
                lines.Add(scdata.equip_2_id);
                dijk.nodes.Add(new Dijkstra.Node(scdata.id, scdata.lat, scdata.lng, lines, "scada测点"));
            }
            foreach (user_data usdata in devicedatas.usdata)
            {
                List<int> lines = new List<int>();
                lines.Add(usdata.equip_1_id);
                lines.Add(usdata.equip_2_id);
                dijk.nodes.Add(new Dijkstra.Node(usdata.id, usdata.lat, usdata.lng, lines, "终端用户"));
            }
            foreach (pel_data pedata in devicedatas.pedata)
            {
                List<int> lines = new List<int>();
                lines.Add(pedata.equip_1_id);
                lines.Add(pedata.equip_2_id);
                dijk.nodes.Add(new Dijkstra.Node(pedata.id, pedata.lat, pedata.lng, lines, "燃气"));
            }
            foreach (valve_data vadata in devicedatas.vadata)
            {
                List<int> lines = new List<int>();
                lines.Add(vadata.equip_1_id);
                lines.Add(vadata.equip_2_id);
                dijk.nodes.Add(new Dijkstra.Node(vadata.id, vadata.lat, vadata.lng, lines, "阀门"));
            }
            foreach (regulatestation_data redata in devicedatas.redata)
            {
                List<int> lines = new List<int>();
                lines.Add(redata.equip_1_id);
                lines.Add(redata.equip_2_id);
                dijk.nodes.Add(new Dijkstra.Node(redata.id, redata.lat, redata.lng, lines, "调压站"));
            }
            foreach (gasstation_data gadata in devicedatas.gadata)
            {
                List<int> lines = new List<int>();
                lines.Add(gadata.equip_1_id);
                lines.Add(gadata.equip_2_id);
                dijk.nodes.Add(new Dijkstra.Node(gadata.id, gadata.lat, gadata.lng, lines, "供气站"));
            }

            //处理所有设备连线
            foreach (line_attribute_t line in devicedatas.linedata)
            {
                bool ret = dijk.nodes.addArc_By_Line(line.id);
            }
            MapMark ma = new MapMark();
            //计算所有节点之间通路
            for (int i = 0; i < dijk.nodes.Count; i++)
            {
                for (int j = i + 1; j < dijk.nodes.Count; j++)
                {
                    Dijkstra.FindPath fp = new Dijkstra.FindPath();
                    if (!fp.DijkstraSearch(dijk.nodes, i, j))
                    {
                        disConDevs.addDevUnreached(dijk.nodes[i].DeviceID, dijk.nodes[j].DeviceID, dijk.nodes[i].mtype);
                        //DataRow markDB = ma.signcheck(mark.sign, mark.Name).Rows[0];
                        //rectDatas.Add(new ShowDatas { Id = dijk.nodes[i].DeviceID, Name = "", Type = "" });
                        //disConDevs.addDevUnreached(i, j);
                        //break;  //不需要全部列表

                    }
                }
            }
            for (int i = 0; i < disConDevs.Count; i++)
            {
                rectDatas.Add(new ShowDatas { Id = disConDevs[i].id, Name = disConDevs[i].id.ToString(), Type = disConDevs[i].notype });
            }
            checkpanel.Visibility = Visibility.Visible;
            warming.itemSourceList = new ObservableCollection<ShowDatas>();
            warming.itemSourceList = new ObservableCollection<ShowDatas>(rectDatas);
            warming.init();
            warmingframe.Content = warming;
        }
        #endregion

        #region 模拟展示
        public List<testnode> nodelist { get; set; }
        private void KapDisplay_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            scenename = null;
            itemSourceLists = new List<LineColorModel>();
            if (workpath == "C:\\work\\")
            {
                MessageBox.Show("请先导出需要计算的数据");
                return;
            }
            Sight scene = new Sight();
            scene.Owner = this;
            scene.ShowDialog();
            if (scenename == null) return;
            string path;
            string filename = "C:\\work\\" + timename + "\\" + scenename + ".dat";
            savepath = filename;
            Sence senceOnce = new Sence { Name = scenename, Modify = "计算中...", Result = string.Empty, FileName = filename };
            if (sencesList.Where(o => o.FileName == filename).Count() == 0) { sencesList.Add(senceOnce); tempSence = senceOnce; tempModifyStr = string.Empty; }
            else { tempSence = sencesList.Where(o => o.Name == savepath).First(); tempModifyStr = string.Empty; }
            sence.ItemsSource = sencesList;
            try
            {
                //异步计算
                var asyAction = new Func<string, bool>((x) =>
                {
                    try
                    {
                        modDoNetCalculate.doCalculate(x);
                    }
                    catch { return false; }
                    return true;
                });
                asyAction.BeginInvoke(timename, new AsyncCallback(x =>
                {
                    var func = x.AsyncState as Func<string, bool>;
                    bool result = func.EndInvoke(x);
                    if (result)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (workpath == "")
                            {
                                path = elemt_Datas.filepath;
                            }
                            else
                            {
                                path = workpath + "PipesResults.csv";
                            }
                            resultfilename = path; // System.IO.Path.GetFileName(path);
                            savecountfile(filename);
                            DataTable csvtable = CsvHelper.CsvToDataTable(path, 1);
                            foreach (DataRow rows in csvtable.Rows)
                            {
                                double temp;
                                bool flag = double.TryParse(rows["压降Pa"].ToString(), out temp);
                                if (flag)
                                {
                                    string name = rows["名称"].ToString();
                                    double press1 = checkdouble(Convert.ToString(rows["口1压力"]));
                                    double press2 = checkdouble(Convert.ToString(rows["口2压力"]));
                                    double press = Math.Max(press1, press2);
                                    itemSourceLists.Add(new LineColorModel { ID = Convert.ToInt32(rows["编号"]), Name = name, Temp = Convert.ToDouble(rows["口1温度"]), Pa = press });
                                }
                            }
                            MessageBox.Show("计算成功");
                            sencesList.Where(o => o.FileName == filename).First().Modify = "计算完成";
                            sencesList.Where(o => o.FileName == filename).First().Result = path;
                            sence.ItemsSource = null;
                            sence.ItemsSource = sencesList;
                            calculate.ItemsSource = itemSourceLists;
                                //计算就添加场景列表     
                                prescansal = true;//管线颜色不消失
                            pressure.IsEnabled = true;
                            temperature.IsEnabled = true;
                        }));
                    }
                }), asyAction);
            }
            catch (Exception ex)
            {
                sencesList.Remove(senceOnce);
                sence.ItemsSource = null;
                sence.ItemsSource = sencesList;
                //MessageBox.Show(ex.ToString());
            }
        }
        //计算时保存
        void savecountfile(string fileName)
        {
            addlayermessage();
            BinaryFormatter b = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                b.Serialize(fs, elemt_Datas);
            }
            JsonHelper.InsertNewSences(fileName, new string[] { resultfilename, string.Empty });
        }
        //加载场景时treeview生成

        void ShowResultToDg(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            string csvPath = JsonHelper.GetDataFilePath(item.Header.ToString());
            DataTable csvtable = CsvHelper.CsvToDataTable(csvPath, 1);
            foreach (DataRow rows in csvtable.Rows)
            {
                itemSourceLists.Add(new LineColorModel { ID = Convert.ToInt32(rows["编号"]), Name = rows["名称"].ToString(), Temp = Convert.ToDouble(rows["口1温度"]), Pa = Convert.ToDouble(rows["压降Pa"]) });
            }
            calculate.ItemsSource = itemSourceLists;
        }
        //更改管线颜色
        void ModifyPipColorByData(object sender, MouseButtonEventArgs e)
        {

            TreeViewItem item = e.Source as TreeViewItem;
            string csvPath = JsonHelper.GetDataFilePath(item.Header.ToString());
            DataTable csvtable = CsvHelper.CsvToDataTable(csvPath, 1);
            foreach (Line node in linelayer.Children)
            {
                try
                {
                    DataRow LineTable = dbOpera.Instance.getData("line_attribute_t", "*", "名称 = '" + node.Name + "'").Rows[0];
                    foreach (DataRow row in csvtable.Rows)
                    {
                        if (Convert.ToInt32(LineTable["编号"]) == Convert.ToInt32(row["编号"]))
                        {
                            node.Stroke = checkValueToColorBrush(Convert.ToDouble(row["压降Pa"]));
                            break;
                        }
                    }
                }
                catch { }
            }
        }

        void RefreshBySences(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;

        }

        public SolidColorBrush checkValueToColorBrush(double kaspNum)
        {
            double pa = Math.Abs(kaspNum);
            double min = sympton.Legend_Minimum;
            double max = sympton.Legend_Maximum;
            int part = (int)(((max - min) / 8) + 0.5F);
            int kasp = 0;
            if (pa <= color[0])
            {
                kasp = 0;
            }
            else if (pa > color[0] && pa <= color[1])
            {
                kasp = 1;
            }
            else if (pa > color[1] && pa <= color[2])
            {
                kasp = 2;
            }
            else if (pa > color[2] && pa <= color[3])
            {
                kasp = 3;
            }
            else if (pa > color[3] && pa <= color[4])
            {
                kasp = 4;
            }
            else if (pa > color[4] && pa <= color[5])
            {
                kasp = 5;
            }
            else if (pa > color[5] && pa <= color[6])
            {
                kasp = 6;
            }
            else if (pa > color[6] && pa <= color[7])
            {
                kasp = 7;
            }
            else if (pa > color[7] && pa <= color[8])
            {
                kasp = 8;
            }
            else if (pa > color[8] && pa <= color[9])
            {
                kasp = 9;
            }
            SolidColorBrush brush = new SolidColorBrush();
            switch (kasp)
            {
                case 0:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF0000FF");
                    break;
                case 1:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF0099FF");
                    break;
                case 2:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF00FFFF");
                    break;
                case 3:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF00FF00");
                    break;
                case 4:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF99FF00");
                    break;
                case 5:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFCCFF00");
                    break;
                case 6:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF00");
                    break;
                case 7:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFCC00");
                    break;
                case 8:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFF9900");
                    break;
                case 9:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFF0000");
                    break;
            }
            return brush;
        }

        public SolidColorBrush checkValueToColorBrush1(double kaspNum)
        {
            double pa = Math.Abs(kaspNum);
            double min = sympton.Legend_Minimum;
            double max = sympton.Legend_Maximum;
            double part = ((max - min) / 9);
            int kasp = 0;
            if (pa <= min)
            {
                kasp = 0;
            }
            else if (pa > min && pa <= (min + part * 1))
            {
                kasp = 1;
            }
            else if (pa > (min + part * 1) && pa <= (min + part * 2))
            {
                kasp = 2;
            }
            else if (pa > (min + part * 2) && pa <= (min + part * 3))
            {
                kasp = 3;
            }
            else if (pa > (min + part * 3) && pa <= (min + part * 4))
            {
                kasp = 4;
            }
            else if (pa > (min + part * 4) && pa <= (min + part * 5))
            {
                kasp = 5;
            }
            else if (pa > (min + part * 5) && pa <= (min + part * 6))
            {
                kasp = 6;
            }
            else if (pa > (min + part * 6) && pa <= (min + part * 7))
            {
                kasp = 7;
            }
            else if (pa > (min + part * 7) && pa <= (min + part * 8))
            {
                kasp = 8;
            }
            else if (pa > (min + part * 8) && pa <= max)
            {
                kasp = 9;
            }
            SolidColorBrush brush = new SolidColorBrush();
            switch (kasp)
            {
                case 0:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF0000FF");
                    break;
                case 1:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF0099FF");
                    break;
                case 2:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF00FFFF");
                    break;
                case 3:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF00FF00");
                    break;
                case 4:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF99FF00");
                    break;
                case 5:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFCCFF00");
                    break;
                case 6:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF00");
                    break;
                case 7:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFCC00");
                    break;
                case 8:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFF9900");
                    break;
                case 9:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFF0000");
                    break;
            }
            return brush;
        }

        public SolidColorBrush checktemperature(double tem)
        {
            double min = sympton.Legend_Minimum;
            double max = sympton.Legend_Maximum;
            double part = ((max - min) / 9);
            int t = 0;
            if (tem <= min)
            {
                t = 0;
            }
            else if (tem > min && tem <= (min + part))
            {
                t = 1;
            }
            else if (tem > (min + part) && tem <= (min + part * 2))
            {
                t = 2;
            }
            else if (tem > (min + part * 2) && tem <= (min + part * 3))
            {
                t = 3;
            }
            else if (tem > (min + part * 3) && tem <= (min + part * 4))
            {
                t = 4;
            }
            else if (tem > (min + part * 4) && tem <= (min + part * 5))
            {
                t = 5;
            }
            else if (tem > (min + part * 5) && tem <= (min + part * 6))
            {
                t = 6;
            }
            else if (tem > (min + part * 6) && tem <= (min + part * 7))
            {
                t = 7;
            }
            else if (tem > (min + part * 7) && tem <= (min + part * 8))
            {
                t = 8;
            }
            else if (tem > (min + part * 8) && tem <= max)
            {
                t = 9;
            }
            SolidColorBrush brush = new SolidColorBrush();
            switch (t)
            {
                case 0:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF0000FF");
                    break;
                case 1:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF0099FF");
                    break;
                case 2:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF00FFFF");
                    break;
                case 3:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF00FF00");
                    break;
                case 4:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF99FF00");
                    break;
                case 5:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFCCFF00");
                    break;
                case 6:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF00");
                    break;
                case 7:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFCC00");
                    break;
                case 8:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFF9900");
                    break;
                case 9:
                    brush = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFF0000");
                    break;
            }
            return brush;
        }
        #endregion

        #region 保存提示
        private void Mainwindow_Closing(object sender, CancelEventArgs e)
        {
            showcomd = false;
            sqlStringList = new List<string>();
            MessageBoxResult result = MessageBox.Show("还未保存数据，是否保存数据？", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (savepath == null)
                {
                    saveAllfile();
                    closed = true;
                }
                else
                {
                    Savethis_ItemClick(null, null);
                    closed = true;
                }
                if (closed == true)
                {
                    e.Cancel = false;
                    sqlStringList.Add(deletext);
                    dbOpera.Instance.ExcuteQuery(sqlStringList);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (result == MessageBoxResult.No)
            {
                e.Cancel = false;
                sqlStringList.Add(deletext);
                dbOpera.Instance.ExcuteQuery(sqlStringList);
            }
            else
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region 计算结果
        public class SourceComparer : IEqualityComparer<LineColorModel>
        {
            public bool Equals(LineColorModel x, LineColorModel y)
            {
                if (y != null && (x != null && x.Pa == y.Pa))
                    return true;
                else
                    return false;
            }
            public int GetHashCode(LineColorModel ob)
            {
                return 0;
            }
        }
        private void Pressure_Checked(object sender, RoutedEventArgs e)
        {
            prescansal = true;//管线颜色不消失
            try
            {
                sympton.Legend_mode = 1;
                sympton.Visibility = Visibility.Visible;
                sympton.Legend_Maximum = maxpreandtem(sympton.Legend_mode);
                sympton.Legend_Minimum = minpreandtem(sympton.Legend_mode);
                if (itemSourceLists.Count < 10)
                {
                    changcolors(itemSourceLists.Count);
                }
                else
                {
                    itemSourceLists.Sort((a, b) => a.Pa.CompareTo(b.Pa));
                    var pList = new List<LineColorModel>();
                    pList = itemSourceLists.Distinct(new SourceComparer()).ToList();//去重
                    if (pList.Count < 10)
                    {
                        changcolors(itemSourceLists.Count);
                    }
                    else
                    {
                        int num = itemSourceLists.Count / 10;//分级
                        color[0] = Math.Abs(itemSourceLists[num - 1].Pa);
                        color[1] = Math.Abs(itemSourceLists[2 * num - 1].Pa);
                        color[2] = Math.Abs(itemSourceLists[3 * num - 1].Pa);
                        color[3] = Math.Abs(itemSourceLists[4 * num - 1].Pa);
                        color[4] = Math.Abs(itemSourceLists[5 * num - 1].Pa);
                        color[5] = Math.Abs(itemSourceLists[6 * num - 1].Pa);
                        color[6] = Math.Abs(itemSourceLists[7 * num - 1].Pa);
                        color[7] = Math.Abs(itemSourceLists[8 * num - 1].Pa);
                        color[8] = Math.Abs(itemSourceLists[9 * num - 1].Pa);
                        color[9] = Math.Abs(itemSourceLists[10 * num - 1].Pa);
                        Array.Sort(color);
                        sympton.Legend_Step1 = color[0];
                        sympton.Legend_Step2 = color[1];
                        sympton.Legend_Step3 = color[2];
                        sympton.Legend_Step4 = color[3];
                        sympton.Legend_Step5 = color[4];
                        sympton.Legend_Step6 = color[5];
                        sympton.Legend_Step7 = color[6];
                        sympton.Legend_Step8 = color[7];
                        sympton.Legend_Step9 = color[8];
                        sympton.Legend_Step10 = color[9];
                        changcolors1(itemSourceLists.Count);
                    }

                }

            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }

        }

        private void Temperature_Checked(object sender, RoutedEventArgs e)
        {
            prescansal = true;//管线颜色不消失
            try
            {
                sympton.Legend_mode = 2;
                sympton.Visibility = Visibility.Visible;
                sympton.Legend_Maximum = (int)maxpreandtem(sympton.Legend_mode);
                sympton.Legend_Minimum = (int)minpreandtem(sympton.Legend_mode);
                if (sympton.Legend_Minimum == sympton.Legend_Maximum)
                {
                    sympton.Legend_Minimum = 0;
                }
                sympton.Legend_count = 0;
                if (sence.SelectedItem == null)
                {
                    if (itemSourceLists != null && itemSourceLists.Count > 0)
                    {
                        foreach (Line node in linelayer.Children)
                        {

                            DataRow LineTable = dbOpera.Instance.getData("line_attribute_t", "*", "名称 = '" + node.Name + "'").Rows[0];
                            foreach (LineColorModel models in itemSourceLists)
                            {
                                if (Convert.ToInt32(LineTable["编号"]) == models.ID)
                                {
                                    node.Stroke = checktemperature(Convert.ToDouble(models.Temp));
                                    break;
                                }
                            }
                        }
                    }
                    return;
                }
                DataTable csvtable = CsvHelper.CsvToDataTable(((Sence)sence.SelectedItem).Result, 1);
                foreach (Line node in linelayer.Children)
                {

                    DataRow LineTable = dbOpera.Instance.getData("line_attribute_t", "*", "名称 = '" + node.Name + "'").Rows[0];
                    foreach (DataRow row in csvtable.Rows)
                    {
                        if (Convert.ToInt32(LineTable["编号"]) == Convert.ToInt32(row["编号"]))
                        {
                            node.Stroke = checktemperature(Convert.ToDouble(row["口1温度"]));
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private void changcolors(int count)
        {

            sympton.Legend_count = 0;
            if (sence.SelectedItem == null)
            {
                if (itemSourceLists != null && itemSourceLists.Count > 0)
                {
                    foreach (Line node in linelayer.Children)
                    {
                        DataRow LineTable = dbOpera.Instance.getData("line_attribute_t", "*", "名称 = '" + node.Name + "'").Rows[0];
                        foreach (LineColorModel models in itemSourceLists)
                        {
                            if (Convert.ToInt32(LineTable["编号"]) == models.ID)
                            {
                                node.Stroke = checkValueToColorBrush1(Convert.ToDouble(models.Pa));
                                break;
                            }
                        }
                    }
                }
                return;
            }
            DataTable csvtable = CsvHelper.CsvToDataTable(((Sence)sence.SelectedItem).Result, 1);
            foreach (Line node in linelayer.Children)
            {

                DataRow LineTable = dbOpera.Instance.getData("line_attribute_t", "*", "名称 = '" + node.Name + "'").Rows[0];
                foreach (DataRow row in csvtable.Rows)
                {
                    if (Convert.ToInt32(LineTable["编号"]) == Convert.ToInt32(row["编号"]))
                    {
                        double press1 = checkdouble(Convert.ToString(row["口1压力"]));
                        double press2 = checkdouble(Convert.ToString(row["口2压力"]));
                        double press = Math.Max(press1, press2);
                        node.Stroke = checkValueToColorBrush1(press);
                        break;
                    }
                }
            }
        }

        private void changcolors1(int count)
        {
            sympton.Legend_count = count;
            if (sence.SelectedItem == null)
            {
                if (itemSourceLists != null && itemSourceLists.Count > 0)
                {
                    foreach (Line node in linelayer.Children)
                    {
                        DataRow LineTable = dbOpera.Instance.getData("line_attribute_t", "*", "名称 = '" + node.Name + "'").Rows[0];
                        foreach (LineColorModel models in itemSourceLists)
                        {
                            if (Convert.ToInt32(LineTable["编号"]) == models.ID)
                            {
                                node.Stroke = checkValueToColorBrush(Convert.ToDouble(models.Pa));
                                break;
                            }
                        }
                    }
                }
                return;
            }
            DataTable csvtable = CsvHelper.CsvToDataTable(((Sence)sence.SelectedItem).Result, 1);
            foreach (Line node in linelayer.Children)
            {

                DataRow LineTable = dbOpera.Instance.getData("line_attribute_t", "*", "名称 = '" + node.Name + "'").Rows[0];
                foreach (DataRow row in csvtable.Rows)
                {
                    if (Convert.ToInt32(LineTable["编号"]) == Convert.ToInt32(row["编号"]))
                    {
                        double press1 = checkdouble(Convert.ToString(row["口1压力"]));
                        double press2 = checkdouble(Convert.ToString(row["口2压力"]));
                        double press = Math.Max(press1, press2);
                        node.Stroke = checkValueToColorBrush(press);
                        break;
                    }
                }
            }
        }

        private double maxpreandtem(int i)
        {
            double max = 0;
            double pre = 0;
            switch (i)
            {
                case 1:
                    foreach (LineColorModel models in itemSourceLists)
                    {
                        double a = Math.Abs(models.Pa);
                        if (a >= pre)
                        {
                            pre = a;
                            max = a;
                        }
                    }
                    break;
                case 2:
                    foreach (LineColorModel models in itemSourceLists)
                    {
                        //double b = Math.Abs(models.Temp);
                        if (models.Temp >= pre)
                        {
                            pre = models.Temp;
                            max = models.Temp;
                        }
                    }
                    break;
            }

            return max;
        }

        private double minpreandtem(int i)
        {
            double min = 0;
            double pre = 200000000;
            switch (i)
            {
                case 1:
                    foreach (LineColorModel models in itemSourceLists)
                    {
                        double a = Math.Abs(models.Pa);
                        if (a <= pre)
                        {
                            pre = a;
                            min = a;
                        }
                    }
                    break;
                case 2:
                    foreach (LineColorModel models in itemSourceLists)
                    {
                        //double b = Math.Abs(models.Temp);
                        if (models.Temp <= pre)
                        {
                            pre = models.Temp;
                            min = models.Temp;
                        }
                    }
                    break;
            }

            return min;
        }
        #endregion

        #region 新建地图
        private void Newmap_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            Newcreat nec = new Newcreat();
            nec.Owner = this;
            MessageBoxResult result = MessageBox.Show("新建地图前，是否保存当前数据？", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (savepath == null)
                {
                    saveAllfile();
                }
                else
                {
                    Savethis_ItemClick(null, null);
                }
                ClearLayer(null, null);
                sqlStringList.Add(deletext);
                dbOpera.Instance.ExcuteQuery(sqlStringList);
                nec.ShowDialog();
            }
            else if (result == MessageBoxResult.No)
            {
                ClearLayer(null, null);
                sqlStringList.Add(deletext);
                dbOpera.Instance.ExcuteQuery(sqlStringList);
                nec.ShowDialog();
            }
            else { return; }
            savepath = null;
            timename = null;
            sence.ItemsSource = null;
            simuchart.ItemsSource = null;
            calculate.ItemsSource = null;
            sencesList = new ObservableCollection<Sence>();
            paDatasList = new ObservableCollection<ChartData>();
            tempDatasList = new ObservableCollection<ChartData>();
            itemSourceLists = new List<LineColorModel>();
            prescansal = false;
            layer.Children.Clear();
            drawrectangle.Background = null;
            logicrect = null;
            simuchart.Visibility = Visibility.Collapsed;
            Seletedrect.Visibility = Visibility.Collapsed;
            sympton.Visibility = Visibility.Collapsed;
            rec.ForEach(p => selectcav.Children.Remove(p));
            deprop.Visibility = Visibility.Collapsed;
            lineprop.Visibility = Visibility.Collapsed;
            checkpanel.Visibility = Visibility.Collapsed;
            chartpage.Visibility = Visibility.Collapsed;
            linepiplistprop.Visibility = Visibility.Collapsed;
            chartGroup.Visibility = Visibility.Collapsed;
            pressure.IsChecked = false;
            temperature.IsChecked = false;
            pressure.IsEnabled = false;
            temperature.IsEnabled = false;
            linelist.Clear();
            marklist.Clear();
            elemt_Datas = new elemt_data();
        }
        #endregion

        #region 地图控件
        private void Single_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            optState = optState_Type.pointmode;
            command = 0;
            directive = 0;
            showcomd = false;
            mapcontrol.DragButton = MouseButton.Right;
            this.Cursor = Cursors.Arrow;
        }

        private void Zoomin_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            mapcontrol.Zoom += 1;
        }

        private void Zoomout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            mapcontrol.Zoom -= 1;
        }

        private void Move_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            mapcontrol.DragButton = MouseButton.Left;
            this.Cursor = Cursors.Hand;
        }

        private void Cancel_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            optState = optState_Type.pointmode;
            command = 0;
            directive = 0;
            selectall = false;
            Seletedrect.Visibility = Visibility.Collapsed;
            alldrect.Visibility = Visibility.Collapsed;
            deprop.Visibility = Visibility.Collapsed;
            lineprop.Visibility = Visibility.Collapsed;
            linepiplistprop.Visibility = Visibility.Collapsed;
            foreach (MapMark node in singglenode)
            {
                node.alteration.Visibility = Visibility.Collapsed;
            }
            singglenode.Clear();
            foreach (MapMark node in selcetnode)
            {
                if (selcetnode.Count > 0)
                {
                    selectImg(node);
                }
            }
            if (!prescansal)
            {
                foreach (Line node in linelayer.Children)
                {
                    node.Stroke = Brushes.Black;
                }
            }
            selcetnode = new List<MapMark>();
            drawrectangle.Children.Clear();
            drawrectangle.Background = null;
            rect = null;
            logicrect = null;
        }

        private void Showdevice_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            mousecount += 1;
            int a = mousecount % 2;
            if (a == 1)
            {
                showdevice.Content = "设备隐藏";
                marklayer.Visibility = Visibility.Collapsed;
            }
            if (a == 0)
            {
                showdevice.Content = "设备显示";
                marklayer.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region 场景列表
        /// <summary>
        /// 场景列表选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sence senceOnce = sence.SelectedItem as Sence;
            tempSence = senceOnce;
            tempModifyStr = string.Empty;
            pressure.IsChecked = true;
            try
            {
                using (FileStream fs = new FileStream(senceOnce.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryFormatter b = new BinaryFormatter();
                    elemt_Datas = b.Deserialize(fs) as elemt_data;
                    ClearLayer(null, null);
                    RefreshUI();
                    DataTable csvtable = CsvHelper.CsvToDataTable(senceOnce.Result, 1);
                    itemSourceLists.Clear();
                    foreach (DataRow rows in csvtable.Rows)
                    {
                        double press1 = checkdouble(Convert.ToString(rows["口1压力"]));
                        double press2 = checkdouble(Convert.ToString(rows["口2压力"]));
                        double press = Math.Max(press1, press2);
                        itemSourceLists.Add(new LineColorModel { ID = Convert.ToInt32(rows["编号"]), Name = rows["名称"].ToString(), Temp = Convert.ToDouble(rows["口1温度"]), Pa = press });
                    }
                    calculate.ItemsSource = null;
                    calculate.ItemsSource = itemSourceLists;
                    Pressure_Checked(null, null);
                }
            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 选项卡切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ChartGroup_SelectedItemChanged(object sender, DevExpress.Xpf.Docking.Base.SelectedItemChangedEventArgs e)
        {
            TabbedGroup tg = sender as TabbedGroup;
            tabflag = tg.SelectedTabIndex;
            if (tg.SelectedTabIndex == 0) simuchart.ItemsSource = paDatasList;
            else
                simuchart.ItemsSource = tempDatasList;
        }
        #endregion

        #region application按钮
        private void Closed_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Onew_Click(object sender, EventArgs e)
        {
            Newmap_ItemClick(null, null);
        }

        private void Osava_Click(object sender, EventArgs e)
        {
            Savethis_ItemClick(null, null);
        }

        private void Osa_Click(object sender, EventArgs e)
        {
            Savelayer_ItemClick(null, null);
        }

        private void Oopen_Click(object sender, EventArgs e)
        {
            Loadlayer_ItemClick(null, null);
        }
        #endregion

        #region 画图中心点
        private void Home_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //if(marklayer.Children.Count!=0)
            // {
            //     mapcontrol.Zoom = 16;
            //     List<MapMark> mak = new List<MapMark>();
            //     foreach(MapMark node in marklayer.Children)
            //     {
            //         mak.Add(node);
            //     }
            //     int i = (int)(mak.Count / 2);
            //     DataTable dr= mak[i].getposition(mak[i].sign, mak[i].Name);
            //     mapcontrol.setCenter((double)dr.Rows[0]["纬度"], (double)dr.Rows[0]["经度"]);
            // }
            try
            {
                mapcontrol.Zoom = elemt_Datas.myGmap[0].mzoom;
                mapcontrol.Position = elemt_Datas.myGmap[0].center;
            }
            catch
            {
                MessageBox.Show("没有设置中心点");
            }
        }

        private void Sethome_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            myMap map = new myMap();
            map.mzoom = Convert.ToInt32(mapcontrol.Zoom);
            map.center = mapcontrol.Position;
            elemt_Datas.myGmap.Clear();
            elemt_Datas.myGmap.Add(map);
        }

        #endregion

        #region 导入数据
        /// <summary>
        /// 导入csv文件并解析里面的数据，在地图上画线和点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Import_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            elemt_Datas = new elemt_data();
            showcomd = false;
            command = 0;
            optState = optState_Type.pointmode;
            pressure.IsChecked = false;
            temperature.IsChecked = false;
            prescansal = false;
            mapcontrol.Zoom = 18;
            mapcontrol.setCenter(39.8, 116.5);
            itemSourceLists.Clear();
            checkpanel.Visibility = Visibility.Collapsed;
            linepiplistprop.Visibility = Visibility.Collapsed;
            deprop.Visibility = Visibility.Collapsed;
            sqlStringList = new List<string>();
            MessageBoxResult result = MessageBox.Show("导入文件前，是否保存当前数据？", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (savepath == null)
                {
                    saveAllfile();
                }
                else
                {
                    Savethis_ItemClick(null, null);
                }
                ClearLayer(null, null);
                Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
                openFile.Filter = "CSV文件|*.csv";
                openFile.InitialDirectory = filepath;
                openFile.RestoreDirectory = false;
                openFile.Title = "选择需要导入的文件";
                bool? opem = openFile.ShowDialog();
                if (!(bool)opem) return;
                string filename = openFile.FileName;
                impath = filename;
                filepath = filename.Substring(0, filename.LastIndexOf("\\"));//获取文件路径，不带文件名
                sqlStringList.Add(deletext);
                dbOpera.Instance.ExcuteQuery(sqlStringList);
            }
            else if (result == MessageBoxResult.No)
            {
                ClearLayer(null, null);
                //backWorkDel.BeginInvoke(null, null);
                Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
                openFile.Filter = "CSV文件|*.csv";
                openFile.InitialDirectory = filepath;
                openFile.RestoreDirectory = false;
                openFile.Title = "选择需要导入的文件";
                bool? opem = openFile.ShowDialog();
                if (!(bool)opem) return;
                string filename = openFile.FileName;
                impath = filename;
                filepath = filename.Substring(0, filename.LastIndexOf("\\"));//获取文件路径，不带文件名
                sqlStringList.Add(deletext);
                dbOpera.Instance.ExcuteQuery(sqlStringList);
            }
            else
            {
                return;
            }
            elemt_Datas = imprtcsvtomap(impath);
            insertline.BeginInvoke(excelNodes, (IAsyncResult) => { MessageBox.Show("插入成功"); }, null);
            ClearLayer(null, null);
            //PointLatLng p = mapcontrol.FromLocalToLatLng((int)elemt_Datas)
            //mapcontrol.setCenter(p.Lat, p.Lng);
            addequipemt(elemt_Datas);
            addlienpipe(elemt_Datas);
            List<excelInNode> two = new List<excelInNode>();
            two = excelNodes.Where(o => o.count == 2).ToList();

            List<excelInNode> three = new List<excelInNode>();
            three = excelNodes.Where(o => o.count == 3).ToList();
            List<excelInNode> four = new List<excelInNode>();
            four = excelNodes.Where(o => o.count == 4).ToList();
            List<excelInNode> five = new List<excelInNode>();
            five = excelNodes.Where(o => o.count == 5).ToList();
            List<excelInNode> station = new List<excelInNode>();
            station = excelNodes.Where(o => o.count == 1).ToList();
            if (two.Count > 0)
                addequipemt(two);
            if (three.Count > 0)
                addequipemt(three);
            if (four.Count > 0)
                addequipemt(four);
            if (five.Count > 0)
                addequipemt(five);
            if (station.Count > 0)
                addequipemt(station);

        }
        void InserDatas(IList<excelInNode> node)
        {
            sqlStringList = new List<string>();
            // node.GroupBy(o => o.id).ToList().ForEach(p => { if (p.Key > 1) Console.WriteLine(p.Key); });
            foreach (var item in node)
            {

                DoInsertMark(item);
                //if (item.lines.Count > 4)
                //{
                //    Console.WriteLine("------------------");
                //    foreach (var links in item.lines)
                //    {
                //        Console.WriteLine(links.id);
                //    }
                //}
                for (int i = 0; i < item.lines.Count; i++)
                {
                    //FieldInfo info = item.GetType().GetField("equip_" + (i + 1) + "_id");
                    //info.SetValue(item, item.lines[i].id);
                    //FieldInfo info1 = item.GetType().GetField("eqinter_" + (i + 1) + "_id");
                    //info.SetValue(item, i + 1);
                    FieldInfo info2 = item.GetType().GetField("diameter_" + (i + 1));
                    info2.SetValue(item, 0);
                    if (item.lines[i].eqinter_2_id == 1)
                    {
                        //item.lines[i].equip_1_id = item.id;
                        item.lines[i].eqinter_2_id = item.count == 1 ? 0 : item.lines[i].eqinter_2_id;
                    }
                    else
                    {
                        //item.lines[i].equip_2_id = item.id;
                        item.lines[i].eqinter_1_id = item.count == 1 ? 0 : item.lines[i].eqinter_1_id;
                    }
                }
                switch (item.lines.Count)
                {
                    case 1:
                        item.name = "us" + (item.id + 60000);
                        sqlStringList.Add("insert into station_data('编号','名称','端口数','类型','经度','纬度','口1连接元件编号','口1连接元件接口编号','口2连接元件编号','口2连接元件接口编号') " +
                            "values(" + item.id + ",'" + item.name + "',1,3 ," + item.lng + "," + item.lat +
                        "," + item.equip_1_id + "," + "2" + "," + item.equip_2_id + "," + item.eqinter_2_id + ")");
                        break;
                    case 2:
                        item.name = "tw" + (item.id + 20000);
                        sqlStringList.Add("insert into twowayvalve_data('编号','名称','端口数','经度','纬度','口1连接元件编号','口1连接元件接口编号','口2连接元件编号','口2连接元件接口编号') " +
                            "values(" + item.id + ",'" + item.name + "',0," + item.lng + "," + item.lat +
                        "," + item.equip_1_id + "," + item.eqinter_1_id + "," + item.equip_2_id + "," + item.eqinter_2_id + ")");
                        break;
                    case 3:
                        item.name = "th" + (item.id + 30000);
                        sqlStringList.Add("insert into threewayvalve_data('编号','名称','端口数','经度','纬度','口1连接元件编号','口1连接元件接口编号','口2连接元件编号','口2连接元件接口编号','口3连接元件编号','口3连接元件接口编号') " +
                            "values(" + item.id + ",'" + item.name + "',0," + item.lng + "," + item.lat +
                        "," + item.equip_1_id + "," + item.eqinter_1_id + "," + item.equip_2_id + "," + item.eqinter_2_id + "," + item.equip_3_id + "," + item.eqinter_3_id + ")");
                        break;
                    case 4:
                        item.name = "fo" + (item.id + 40000);
                        sqlStringList.Add("insert into fourwayvalve_data('编号','名称','端口数','经度','纬度','口1连接元件编号','口1连接元件接口编号','口2连接元件编号','口2连接元件接口编号','口3连接元件编号','口3连接元件接口编号','口4连接元件编号','口4连接元件接口编号') " +
                            "values(" + item.id + ",'" + item.name + "',0," + item.lng + "," + item.lat +
                        "," + item.equip_1_id + "," + item.eqinter_1_id + "," + item.equip_2_id + "," + item.eqinter_2_id + "," + item.equip_3_id + "," + item.eqinter_3_id + "," + item.equip_4_id + "," + item.eqinter_4_id + ")");
                        break;
                        //case 5:
                        //    item.name = "fi" + (item.id + 50000);
                        //    sqlStringList.Add("insert into fivewayvalve_data('编号','名称','端口数','经度','纬度','口1连接元件编号','口1连接元件接口编号','口2连接元件编号','口2连接元件接口编号','口3连接元件编号','口3连接元件接口编号','口4连接元件编号','口4连接元件接口编号','口5连接元件编号','口5连接元件接口编号') " +
                        //        "values(" + item.id + ",'" + item.name + "',0," + item.lng + "," + item.lat +
                        //    "," + item.equip_1_id + "," + item.eqinter_1_id + "," + item.equip_2_id + "," + item.eqinter_2_id + "," + item.equip_3_id + "," + item.eqinter_3_id + "," + item.equip_4_id + "," + item.eqinter_4_id + "," + item.equip_5_id + "," + item.eqinter_5_id + ")");
                        //    break;

                }

            }
            //node.GroupBy(o => o.id).ToList().ForEach(p => { if (p.Key > 1) Console.WriteLine(p.Key); });
            foreach (var line in elemt_Datas.Lines)
            {
                sqlStringList.Add("insert into line_attribute_t('编号','名称','材质','长度','经度','纬度','口1连接元件编号','口1连接元件接口编号'," +
                    "'口2连接元件编号','口2连接元件接口编号','内径','粗糙度') values(" + (line.id + 10000) + ",'" + line.name + "'," + "'" + line.material + "'," + line.length + "," + line.lng + "," + line.lat +
                    "," + line.equip_1_id + "," + line.eqinter_1_id + "," + line.equip_2_id + "," + line.eqinter_2_id + "," + line.diameter + "," + line.roughness + ")");
            }
            dbOpera.Instance.ExcuteQuery(sqlStringList);
        }
        /// <summary>
        /// 根据文件给设备添加信息
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private elemt_data imprtcsvtomap(string filename)
        {
            elemt_data elemt = new elemt_data();
            DataTable csvtable = CsvHelper.CsvToDataTable(filename, 1);
            if (csvtable.Columns.Contains("TYPE"))
            {
                string symb;
                foreach (DataRow rows in csvtable.Rows)
                {
                    symb = rows["TYPE"].ToString();
                    switch (symb)
                    {
                        case "":
                            break;
                        case "调压站":
                            Point p = mapcontrol.setLatLng1(Convert.ToDouble(rows["pointy"]), Convert.ToDouble(rows["pointx"]));
                            elemt.mapnode.Add(new Mapnode
                            {
                                sign = "调压站",
                                height = 24,
                                imgwidth = 24,
                                imgheight = 24,
                                width = 24,
                                move = p,
                                name = rows["NAME"].ToString(),
                                path = checkpath(symb),
                                Scale = 1
                            });
                            elemt.regul.Add(new regulatestation_data
                            {
                                sign = 3,
                                name = rows["NAME"].ToString(),
                                lat = Convert.ToDouble(rows["pointy"]) + 0.0013103458580,
                                lng = Convert.ToDouble(rows["pointx"]) + 0.005825757980,
                                id = Convert.ToInt32(rows["FID"])
                            });
                            addDeletedInDB<regulatestation_data>("regulatestation_data", elemt.regul);
                            break;
                    }
                }
            }
            else
            {
                List<excelInNode> Nodes = new List<excelInNode>();
                foreach (DataRow rows in csvtable.Rows)
                {
                    double startx = Convert.ToDouble(rows["startx"]) + 0.005825757980;
                    double starty = Convert.ToDouble(rows["starty"]) + 0.0013103458580;
                    double endx = Convert.ToDouble(rows["endx"]) + 0.005825757980;
                    double endy = Convert.ToDouble(rows["endy"]) + 0.0013103458580;
                    int id = Convert.ToInt32(rows["FID"]);
                    Point sp = mapcontrol.setLatLng1(starty, startx);
                    Point ep = mapcontrol.setLatLng1(endy, endx);
                    Point cp = new Point((starty + endy) / 2, (startx + endx) / 2);
                    elemt.myline.Add(new myline
                    {
                        name = "line" + (id + 10000).ToString(),
                        X1 = sp.X,
                        X2 = ep.X,
                        Y1 = sp.Y,
                        Y2 = ep.Y
                    });
                    line_attribute_t line = new line_attribute_t
                    {
                        name = "line" + (id + 10000).ToString(),
                        id = id,
                        length = Convert.ToDouble(rows["LENGTH"]),
                        lat = cp.X,
                        startX = starty,
                        startY = startx,
                        lng = cp.Y,
                        endX = endy,
                        endY = endx,
                        material = rows["MATERIAL"].ToString(),
                        roughness = Convert.ToDouble(rows["THICKNESS"]),
                        diameter = Convert.ToDouble(rows["DIAMETERO"])
                    };

                    getNodeFromNodes(line, starty, startx, Nodes, true);
                    getNodeFromNodes(line, endy, endx, Nodes, false);
                    elemt.Lines.Add(line);
                }
                excelNodes = Nodes;

            }

            return elemt;
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="Data"></param>
        public void addequipemt(elemt_data Data)
        {
            for (int i = 0; i < Data.mapnode.Count; i++)
            {
                MapMark eui = new MapMark();
                eui.Name = Data.mapnode[i].name;
                eui.sign = Data.mapnode[i].sign;
                eui.Width = Data.mapnode[i].width;
                eui.Height = Data.mapnode[i].height;
                eui.node.Width = 0;
                eui.node.Height = 0;
                eui.Movie = Data.mapnode[i].move;
                eui.imgheight = Data.mapnode[i].imgheight;
                eui.imgwidth = Data.mapnode[i].imgwidth;
                eui.Scale = Data.mapnode[i].Scale;
                eui.nodeimage.Source = new BitmapImage(new Uri(Data.mapnode[i].path));
                eui.click_proc = Node_Click;
                eui.GetRefreshpage = repage;
                eui.deletdevice = delete;
                eui.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                if (eui.sign == "scada测点")
                {
                    eui.MouseLeftButtonDown += Scada_ShowDataChart;
                }
                marklayer.Children.Add(eui);
            }
        }

        /// <summary>
        /// 添加缺失的设备显示在图层上
        /// </summary>
        /// <param name="Data"></param>
        private void addequipemt(List<excelInNode> Data)
        {
            for (int i = 0; i < Data.Count; i++)
            {
                MapMark eui = new MapMark();
                eui.Width = eui.Height = 24;
                eui.imgheight = 24;
                eui.imgwidth = 24;
                eui.Scale = 1;

                switch (Data[i].count)
                {

                    case 1:
                        eui.Name = "us" + (Data[i].id + 60000);
                        eui.sign = "终端用户";
                        eui.ultisign = 3;
                        eui.Movie = mapcontrol.setLatLng1(Data[i].lat, Data[i].lng);
                        eui.nodeimage.Source = userImg;
                        eui.click_proc = Node_Click;
                        eui.GetRefreshpage = repage;
                        eui.deletdevice = delete;
                        eui.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                        marklayer.Children.Add(eui);
                        break;
                    case 2:
                        eui.Name = "tw" + (Data[i].id + 20000);
                        eui.sign = "两通";
                        eui.Movie = mapcontrol.setLatLng1(Data[i].lat, Data[i].lng);
                        eui.nodeimage.Source = towiImg;
                        eui.click_proc = Node_Click;
                        eui.GetRefreshpage = repage;
                        eui.deletdevice = delete;
                        eui.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                        marklayer.Children.Add(eui);
                        break;
                    case 3:
                        eui.Name = "th" + (Data[i].id + 30000);
                        eui.sign = "三通";
                        eui.Movie = mapcontrol.setLatLng1(Data[i].lat, Data[i].lng);
                        eui.nodeimage.Source = threeiImg;
                        eui.click_proc = Node_Click;
                        eui.GetRefreshpage = repage;
                        eui.deletdevice = delete;
                        eui.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                        marklayer.Children.Add(eui);
                        break;
                    case 4:
                        eui.Name = "fo" + (Data[i].id + 40000);
                        eui.sign = "四通";
                        eui.Movie = mapcontrol.setLatLng1(Data[i].lat, Data[i].lng);
                        eui.nodeimage.Source = fourImg;
                        eui.click_proc = Node_Click;
                        eui.GetRefreshpage = repage;
                        eui.deletdevice = delete;
                        eui.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                        marklayer.Children.Add(eui);
                        break;
                        //case 5:
                        //    eui.Name = "fi" + (Data[i].id + 50000);
                        //    eui.sign = "五通";
                        //    eui.Movie = mapcontrol.setLatLng1(Data[i].lat, Data[i].lng);
                        //    eui.nodeimage.Source = fiveImg;
                        //    eui.click_proc = Node_Click;
                        //    eui.GetRefreshpage = repage;
                        //    eui.deletdevice = delete;
                        //    eui.MouseLeftButtonDown += Mark_MouseLeftButtonDown;
                        //    marklayer.Children.Add(eui);
                        //    break;
                }
                if (i % 1000 == 0)
                {
                    GC.Collect();
                }

            }
        }

        /// <summary>
        /// 添加管线
        /// </summary>
        /// <param name="Data"></param>
        public Task<int> addlienpipe(elemt_data Data)
        {
            for (int i = 0; i < Data.myline.Count; i++)
            {
                Line wire = new Line();
                wire.Name = Data.myline[i].name;
                wire.X1 = Data.myline[i].X1;
                wire.Y1 = Data.myline[i].Y1;
                wire.X2 = Data.myline[i].X2;
                wire.Y2 = Data.myline[i].Y2;
                wire.StrokeThickness = 6;
                wire.StrokeDashCap = PenLineCap.Round;
                wire.Cursor = System.Windows.Input.Cursors.Hand;
                wire.Stroke = Brushes.Black;
                wire.MouseLeftButtonUp += line_mouseLeftButtonUp;
                wire.MouseEnter += Myline_MouseEnter;
                wire.MouseRightButtonUp += Myline_MouseRightButtonUp;
                linelayer.Children.Add(wire);
            }
            return Task.FromResult<int>(0);
        }

        public class excelInNode : fivewayvalve_data
        {
            public new int count = 1;
            public int flag;
            public IList<line_attribute_t> lines = new List<line_attribute_t>();
        }   //地图的节点对象

        /// <summary>
        /// 比较起点经纬度相同的就确定为连接点
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="excelDatas"></param>
        /// <returns></returns>
        //通过端点位置取出地图上的节点，如果不存在，则新建立一个节点
        void getNodeFromNodes(line_attribute_t line, double lat, double lng, List<excelInNode> excelDatas, bool isFirst)
        {
            excelInNode node = null;
            double checkValue = 0.00000004;

            node = excelDatas.FirstOrDefault(n =>
            {
                return Math.Abs(lat - n.lat) < checkValue && Math.Abs(lng - n.lng) < checkValue;
            });

            if (node == null)  //如果节点不存在列表中则新建一个
            {
                node = new excelInNode();
                //get GUID
                //属性添加
                node.count = 1;
                node.lat = lat;
                node.lng = lng;
                node.id = nodeID++;
                node.equip_1_id = line.id + 10000;
                excelDatas.Add(node);
            }
            else
            {
                node.count++;
            }
            if (isFirst)
            {
                line.equip_1_id = node.id;
                line.eqinter_1_id = node.count;
            }
            else
            {
                line.equip_2_id = node.id;
                line.eqinter_2_id = node.count;
            }
            switch (node.count)
            {
                case 1:
                    node.equip_1_id = line.id + 10000;
                    node.eqinter_1_id = isFirst ? 1 : 2;
                    break;
                case 2:
                    node.equip_2_id = line.id + 10000;
                    node.eqinter_2_id = isFirst ? 1 : 2;
                    break;
                case 3:
                    node.equip_3_id = line.id + 10000;
                    node.eqinter_3_id = isFirst ? 1 : 2;
                    break;
                case 4:
                    node.equip_4_id = line.id + 10000;
                    node.eqinter_4_id = isFirst ? 1 : 2;
                    break;
                    //case 5:
                    //    node.equip_5_id = line.id + 10000;
                    //    node.eqinter_5_id = isFirst ? 1 : 2;
                    //    break;
            }

        }
        /// <summary>
        /// 把缺失设备和管线数据关系保存下来
        /// </summary>
        /// <param name="item"></param>
        public void DoInsertMark(excelInNode item)
        {
            for (int i = 0; i < elemt_Datas.Lines.Count; i++)
            {
                if (elemt_Datas.Lines[i].startX == item.lat && elemt_Datas.Lines[i].startY == item.lng)
                {
                    item.lines.Add(elemt_Datas.Lines[i]);
                    continue;
                }
                if (elemt_Datas.Lines[i].endX == item.lat && elemt_Datas.Lines[i].endY == item.lng)
                {
                    item.lines.Add(elemt_Datas.Lines[i]);
                    continue;
                }
            }
        }
        #endregion

        #region 图片选择
        /// <summary>
        /// 选择普通选中状态图片
        /// </summary>
        /// <param name="node"></param>
        private void selectnormalImg(MapMark node)
        {
            switch (node.sign)
            {
                case "两通":
                    node.nodeimage.Source = towpiImg;
                    break;
                case "三通":
                    node.nodeimage.Source = threpiImg;
                    break;
                case "四通":
                    node.nodeimage.Source = fourpImg;
                    break;
                case "五通":
                    node.nodeimage.Source = fivepImg;
                    break;
                case "供气站":
                    node.nodeimage.Source = gaspImg;
                    break;
                case "调压站":
                    node.nodeimage.Source = relgupImg;
                    break;
                case "阀门":
                    node.nodeimage.Source = valvepImg;
                    break;
                case "终端用户":
                    node.nodeimage.Source = userpImg;
                    break;
                case "scada测点":
                    node.nodeimage.Source = scadapImg;
                    break;
                case "燃气":
                    node.nodeimage.Source = pelpImg;
                    break;
            }
        }
        /// <summary>
        /// 选择图片
        /// </summary>
        /// <param name="node"></param>
        private void selectImg(MapMark node)
        {
            switch (node.sign)
            {
                case "两通":
                    node.nodeimage.Source = towiImg;
                    break;
                case "三通":
                    node.nodeimage.Source = threeiImg;
                    break;
                case "四通":
                    node.nodeimage.Source = fourImg;
                    break;
                case "五通":
                    node.nodeimage.Source = fiveImg;
                    break;
                case "供气站":
                    node.nodeimage.Source = gasImg;
                    break;
                case "调压站":
                    node.nodeimage.Source = relguImg;
                    break;
                case "阀门":
                    node.nodeimage.Source = valveImg;
                    break;
                case "终端用户":
                    node.nodeimage.Source = userImg;
                    break;
                case "scada测点":
                    node.nodeimage.Source = scadaImg;
                    break;
                case "燃气":
                    node.nodeimage.Source = pelImg;
                    break;
            }
        }
        #endregion
    }
}
