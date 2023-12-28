using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace SOMIOD.AppGenerator
{
    public partial class SubscriptionForm : Form
    {
        private HttpClient httpClient;
        public SubscriptionForm()
        {
            InitializeComponent();

            httpClient = WebClient.CreateHttpClient();

            applications.DataSource = GetAllApplicationNames(httpClient);
            //containers.DataSource = GetAllContainersNames(httpClient);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //Get All Subscriptions
        private void button1_Click(object sender, EventArgs e)
        {
            allSubs.DataSource = GetAllSubscriptionNames(httpClient);
        }

        //Delete Subscription
        private void button4_Click(object sender, EventArgs e)
        {
            DeleteSubscription();
        }

        //Create Subscription
        private void button2_Click(object sender, EventArgs e)
        {
            CreateSubscriptionForm form = new CreateSubscriptionForm();
            form.ShowDialog();
        }

        private void applications_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var selected = applications.SelectedItems[0];
            //string APIPath = $"http://localhost:59707/somiod/{selected}";
            //HttpClient client = new HttpClient();
            //var response = client.GetAsync(APIPath);
            //if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            //{
            //    //show error message
            //}

            //var listaContainers = response.Result.Content;
            //containers.DataSource = listaContainers;
        }

        private List<string> GetAllApplicationNames(HttpClient client)
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

        private List<string> GetAllSubscriptionNames(HttpClient client)
        {
            WebClient.AddOperationTypeHeader(client, Library.Headers.Subscription);

            var response = client.GetAsync(client.BaseAddress + "application/container");
            if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            {
                //show error message
                return null;
            }

            return SetObjectNamesList(response,"//subscription");
        }

        private List<string> GetAllContainersNamesFromApplication(HttpClient client, string applicationName)
        {
            var response = client.GetAsync(client.BaseAddress + "/" + applicationName);
            if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            {
                //show error message
                return null;
            }

            return SetObjectNamesList(response, "//applications");
        }

        private List<string> GetAllSubscriptionNamesFromContainer(HttpClient client, string containerName)
        {
            var response = client.GetAsync(client.BaseAddress + "application");
            if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            {
                //show error message
                return null;
            }

            var result = XElement.Parse(response.Result.Content.ReadAsStringAsync().Result).Value;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(result);

            XmlNodeList appRootNode = xmlDocument.SelectNodes("//applications");

            List<string> applicationNames = new List<string>();
            foreach (XmlNode appNode in appRootNode)
            {
                foreach (XmlNode childNode in appNode.ChildNodes)
                {
                    applicationNames.Add(childNode.Attributes[0].Value);
                }
            }

            return applicationNames;
        }

        private void DeleteSubscription()
        {
            string applicationName = applications.SelectedItem.ToString();
            string containerName = containers.SelectedItem.ToString();
            string subscriptionName = subscriptions.SelectedItem.ToString();

            var response = httpClient.DeleteAsync(httpClient.BaseAddress + applicationName + "/" + containerName + "/sub/" + subscriptionName);
            if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //show error message
            }
        }

        private List<string> SetObjectNamesList(Task<HttpResponseMessage> response, string queryNode)
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
                    objectNames.Add(grandChildNode.Attributes[0].Value);
                }
            }

            return objectNames;
        }
    }
}
