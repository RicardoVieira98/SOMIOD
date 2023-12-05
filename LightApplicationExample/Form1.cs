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

namespace LightApplicationExample
{
    public partial class Form1 : Form
    {
        private bool lightState = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Luz";
            if(checkBox1.Checked)
            {
                lightState = true;
                label1.Text = label1.Text + " - Ligado";
            }
            else
            {
                lightState = false;
                label1.Text = label1.Text + " - Desligado";
            }


            //Criar o XML template. APP, DATA
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);

            XmlElement application = doc.CreateElement("Application");
            application.SetAttribute("Name","Lighting");

            XmlElement container = doc.CreateElement("Container");
            container.SetAttribute("Name", "LightBulb");

            XmlElement data = doc.CreateElement("Data");
            data.InnerText = lightState.ToString();
            
            container.AppendChild(data);
            application.AppendChild(container);
            doc.AppendChild(application);
            string xmlOutput = doc.OuterXml;


            //Fazer pedido para API para criar a APlication
            string APIPath = "http://localhost:59707/somiod";
            HttpClient client = new HttpClient();
            var httpContent = new StringContent(xmlOutput, Encoding.UTF8, "text/xml");
            
            client.PostAsync(APIPath, httpContent);
        }
    }
}
