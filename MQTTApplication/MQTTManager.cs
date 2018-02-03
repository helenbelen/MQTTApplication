using System;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using System.Configuration;

namespace MQTTApplication
{
  
    
    public class MQTTManager :IDisposable
    {
        string messageSent;
        MqttClient client;
       
        
        public MQTTManager()
        {
            //connect to database
            //connect to mosquitto
           client = new MqttClient("34.231.187.147");
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
          

        }

       
        public bool addData(string clientId, string newData = null, int[] dataArray = null)
        {
            return false;
        }

        public string getData(string clientId, DateTime start, DateTime end)
        {
            return "data";
        }

        public bool addClient(string id, string dataType)
        {
            return false;
        }

        public string getClients()
        {
            return "all clients";
        }

        
        public bool setupAdmin(string name, string password)
        {
            return false;
        }

        public bool publishData(string [] args)
        {
           
           if(args.Length == 3)
            { 
                //client id, DateTime, Data
                string clientID = args[0];
                string timeStamp = args[1];
                string data = args[2];
                
               foreach(string s in args)
                {
                    System.Console.Write(s);
                }

               client.Publish("test/Linux", System.Text.Encoding.UTF8.GetBytes("Data was submitted to Database"));
                return true;
            }
            else
            {
                client.Publish("test/Linux", System.Text.Encoding.UTF8.GetBytes("Data was not formated Correctly. Please ensure you submit in the following format: [clientid]*[Date (yyyy-MM-dd HH: mm tt)]*[Data]. Do not include the * character in your data"));
                return false;              
            }

                      
           
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
        
           
            System.Console.WriteLine("The Message format status was : " + RecentMessage);
            
        }

     
        // PUBLISHER

        public string RecentMessage { set { messageSent = value; } get { return messageSent; } }

        public void Dispose()
        {
            client.Disconnect();
        }
    }
}
