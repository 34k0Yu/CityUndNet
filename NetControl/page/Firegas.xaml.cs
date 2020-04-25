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
    /// Firegas.xaml 的交互逻辑
    /// </summary>
    public partial class Firegas : Page
    {
        string defaultID;
        string defaultName;
        public Firegas()
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
            methane.Text = dt.Rows[0]["甲烷"].ToString();
            ethane.Text = dt.Rows[0]["乙烷"].ToString();
            propane.Text = dt.Rows[0]["丙烷"].ToString();
            nitrogen.Text = dt.Rows[0]["氮气"].ToString();
            hydrogen.Text = dt.Rows[0]["氢气"].ToString();
            carbon.Text = dt.Rows[0]["二氧化碳"].ToString();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (dbOpera.Instance.getData("firegas_data", "*", "名称='" + inpname.Text.Trim() + "' and 名称!='" + defaultName + "'" +
                "or 编号='" + inputid.Text.Trim() + "' and 编号!='" + defaultID + "'").Rows.Count > 0)
            {
                MessageBox.Show("已存在相同名称或id");
                return;
            }
            DataTable dt = MapMark.dt;
            string linename = dt.Rows[0]["名称"].ToString();
            string table = "firegas_data";
            string id = "编号=" + inputid.Text;
            string name = "名称='" + inpname.Text + "'";
            string texture = "甲烷=" + methane.Text + "";
            string diameter = "乙烷=" + ethane.Text;
            string width = "丙烷=" + propane.Text;
            string coeffi = "氮气=" + nitrogen.Text;
            string fl = "氢气=" + hydrogen.Text;
            string thick = "二氧化碳=" + carbon.Text;
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
            DataTable dt = dbOpera.Instance.getData("firegas_data", "*", "编号='" + inputid.Text.Trim() + "' and 编号!='" + defaultID + "'");
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
            DataTable dt = dbOpera.Instance.getData("firegas_data", "*", "名称='" + inpname.Text.Trim() + "' and 名称!='" + defaultName + "'");
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
