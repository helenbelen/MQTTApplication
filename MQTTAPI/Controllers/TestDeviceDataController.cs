using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTTAPI.Models;

namespace MQTTAPI.Controllers
{
   
    [Produces("application/json")]
    [Route("api/TestDeviceData")]
    public class TestDeviceDataController : Controller
    {
        

        List<DeviceData> testData = new List<DeviceData>();

        public TestDeviceDataController() { }

        public TestDeviceDataController(List<DeviceData> data)
        {
            this.testData = data;
        }

        public IEnumerable<DeviceData> Get()
        {
            return testData.ToList();
        }

        public IActionResult Get(string name)
        {
            var device = testData.FirstOrDefault(d => d.DeviceName == name);
            if (device == null)
            {
                return NotFound();
            }

            return new ObjectResult(device);

        }

       
    }
}