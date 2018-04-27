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
        
        List<Device> deviceList;
        List<DataItem> dataItems;

        public IActionResult Index()
        {
            UpdateDeviceList();
            ViewData["DeviceList"]= deviceList;
            UpdateDataList();
            ViewData["DataItems"] = dataItems;
            Device device = deviceList.FirstOrDefault(dev => dev.DeviceName.Contains("Music"));

            DataItem data = dataItems.LastOrDefault(dat => dat.DeviceId == device.DeviceId);
            ViewData["MusicInfo"] = data;

            Dictionary<string, double>[] lists = this.GetChartData();
            ViewBag.AverageChart_X = lists[0].Keys.ToList<string>();
            ViewBag.AverageChart_Y = lists[0].Values.ToList<double>();
            ViewBag.DateChart_X = lists[1].Keys.ToList<string>();
            ViewBag.DateChart_Y = lists[1].Values.ToList<double>();
            return View();
        }

       

        public IActionResult Graphs()
        {
                 
            return PartialView();
        }
        public IActionResult Error()
        {
            return PartialView(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public  IActionResult DataItems()
        {
            UpdateDataList();
          
            return PartialView(dataItems);
          
        }
        public IActionResult DeviceList()
        {
            UpdateDeviceList();
            

            return PartialView(deviceList);

        }
        public IActionResult EditDevice(Device selecteddevice)
        {
            UpdateDeviceList();
            
            Device device = deviceList.FirstOrDefault(dev => dev.DeviceId == selecteddevice.DeviceId);

            return View(device);
        }
        public IActionResult Music()
        {
            UpdateDataList();
            Device device = deviceList.FirstOrDefault(dev => dev.DeviceName.Contains("Music"));

            DataItem data = dataItems.LastOrDefault(dat => dat.DeviceId == device.DeviceId);

            ViewData["MusicInfo"] = data;
            return PartialView(data);
        }
        public IActionResult UpdateDevice(Device updatedDevice)
        {
            UpdateDeviceList();


            Device device = deviceList.FirstOrDefault(dev => dev.DeviceId == updatedDevice.DeviceId);
           
          if(device.DeviceName != null && updatedDevice.DeviceLocation != device.DeviceLocation)
            {
                APIAdapter.UpdateDeviceLocation(updatedDevice.DeviceId, updatedDevice.DeviceLocation).Wait();
            }


            return RedirectToAction("Index");

        }

        public void UpdateDataList()
        {
          
            APIAdapter.GetInfo(URLType.DEVICEDATA).Wait();
           dataItems = JsonConvert.DeserializeObject<List<DataItem>>(APIAdapter.APIResponse.ToString());
            ViewData["DataItems"] = dataItems;
        }

        public void UpdateDeviceList()
        {
            APIAdapter.GetInfo(URLType.DEVICELIST).Wait();
            deviceList = JsonConvert.DeserializeObject<List<Device>>(APIAdapter.APIResponse.ToString());
            ViewData["DeviceList"] = deviceList;

        }

        public Dictionary<string,double>[] GetChartData()
        {

            Dictionary<string, double> avgchartData = new Dictionary<string, double>();
            Dictionary<string, double> datachartData = new Dictionary<string, double>();

            double count;
            double count_average;
            double total;
            foreach (Device d in deviceList)
            {
                count = 1;
                count_average = 0;
                total = 0;
                foreach (DataItem item in dataItems)
                {
                    //Calculate Data Average
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

                    //Get Data By Date

                    string date = item.TimeStamp.ToShortDateString();

                   
                    if (!datachartData.ContainsKey(date))
                    {
                        count_average = 0;
                        foreach (DataItem dataItem in dataItems)
                        {
                            if (item.TimeStamp.ToShortDateString() == date)
                            {
                                count_average++;
                            }
                        }

                        datachartData.Add(date, count);
                    }
                    else if (datachartData.ContainsKey(date))
                    {
                        count_average = 0;
                        foreach (DataItem dataItem in dataItems)
                        {
                            if (item.TimeStamp.ToShortDateString() == date)
                            {
                                count_average++;
                            }
                        }

                        datachartData.Remove(date);
                        datachartData.Add(date, count);
                    }
                }

                avgchartData.Add(d.DeviceName, (double)(total / count));

            }

           
            return new Dictionary<string, double>[] { avgchartData, datachartData };
        }

      

    }
}
