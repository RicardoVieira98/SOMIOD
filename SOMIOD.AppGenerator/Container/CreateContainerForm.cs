﻿using SOMIOD.Library;
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
    public partial class CreateContainerForm : Form
    {
        private bool create;
        private string ApplicationParent;
        public CreateContainerForm(bool create,string applicationName, Library.Models.Container container)
        {
            this.create = create;
            InitializeComponent();
            button1.Text = create ? "Create" : "Edit";
            ApplicationParent = applicationName;
            if (!create)
            {
                FillFormForEdition(container);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpClient client = WebClient.CreateHttpClient();

            Library.Models.Container container = new Library.Models.Container()
            {
                Id = create ? 0 : Int32.Parse(textBox2.Text),
                Name = textBox1.Text,
                CreatedDate = dateTimePicker1.Value
            };

            string request = XmlHandler.GetContainerXml(container);

            HttpContent content = new StringContent(request, Encoding.UTF8, "application/xml");
            
            var response = create ? client.PostAsync(client.BaseAddress + "/" + ApplicationParent + "/insert", content) : client.PutAsync(client.BaseAddress + "/" + ApplicationParent + "/update", content);
            if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Shared.ShowError(response.Result);
                return;
            }

            var message = create ? "created" : "edited";
            Shared.ShowMessage("Container successfully " + message, "Container " + message, MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void FillFormForEdition(Library.Models.Container container)
        {
            textBox2.Text = container.Id.ToString();
            textBox1.Text = container.Name;
            dateTimePicker1.Value = container.CreatedDate;
        }
    }
}