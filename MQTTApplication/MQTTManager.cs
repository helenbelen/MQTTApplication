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
    public struct DataPackage {

        public string clientName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Data { get; set; }
    }



    public class MQTTManager : IDisposable
    {

        MqttClient client;
        static HttpClient httpClient;
        static string baseURL = ConfigurationManager.AppSettings["APIurl"];
        DataPackage dataPackage;
        public MQTTManager()
        {
            //connect to mosquitto
            client = new MqttClient(ConfigurationManager.AppSettings["MosquittoIP"]);
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

        }

        public string Topic {get;set;}

        public bool publishData(string [] args)
        {
            
           if (checkDataFormat(args))
            {
               
                PostDeviceData(dataPackage).Wait();

                System.Console.WriteLine("Database Values:");
                foreach (string s in args)
                {
                    System.Console.WriteLine(s);
                }
              
              
                return true;
            }

           

            return false;      
           
        }

        public bool checkDataFormat(string [] args)
        {
            dataPackage = new DataPackage();
            dataPackage.clientName = args[0];
            dataPackage.TimeStamp = DateTime.Now;
            dataPackage.Data = args[1];
           //Check if Device Exists
            return args.Length == 2 ? true : false;
        }
        public void subscribe()
        {
            // register to message received
           client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                        
           // subscribe to the topic "/home/temperature" with QoS 2
           client.Subscribe(new string[] { ConfigurationManager.AppSettings["DefaultTopic"] }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
          

        }

        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
                       
            this.publishData(System.Text.Encoding.UTF8.GetString(e.Message).Split(" "));
                                  
        }

     
        public static async Task<Uri> PostDeviceData(DataPackage newData)
        {
            httpClient = new HttpClient();
            JObject device = new JObject(new JProperty("DeviceName", newData.clientName), 
                new JProperty("TimeStamp", newData.TimeStamp), new JProperty("Data", newData.Data));

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(baseURL, device);
            
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public static async Task<Uri> GetDevice(DataPackage newData)
        {
            httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(baseURL +"/"+ newData.clientName);
            return response.Headers.Location;
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
