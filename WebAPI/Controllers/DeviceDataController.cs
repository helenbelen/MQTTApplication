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
            var ip = _context.ConnectionInfo.FirstOrDefault(d => d.InfoName == "mosquitto");
            if(ip == null)
            {
                return new NoContentResult();
            }
            return new ObjectResult(ip.InfoString);
        }

       
        //GET: api/DeviceData/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            
            var deviceData = _context.DeviceData.Where(d => d.DeviceId == id);
          
            return new ObjectResult(deviceData);
        }


        // POST: api/DeviceData/5

        [HttpPost("{id}")]
        public IActionResult Post(int id, [FromBody]string data)
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


        
    }
}
