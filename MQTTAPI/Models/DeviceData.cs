using System;
using System.Collections.Generic;

namespace MQTTAPI.Models
{
    public partial class DeviceData
    {
        public string DeviceName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Data { get; set; }
    }
}
