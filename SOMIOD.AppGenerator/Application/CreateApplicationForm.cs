using SOMIOD.Library.Models;
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
using Application = SOMIOD.Library.Models.Application;

namespace SOMIOD.AppGenerator
{
    public partial class CreateApplicationForm : Form
    {
        private bool create;

        public CreateApplicationForm(bool create, Application application)
        {
            this.create = create;
            InitializeComponent();
            button1.Text = create ? "Create" : "Edit";
            if (!create)
            {
                FillFormForEdition(application);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpClient client = WebClient.CreateHttpClient();

            Application application = new Application()
            {
                Id = create ? 0 : Int32.Parse(textBox2.Text),
                Name = textBox1.Text,
                CreatedDate = dateTimePicker1.Value
            };

            string request = XmlHandler.GetApplicationXml(application);
            HttpContent content = new StringContent(request, Encoding.UTF8, "application/xml");

            var response = create ? client.PostAsync(client.BaseAddress, content) : client.PutAsync(client.BaseAddress, content);
            if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Shared.ShowError(response.Result);
                return;
            }
            var message = create ? "created" : "updated";
            Shared.ShowMessage("Application " + message + " successfully", "Application " + message, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void FillFormForEdition(Application application)
        {
            textBox2.Text = application?.Id.ToString();
            textBox1.Text = application?.Name;
            dateTimePicker1.Value = (application?.CreatedDate).Value;
        }
    }
}
