using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetControl
{
    /// <summary>
    /// MapMark.xaml 的交互逻辑
    /// </summary>
    public partial class MapMark : UserControl
    {
        public MapMark()
        {
            InitializeComponent();
        }
        public double x;
        public double y;
        public bool refresh = false;
        public static int couva;
        public static string table;
        public Border b = new Border();
        public static DataTable dt = new DataTable();
        public static MapMark mark = new MapMark();
        List<MapMark> marks = new List<MapMark>();
        public List<string> chil = new List<string>();

        [Description("宽"), Category("控件属性")]
        public double imgwidth
        {
            get { return node.Width; }
            set { node.Width = value; }
        }

        [Description("高"), Category("控件属性")]
        public double imgheight
        {
            get { return node.Height; }
            set { node.Height = value; }
        }

        [Description("平移"), Category("控件属性")]
        public Point Movie
        {
            get
            {
                TransformGroup tg = node.RenderTransform as TransformGroup;
                TranslateTransform tt = tg.Children[0] as TranslateTransform;
                return new Point(tt.X, tt.Y);
            }
            set
            {
                TransformGroup tg = node.RenderTransform as TransformGroup;
                TranslateTransform tt = tg.Children[0] as TranslateTransform;
                x = tt.X = value.X - node.Width / 2;
                y = tt.Y = value.Y - node.Height / 2;
            }
        }

        [Description("设备类型"), Category("控件属性")]
        public string sign
        {
            get { return (string)GetValue(signProperty); }
            set { SetValue(signProperty, value); }
        }
        public static readonly DependencyProperty signProperty =
            DependencyProperty.Register("sign", typeof(string), typeof(MapMark));

        [Description("阀门状态"), Category("控件属性")]
        public string damage
        {
            get { return (string)GetValue(damageProperty); }
            set { SetValue(damageProperty, value); }
        }
        public static readonly DependencyProperty damageProperty =
            DependencyProperty.Register("damage", typeof(string), typeof(MapMark));

        [Description("终端类型"), Category("控件属性")]
        public int ultisign
        {
            get { return (int)GetValue(ultisignProperty); }
            set { SetValue(ultisignProperty, value); }
        }
        public static readonly DependencyProperty ultisignProperty =
            DependencyProperty.Register("ultisign", typeof(int), typeof(MapMark));

        [Description("缩放设置"), Category("控件属性")]
        public double Scale
        {
            get
            {
                TransformGroup tg = node.RenderTransform as TransformGroup;
                ScaleTransform st = tg.Children[2] as ScaleTransform;
                return st.ScaleX;
            }
            set
            {
                TransformGroup tg = node.RenderTransform as TransformGroup;
                ScaleTransform st = tg.Children[2] as ScaleTransform;
                st.CenterX = x;
                st.CenterY = y;
                st.ScaleX = st.ScaleY = value;
            }
        }

        [Description("选中状态"), Category("控件属性")]
        public bool Alter
        {
            get {
                //if (AlterProperty == null)
                //    return false;
                //else
                    return (bool)GetValue(AlterProperty); }
            set { SetValue(AlterProperty, value); }
        }
        public static readonly DependencyProperty AlterProperty =
            DependencyProperty.Register("Alter", typeof(bool), typeof(MapMark), new PropertyMetadata(false));


        public delegate void RecProcDelegate(Point position);
        private static void default_click_proc(Point position) { }
        public RecProcDelegate click_proc = new RecProcDelegate(default_click_proc);

        //刷新device页面
        public delegate void refreshpage();
        private static void repage() { }
        public refreshpage GetRefreshpage = new refreshpage(repage);

        //删除设备
        public delegate void deletequip();
        private static void delete() { }
        public deletequip deletdevice = new deletequip(delete);

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MapMark m = (MapMark)sender;
            mark = m;
            
            if(MainWindow.optState == MainWindow.optState_Type.pointmode)
            {
                MainWindow.command = 0;
                dt = signcheck(mark.sign);
                GetRefreshpage();
            }
            else if (MainWindow.optState == MainWindow.optState_Type.linemode)
            {
                if(MainWindow.tablename.Count>=2)//保证下面列表中只有2个数据
                {
                    MainWindow.tablename = new List<string>();
                    MainWindow.markname = new List<string>();
                    MainWindow.marksign = new List<string>();
                }
                table = Device.checktable(mark.sign);
                MainWindow.tablename.Add(table);
                MainWindow.markname.Add(mark.Name);
                MainWindow.marksign.Add(mark.sign);
                dt = signcheck(mark.sign);
                couva = System.Convert.ToInt16(dt.Rows[0]["端口数"]);
                if (couva <= 0)
                {
                    MessageBox.Show("端口已被占用");
                    MainWindow.command = 0;
                    MainWindow.optState = MainWindow.optState_Type.pointmode;
                    MainWindow.pline = new List<Point>();
                }
                click_proc(new Point(x + node.Width / 2, y + node.Height / 2));

            }
        }

        #region 检查类型
        public DataTable signcheck(string sign)
        {
            DataTable table = new DataTable();
            switch (sign)
            {
                case "两通":
                    table = dbOpera.Instance.getData("twowayvalve_data", "*", "名称='" + mark.Name + "'");
                    break;
                case "三通":
                    table = dbOpera.Instance.getData("threewayvalve_data", "*", "名称='" + mark.Name + "'");
                    break;
                case "四通":
                    table = dbOpera.Instance.getData("fourwayvalve_data", "*", "名称='" + mark.Name + "'");
                    break;
                case "五通":
                    table = dbOpera.Instance.getData("fivewayvalve_data", "*", "名称='" + mark.Name + "'");
                    break;
                case "供气站":
                    table = dbOpera.Instance.getData("station_data", "*", "名称='" + mark.Name + "'");
                    break;
                case "调压站":
                    table = dbOpera.Instance.getData("regulatestation_data", "*", "名称='" + mark.Name + "'");
                    break;
                case "阀门":
                    table = dbOpera.Instance.getData("twowayvalve_data", "*", "名称='" + mark.Name + "'");
                    break;
                case "终端用户":
                    table = dbOpera.Instance.getData("station_data", "*", "名称='" + mark.Name + "'");
                    break;
                case "scada测点":
                    table = dbOpera.Instance.getData("station_data", "*", "名称='" + mark.Name + "'");
                    break;
                case "燃气":
                    table = dbOpera.Instance.getData("firegas_data", "*", "名称='" + mark.Name + "'");
                    break;
            }
            return table;
        }

        public DataTable signcheck(string sign,string name)
        {
            DataTable table = new DataTable();
            switch (sign)
            {
                case "两通":
                    table = dbOpera.Instance.getData("twowayvalve_data", "*", "名称='" + name + "'");
                    break;
                case "三通":
                    table = dbOpera.Instance.getData("threewayvalve_data", "*", "名称='" + name + "'");
                    break;
                case "四通":
                    table = dbOpera.Instance.getData("fourwayvalve_data", "*", "名称='" + name + "'");
                    break;
                case "五通":
                    table = dbOpera.Instance.getData("fivewayvalve_data", "*", "名称='" + name + "'");
                    break;
                case "供气站":
                    table = dbOpera.Instance.getData("station_data", "*", "名称='" + name + "'");
                    break;
                case "调压站":
                    table = dbOpera.Instance.getData("regulatestation_data", "*", "名称='" + name + "'");
                    break;
                case "阀门":
                    table = dbOpera.Instance.getData("twowayvalve_data", "*", "名称='" + name + "'");
                    break;
                case "终端用户":
                    table = dbOpera.Instance.getData("station_data", "*", "名称='" + name + "'");
                    break;
                case "scada测点":
                    table = dbOpera.Instance.getData("station_data", "*", "名称='" + name + "'");
                    break;
                case "燃气":
                    table = dbOpera.Instance.getData("firegas_data", "*", "名称='" + name + "'");
                    break;
            }
            return table;
        }

        public DataTable getposition(string sign, string name)
        {
            DataTable table = new DataTable();
            switch (sign)
            {
                case "两通":
                    table = dbOpera.Instance.getData("twowayvalve_data", "纬度,经度", "名称='" + name + "'");
                    break;
                case "三通":
                    table = dbOpera.Instance.getData("threewayvalve_data", "纬度,经度", "名称='" + name + "'");
                    break;
                case "四通":
                    table = dbOpera.Instance.getData("fourwayvalve_data", "纬度,经度", "名称='" + name + "'");
                    break;
                case "五通":
                    table = dbOpera.Instance.getData("fivewayvalve_data", "纬度,经度", "名称='" + name + "'");
                    break;
                case "供气站":
                    table = dbOpera.Instance.getData("station_data", "纬度,经度", "名称='" + name + "'");
                    break;
                case "调压站":
                    table = dbOpera.Instance.getData("regulatestation_data", "纬度,经度", "名称='" + name + "'");
                    break;
                case "阀门":
                    table = dbOpera.Instance.getData("twowayvalve_data", "纬度,经度", "名称='" + name + "'");
                    break;
                case "终端用户":
                    table = dbOpera.Instance.getData("station_data", "纬度,经度", "名称='" + name + "'");
                    break;
                case "scada测点":
                    table = dbOpera.Instance.getData("station_data", "纬度,经度", "名称='" + name + "'");
                    break;
                case "燃气":
                    table = dbOpera.Instance.getData("firegas_data", "纬度,经度", "名称='" + name + "'");
                    break;
            }
            return table;
        }
        #endregion

        private void UserControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MapMark mark = (MapMark)sender;
            marks.Add(mark);
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = new MenuItem();
            menuItem.Header = "删除设备";
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Add(menuItem);
            mark.ContextMenu = contextMenu;
            MainWindow.mapMark = mark;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            deletdevice();
        }
    }

    public class selectmarkshow : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool select = (bool)value;
            if (select == true)
            {
                return Visibility.Visible;
            }
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
