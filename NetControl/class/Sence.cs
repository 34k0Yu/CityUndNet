using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetControl
{
    public class Sence
    {
        public string Name { get; set; }
        public string Result { get; set; }
        public string Modify { get; set; }
        public string FileName { get; set; }
    }
    public class ChartData
    {
        public string Time { get; set; }

        public int  Num { get; set; }

        public double Count { get; set; }
    }
}
