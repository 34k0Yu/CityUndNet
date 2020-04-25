using System;
using System.Collections.Generic;
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
using System.Data;

namespace NetControl
{
    /// <summary>
    /// Regulation.xaml 的交互逻辑
    /// </summary>
    public partial class Regulation : Page
    {
        string defaultID;
        string defaultName;
        public Regulation()
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
        public void TextSearch()
        {
            DataTable dt = MapMark.dt;
            string linename = dt.Rows[0]["名称"].ToString();
            inputid.Text = dt.Rows[0]["编号"].ToString();
            inpname.Text = dt.Rows[0]["名称"].ToString();
            defaultID = dt.Rows[0]["编号"].ToString();
            defaultName = dt.Rows[0]["名称"].ToString();
            inptpa.Text = dt.Rows[0]["进口压强"].ToString();
            inptflu.Text = dt.Rows[0]["进口流量"].ToString();
            inpttem.Text = dt.Rows[0]["进口温度"].ToString();
            exportpa.Text = dt.Rows[0]["出口压强"].ToString();
            exportflu.Text = dt.Rows[0]["出口流量"].ToString();
            exporttem.Text = dt.Rows[0]["出口温度"].ToString();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (dbOpera.Instance.getData("regulatestation_data", "*", "名称='" + inpname.Text.Trim() + "' and 名称!='" + defaultName + "'" +
                "or 编号='" + inputid.Text.Trim() + "' and 编号!='" + defaultID + "'").Rows.Count > 0)
            {
                MessageBox.Show("已存在相同名称或id");
                return;
            }
            DataTable dt = MapMark.dt;
            string linename = dt.Rows[0]["名称"].ToString();
            string table = "regulatestation_data";
            string id = "编号=" + inputid.Text;
            string name = "名称='" + inpname.Text + "'";
            string texture = "进口压强=" + inptpa.Text + "";
            string diameter = "进口流量=" + inptflu.Text;
            string width = "进口温度=" + inpttem.Text;
            string coeffi = "出口压强=" + exportpa.Text;
            string fl = "出口流量=" + exportflu.Text;
            string thick = "出口温度=" + exporttem.Text;
            try
            {
                dbOpera.Instance.updata(table, "名称='" + linename + "'", id + "," + name + "," + texture + "," + diameter + "," + width + "," + coeffi + "," + fl + "," + thick);
                MainWindow.lin.name = inpname.Text;
                parentWindow.changmname(inpname.Text);//使线段和数据库里字段相同
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show("请填写完整数据");
            }
        }

        private void Inputid_LostFocus(object sender, RoutedEventArgs e)
        {
            DataTable dt = dbOpera.Instance.getData("regulatestation_data", "*", "编号='" + inputid.Text.Trim() + "' and 编号!='" + defaultID + "'");
            if (dt.Rows.Count > 0)
            {
                validate_ID.Visibility = Visibility.Visible;
            }
            else
            {
                validate_ID.Visibility = Visibility.Collapsed;
            }
        }

        private void Inpname_LostFocus(object sender, RoutedEventArgs e)
        {
            DataTable dt = dbOpera.Instance.getData("regulatestation_data", "*", "名称='" + inpname.Text.Trim() + "' and 名称!='" + defaultName + "'");
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
