using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace SOMIOD.AppGenerator
{
    public partial class SubscriptionForm : Form
    {
        private HttpClient httpClient;
        public SubscriptionForm()
        {
            InitializeComponent();

            httpClient = WebClient.CreateHttpClient();

            applications.DataSource = Shared.GetAllApplicationNames(httpClient);
            //containers.DataSource = Shared.GetAllContainersNames(httpClient);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //Get All Subscriptions
        private void button1_Click(object sender, EventArgs e)
        {
            allSubs.DataSource = GetAllSubscriptionNames(httpClient);
        }

        //Create Subscription
        private void button2_Click(object sender, EventArgs e)
        {
            CreateSubscriptionForm form = new CreateSubscriptionForm();
            form.ShowDialog();
        }

        //Delete Subscription
        private void button4_Click(object sender, EventArgs e)
        {
            var response = MessageBox.Show("Are you sure you want to delete this subscription?", "Delete Subscription", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if(response == DialogResult.Yes)
            {
                DeleteSubscription();
                MessageBox.Show("Subscription deleted successfully", "Delete Subscription", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Back button
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void applications_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedApplication = applications.SelectedItems[0].ToString();

            containers.DataSource = Shared.GetAllContainersNamesFromApplication(httpClient, selectedApplication);
        }

        private void containers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedApplication = applications.SelectedItems[0].ToString();
            var selectedContainer = containers.SelectedItems[0].ToString();
            containers.DataSource = GetAllSubscriptionNamesFromContainer(httpClient, selectedApplication, selectedContainer);
        }

        private List<string> GetAllSubscriptionNames(HttpClient client)
        {
            WebClient.AddOperationTypeHeader(client, Library.Headers.Subscription);
            var response = client.GetAsync(client.BaseAddress + "application/container");
            if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            {
                //show error message
                return null;
            }

            return Shared.SetObjectNamesList(response,"//subscriptions");
        }

        private List<string> GetAllSubscriptionNamesFromContainer(HttpClient client, string applicationName, string containerName)
        {
            WebClient.AddOperationTypeHeader(client, Library.Headers.Subscription);
            var response = client.GetAsync(client.BaseAddress + applicationName + "/" + containerName );
            if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            {
                //show error message
                return null;
            }

            return Shared.SetObjectNamesList(response,"//subscriptions");
        }

        private void DeleteSubscription()
        {
            string applicationName = applications.SelectedItem.ToString();
            string containerName = containers.SelectedItem.ToString();
            string subscriptionName = subscriptions.SelectedItem.ToString();

            Shared.DeleteSubscription(httpClient, applicationName, containerName, subscriptionName);
            subscriptions.DataSource = ((List<string>)subscriptions.DataSource).FindAll(x => !String.Equals(x, subscriptionName));
        }
    }
}
