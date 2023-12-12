using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CreateApplicationExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        //Create Application
        private void button1_Click(object sender, EventArgs e)
        {
            //Criar o XML template. APP, DATA
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);

            XmlElement application = doc.CreateElement("Application");
            application.SetAttribute("Name", "Lighting");

            XmlElement container = doc.CreateElement("Container");
            container.SetAttribute("Name", "LightBulb");

            XmlElement data = doc.CreateElement("Data");

            container.AppendChild(data);
            application.AppendChild(container);
            doc.AppendChild(application);
            string xmlOutput = doc.OuterXml;


            //Fazer pedido para API para criar a Application
            string APIPath = "http://localhost:59707/somiod/create";
            HttpClient client = new HttpClient();
            var httpContent = new StringContent(xmlOutput, Encoding.UTF8, "application/xml");
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            client.PostAsync(APIPath, httpContent);
        }

        //Create Container
        private void button2_Click(object sender, EventArgs e)
        {

        }

    }
}
