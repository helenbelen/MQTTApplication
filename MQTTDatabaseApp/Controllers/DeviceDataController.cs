using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MQTTDatabaseApp.Entities;
using Microsoft.AspNetCore.Mvc;


namespace MQTTDatabaseApp.Controllers
{
    [Route("api/DeviceData")]
    public class DeviceDataController : ApiController
    {
        private readonly MQTTContext _context;
      public DeviceDataController()
        {

        }
        public DeviceDataController (MQTTContext context)
        {
            _context = context;
        }
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<DeviceData> Get()
        {

            return _context.DeviceData.ToList();
        }

        // GET api/<controller>/5
        [Route ("api/DeviceData/{id}")]
        [HttpGet]
        public string Get(int id)
        {
            var device = _context.DeviceData.FirstOrDefault(newDevice => newDevice.DeviceId == id);
            if (device == null)
            {
                return "Device Not Found";
               


            }

            DeviceData d = new DeviceData();
            d.DeviceId = device.DeviceId;
            d.Timestamp = device.Timestamp;
           return "Device: " + d.DeviceId + "Posted Last On: " + d.Timestamp;
            
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}