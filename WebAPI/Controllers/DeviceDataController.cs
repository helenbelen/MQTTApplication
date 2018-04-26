using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DeviceDataController : Controller
    {
        private readonly HomeAutomationContext _context;
        public DeviceDataController(HomeAutomationContext context)
        {
            _context = context;
            
            
        }

        // GET: api/DeviceData
        private void SaveIPString()
        {

            var ip = _context.ConnectionInfo.FirstOrDefault(d => d.InfoName == "mosquitto");
            if (ip == null)
            {
                Models.ConnectionInfo info = new Models.ConnectionInfo();
                info.InfoName = "mosquitto";
                info.InfoString = MQTTCommon.Resources.brokerUrl;
                _context.ConnectionInfo.Add(info);
                _context.SaveChanges();
            }
          
        }

        [HttpGet]
        public IEnumerable<DeviceData> Get()
        {
            return _context.DeviceData.ToList();
        }

        //GET: api/DeviceData/GetConnectionInfo
        [Route("~/api/DeviceData/GetConnectionInfo")]
        [HttpGet]
        public IActionResult GetConnectionInfo()
        {
            SaveIPString();
            var ip = _context.ConnectionInfo.FirstOrDefault(d => d.InfoName == "mosquitto");
           
            return new ObjectResult(ip.InfoString);
        }


        //GET: api/DeviceData/5
        [Route("~/api/DeviceData/GetDeviceData/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {

            var deviceData = _context.DeviceData.Where(d => d.DeviceId == id);

            return new ObjectResult(deviceData);
        }


        // POST: api/DeviceData/5
        [Route("~/api/DeviceData/AddData/{deviceID}")]
        [HttpPost]
        public IActionResult AddData(int deviceID, [FromBody] MQTTCommon.WebApiDeviceAddData json)
        {
            if (deviceID <= 0 || string.IsNullOrWhiteSpace(json.Data) || string.IsNullOrWhiteSpace(json.Topic))
            {
                return BadRequest();
            }

            if(_context.DeviceList.Any(d => d.DeviceId == deviceID && d.DeviceName != null))
            {
                var deviceData = new DeviceData
                {
                    DeviceId = deviceID,
                    TimeStamp = DateTime.Now,
                    Data = json.Data,
                    Topic = json.Topic
                };
                _context.DeviceData.Add(deviceData);
                _context.SaveChanges();
                return new ObjectResult("Data Added");
            }

            return new BadRequestObjectResult("An Unknown Error Occured.");
        }
    }
}
