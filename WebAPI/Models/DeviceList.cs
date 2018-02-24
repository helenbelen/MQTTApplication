using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public partial class DeviceList
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceLocation { get; set; }

        public DeviceList Device { get; set; }
        public DeviceList InverseDevice { get; set; }
    }
}
