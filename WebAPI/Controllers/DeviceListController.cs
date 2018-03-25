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
    public class DeviceListController : Controller
    {
        
        private readonly HomeAutomationContext _context;
        public DeviceListController(HomeAutomationContext context)
        {
            _context = context;
            
           
        }
        // GET: api/DeviceList
       
      

        [HttpGet]
        public IEnumerable<DeviceList> Get()
        {
            
            return _context.DeviceList.ToList();
        }

        // GET: api/DeviceList/5
        [Route("~/api/DeviceList/GetDevice/{id}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            
            var deviceData = _context.DeviceList.Where(d => d.DeviceId == id);

            return new ObjectResult(deviceData);
        }



        // POST: api/DeviceList
       [Route("~/api/DeviceList/RegisterDevice/{name}")]
        [HttpPost]
        public IActionResult RegisterDevice(string name)
        {
           
            if (_context.DeviceList.Any(d => d.DeviceName == name))
            {
                return new ObjectResult("This Device Has Already Been Registered");
            }
            else
            {
                try
                {
                    DeviceList device = new DeviceList();
                   
                    device.DeviceName = name;
                    _context.Add(device);
                    _context.SaveChanges();
                    return new ObjectResult("Your Device is registered and has a device Id of: " + device.DeviceId + " Please keep this for your records. You will not be able to retrieve this again without contacting the API Admin");
                }
                catch (Exception ex)
                {
                    return BadRequest("An error has occured and your device was not registered. Please contact the API Admin  " + ex.Message);
                }
            }
        }

        // PUT: api/DeviceList/5
        [Route("~/api/DeviceList/UpdateLocation/{id}/{location}")]
        [HttpPut]

        public IActionResult UpdateLocation(int id, string location)
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
        
       
    }
}
