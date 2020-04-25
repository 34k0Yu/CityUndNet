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
    public class table
    {
        public string Name { get; set; }
        public int Tag { get; set; }
    }
    public class center
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
    /// <summary>
    /// Newcreat.xaml 的交互逻辑
    /// </summary>
    public partial class Newcreat : Window
    {
        
        int id=6;
        double lat= 39.8;
        double lng = 116.5;
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
            List<center> ce = new List<center>();
            //ta.Add(new table { Name = "形状图层", Tag = 1 });
            //ta.Add(new table { Name = "行政图层", Tag = 3 });
            //ta.Add(new table { Name = "卫星图层", Tag = 4 });
            ta.Add(new table { Name = "空白图层", Tag = 5 });
            ta.Add(new table { Name = "普通图层", Tag = 6 });
            ce.Add(new center { Name = "北京", Id = 1 });
            ce.Add(new center { Name = "上海", Id = 2 });
            selectlayer.ItemsSource = ta;
            selectcent.ItemsSource = ce;
        }

        private void Selectlayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            id = Convert.ToInt32(selectlayer.SelectedValue);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            //double lat = Convert.ToDouble(inplat.Text);
            //double lng = Convert.ToDouble(inplng.Text);
            parentWindow.mapcontrol.changeLayer(id);
            parentWindow.mapcontrol.mapCenter = new GMap.NET.PointLatLng(lat, lng);
            parentWindow.mapcontrol.setCenter(lat, lng);
            parentWindow.mapcontrol.Zoom = 18;
            if(Convert.ToInt32(selectlayer.SelectedValue) == 5)
            {
                parentWindow.changelayer.EditValue = "空白图层";
            }
            else
            {
                parentWindow.changelayer.EditValue = "普通图层";
            }
            this.Close();
        }

        private void Selectcent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(Convert.ToInt32(selectcent.SelectedValue))
            {
                case 1:
                    lat = 39.8;
                    lng = 116.5;
                    break;
                case 2:
                    lat = 31.175209828310848;
                    lng = 121.46209716796875;
                    break;
            }
        }
    }
}
