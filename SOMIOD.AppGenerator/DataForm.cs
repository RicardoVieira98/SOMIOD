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
    public partial class DataForm : Form
    {
        private HttpClient httpClient;
        public DataForm()
        {
            InitializeComponent();

            httpClient = WebClient.CreateHttpClient();

            applications.DataSource = Shared.GetAllApplicationNames(httpClient);
            containers.DataSource = Shared.GetAllContainersNames(httpClient);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           

        }

        private void containers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedApplication = applications.SelectedItems[0].ToString();
            var selectedContainer = containers.SelectedItems[0].ToString();
            containers.DataSource = Shared.GetAllSubscriptionNamesFromContainer(httpClient, selectedApplication, selectedContainer);
        }

        private void applications_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedApplication = applications.SelectedItems[0].ToString();

            containers.DataSource = Shared.GetAllContainersNamesFromApplication(httpClient, selectedApplication);
        }

        private void subscriptions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void allDatas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateDataForm form = new CreateDataForm();
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var response = MessageBox.Show("Are you sure you want to delete this data?", "Delete Data", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (response == DialogResult.Yes)
            {
                var deleted = DeleteData();
                if (deleted) MessageBox.Show("Data deleted successfully", "Delete Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool DeleteData()
        {
            return false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
