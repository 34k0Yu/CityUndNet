using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetControl
{
    /// <summary>
    /// Property.xaml 的交互逻辑
    /// </summary>
    public partial class Device : Page
    {
        public Device()
        {
            InitializeComponent();
        }
        public static MainWindow parentWindow;
        public static MainWindow ParentWindow
        {
            get { return parentWindow; }
            set { parentWindow = value; }
        }
        MapMark eq = MapMark.mark;
        string damage;
        string defaultID = "";
        string defaultName = "";
        public void TextSearch()
        {
            DataTable dt = MapMark.dt;
            string eqname = dt.Rows[0]["名称"].ToString();
            inpid.Text = dt.Rows[0]["编号"].ToString();
            inpna.Text = dt.Rows[0]["名称"].ToString();
            defaultID = dt.Rows[0]["编号"].ToString();
            defaultName = dt.Rows[0]["名称"].ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            eq.sign = MapMark.mark.sign;
            if (dbOpera.Instance.getData(checktable(MapMark.mark.sign), "*", "名称='" + inpna.Text.Trim() + "' and 名称!='" + defaultName + "'" +
               "or 编号='" + inpid.Text.Trim() + "' and 编号!='" + defaultID + "'").Rows.Count > 0)
            {
                MessageBox.Show("已存在相同名称或id");
                return;
            }
            DataTable dt = MapMark.dt;
            string eqname = dt.Rows[0]["名称"].ToString();
            string id = "编号=" + inpid.Text;
            string name = "名称='" + inpna.Text + "'";
            string table = checktable(eq.sign);
            try
            {
                dbOpera.Instance.updata(table, "名称='" + eqname + "'", id + "," + name + ",设备状态='" + damage + "'");
                string eqid = dt.Rows[0]["编号"].ToString();
                dbOpera.Instance.updata("line_attribute_t", "口1连接元件编号=" + eqid, "口1连接元件编号=" + inpid.Text);
                dbOpera.Instance.updata("line_attribute_t", "口2连接元件编号=" + eqid, "口2连接元件编号=" + inpid.Text);
                parentWindow.changmname(inpna.Text);//使设备和数据库里字段相同
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show("请添加编号");
            }
            
        }

        public static string checktable(string sign)
        {
            string tablename = "";
            switch(sign)
            {
                case "两通":
                    tablename = "twowayvalve_data";
                    break;
                case "三通":
                    tablename = "threewayvalve_data";
                    break;
                case "四通":
                    tablename = "fourwayvalve_data";
                    break;
                case "五通":
                    tablename = "fivewayvalve_data";
                    break;
                case "供气站":
                    tablename = "station_data";
                    break;
                case "调压站":
                    tablename = "regulatestation_data";
                    break;
                case "阀门":
                    tablename = "twowayvalve_data";
                    break;
                case "终端用户":
                    tablename = "station_data";
                    break;
                case "scada测点":
                    tablename = "station_data";
                    break;
                case "燃气":
                    tablename = "firegas_data";
                    break;
            }
            return tablename;
        }

        private void Normal_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton s = (RadioButton)sender;
            string bre = s.Content.ToString();
            damage = bre;
        }

        private void Break_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton s = (RadioButton)sender;
            string bre = s.Content.ToString();
            damage = bre;
        }

        private void Inpid_LostFocus(object sender, RoutedEventArgs e)
        {
            DataTable dt = dbOpera.Instance.getData(checktable(MapMark.mark.sign), "*", "编号='" + inpid.Text.Trim() + "' and 编号!='" + defaultID + "'");
            if (dt.Rows.Count > 0)
            {
                validate_ID.Visibility = Visibility.Visible;
            }
            else
            {
                validate_ID.Visibility = Visibility.Collapsed;
            }
        }

        private void Inpna_LostFocus(object sender, RoutedEventArgs e)
        {
            DataTable dt = dbOpera.Instance.getData(checktable(MapMark.mark.sign), "*", "名称='" + inpna.Text.Trim() + "' and 名称!='" + defaultName + "'");
            if (dt.Rows.Count > 0)
            {
                validate_Name.Visibility = Visibility.Visible;
            }
            else
            {
                validate_Name.Visibility = Visibility.Collapsed;
            }
        }
    }
}