using SOMIOD.AppGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebClient = SOMIOD.AppGenerator.WebClient;
using Container = SOMIOD.Library.Models.Container;
using SOMIOD.Library;

namespace SOMIOD.WebChatZ
{
    public partial class CreateChat : Form
    {
        private HttpClient client;
        private string UserName;

        public string ChatName;
        public CreateChat(string userName)
        {
            UserName = userName;
            client = WebClient.CreateHttpClient();

            InitializeComponent();
            comboBox1.DataSource = ApiCaller.GetAllApplicationNames(client);
        }

        private void CreateChat_Load(object sender, EventArgs e)
        {

        }

        //Create Chat
        private void button1_Click(object sender, EventArgs e)
        {
            ChatName = UserName + "-" + comboBox1.SelectedItem.ToString();

            Container newChat = new Container()
            {
                Name = ChatName,
                CreatedDate = DateTime.Now,
                Parent = ApiCaller.GetApplication(client,UserName).Id
            };

            string request = XmlHandler.GetContainerXml(newChat);
            HttpContent content = new StringContent(request, Encoding.UTF8, "application/xml");

            var response = client.PostAsync(client.BaseAddress + "/" + UserName + "/insert", content);
            if (response.Result.StatusCode != HttpStatusCode.OK)
            {
                Shared.ShowError(response.Result);
                return;
            }

            Shared.ShowMessage("Chat created successfully", "Chat created", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //CREATE SUBSCRIPTION! NAME, EVENT, ENDPOINT

            this.Close();
        }
    }
}
