using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetControl
{
    public class LineColorModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Temp { get; set; }
        public double Pa { get; set; }
    }

    public class LinePMModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Flu { get; set; }
        public double Pa { get; set; }
    }
}
