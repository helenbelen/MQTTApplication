using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;

namespace WebAPI
{
    public class MQTTLocationBroadcast
    {
        MqttClient client = new MqttClient(MQTTCommon.Resources.brokerUrl);

        public MQTTLocationBroadcast()
        {
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.Subscribe(new string[] { "Linux/location" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
        }
        public void broadcastLocation(string[] args)
        {
            // subscribe to the topic "/home/temperature" with QoS 2
            if (args.Length == 3 && args[0] == "LOCATION")
            {
                client.Publish("Linux/locationUpdates", System.Text.Encoding.UTF8.GetBytes(args[1] + "-" + args[2]));
            }
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received
        }
    }
}
