using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace NetControl
{
    public class JsonHelper
    {
        private static readonly string configFilePath = AppDomain.CurrentDomain.BaseDirectory + "DataFileLocationConfig.json";
        public static JObject All_jObject = null;
        public static string[] getFilesLocationByConfig(string senceName)
        {
            if (string.IsNullOrEmpty(senceName)) return null;
            try
            {
                JArray array = getAllSences();
                if (array.Count == 0) return null;
                string[] tempFileStr = null;
                foreach (var item in array)
                {
                    if (((JObject)item).Property(senceName) != null)
                    {
                        JArray fileArray = (JArray)item[senceName];
                        if (fileArray.Count == 0) return null;
                        tempFileStr = new string[fileArray.Count];
                        for (int i = 0; i < fileArray.Count; i++)
                        {
                            tempFileStr[i] = fileArray[i].ToString();
                        }
                        return tempFileStr;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 增加场景数据
        /// </summary>
        /// <param name="senceName">场景名称</param>
        /// <param name="fileNames">数据名称</param>
        /// <returns></returns>
        public static bool InsertNewSences(string senceName, string[] fileNames)
        {
            if (string.IsNullOrEmpty(senceName)) return false;
            try
            {
                JArray array = getAllSences();
                foreach (var item in array)
                {
                    if (((JObject)item).Property(senceName) != null)
                    {
                        JArray fileArray = (JArray)item[senceName];
                        if (fileNames == null || fileNames.Length == 0) return true;
                        for (int i = 0; i < fileNames.Length; i++)
                        {
                            fileArray[i] = fileNames[i];
                        }
                        File.WriteAllText(configFilePath, All_jObject.ToString());
                        return true;
                    }
                }
                //不存在此场景
                if (fileNames.Length == 0)
                    array.Add(new JObject { { senceName, new JArray() } });
                else
                {
                    JArray newArray = new JArray();
                    foreach (var str in fileNames)
                    {
                        newArray.Add(str);
                    }
                    array.Add(new JObject { { senceName, newArray } });
                }
                File.WriteAllText(configFilePath, All_jObject.ToString());
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return false;
            }
        }/// <summary>
         /// 获取所有场景
         /// </summary>
         /// <returns></returns>
        public static JArray getAllSences()
        {
            if (!File.Exists(configFilePath)) throw new FileNotFoundException("数据配置文件不存在");
            using (StreamReader sr = File.OpenText(configFilePath))
            {
                using (JsonTextReader textReader = new JsonTextReader(sr))
                {
                    All_jObject = JObject.Load(textReader);
                    JArray array = (JArray)All_jObject["sences"];
                    if (array.Count == 0) return null;
                    return array;
                }
            }
        }
        /// <summary>
        /// 获取指定数据文件的路径
        /// </summary>
        /// <param name="datafileName"></param>
        /// <returns></returns>
        public static string GetDataFilePath(string datafileName)
        {
            if (!File.Exists(configFilePath)) throw new FileNotFoundException("数据配置文件不存在");
            using (StreamReader sr = File.OpenText(configFilePath))
            {
                using (JsonTextReader textReader = new JsonTextReader(sr))
                {
                    All_jObject = JObject.Load(textReader);
                    string parentPath = All_jObject["location"].ToString();
                    return parentPath /*+ "//"*/ + datafileName;
                }
            }
        }
    }
}
