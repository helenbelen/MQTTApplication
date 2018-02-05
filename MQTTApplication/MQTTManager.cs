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

        int _clientid;
        DateTime _timestamp;
        string _data;
        public int ClientID { get { return _clientid; } set { _clientid = value; } }
        public DateTime TimeStamp { get { return _timestamp; } set { _timestamp = value; } }
        public string Data { get { return _data; } set { _data = value; } }
    }

  
    
    public class MQTTManager :IDisposable
    {
        string messageSent;
        MqttClient client;
        static HttpClient httpClient;
        static string baseURL = "http://mqttapi-dev.us-east-1.elasticbeanstalk.com//api/DeviceData/";
        DataPackage dataPackage;
        public MQTTManager()
        {
            //connect to database
            //connect to mosquitto
           client = new MqttClient("34.231.187.147");
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
           

        }

        public DataPackage managerData
        {
            get { return dataPackage; }
            set { dataPackage = value; }
        }

        public bool publishData(string [] args)
        {
            string confirmedString = "Data was submitted to Database";
            string badformatString = "Data was not formated Correctly. Please ensure you submit in the following format: [clientid]*[Date (yyyy-MM-dd HH: mm tt)]*[Data]. Do not include the * character in your data";
            string[] confirmedArray = confirmedString.Split(" ");
            string[] badformatArray= badformatString.Split(" ");
           if (args.Length == 3)
            {
                //client id, DateTime, Data
               
                dataPackage = new DataPackage();
                dataPackage.ClientID = Int32.Parse(args[0]);
                dataPackage.TimeStamp = Convert.ToDateTime(args[1]);
                dataPackage.Data = args[2];
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

        public void subscribe()
        {

            // register to message received
           client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            
            
                // subscribe to the topic "/home/temperature" with QoS 2
                client.Subscribe(new string[] { "test/Linux" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            
            


        }

        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

           
            this.RecentMessage = System.Text.Encoding.UTF8.GetString(e.Message);
            this.publishData(this.RecentMessage.Split(" "));
            
           // System.Console.WriteLine("The Message format status was : " + RecentMessage);
            
        }

     
        public static async Task<Uri> PostDeviceData(DataPackage newData)
        {
            httpClient = new HttpClient();
            JObject device = new JObject(new JProperty("DeviceID", newData.ClientID), new JProperty("TimeStamp", newData.TimeStamp), new JProperty("Data", newData.Data));

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(baseURL, device);
            
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public string RecentMessage { set { messageSent = value; } get { return messageSent; } }

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
