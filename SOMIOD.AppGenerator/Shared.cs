using SOMIOD.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.CodeDom;

namespace SOMIOD.AppGenerator
{
    public class Shared
    {
        public Shared() { }

        public static List<string> GetAllApplicationNames(HttpClient client)
        {
            WebClient.AddOperationTypeHeader(client, Library.Headers.Application);
            var response = client.GetAsync(client.BaseAddress);
            if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            {
                //show error message
                return null;
            }

            return SetObjectNamesList(response, "//applications");
        }

        public static Application GetApplication(HttpClient client, string applicationName)
        {
            var response = client.GetAsync(client.BaseAddress + applicationName);

            if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //show error message
            }

            Application app = new Application();
            return GetObject(response, app) as Application;
        }

        public static List<string> GetAllContainersNamesFromApplication(HttpClient client, string applicationName)
        {
            WebClient.AddOperationTypeHeader(client, Library.Headers.Container);
            var response = client.GetAsync(client.BaseAddress + "/" + applicationName);
            if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            {
                //show error message
                return null;
            }

            return SetObjectNamesList(response, "//applications");
        }

        public static List<string> SetObjectNamesList(Task<HttpResponseMessage> response, string queryNode)
        {
            var result = XElement.Parse(response.Result.Content.ReadAsStringAsync().Result).Value;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(result);

            XmlNodeList rootNode = xmlDocument.SelectNodes(queryNode);

            List<string> objectNames = new List<string>();
            foreach (XmlNode childNode in rootNode)
            {
                foreach (XmlNode grandChildNode in childNode.ChildNodes)
                {
                    objectNames.Add(grandChildNode.Attributes["name"].Value);
                }
            }

            return objectNames;
        }

        public static object GetObject(Task<HttpResponseMessage> response, object obj) 
        {
            var result = XElement.Parse(response.Result.Content.ReadAsStringAsync().Result).Value;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(result);

            if (obj.GetType() == typeof(Application))
            {
                Application app = (Application)obj;

                XmlNodeList rootNode = xmlDocument.SelectNodes("//applications");

                foreach (XmlNode childNode in rootNode)
                {
                    foreach (XmlNode grandChildNode in childNode.ChildNodes)
                    {
                        app.Id = Int32.Parse(grandChildNode.Attributes[0].Value);
                        app.Name = grandChildNode.Attributes[1].Value;
                        app.CreatedDate = DateTime.Parse(grandChildNode.Attributes[2].Value);
                    }
                }
                obj = app;
            }
            else if (obj.GetType() == typeof(Container))
            {
                Container con = (Container)obj;

                XmlNodeList rootNode = xmlDocument.SelectNodes("//containers");

                foreach (XmlNode childNode in rootNode)
                {
                    foreach (XmlNode grandChildNode in childNode.ChildNodes)
                    {
                        con.Name = grandChildNode.Attributes[0].Value;
                        con.CreatedDate = DateTime.Parse(grandChildNode.Attributes[1].Value);
                        con.Parent = Int32.Parse(grandChildNode.Attributes[2].Value);
                    }
                }
                obj = con;
            }
            else if(obj.GetType() == typeof(Subscription))
            {
                Subscription sub = (Subscription)obj;

                XmlNodeList rootNode = xmlDocument.SelectNodes("//subscriptions");

                foreach (XmlNode childNode in rootNode)
                {
                    foreach (XmlNode grandChildNode in childNode.ChildNodes)
                    {
                        sub.Name = grandChildNode.Attributes[0].Value;
                        sub.CreatedDate = DateTime.Parse(grandChildNode.Attributes[1].Value);
                        sub.Parent = Int32.Parse(grandChildNode.Attributes[2].Value);
                        sub.Event = grandChildNode.Attributes[3].Value;
                        sub.Endpoint = grandChildNode.Attributes[4].Value;
                    }
                }
                obj = sub;
            }
            else
            {
                throw new Exception();
            }

            return obj;
        }
    }
}