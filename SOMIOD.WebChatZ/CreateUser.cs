using SOMIOD.AppGenerator;
using SOMIOD.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = SOMIOD.Library.Models.Application;

namespace SOMIOD.WebChatZ
{
    public partial class CreateUser : Form
    {
        HttpClient client;
        public CreateUser()
        {
            InitializeComponent();
            client = AppGenerator.WebClient.CreateHttpClient();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application application = new Application()
            {
                Name = textBox1.Text,
                CreatedDate = DateTime.Now
            };

            string request = XmlHandler.GetApplicationXml(application);
            HttpContent content = new StringContent(request, Encoding.UTF8, "application/xml");

            var response = client.PostAsync(client.BaseAddress, content);
            if (response.Result.StatusCode != HttpStatusCode.OK)
            {
                Shared.ShowError(response.Result);
                return;
            }

            Shared.ShowMessage("Username created successfully", "Username", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
