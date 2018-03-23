﻿using System;
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
            if (ip == null)
            {
                return new NoContentResult();
            }
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
        [Route("~/api/DeviceData/AddData/{deviceID}/{data}")]
        [HttpPut]
        public IActionResult AddData(int deviceID, string data)
        {
            if (deviceID <= 0 || data == null)
            {
                return BadRequest();
            }
            if(_context.DeviceList.Any(d => d.DeviceId == deviceID && d.DeviceName != null))
            {
<<<<<<< HEAD

                var lastData = _context.DeviceData.LastOrDefault(d => d.Data != null);
                int dataID = lastData.DataId + 1;
                DateTime newTimestamp = DateTime.Now;
                DeviceData deviceData = new DeviceData();
                deviceData.DataId = dataID;
                deviceData.DeviceId = deviceID;
                deviceData.TimeStamp = newTimestamp;
                deviceData.Data = data;
=======
                DeviceData deviceData = new DeviceData
                {
                    DeviceId = device,
                    TimeStamp = DateTime.Now,
                    Data = data
                };
>>>>>>> 1b7ccec37e41bbe33e0a8fea02309be090294d01
                _context.DeviceData.Add(deviceData);
                _context.SaveChanges();
                return new ObjectResult("Data Added");
            }
            return new BadRequestObjectResult("An Unknown Error Occured.");
            
        }


        
    }
}
