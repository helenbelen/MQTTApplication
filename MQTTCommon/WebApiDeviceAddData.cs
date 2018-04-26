using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MQTTCommon
{
    /// <summary>
    /// These are the data parameters expected by the WebAPI's "~/api/DeviceData/AddData/{deviceID}" route
    /// Structs are slightly more performant than classes for pure storage like this.
    /// </summary>
    public struct WebApiDeviceAddData
    {
        public string Topic;
        public string Data;
    }
}
