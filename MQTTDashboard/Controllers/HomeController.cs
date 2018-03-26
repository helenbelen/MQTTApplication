using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MQTTDashboard.Models;
using System.Configuration;
using Newtonsoft.Json;

namespace MQTTDashboard.Controllers
{
    public class HomeController : Controller
    {
        Dictionary<string, double> chartData;
       List<Device> deviceList;
        List<DataItem> dataItems;

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
            UpdateDeviceList();
            UpdateDataList();
            chartData = this.GetAverageData_Chart();
            ViewBag.AverageChart_X = chartData.Keys.ToList<string>();
            ViewBag.AverageChart_Y = chartData.Values.ToList<double>();
            chartData = this.GetDataByDate_Chart();
            ViewBag.DateChart_X = chartData.Keys.ToList<string>();
            ViewBag.DateChart_Y = chartData.Values.ToList<double>();
            

            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public  IActionResult DataItems()
        {
            UpdateDataList();
           return View(dataItems);
          
        }
        public IActionResult DeviceList()
        {
            UpdateDeviceList();
            return View(deviceList);

        }
        public IActionResult EditDevice(int id)
        {
           
            Device device = deviceList.FirstOrDefault(dev => dev.DeviceId == id);

            return View(device);
        }
        public IActionResult UpdateDevice(Device updatedDevice)
        {

           
            Device device = deviceList.FirstOrDefault(dev => dev.DeviceId == updatedDevice.DeviceId);
           
          if(device.DeviceName != null && updatedDevice.DeviceLocation != device.DeviceLocation)
            {
                APIAdapter.UpdateDeviceLocation(updatedDevice.DeviceId, updatedDevice.DeviceLocation).Wait();
            }


            return RedirectToAction("DeviceList");

        }

        public void UpdateDataList()
        {
          
            APIAdapter.GetInfo(URLType.DEVICEDATA).Wait();
           dataItems = JsonConvert.DeserializeObject<List<DataItem>>(APIAdapter.APIResponse.ToString());
        }

        public void UpdateDeviceList()
        {
            APIAdapter.GetInfo(URLType.DEVICELIST).Wait();
            deviceList = JsonConvert.DeserializeObject<List<Device>>(APIAdapter.APIResponse.ToString());
          
        }

        public Dictionary<string,double> GetAverageData_Chart()
        {

            Dictionary<string, double> avgchartData = new Dictionary<string, double>();
            
            double count;
            double total;
            foreach (Device d in deviceList)
            {
                count = 1;
                total = 0;
                foreach (DataItem item in dataItems)
                {
                    if (d.DeviceId == item.DeviceId)
                    {

                        try
                        {
                            double number = Double.Parse(item.Data);
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

                avgchartData.Add(d.DeviceName, (double)(total / count));

            }

            return avgchartData;
        }

        public Dictionary<string,double> GetDataByDate_Chart()
        {
          
            Dictionary<string,double> datachartData = new Dictionary<string,double>();
            
            double count;
            foreach(DataItem d in dataItems)
            {
               
                string date = d.TimeStamp.ToShortDateString();


                if (!datachartData.ContainsKey(date))
                {
                    count = 0;
                    foreach (DataItem item in dataItems)
                    {
                        if (item.TimeStamp.ToShortDateString() == date)
                        {
                            count++;
                        }
                    }

                    datachartData.Add(date, count);
                }
                else if (datachartData.ContainsKey(date))
                {
                    count = 0;
                    foreach (DataItem item in dataItems)
                    {
                        if (item.TimeStamp.ToShortDateString() == date)
                        {
                            count++;
                        }
                    }

                    datachartData.Remove(date);
                    datachartData.Add(date, count);
                }
            }
            return datachartData;
        }

    }
}
