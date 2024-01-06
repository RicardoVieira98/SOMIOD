using SOMIOD.Library;
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

namespace SOMIOD.AppGenerator
{
    public partial class CreateSubscriptionForm : Form
    {
        public string ApplicationName { get; set; }
        public string ContainerName { get; set; }
        public CreateSubscriptionForm(string applicationName, string containerName)
        {
            ApplicationName = applicationName;
            ContainerName = containerName;
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        //Create
        private void button1_Click(object sender, EventArgs e)
        {
            HttpClient client = WebClient.CreateHttpClient();
            string events;

            if (checkBox1.Checked && !checkBox2.Checked)
            {
                events = Library.Events.Creation.ToString();
            }
            else if (!checkBox1.Checked && checkBox2.Checked)
            {
                events = Library.Events.Deletion.ToString();
            }
            else if (checkBox1.Checked && checkBox2.Checked)
            {
                events = Library.Events.Both.ToString();
            }
            else
            {
                events = Library.Events.None.ToString();
            }

            Library.Models.Subscription subscription = new Library.Models.Subscription()
            {
                Name = textBox1.Text,
                CreatedDate = dateTimePicker1.Value,
                Endpoint = textBox2.Text,
                Event = (Events)Enum.Parse(typeof(Events),events)
            };

            string request = XmlHandler.GetSubscriptionXml(subscription);
            HttpContent content = new StringContent(request, Encoding.UTF8, "application/xml");
            var response = client.PostAsync(client.BaseAddress + "/" + ApplicationName + "/" + ContainerName + "/sub", content);
            if(response.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Shared.ShowError(response.Result);
                return;
            }

            Shared.ShowMessage("Subscription created successfully", "Subscription created", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
