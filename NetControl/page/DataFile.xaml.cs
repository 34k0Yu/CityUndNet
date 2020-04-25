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
    /// DataFile.xaml 的交互逻辑
    /// </summary>
    public partial class DataFile : Window
    {
        string filepath = "C:\\";
        public DataFile()
        {
            InitializeComponent();
        }
        int tag;
        DataTable dt = new DataTable();
        public class table
        {
            public string Name { get; set; }
            public int Tag { get; set; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<table> ta = new List<table>();
            ta.Add(new table { Name = "管线表", Tag = 1 });
            ta.Add(new table { Name = "两通设备表", Tag = 2 });
            ta.Add(new table { Name = "三通设备表", Tag = 3 });
            ta.Add(new table { Name = "四通设备表", Tag = 4 });
            ta.Add(new table { Name = "五通设备表", Tag = 5 });
            ta.Add(new table { Name = "汇源表", Tag = 6 });
            tables.ItemsSource = ta;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder str = new StringBuilder();
            try
            {
                switch (tag)
                {
                    case 1:
                        foreach (Line mline in MainWindow.linelist)
                        {
                            str.Append("'" + mline.Name + "',");
                        }
                        string resultStrs = str.ToString();
                        resultStrs = resultStrs.Substring(0, resultStrs.Length - 1);
                        dt = dbOpera.Instance.getData("view_lineView", "*", "名称 in (" + resultStrs + ")");
                        break;
                    case 2:
                        foreach (MapMark node in MainWindow.marklist)
                        {
                            str.Append("'" + node.Name + "',");
                        }
                        string two = str.ToString();
                        two = two.Substring(0, two.Length - 1);
                        dt = dbOpera.Instance.getData("view_two", "*", "名称 in (" + two + ")");
                        break;
                    case 3:
                        foreach (MapMark node in MainWindow.marklist)
                        {
                            str.Append("'" + node.Name + "',");
                        }
                        string three = str.ToString();
                        three = three.Substring(0, three.Length - 1);
                        dt = dbOpera.Instance.getData("view_three", "*", "名称 in (" + three + ")");
                        break;
                    case 4:
                        foreach (MapMark node in MainWindow.marklist)
                        {
                            str.Append("'" + node.Name + "',");
                        }
                        string four = str.ToString();
                        four = four.Substring(0, four.Length - 1);
                        dt = dbOpera.Instance.getData("four", "*", "名称 in (" + four + ")");
                        break;
                    case 5:
                        foreach (MapMark node in MainWindow.marklist)
                        {
                            str.Append("'" + node.Name + "',");
                        }
                        string five = str.ToString();
                        five = five.Substring(0, five.Length - 1);
                        dt = dbOpera.Instance.getData("five", "*", "名称 in (" + five + ")");
                        break;
                    case 6:
                        foreach (MapMark node in MainWindow.marklist)
                        {
                            if (node.sign == "燃气") { }
                            else
                            {
                                str.Append("'" + node.Name + "',");
                            }

                        }
                        string station = str.ToString();
                        station = station.Substring(0, station.Length - 1);
                        dt = dbOpera.Instance.getData("station_data", "*", "名称 in (" + station + ")");
                        dt.Columns.Remove("端口数");
                        dt.Columns.Remove("经度");
                        dt.Columns.Remove("纬度");
                        dt.Columns.Remove("口2连接元件编号");
                        dt.Columns.Remove("口2连接元件接口编号");
                        dt.Columns["口1连接元件编号"].ColumnName = "连接元件编号";
                        break;
                }
                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.Filter = "CSV文件|*.CSV";
                saveFile.InitialDirectory = filepath;
                saveFile.RestoreDirectory = false;
                string localFilePath;
                bool? result = saveFile.ShowDialog();
                if (result == true)
                {
                    localFilePath = saveFile.FileName.ToString();
                    filepath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));//获取文件路径，不带文件名 
                    string filename = saveFile.FileName;
                    dbOpera.Instance.SaveCSV(dt, filename);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void Tables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tag = System.Convert.ToInt16(tables.SelectedValue.ToString());
        }
    }
}
