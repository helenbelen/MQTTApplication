using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MQTTDashboard.Models
{
    [Serializable]
    public class DataItem
    {
        public int DataId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Data { get; set; }
        public string Topic { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
