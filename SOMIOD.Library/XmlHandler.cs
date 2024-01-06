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
            XmlDocument doc = new XmlDocument();

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
            XmlDocument doc = new XmlDocument();

            XmlElement applicationElement = doc.CreateElement("applications");

            foreach (var app in apps)
            {
                applicationElement.AppendChild(SetApplicationXmlSection(app, doc));
            }
            doc.AppendChild(applicationElement);

            return doc.OuterXml;
        }

        public static string FullContainerXml(Container container, List<Subscription> subscriptions, List<Data> datas)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement containerElement = SetContainerXmlSection(container, doc);

            foreach (var data in datas)
            {
                XmlElement dataElement = SetDataXmlSection(data, doc);
                containerElement.AppendChild(dataElement);
            }

            foreach (var subscription in subscriptions)
            {
                XmlElement subscriptionElement = SetSubscriptionXmlSection(subscription, doc);

                containerElement.AppendChild(subscriptionElement);
            }

            doc.AppendChild(containerElement);
            return doc.OuterXml;
        }

        public static string OnlyContainersXml(List<Container> containers)
        {
            XmlDocument doc = new XmlDocument();

            foreach (var container in containers)
            {
                SetContainerXmlSection(container, doc);   
            }
            return doc.OuterXml;
        }

        public static string OnlySubscriptionsXml(List<Subscription> subscriptions)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement subscriptionsElement = doc.CreateElement("subscriptions");
            foreach (var subscription in subscriptions)
            {
                subscriptionsElement.AppendChild(SetSubscriptionXmlSection(subscription, doc));
            }

            doc.AppendChild(subscriptionsElement);
            return doc.OuterXml;
        }

        public static string GetApplicationXml(Application application)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement element = SetApplicationXmlSection(application,doc);
            doc.AppendChild(element);
            return doc.OuterXml;
        }

        public static string GetSubscriptionXml(Subscription subscription)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement element = SetSubscriptionXmlSection(subscription, doc);
            doc.AppendChild(element);
            return doc.OuterXml;
        }

        public static string OnlyDataXml(List<Data> datas)
        {
            XmlDocument doc = new XmlDocument();

            foreach (var data in datas)
            {
                SetDataXmlSection(data, doc);
            }
            return doc.OuterXml;
        }
        public static string GetDataXml(Data data)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement element = SetDataXmlSection(data, doc);
            doc.AppendChild(element);
            return doc.OuterXml;
        }

        private static XmlElement SetApplicationXmlSection(Application app, XmlDocument doc)
        {
            XmlElement applicationElement = doc.CreateElement("application");
            applicationElement.SetAttribute("id",app?.Id.ToString());
            applicationElement.SetAttribute("name", app?.Name);
            applicationElement.SetAttribute("createddate", FormatDate(app?.CreatedDate));
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
            XmlElement subscriptionElement = doc.CreateElement("subscription");
            subscriptionElement.SetAttribute("name", subscription?.Name);
            subscriptionElement.SetAttribute("createdDate", subscription?.CreatedDate.ToString());
            subscriptionElement.SetAttribute("parent", subscription?.Parent.ToString());
            subscriptionElement.SetAttribute("event", subscription?.Event);
            subscriptionElement.SetAttribute("endpoint", subscription.Endpoint);

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

        private static string FormatDate(DateTime? datetime)
        {
            return datetime.Value.ToString("dd-MM-yyyy hh:mm:ss");
        }
    }
}
