using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MQTTDashboard.Models
{
    [Serializable]
    public partial class Device
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceLocation { get; set; }
    }
}
