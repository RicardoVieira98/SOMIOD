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
        private string ApplicationName;
        private string ContainerName;
        public SubscriptionForm()
        {
            InitializeComponent();
            httpClient = WebClient.CreateHttpClient();
            applications.DataSource = Shared.GetAllApplicationNames(httpClient);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //Fill Containers with Application
        private void applications_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplicationName = applications.SelectedItem as string;

            containers.DataSource = Shared.GetAllContainersNamesFromApplication(httpClient, ApplicationName);
        }

        //Fill Subscriptions with Container
        private void containers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContainerName = containers.SelectedItem as string;
            subscriptions.DataSource = Shared.GetAllSubscriptionNamesFromContainer(httpClient, ApplicationName, ContainerName);
        }

        //Get All Subscriptions
        private void button1_Click(object sender, EventArgs e)
        {
            allSubs.DataSource = Shared.GetAllSubscriptionNames(httpClient);
        }

        //Create Subscription
        private void button2_Click(object sender, EventArgs e)
        {
            CreateSubscriptionForm form = new CreateSubscriptionForm(ApplicationName,ContainerName);
            form.ShowDialog();
        }

        //Delete Subscription
        private void button4_Click(object sender, EventArgs e)
        {
            var response = MessageBox.Show("Are you sure you want to delete this subscription?", "Delete Subscription", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (response == DialogResult.Yes)
            {
                var deleted = DeleteSubscription();
                if(deleted) MessageBox.Show("Subscription deleted successfully", "Delete Subscription", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Back button
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool DeleteSubscription()
        {
            string subscriptionName = subscriptions.SelectedItem.ToString();

            var res = Shared.DeleteSubscription(httpClient, ApplicationName, ContainerName, subscriptionName);
            if (res)
            {
                subscriptions.DataSource = ((List<string>)subscriptions.DataSource).FindAll(x => !String.Equals(x, subscriptionName));
            }
            return res;
        }
    }
}