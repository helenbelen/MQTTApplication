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

        public IActionResult Graphs()
        {
            APIAdapter.GetDeviceData().Wait();
            APIAdapter.GetDeviceList().Wait();
            ViewBag.AverageChart_X = this.GetAverageData_Chart().Keys;
            ViewBag.AverageChart_Y = this.GetAverageData_Chart().Values;

            ViewBag.DateChart_X = this.GetDataByDate_Chart().Keys;
            ViewBag.DateChart_Y = this.GetDataByDate_Chart().Values;


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

        public Dictionary<string,double> GetAverageData_Chart()
        {
            Dictionary<string, double> chartData = new Dictionary<string, double>();
            int count;
            int total;
            foreach (Device d in APIAdapter.DeviceList)
            {
                count = 0;
                total = 0;
                foreach (DataItem item in APIAdapter.DataList)
                {
                    if (d.DeviceId == item.DeviceId)
                    {

                        try
                        {
                            int number = Int32.Parse(item.Data);
                            total = total + number;
                            count++;

                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("{0}: Bad Format", item.Data);
                        }
                        catch (OverflowException)
                        {
                            Console.WriteLine("{0}: Overflow", item.Data);
                        }


                    }
                }

                chartData.Add(d.DeviceName, (double)(total / count));

            }

            return chartData;
        }

        public Dictionary<string,int> GetDataByDate_Chart()
        {
            Dictionary<string, int> chartData = new Dictionary<string,int>();
            int count;
            foreach(DataItem d in APIAdapter.DataList)
            {
                count = 0;
                string date = d.Timestamp.ToShortDateString();


                if (!chartData.ContainsKey(date))
                {
                    foreach(DataItem item in APIAdapter.DataList)
                    {
                        if(item.Timestamp.ToShortDateString() == date)
                        {
                            count++;
                        }
                    }   
                }

                chartData.Add(date, count);
            }
            return chartData;
        }

    }
}
