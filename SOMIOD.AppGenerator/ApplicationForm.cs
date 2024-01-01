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
    public partial class ApplicationForm : Form
    {
        private HttpClient client = WebClient.CreateHttpClient();
        public ApplicationForm()
        {
            InitializeComponent();
        }

        //Get
        private void button1_Click(object sender, EventArgs e)
        {
            applications.DataSource = Shared.GetAllApplicationNames(client);
        }

        //Create
        private void button4_Click(object sender, EventArgs e)
        {
            CreateApplicationForm createApplicationForm = new CreateApplicationForm(true,null);
            createApplicationForm.ShowDialog();
        }

        //Update
        private void button2_Click(object sender, EventArgs e)
        {
            var applicationName = applications.SelectedItem?.ToString();
            CreateApplicationForm createApplicationForm = new CreateApplicationForm(false, Shared.GetApplication(client,applicationName));
            createApplicationForm.ShowDialog();
        }

        //Delete
        private void button3_Click(object sender, EventArgs e)
        {
            DeleteApplication();
        }

        private void DeleteApplication()
        {
            string applicationName = applications.SelectedItem?.ToString();
            var response = client.DeleteAsync(client.BaseAddress + applicationName);

            if(response.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //show error message
            }

            applications.DataSource = ((List<string>)applications.DataSource).FindAll(x => !String.Equals(x, applicationName));
            //show success message
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
