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
        private string ApplicationSelected;
        private string ContainerSelected;
        public ContainerForm()
        {
            InitializeComponent();
            client = WebClient.CreateHttpClient();
            applications.DataSource = ApiCaller.GetAllApplicationNames(client);
        }

        //Get Applications
        private void button6_Click(object sender, EventArgs e)
        {
            applications.DataSource = ApiCaller.GetAllApplicationNames(client);
        }

        //Get Containers
        private void button1_Click(object sender, EventArgs e)
        {
            containers.DataSource = ApiCaller.GetAllContainersNames(client);
        }

        //Fill Containers by Application
        private void applications_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplicationSelected = applications.SelectedItem as string;
            containers.DataSource = ApiCaller.GetAllContainersNamesFromApplication(client, ApplicationSelected);
        }

        private void containers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContainerSelected = containers.SelectedItem?.ToString();
        }

        //Create
        private void button2_Click(object sender, EventArgs e)
        {
            CreateContainerForm createContainerForm = new CreateContainerForm(true, ApplicationSelected, null);
            createContainerForm.ShowDialog();
            containers.DataSource = ApiCaller.GetAllContainersNamesFromApplication(client, ApplicationSelected);
        }

        //Update
        private void button3_Click(object sender, EventArgs e)
        {
            CreateContainerForm createContainerForm = new CreateContainerForm(false, ApplicationSelected, ApiCaller.GetContainer(client, ApplicationSelected, ContainerSelected));
            createContainerForm.ShowDialog();
            containers.DataSource = ApiCaller.GetAllContainersNamesFromApplication(client, ApplicationSelected);
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
            var res = ApiCaller.DeleteContainer(client, ApplicationSelected, ContainerSelected);
            if (res)
            {
                containers.DataSource = ((List<string>)containers.DataSource).FindAll(x => !String.Equals(x, ContainerSelected));
            }
            return res;
        }
    }
}
