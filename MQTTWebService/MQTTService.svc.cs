using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;


namespace MQTTWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class MQTTWebService : MQTTService
    {
        MqttClient client = new MqttClient("34.231.187.147");

        public void BroadcastLocation(string [] args)
        {
            if (args.Length == 3 && args[0] == "LOCATION")
            {

                client.Publish("Linux/locationUpdates", System.Text.Encoding.UTF8.GetBytes(args[1] + " " + args[2]));

            }
           
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
