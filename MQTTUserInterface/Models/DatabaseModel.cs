using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MQTTUserInterface.Models
{
    

    public class DatabaseModel
    {
        static HttpClient httpClient = new HttpClient();
        static HttpResponseMessage response;
        static string baseURL = ConfigurationManager.AppSettings["APIurl"];

        public static async Task<Uri> AsyncGetDevice(string clientName)
        {
            response = await httpClient.GetAsync(baseURL + "/GetDevice/" + clientName);
          

            return response.Headers.Location;
        }

        public static async Task<Uri> AsyncGetAllDevices()
        {
             response = await httpClient.GetAsync(baseURL + "/GetAllDevices");
            return response.Headers.Location;
        }

        public static async Task<Uri> AsyncGetDeviceData(string deviceName)
        {
             response = await httpClient.GetAsync(baseURL + "/GetDeviceData/" + deviceName);
            return response.Headers.Location;
        }

        public static async Task<Uri> AsyncUpdateDeviceLocation(string deviceName, string location)
        {
             response = await httpClient.GetAsync(baseURL + "/UploadLocation/" + deviceName +"/"+location);
            return response.Headers.Location;
        }



        public Uri GetDevice(string deviceName)
        {
            return AsyncGetDevice(deviceName).Result;
        }

        public Uri GetAllDevices()
        {
            return AsyncGetAllDevices().Result;
        }

        public Uri GetDeviceData(string deviceName)
        {
            return AsyncGetDeviceData(deviceName).Result;
        }

        public Uri UpdateDeviceLocation(string deviceName, string location)
        {
            return AsyncUpdateDeviceLocation(deviceName, location).Result;
        }
    }
}
