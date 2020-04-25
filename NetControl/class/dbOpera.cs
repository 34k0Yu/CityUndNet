using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace NetControl
{
    class dbOpera
    {
        string constr = "Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "network.db";
        static dbOpera _dbOperaInstance;
        SQLiteConnection conn;

        private static object Singleton_Lock = new object();
        public static dbOpera Instance
        {
            get
            {
                if (_dbOperaInstance == null) //双if +lock
                {
                    lock (Singleton_Lock)
                    {
                        if (_dbOperaInstance == null)
                        {
                            _dbOperaInstance = new dbOpera();
                        }
                    }
                }
                return _dbOperaInstance;
            }
        }
        private dbOpera()
        {
            try
            {
                conn = new SQLiteConnection(constr);
                conn.Open();
                //MessageBox.Show("ok");
            }
            catch
            {
                // MessageBox.Show("no connnection");
            }
        }

        public void insetdata(string table, string value)
        {
            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    string sql = "insert into " + table + " values (" + value + ")";
                    SQLiteCommand command = new SQLiteCommand(sql, conn);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    command.Dispose();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        public void ExcuteQuery(IList<string> sqls)
        {
            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    SQLiteCommand command = conn.CreateCommand();
                    foreach (var str in sqls)
                    {
                        command.CommandText = str;
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    command.Dispose();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }
        public void insetdata(string table, string column, string value)
        {
            string sql = "insert into " + table + "(" + column + ") values (" + value + ")";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void closedata()
        {
            conn.Close();
        }

        //跟新数据
        public void updata(string table, string condition, string assignment)
        {
            string s = "update " + table + " set " + assignment + " where " + condition;
            SQLiteCommand command = new SQLiteCommand(s, conn);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void updata(string table, string assignment)
        {
            string s = "update " + table + " set " + assignment;
            SQLiteCommand command = new SQLiteCommand(s, conn);
            command.ExecuteNonQuery();
            command.Dispose();
        }


        //离线adapter执行
        DataTable cmdGet(string cmdStr)
        {
            DataTable dt = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmdStr, conn); //执行结果集存入离线adapter中
            try
            {
                adapter.Fill(dt);
            }
            catch { }
            return dt;
        }
        //获取数据
        public DataTable getData(string table, string varList, string condition)
        {
            using (SQLiteTransaction transaction = conn.BeginTransaction())
            {
                DataTable dt = new DataTable();
                string s = "select " + varList + " from " + table + " where " + condition;
                dt = cmdGet(s);
                return dt;
            }
               
        }
        public DataTable getData(string table, string varList)
        {
            DataTable dt = new DataTable();
            string s = "select " + varList + " from " + table;
            dt = cmdGet(s);
            return dt;
        }
        public DataTable getData(string FdCmd)
        {
            DataTable dt = new DataTable();
            dt = cmdGet(FdCmd);
            return dt;
        }
        public DataTable deleteData(string table, string condition)
        {
            DataTable dt = new DataTable();
            string s = "delete from " + table + " where " + condition;
            dt = cmdGet(s);
            return dt;
        }

        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public void SaveCSV(DataTable dt, string filename)
        {
            FileStream fs = new FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";
            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);
            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    data += dt.Rows[i][j].ToString();
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();
            //System.Windows.MessageBox.Show("CSV文件保存成功！");
        }
    }
}
