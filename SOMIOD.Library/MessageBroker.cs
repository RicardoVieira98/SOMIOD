using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SOMIOD.Library
{
    public class MessageBroker
    {
        [Obsolete]
        public static bool CallMessageBroker(string subscriptionName, string endpoint, Events subscriptionEvent)
        {
            MqttClient mcClient = new MqttClient(IPAddress.Parse(endpoint));
            string[] topics = { subscriptionName };
            string clientId = Guid.NewGuid().ToString(); //save this client id in appsettings
            mcClient.Connect(clientId);

            switch (subscriptionEvent)
            {
                case Events.Creation:
                    return Subscribe(mcClient, topics);
                case Events.Deletion:
                    return Unsubscribe(mcClient, topics);
                default:
                    return false;
            }
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
        }

        static bool Subscribe(MqttClient mcClient, string[] topics)
        {
            if (!mcClient.IsConnected)
            {
                return false;
            }

            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
            mcClient.Subscribe(topics, qosLevels);
            return true;
        }

        static bool Unsubscribe(MqttClient mcClient, string[] topics)
        {
            if (!mcClient.IsConnected)
            {
                return false;
            }

            mcClient.Unsubscribe(topics);
            mcClient.Disconnect();
            return true;
        }
    }
}
