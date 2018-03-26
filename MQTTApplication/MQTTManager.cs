using System;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using System.Configuration;
using System.Net;
using System.Net.Http;


using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MQTTApplication
{
    public struct DataPackage
    {

        public string clientID { get; set; }
        public string Data { get; set; }
    }



    public class MQTTManager : IDisposable
    {

        MqttClient client;
        static HttpClient httpClient;
        static Uri baseURL = new Uri(MQTTCommon.Resources.webApiUrl);
        DataPackage dataPackage;
        public MQTTManager()
        {
            //connect to mosquitto
            client = new MqttClient(MQTTCommon.Resources.brokerUrl);
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

        }

        public string Topic { get; set; }

        public bool publishData(string[] args)
        {

            if (args.Length ==2 && checkDataFormat(args))
            {
               
                try
                {
                    PostDeviceData(dataPackage).Wait();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Failed to submit data to API.");
                }
            }

          
            return false;
        }

        public bool checkDataFormat(string[] args)
        {
           
                dataPackage = new DataPackage();
                dataPackage.clientID = args[0];

                dataPackage.Data = args[1];
                //Check if Device Exists

                return GetDevice(dataPackage).Result != null ? true : false;
            
          
        }
        public void subscribe()
        {
            // register to message received
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            // subscribe to the topic "/home/temperature" with QoS 2
            client.Subscribe(new string[] { ConfigurationManager.AppSettings["DefaultTopic"] }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });


        }

        public void broadcastLocation (string [] args)
        {
            if (args.Length == 3 && args[0] == "LOCATION")
            {

                client.Publish("Linux/locationUpdates", System.Text.Encoding.UTF8.GetBytes(args[1] + " " + args[2] ));
                
            }

        }
        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

            this.publishData(System.Text.Encoding.UTF8.GetString(e.Message).Split("-"));

        }


        public static async Task<Uri> PostDeviceData(DataPackage newData)
        {
            httpClient = new HttpClient();
            var data = new JValue(newData.Data);
            var url = baseURL + $"DeviceData/AddData/{newData.clientID}/{newData.Data}";

            var response = await httpClient.PostAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public static async Task<HttpContent> GetDevice(DataPackage newData)
        {
            httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(baseURL + "/" + newData.clientID);
            return response.Content;
        }


        public void Dispose()
        {
            if (client.IsConnected)
            {
                client.Disconnect();
            }
            if (httpClient != null)
            {
                httpClient.Dispose();
            }
        }
    }
}
