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

namespace SOMIOD.AppGenerator.Container
{
    public partial class ContainerForm : Form
    {
        private HttpClient client;
        public ContainerForm()
        {
            InitializeComponent();
            client = WebClient.CreateHttpClient();
            applications.DataSource = Shared.GetAllApplicationNames(client);
        }

        //Get Applications
        private void button6_Click(object sender, EventArgs e)
        {
            applications.DataSource = Shared.GetAllApplicationNames(client);
        }

        //Get Containers
        private void button1_Click(object sender, EventArgs e)
        {
            containers.DataSource = Shared.GetAllContainersNames(client);
        }

        //Fill Containers by Application
        private void applications_SelectedIndexChanged(object sender, EventArgs e)
        {
            string applicationName = applications.SelectedItem as string;
            containers.DataSource = Shared.GetAllContainersNamesFromApplication(client, applicationName);
        }

        //Create
        private void button2_Click(object sender, EventArgs e)
        {
            CreateContainerForm createContainerForm = new CreateContainerForm(true,null);
            createContainerForm.ShowDialog();
        }

        //Update
        private void button3_Click(object sender, EventArgs e)
        {
            string applicationName = applications.SelectedItem.ToString();
            string containerName = containers.SelectedItem.ToString();
            CreateContainerForm createContainerForm = new CreateContainerForm(false,Shared.GetContainer(client,applicationName,containerName));
            createContainerForm.ShowDialog();
        }

        //Delete
        private void button4_Click(object sender, EventArgs e)
        {
            var response = MessageBox.Show("Are you sure you want to delete this container?", "Delete Container", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (response == DialogResult.Yes)
            {
                var deleted = DeleteContainer();
                if (deleted) MessageBox.Show("Container deleted successfully", "Delete Container", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Go back
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool DeleteContainer()
        {
            string containerName = containers.SelectedItem?.ToString();
            string applicationName = applications.SelectedItem?.ToString();
            var res = Shared.DeleteContainer(client, applicationName, containerName);
            if (res)
            {
                containers.DataSource = ((List<string>)containers.DataSource).FindAll(x => !String.Equals(x, containerName));
            }
            return res;
        }
    }
}
