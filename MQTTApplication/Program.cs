using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Configuration;

using MySql.Data;
using MySql.Data.MySqlClient;



namespace MQTTApplication
{
    class Program
    {
        static MQTTManager myManager = new MQTTManager();


        static void Main(string[] args)
        {


            using (myManager)
            {

               
                // client.Connect(clientId, "adAgency", "password", true, 60);
                myManager.subscribe();

                Console.WriteLine("Welcome To The MQTT Manager");
                
                string s = "";
                while (s != "exit")
                {
                    Console.WriteLine("Write A Message");
                    s = Console.ReadLine();
                    
                    myManager.publishData(s.ToString().Split(" "));
                    
                }

            }
            
        }
        

       
    }
    
} 
