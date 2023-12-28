using Newtonsoft.Json;
using SOMIOD.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SOMIOD.AppGenerator
{
    public partial class Subscription : Form
    {
        public Subscription()
        {
            InitializeComponent();

            HttpClient client = WebClient.CreateHttpClient();
            WebClient.AddOperationTypeHeader(client, Library.Headers.Application);
            var response = client.GetAsync(client.BaseAddress);
            if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            {
                //show error message~
                return;
            }

            var result = XElement.Parse(response.Result.Content.ReadAsStringAsync().Result).Value;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(result);

            XmlNodeList appNodes = xmlDocument.SelectNodes("//applications");

            List<string> applicationNames = new List<string>();
            foreach (XmlNode appNode in appNodes)
            {
                foreach(XmlNode childNode in appNode.ChildNodes)
                {
                    Library.Models.Application application = new Library.Models.Application()
                    {
                        Name = childNode.Attributes[0].Value,
                        CreatedDate = DateTime.Parse(childNode.Attributes[1].Value)
                    };
                    applicationNames.Add(application.Name);
                }
            }

            applications.DataSource = applicationNames;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

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
    }
}
