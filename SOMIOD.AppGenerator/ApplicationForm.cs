using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Forms;

namespace SOMIOD.AppGenerator
{
    public partial class ApplicationForm : Form
    {
        private HttpClient client;
        public ApplicationForm()
        {
            InitializeComponent();
            client = WebClient.CreateHttpClient();
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
            var response = MessageBox.Show("Are you sure you want to delete this application?", "Delete Application", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (response == DialogResult.Yes)
            {
                DeleteApplication();
                MessageBox.Show("Application deleted successfully", "Delete Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Go Back
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteApplication()
        {
            string applicationName = applications.SelectedItem?.ToString();
            Shared.DeleteApplication(client,applicationName);
            applications.DataSource = ((List<string>)applications.DataSource).FindAll(x => !String.Equals(x, applicationName));
        }
    }
}
