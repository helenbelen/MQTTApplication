using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MQTTDashboard.Models;
using System.Configuration;


namespace MQTTDashboard.Controllers
{
    public class HomeController : Controller
    {
      
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public  IActionResult DataItems()
        {
            APIAdapter.GetDeviceData().Wait();
           return View(APIAdapter.DataList);
          
        }
        public IActionResult DeviceList()
        {
            APIAdapter.GetDeviceList().Wait();
            return View(APIAdapter.DeviceList);

        }
        public IActionResult EditDevice(int id)
        {
            Device device = APIAdapter.DeviceList.FirstOrDefault(dev => dev.DeviceId == id);

            return View(device);
        }
        public IActionResult UpdateDevice(Device updatedDevice)
        {

            APIAdapter.GetDeviceList().Wait();
            Device device = APIAdapter.DeviceList.FirstOrDefault(dev => dev.DeviceId == updatedDevice.DeviceId);
           
          if(device.DeviceName != null && updatedDevice.DeviceLocation != device.DeviceLocation)
            {
                APIAdapter.UpdateDeviceLocation(updatedDevice.DeviceId, updatedDevice.DeviceLocation).Wait();
            }


            return RedirectToAction("DeviceList");

        }



    }
}
