using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NetControl
{
    /// <summary>
    /// ultimate.xaml 的交互逻辑
    /// </summary>
    public partial class ultimate : Page
    {
        public ultimate()
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
        string defaultID = "";
        string defaultName = "";
        public void TextSearch()
        {
            DataTable dt = MapMark.dt;
            defaultID = dt.Rows[0]["编号"].ToString();
            defaultName = dt.Rows[0]["名称"].ToString();
            string eqname = dt.Rows[0]["名称"].ToString();
            inpid.Text = dt.Rows[0]["编号"].ToString();
            inpna.Text = dt.Rows[0]["名称"].ToString();
            inppa.Text = dt.Rows[0]["压力"].ToString();
            inpsped.Text = dt.Rows[0]["速度"].ToString();
            inphet.Text = dt.Rows[0]["温度"].ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            eq.sign = MapMark.mark.sign;
            if (dbOpera.Instance.getData("station_data", "*", "名称='" + inpna.Text.Trim() + "' and 名称!='" + defaultName + "'" +
               "or 编号='" + inpid.Text.Trim() + "' and 编号!='" + defaultID + "'").Rows.Count > 0)
            {
                MessageBox.Show("已存在相同名称或id");
                return;
            }
            DataTable dt = MapMark.dt;
            string eqname = dt.Rows[0]["名称"].ToString();
            string id = "编号=" + inpid.Text;
            string name = "名称='" + inpna.Text + "'";
            string Pa = "压力=" + inppa.Text + "";
            string heat = "温度=" + inphet.Text + "";
            string sped = "速度=" + inpsped.Text + "";
            try
            {
                dbOpera.Instance.updata("station_data", "名称='" + eqname + "'", id + "," + name + ","+ Pa + "," + heat + "," + sped);
                string eqid = dt.Rows[0]["编号"].ToString();
                dbOpera.Instance.updata("line_attribute_t", "口1连接元件编号=" + eqid, "口1连接元件编号=" + inpid.Text);
                dbOpera.Instance.updata("line_attribute_t", "口2连接元件编号=" + eqid, "口2连接元件编号=" + inpid.Text);
                parentWindow.changmname(inpna.Text);//使设备和数据库里字段相同
                if(eq.sign== "scada测点")
                {
                    parentWindow.addchartdata(MapMark.mark);
                }
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show("请添加编号");
            }

        }

        private void Inpid_LostFocus(object sender, RoutedEventArgs e)
        {
            DataTable dt = dbOpera.Instance.getData("station_data", "*", "编号='" + inpid.Text.Trim() + "' and 编号!='" + defaultID + "'");
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
            DataTable dt = dbOpera.Instance.getData("station_data", "*", "名称='" + inpna.Text.Trim() + "' and 名称!='" + defaultName + "'");
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
