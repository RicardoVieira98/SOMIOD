using SOMIOD.Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SOMIOD.Library
{
    public class XmlHandler
    {
        public static string FullApplicationXml(Application app, List<Container> containers, List<Subscription> subscriptions, List<Data> datas)
        {
            XmlDocument doc = CreateDocumentHeader();

            var applicationElement = SetApplicationXmlSection(app, doc);

            foreach (var container in containers)
            {
                XmlElement containerElement = SetContainerXmlSection(container,doc);
                foreach (var data in datas)
                {
                    XmlElement dataElement = SetDataXmlSection(data,doc);
                    containerElement.AppendChild(dataElement);
                }

                foreach (var subscription in subscriptions)
                {
                    XmlElement subscriptionElement = SetSubscriptionXmlSection(subscription,doc);
                    containerElement.AppendChild(subscriptionElement);
                }
                applicationElement.AppendChild(containerElement);
            }

            doc.AppendChild(applicationElement);
            return doc.OuterXml;
        }

        public static string OnlyApplicationsXml(List<Application> apps)
        {
            XmlDocument doc = CreateDocumentHeader();

            foreach (var app in apps)
            {
                SetApplicationXmlSection(app, doc);
            }

            return doc.OuterXml;
        }

        public static string OnlyContainersXml(List<Container> containers)
        {
            XmlDocument doc = CreateDocumentHeader();

            foreach (var container in containers)
            {
                SetContainerXmlSection(container, doc);   
            }
            return doc.OuterXml;
        }

        public static string FullContainerXml(Container container, List<Subscription> subscriptions, List<Data> datas)
        {
            XmlDocument doc = CreateDocumentHeader();

            XmlElement containerElement = SetContainerXmlSection(container,doc);

            foreach (var data in datas)
            {
                XmlElement dataElement = SetDataXmlSection(data,doc);
                containerElement.AppendChild(dataElement);
            }

            foreach (var subscription in subscriptions)
            {
                XmlElement subscriptionElement = SetSubscriptionXmlSection(subscription,doc);

                containerElement.AppendChild(subscriptionElement);
            }

            doc.AppendChild(containerElement);
            return doc.OuterXml;
        }

        public static string OnlySubscriptionsXml(List<Subscription> subscriptions)
        {
            XmlDocument doc = CreateDocumentHeader();

            foreach (var subscription in subscriptions)
            {
                SetSubscriptionXmlSection(subscription, doc);
            }
            return doc.OuterXml;
        }

        public static string OnlyDataXml(List<Data> datas)
        {
            XmlDocument doc = CreateDocumentHeader();

            foreach (var data in datas)
            {
                SetDataXmlSection(data, doc);
            }
            return doc.OuterXml;
        }

        private static XmlDocument CreateDocumentHeader()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            return doc;
        }

        private static XmlElement SetApplicationXmlSection(Application app, XmlDocument doc)
        {
            XmlElement applicationElement = doc.CreateElement("Application");
            applicationElement.SetAttribute("Name", app?.Name);
            applicationElement.SetAttribute("CreatedDate", app?.CreatedDate.ToString());
            doc.AppendChild(applicationElement);
            return applicationElement;
        }

        private static XmlElement SetContainerXmlSection(Container container, XmlDocument doc)
        {
            XmlElement containerElement = doc.CreateElement("Container");
            containerElement.SetAttribute("Name", container?.Name);
            containerElement.SetAttribute("CreatedDate", container?.CreatedDate.ToString());
            containerElement.SetAttribute("Parent", container?.Parent.ToString());
            doc.AppendChild(containerElement);
            return containerElement;
        }

        private static XmlElement SetSubscriptionXmlSection(Subscription subscription, XmlDocument doc)
        {
            XmlElement subscriptionElement = doc.CreateElement("Subscription");
            subscriptionElement.SetAttribute("Name", subscription?.Name);
            subscriptionElement.SetAttribute("CreatedDate", subscription?.CreatedDate.ToString());
            subscriptionElement.SetAttribute("Parent", subscription?.Parent.ToString());
            subscriptionElement.SetAttribute("Event", subscription?.Event);
            subscriptionElement.SetAttribute("Endpoint", subscription.Endpoint);
            doc.AppendChild(subscriptionElement);
            return subscriptionElement;
        }

        private static XmlElement SetDataXmlSection(Data data, XmlDocument doc)
        {
            XmlElement dataElement = doc.CreateElement("Data");
            dataElement.SetAttribute("Name", data?.Name);
            dataElement.SetAttribute("CreatedDate", data?.CreatedDate.ToString());
            dataElement.SetAttribute("Parent", data?.Parent.ToString());
            dataElement.SetAttribute("Content", data?.Content);
            doc.AppendChild(dataElement);
            return dataElement;
        }
    }
}
