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
        //static private readonly string baseURL = "http://webapi-dev.us-east-1.elasticbeanstalk.com/api/Device/";
        static private readonly string baseURL = "http://localhost:51412/api/Device/";
        static HttpClient httpClient;
        static List<DataItem> dataItems = new List<DataItem>();
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

                var response =  httpClient.GetStringAsync(new Uri(baseURL + "GetData")).Result;

                var releases = JArray.Parse(response);
                foreach (Object o in releases)
                {
                    dataItems.Add(JsonConvert.DeserializeObject<DataItem>(o.ToString()));
                }
            }

            
        }


    }
}
