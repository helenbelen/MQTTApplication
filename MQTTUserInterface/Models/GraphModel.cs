using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQTTUserInterface.Models
{
    public class GraphModel
    {

        public string DeviceName { get; set; }
        public List<string> xAxis { get; set; }
        public List<string> yAxis { get; set; }
        public string GraphType { get; set; }
    }
}
