using System;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using System.Configuration;
using System.Net;
using System.Net.Http;


using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MQTTCommon;


namespace MQTTApplication
{
    public struct DataPackage
    {
        public string ClientId { get; set; }
        public string Data { get; set; }
        public string Topic { get; set; }
    }

    public struct WebApiDeviceAddData
    {
        public string Topic;
        public string Data;
    }

    public class MQTTManager : IDisposable
    {

        MqttClient client;
        HttpClient httpClient;
        Uri baseURL = new Uri("http://webapi-dev.us-east-1.elasticbeanstalk.com/api/");

        public MQTTManager()
        {
            //connect to mosquitto
            client = new MqttClient("34.231.187.147");
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // Create an HttpClient for later use
            httpClient = new HttpClient()
            {
                BaseAddress = baseURL
            };
        }

        public string Topic { get; set; }
        
        public void subscribe()
        {
            // register to message received
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // subscribe to the topic "/home/temperature" with QoS 2
            client.Subscribe(new string[] {"/test/+/soilMoisture" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }

        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var str = System.Text.Encoding.UTF8.GetString(e.Message);
            var parts = str.Split("-",StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                var dataPackage = new WebApiDeviceAddData()
                {
                    Data = parts[1].Trim(),
                    Topic = e.Topic,
                };

                try
                {
                    PostDeviceData(parts[0].Trim(), dataPackage).Wait();
                    Console.WriteLine("Data Saved");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to submit data to API.");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task<Uri> PostDeviceData(string clientId, WebApiDeviceAddData newData)
        {
            Console.WriteLine($"Submitting data - ID: {clientId}, Data: {newData.Data}, Topic: {newData.Topic}");


            string url = $"http://webapi-dev.us-east-1.elasticbeanstalk.com/api/DeviceData/AddData/{clientId}";
            var response = await httpClient.PostAsJsonAsync(url, newData);

           // var url = baseURL + "DeviceData/AddData/"+ clientId ;
            //var response = await httpClient.PostAsJsonAsync(new Uri(url), newData);
            response.EnsureSuccessStatusCode();
            
            return response.Headers.Location;
        }

        public async Task<HttpContent> GetDevice(DataPackage newData)
        {
            HttpResponseMessage response = await httpClient.GetAsync(baseURL + "DeviceList/GetDevice/" + newData.ClientId);
            return response.Content;
        }

        public void Dispose()
        {
            if (client.IsConnected)
            {
                client.Disconnect();
            }
            httpClient?.Dispose();
        }
    }
}
