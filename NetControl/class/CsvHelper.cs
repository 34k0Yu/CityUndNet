using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetControl
{ 
    public class CsvHelper
    {
        public static System.Data.DataTable CsvToDataTable(string filePath, int n)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default, false);
            try
            { 
                int m = 0;
                while (!reader.EndOfStream)
                {
                    m = m + 1;
                    string str = reader.ReadLine();
                    string[] split = str.Split(',');
                    if (m == n)
                    {
                        System.Data.DataColumn column; //列名
                        for (int c = 0; c < split.Length; c++)
                        {
                            column = new System.Data.DataColumn();
                            column.DataType = System.Type.GetType("System.String");
                            column.ColumnName = split[c];
                            if (dt.Columns.Contains(split[c]))
                                column.ColumnName = split[c] + c;
                            dt.Columns.Add(column);
                        }
                    }
                    if (m >= n + 1)
                    {
                        System.Data.DataRow dr = dt.NewRow();
                        for (int i = 0; i < split.Length; i++)
                        {
                            string tempStr = split[i];
                            tempStr=tempStr.Trim('\"');
                            dr[i] = tempStr;
                        }
                        dt.Rows.Add(dr);
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            reader.Close();
            return dt;
        }
    }
}
