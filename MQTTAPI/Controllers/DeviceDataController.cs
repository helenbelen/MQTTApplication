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
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<DeviceData> Get()
        {
            return _context.DeviceData.ToList();
        }

        // GET api/<controller>/5
        [HttpGet("{id}",Name ="GetDevice")]

        public IActionResult Get(int id)
        {
            var device = _context.DeviceData.FirstOrDefault(d => d.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }

           return new ObjectResult(device);

        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Create([FromBody]DeviceData HTTPargs)
        {
            if(HTTPargs == null)
            {
                return BadRequest();
            }

            
            /*DeviceData d = new DeviceData();
            d.DeviceId = int.Parse(args[0]);
            d.Timestamp = new DateTime(int.Parse(args[1]));
            d.Data = args[2];*/
            
            _context.DeviceData.Add(HTTPargs);
            _context.SaveChanges();

            return CreatedAtRoute("GetDevice", new { id = HTTPargs.DeviceId }, HTTPargs);

        }


        // PUT api/<controller>/5
        [HttpPut("{id}")]
        /*public void Put(int id, [FromBody]string value)
        {
            
        }*/

        // DELETE api/<controller>/5
        //Testing Purposes
        [HttpDelete("{id}/{auth}")]
        public IActionResult Delete(int id, string auth)
        {

            var device = _context.DeviceData.FirstOrDefault(d => d.DeviceId == id);
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
