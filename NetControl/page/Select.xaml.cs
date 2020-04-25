using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace NetControl
{
    /// <summary>
    /// Select.xaml 的交互逻辑
    /// </summary>
    public partial class Select : Window
    {
        public Select()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        DataTable node = new DataTable();
        string condition;
        public string result;

        //子窗口向主窗口传值
        public delegate void TansfDelegate(string value);
        public event TansfDelegate TransfEvent;

        public delegate void ClearSelectResult();
        public event ClearSelectResult clearesult;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool stop = false;
            try {
            condition = secomdi.Text;
            dt = dbOpera.Instance.getData("select name from sqlite_master where type='table';");
            int count = dt.Rows.Count;
            for(int i=0;i<count&&stop==false;i++)
            {
                string name = dt.Rows[i][0].ToString();
                node = dbOpera.Instance.getData(name, "*", "编号=" + condition +"");
                if (node.Rows.Count == 0)
                {
                    node = dbOpera.Instance.getData(name, "*", "名称='" + condition + "'");
                }
                if (node.Rows.Count == 1)
                {
                    stop = true;
                }
            }
            result = node.Rows[0]["名称"].ToString();
            TransfEvent(result);
            }
            catch { MessageBox.Show("查询无结果"); }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            clearesult();
        }
    }
}
