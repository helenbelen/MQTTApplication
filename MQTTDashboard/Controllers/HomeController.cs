using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MQTTDashboard.Models;
using System.Configuration;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MQTTDashboard.Controllers
{
    public class HomeController : Controller
    {
        static private readonly string baseURL = "http://webapi-dev.us-east-1.elasticbeanstalk.com/api/DeviceData/";
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
            dataItems.Clear();
            GetDeviceData();
            return View(dataItems);
          
        }


        public static async Task GetDeviceData()
        {
            httpClient = new HttpClient();
            
            using (var httpClient = new HttpClient())
            {

                var response =  httpClient.GetStringAsync(new Uri(baseURL)).Result;

                var releases = JArray.Parse(response);
                foreach (Object o in releases)
                {
                    dataItems.Add(JsonConvert.DeserializeObject<DataItem>(o.ToString()));
                }
            }

            
        }
    }
}
