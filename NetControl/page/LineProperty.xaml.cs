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
using System.Windows.Shapes;
using System.Data;

namespace NetControl
{
    /// <summary>
    /// LineProperty.xaml 的交互逻辑
    /// </summary>
    public partial class LineProperty : Page
    {
        string damage;
        string defaultID;
        string defaultName;
        public LineProperty()
        {
            InitializeComponent();
        }
        
        public static MainWindow parentWindow;
        public static MainWindow ParentWindow
        {
            get { return parentWindow; }
            set { parentWindow = value; }
        }
        public void TextSearch()
        {
            DataTable dt = MainWindow.lineda;
            string type = dt.Rows[0]["材质"].ToString();
            string linename = dt.Rows[0]["名称"].ToString();
            inputid.Text = dt.Rows[0]["编号"].ToString();
            inpname.Text = dt.Rows[0]["名称"].ToString();
            defaultID = dt.Rows[0]["编号"].ToString();
            defaultName = dt.Rows[0]["名称"].ToString();
            inptex.Text = dt.Rows[0]["粗糙度"].ToString();
            inpdiame.Text = dt.Rows[0]["内径"].ToString();
            inpwidth.Text = dt.Rows[0]["长度"].ToString();
            coefficient.Text = dt.Rows[0]["平均摩擦系数"].ToString();
            flow.Text = dt.Rows[0]["质量流量"].ToString();
            thickness.Text = dt.Rows[0]["壁厚"].ToString();
            frictional.Text = dt.Rows[0]["比摩阻"].ToString();
            compress.Text = dt.Rows[0]["压缩因子"].ToString();
            pa.Text = dt.Rows[0]["压降Pa"].ToString();
            switch(dt.Rows[0]["材质"].ToString())
            {
                case "钢管":
                    material.SelectedValue = 1;
                    break;
                case "铸铁管":
                    material.SelectedValue = 2;
                    break;
                case "塑料管":
                    material.SelectedValue = 3;
                    break;
                case "复合管":
                    material.SelectedValue = 4;
                    break;
                case "有色金属管":
                    material.SelectedValue = 5;
                    break;

            }
            
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (dbOpera.Instance.getData("line_attribute_t", "*", "名称='" + inpname.Text.Trim() + "' and 名称!='" + defaultName + "'" +
                "or 编号='" + inputid.Text.Trim() + "' and 编号!='" + defaultID + "'").Rows.Count > 0)
            {
                MessageBox.Show("已存在相同名称或id");
                return;
            }
            DataTable dt = MainWindow.lineda;
            string linename = dt.Rows[0]["名称"].ToString();
            string table = "line_attribute_t";
            string id = "编号=" + inputid.Text;
            string name = "名称='" + inpname.Text + "'";
            string texture = "粗糙度=" + inptex.Text + "";
            string diameter = "内径=" + inpdiame.Text;
            string width = "长度=" + inpwidth.Text;
            string coeffi = "平均摩擦系数=" + coefficient.Text;
            string fl = "质量流量=" + flow.Text;
            string thick = "壁厚=" + thickness.Text;
            string fri = "比摩阻=" + frictional.Text;
            string comp = "压缩因子=" + compress.Text;
            string p = "压降Pa=" + pa.Text;
            string mat = "材质='" + material.Text;
            try
            {
               dbOpera.Instance.updata(table, "名称='" + linename + "'", id + "," + name + "," + texture + "," + diameter + "," + width + "," + coeffi + "," + fl + "," + thick + ","
                + fri + "," + comp + "," + p + "," + mat + "'");
               MainWindow.lin.name = inpname.Text;
               parentWindow.changlname(inpname.Text);//使线段和数据库里字段相同
               MessageBox.Show("保存成功"); 
            }
            catch
            {
                MessageBox.Show("请填写完整数据");
            }
        }

        private void Normol_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton s = (RadioButton)sender;
            string no = s.Content.ToString();
            damage = no;
        }

        private void Break_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton s = (RadioButton)sender;
            string br = s.Content.ToString();
            damage = br;
        }

        private void Inputid_LostFocus(object sender, RoutedEventArgs e)
        {
            DataTable dt = dbOpera.Instance.getData("line_attribute_t", "*", "编号='" + inputid.Text.Trim() + "' and 编号!='" + defaultID + "'");
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
            DataTable dt = dbOpera.Instance.getData("line_attribute_t", "*", "名称='" + inpname.Text.Trim() + "' and 名称!='" + defaultName + "'");
            if (dt.Rows.Count > 0)
            {
                validate_Name.Visibility = Visibility.Visible;
            }
            else
            {
                validate_Name.Visibility = Visibility.Collapsed;
            }
        }

        public class table
        {
            public string Name { get; set; }
            public int Tag { get; set; }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<table> ta = new List<table>();
            ta.Add(new table { Name = "钢管", Tag = 1 });
            ta.Add(new table { Name = "铸铁管", Tag = 2 });
            ta.Add(new table { Name = "塑料管", Tag = 3 });
            ta.Add(new table { Name = "复合管", Tag = 4 });
            ta.Add(new table { Name = "有色金属管", Tag = 5 });
            material.ItemsSource = ta;
        }

        private void Material_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string te = material.SelectedValue.ToString();
                switch (te)
                {
                    case "1":
                        coefficient.Text = "0.16";
                        break;
                    case "2":
                        coefficient.Text = "0.22";
                        break;
                    case "3":
                        coefficient.Text = "0.39";
                        break;
                    case "4":
                        coefficient.Text = "0.26";
                        break;
                    case "5":
                        coefficient.Text = "0.24";
                        break;
                }
            }
            catch { }
        }
    }
}
