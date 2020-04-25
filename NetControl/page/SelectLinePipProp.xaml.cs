using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// SelectLinePipProp.xaml 的交互逻辑
    /// </summary>
    public partial class SelectLinePipProp : Page
    {
        private IList<dynamic> showLists = new List<dynamic>();
        LineProperty LineProperty = new LineProperty();
        Device deviceproperty = new Device();
        public ObservableCollection<ShowDatas> itemSourceList { get; set; }
        public SelectLinePipProp()
        {
            InitializeComponent();
        }
        public static MainWindow parentWindow;
        public static MainWindow ParentWindow
        {
            get { return parentWindow; }
            set { parentWindow = value; }
        }
        public void init()
        {
            rectinsidemarklists.ItemsSource = itemSourceList;
        }
        private void Rectinsidemarklists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rectinsidemarklists.IsReadOnly = true;
            ShowDatas data= ((DataGrid)sender).SelectedItem as ShowDatas;
            if (data == null) return;
            if (data.Type == "管线")
            {
                ParentWindow.lineprop.Visibility = Visibility.Visible;
                ParentWindow.lineframe.Refresh();
                MainWindow.lineda = dbOpera.Instance.getData("line_attribute_t", "*", "名称='" + data.Name + "'");
                if (MainWindow.lineda.Rows.Count == 0) return;
                LineProperty.TextSearch();
                ParentWindow.lineframe.Content = LineProperty;
            }
            else
            {
                MapMark.dt = signcheck(data.Type, data.Id);
                ParentWindow.deframe.Refresh();
                ParentWindow.deprop.Visibility = Visibility.Visible;
                deviceproperty.TextSearch();
                ParentWindow.deframe.Content = deviceproperty;
            }
        }

    public DataTable signcheck(string sign, int id)
    {
        DataTable table = new DataTable();
        switch (sign)
        {
            case "两通":
                table = dbOpera.Instance.getData("twowayvalve_data", "*", "编号='" + id + "'");
                break;
            case "三通":
                table = dbOpera.Instance.getData("threewayvalve_data", "*", "编号='" + id + "'");
                break;
            case "四通":
                table = dbOpera.Instance.getData("fourwayvalve_data", "*", "编号='" + id + "'");
                break;
            case "五通":
                table = dbOpera.Instance.getData("fivewayvalve_data", "*", "编号='" + id + "'");
                break;
            case "供气站":
                table = dbOpera.Instance.getData("station_data", "*", "编号='" + id + "'");
                break;
            case "调压站":
                table = dbOpera.Instance.getData("regulatestation_data", "*", "编号='" + id + "'");
                break;
            case "阀门":
                table = dbOpera.Instance.getData("twowayvalve_data", "*", "编号='" + id + "'");
                break;
            case "终端用户":
                table = dbOpera.Instance.getData("station_data", "*", "编号='" + id + "'");
                break;
            case "scada测点":
                table = dbOpera.Instance.getData("station_data", "*", "编号='" + id + "'");
                break;
            case "燃气":
                table = dbOpera.Instance.getData("firegas_data", "*", "编号='" + id + "'");
                break;
        }
        return table;
    }
}

public class ShowDatas
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private string type;
        public string Type
        {
            get { return this.type; }
            set
            {
                if (value == null) return;
                this.type = value;
                if (type == "管线")
                    this.DataImg = "pack://application:,,,/image/line.png";
                else
                    this.DataImg = SelectLinePipProp.ParentWindow.checkpath(value);
            }
        }
        public string DataImg { get; set; }
    }
    public enum KaspEnum
    {
        MinLow=-1,
        MediumLow=-2,
        MaxLow=-3,
        Medium=0,
        MinHeight=1,
        MediumHeight=2,
        MaxHeight=3
    }
}
