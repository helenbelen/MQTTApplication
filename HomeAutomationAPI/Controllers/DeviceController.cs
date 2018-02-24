using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HomeAutomationAPI.Models;

namespace HomeAutomationAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Device")]
    public class DeviceController : Controller
    {
        private readonly HomeAutomationContext _context;
        public DeviceController(HomeAutomationContext context)
        {
            _context = context;
        }
        // GET: api/Device
        [HttpGet]
        public IEnumerable<DeviceList> GetDevices()
        {
            return _context.DeviceList.ToList();
        }
        [Route("~/api/Device/GetData")]

        [HttpGet]
        public IEnumerable<DeviceData> GetData()
        {
            return _context.DeviceData.ToList();
        }

        [Route("~/api/Device/GetConnectionInfo")]

        [HttpGet]
        public IActionResult GetConnectionInfo()
        {
            var ip = _context.ConnectionInfo.FirstOrDefault(d => d.InfoName == "mosquitto");
            if(ip == null)
            {
                return new NoContentResult();
            }
            return new ObjectResult(ip.InfoString);
        }

 
        [Route("~/api/Device/GetDataByID/{id}")]
        [HttpGet]
        //HttpGet("{id}",Name ="GetDataById")]
        public IActionResult Get(int id)
        {
            
            var deviceData = _context.DeviceData.Where(d => d.DeviceId == id);
          
            return new ObjectResult(deviceData);
        }

        [Route("~/api/Device/GetDeviceByID/{id}")]
        [HttpGet]
        //HttpGet("{id}",Name ="GetDataById")]
        public IActionResult GetDevice(int id)
        {

            var deviceData = _context.DeviceList.Where(d => d.DeviceId == id);

            return new ObjectResult(deviceData);
        }

        // POST: api/Device
        [Route("~/api/Device/AddData/{id}/{data}")]
        [HttpPost]
        public IActionResult Post([FromBody]int id, string data)
        {
             if(_context.DeviceList.Any(d => d.DeviceId == id && d.DeviceName != null))
            {
                DeviceData deviceData = new DeviceData();
                deviceData.DeviceId = id;
                deviceData.Timestamp = DateTime.Now;
                deviceData.Data = data;
                _context.DeviceData.Add(deviceData);
                _context.SaveChanges();
                return new ObjectResult("Data Has Been Added!");
            }
            return BadRequest("Data has not been added. Please ensure Device has been registered");
            
        }

        // PUT: api/Device/5
        [Route("~/api/Device/UpdateDeviceLocation/{id}/{location}")]
        [HttpPut]
        public IActionResult Put(int id, [FromBody]string location)
        {
            if (_context.DeviceList.Any(d => d.DeviceId == id && d.DeviceName != null))
            {
               var device = _context.DeviceList.First(d => d.DeviceId == id);
                device.DeviceLocation = location;
                _context.Update(device);
                _context.SaveChanges();
                return new ObjectResult("Location has been updated for Device #: " + id);
            }
            return BadRequest("Location has not been updated. Please ensure Device has been registered");
        }

        [Route("~/api/Device/RegisterDevice/{name}")]
        [HttpPut]
        public IActionResult RegisterDevice([FromBody]string name)
        {
            
                var device = _context.DeviceList.First(d => d.DeviceName == null);
            if (device != null)
            {
                device.DeviceName = name;
                _context.Update(device);
                _context.SaveChanges();
                return new ObjectResult("Your Device is registered and has a device Id of: " + device.DeviceId + " Please keep this for your records. You will not be able to retrieve this again without contacting the API Admin");
            }
            return BadRequest("An error has occured and your device was not registered. Please contact the API Admin");
        }

        
    }
}
