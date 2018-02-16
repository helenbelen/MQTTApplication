using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MQTTAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MQTTAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class DeviceDataController : Controller
    {
        
        private readonly MQTTContext _context;

        public DeviceDataController(MQTTContext context)
        {
            _context = context;
        }

        // GET api/<controller>/5

        [HttpGet]

        public IActionResult Get(string name)
        {
            var device = _context.DeviceList.FirstOrDefault(d => d.DeviceName == name);
            if (device == null)
            {
                return NotFound();
            }

            return new ObjectResult(device);

        }

        [HttpGet]

        public IActionResult GetBroker()
        {
            var ip = _context.ConnectionInfo.FirstOrDefault(connection => connection.InfoName == "mosquitto");
            if (ip == null)
            {
                return NotFound();
            }

            return new ObjectResult(ip.InfoString);

        }
        //
        // Manage Device List
        //
        [HttpPost]
        public IActionResult RegisterDevice(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            var device = _context.DeviceList.FirstOrDefault(d => d.DeviceName == null);
            device.DeviceName = name;
            _context.DeviceList.Update(device);
            _context.SaveChanges();

            return new NoContentResult();

        }

        [HttpGet]
        public IEnumerable<DeviceList> GetDevices()
        {
            return _context.DeviceList.ToList();
        }


        //
        //    Manage Device Data 
        //
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<DeviceData> GetData()
        {
            return _context.DeviceData.ToList();
        }

        

        // POST api/<controller>
        [HttpPost]
        public IActionResult AddData([FromBody]DeviceData HTTPargs)
        {
            if(HTTPargs == null)
            {
                return BadRequest();
            }

            var device = _context.DeviceList.FirstOrDefault(d => d.DeviceName == HTTPargs.DeviceName);
            if (device == null)
            {
                return BadRequest("This Device Is Not Registered");
            }
                       
            _context.DeviceData.Add(HTTPargs);
            _context.SaveChanges();

            return CreatedAtRoute("GetDevice", new { id = HTTPargs.DeviceName }, HTTPargs);

        }


        // POST api/<controller>
        [HttpPost("{name}/{location}")]
        public IActionResult UpdateLocation(string name, string location)
        {
            if (name == null || location == null)
            {
                return BadRequest();
            }

            var device = _context.DeviceList.FirstOrDefault(d => d.DeviceName == name);
            if (device == null)
            {
                return BadRequest("This Device Is Not Registered");
            }
            device.DeviceLocation = location;
            _context.DeviceList.Update(device);
            _context.SaveChanges();

            return CreatedAtRoute("GetDevice", new { newName = device.DeviceName }, location);

        }


        // DELETE api/<controller>/5
        //Testing Purposes
        [HttpDelete("{name}/{auth}")]
        public IActionResult Delete(string name, string auth)
        {

            var device = _context.DeviceData.FirstOrDefault(d => d.DeviceName == name);
            if (device == null)
            {
                return NotFound();
            }
            else if (auth =="Helen")
            {

                _context.DeviceData.Remove(device);
                _context.SaveChanges();
                return new NoContentResult();
            }

            return BadRequest();
        }
    }
}
