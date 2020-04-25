using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using GMap.NET;

namespace NetControl
{
    /// <summary>
    /// Warming.xaml 的交互逻辑
    /// </summary>
    public partial class Warming : Page
    {
        private IList<dynamic> showLists = new List<dynamic>();
        LineProperty LineProperty = new LineProperty();
        Device deviceproperty = new Device();
        ultimate ultimates = new ultimate();
        SelectLinePipProp PipProp = new SelectLinePipProp();
        public ObservableCollection<ShowDatas> itemSourceList { get; set; }
        public Warming()
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
            lackwarming.ItemsSource = itemSourceList;
        }

        private void Lackwarming_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowDatas data = ((DataGrid)sender).SelectedItem as ShowDatas;
            if (data == null) return;
            if (data.Type == "管线")
            {
                //ParentWindow.lineprop.Visibility = Visibility.Visible;
                //ParentWindow.lineframe.Refresh();
                //MainWindow.lineda = dbOpera.Instance.getData("line_attribute_t", "*", "名称='" + data.Name + "'");
                //if (MainWindow.lineda.Rows.Count == 0) return;
                //LineProperty.TextSearch();
                //ParentWindow.lineframe.Content = LineProperty;
            }
            else
            {
                MapMark.dt = PipProp.signcheck(data.Type, data.Id);
                double lat = System.Convert.ToDouble(MapMark.dt.Rows[0]["纬度"]);
                double lng = System.Convert.ToDouble(MapMark.dt.Rows[0]["经度"]);
                parentWindow.mapcontrol.setCenter(lat, lng);
                ParentWindow.deframe.Refresh();
                ParentWindow.deprop.Visibility = Visibility.Visible;
                parentWindow.deframe.Refresh();
                try
                {
                    if (MapMark.dt.Rows[0]["类型"] != null)
                    {
                        ultimates.TextSearch();
                        ParentWindow.deframe.Content = ultimates;
                    }
                }
                catch
                {
                    deviceproperty.TextSearch();
                    ParentWindow.deframe.Content = deviceproperty;
                }
            }
        }
    }
}
