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
using System.Xml;

namespace SOMIOD.AppGenerator
{
    public partial class Subscription : Form
    {
        public Subscription()
        {
            InitializeComponent();

            //Fazer pedido para API para criar a Application
            //string APIPath = "http://localhost:59707/somiod/";
            HttpClient client = new HttpClient();
            //var response = client.GetAsync(APIPath);
            //if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            //{
            //    //show error message
            //}

            //var listaApps = response.Result.Content;
            //applications.DataSource = listaApps;

            var APIPath = "http://localhost:59707/somiod/container";
            var response = client.GetAsync(APIPath);
            if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            {
                //show error message
            }

            var listaSubsAll = response.Result.Content;
            applications.DataSource = listaSubsAll;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void applications_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = applications.SelectedItems[0];
            string APIPath = $"http://localhost:59707/somiod/{selected}";
            HttpClient client = new HttpClient();
            var response = client.GetAsync(APIPath);
            if (!(response.Result.StatusCode == System.Net.HttpStatusCode.OK))
            {
                //show error message
            }

            var listaContainers = response.Result.Content;
            containers.DataSource = listaContainers;
        }
    }
}
