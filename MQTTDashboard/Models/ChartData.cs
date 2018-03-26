using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQTTDashboard.Models
{
    [Serializable]
    public class ChartData
    {
        public List<string> AverageChart_X { get; set; }
        public List<int> AverageChart_Y { get; set; }
        public List<string> DateChart_X { get; set; }
        public List<int> DateChart_Y{ get; set; }
    }
}
