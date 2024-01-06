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

namespace SOMIOD.AppGenerator
{
    public partial class CreateDataForm : Form
    {
        public string ApplicationName { get; set; }
        public string ContainerName { get; set; }
        public CreateDataForm()
        {
            InitializeComponent();
        }

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

            Library.Models.Data data = new Library.Models.Data()
            {
                Name = textBox1.Text,
                CreatedDate = dateTimePicker1.Value,
                
            };

            string request = XmlHandler.GetDataXml(data);
            HttpContent content = new StringContent(request, Encoding.UTF8, "application/xml");
            var response = client.PostAsync(client.BaseAddress + "/" + ApplicationName + "/" + ContainerName + "/", content);
            if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //show error
            }

            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
