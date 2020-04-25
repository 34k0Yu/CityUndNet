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
using System.Text.RegularExpressions;

namespace NetControl
{
    /// <summary>
    /// Newcreat.xaml 的交互逻辑
    /// </summary>
    public partial class Newcreat : Window
    {
        public class table
        {
            public string Name { get; set; }
            public int Tag { get; set; }
        }
        int id=6;
        public Newcreat()
        {
            InitializeComponent();
        }
        public static MainWindow parentWindow;
        public static MainWindow ParentWindow
        {
            get { return parentWindow; }
            set { parentWindow = value; }
        }
        private void Inplat_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<table> ta = new List<table>();
            ta.Add(new table { Name = "形状图层", Tag = 1 });
            ta.Add(new table { Name = "行政图层", Tag = 3 });
            ta.Add(new table { Name = "卫星图层", Tag = 4 });
            ta.Add(new table { Name = "空白图层", Tag = 5 });
            ta.Add(new table { Name = "普通图层", Tag = 6 });
            selectlayer.ItemsSource = ta;
        }

        private void Selectlayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            id = Convert.ToInt32(selectlayer.SelectedValue);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            double lat = Convert.ToDouble(inplat.Text);
            double lng = Convert.ToDouble(inplng.Text);
            parentWindow.mapcontrol.changeLayer(id);
            parentWindow.mapcontrol.setCenter(lat, lng);
            this.Close();
        }
    }
}
