using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public partial class DeviceData
    {
        public int DataId { get; set; }
        public int DeviceId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Data { get; set; }
    }
}
