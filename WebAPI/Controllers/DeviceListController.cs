using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using uPLibrary.Networking.M2Mqtt;
using WebAPI.Models;
using Newtonsoft.Json;

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

        public struct DeviceRegistrationResult
        {
            public int Id;
            public bool AlreadyRegistered;
            public string Message;
        }


        // POST: api/DeviceList
        [Route("~/api/DeviceList/RegisterDevice/{name}")]
        [HttpPost]
        public IActionResult RegisterDevice(string name)
        {
            DeviceRegistrationResult result = new DeviceRegistrationResult();
            if (_context.DeviceList.Any(d => d.DeviceName == name))
            {
                result.Id = _context.DeviceList.First(d => d.DeviceName == name).DeviceId;
                result.Message = "This Device Has Already Been Registered";
                result.AlreadyRegistered = true;
            }
            else
            {
                try
                {
                    var device = new DeviceList()
                    {
                        DeviceName = name,
                    };
                    _context.Add(device);
                    _context.SaveChanges();

                    result.Id = device.DeviceId;
                    result.Message = $"Your Device is registered and has a device Id of: {device.DeviceId}";
                    result.AlreadyRegistered = false;
                }
                catch (Exception ex)
                {
                    return BadRequest("An error has occured and your device was not registered. Please contact the API Admin");
                }
            }
            return new ObjectResult(result);
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
                UpdateDeviceViaMqtt(device.DeviceName, location);
                return new ObjectResult("Location has been updated for Device #: " + id);
            }
            return BadRequest("Location has not been updated. Please ensure Device has been registered");
        }

        private void UpdateDeviceViaMqtt(string DeviceName, string newLocation)
        {
            var ip = _context.ConnectionInfo.FirstOrDefault(d => d.InfoName == "mosquitto").InfoString;
            if (!string.IsNullOrEmpty(ip))
            {
                MqttClient client = null;
                try
                {
                    client = new MqttClient(ip);
                    client.Connect(Guid.NewGuid().ToString());

                    var result = new { Room = newLocation };
                    var str = JsonConvert.SerializeObject(result);
                    client.Publish($"DeviceConfig/{DeviceName}", System.Text.Encoding.UTF8.GetBytes(str));
                }
                catch
                {
                    // Do nothing
                }
                finally
                {
                    client?.Disconnect();
                }
            }
        }
    }
}
