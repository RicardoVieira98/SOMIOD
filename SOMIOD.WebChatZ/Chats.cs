using SOMIOD.AppGenerator;
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

namespace SOMIOD.WebChatZ
{
    public partial class Chats : Form
    {
        private string UserName;
        private HttpClient client;
        public Chats(string userName)
        {
            UserName = userName;
            client = WebClient.CreateHttpClient();
            InitializeComponent();
        }

        private void Chats_Load(object sender, EventArgs e)
        {
            var chats = ApiCaller.GetAllContainersNamesFromApplication(client, UserName);

            if(chats.Count is 0)
            {
                CreateChat createChat = new CreateChat(UserName);
                createChat.ShowDialog();
                chats.Add(createChat.ChatName);
            }
            comboBox1.DataSource = chats;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Open new window, send UserName and ChatName
            //retrieve all data ('messages') of the chat ('container')
            //show in certain way.
            //retrieve all data ('messages') of the other user HOW? : using Second part of Chat, retrieve the application then by reversing chat name order, retrieving the messages
            //wich message has a createddate, use that to sort the messages correctly.
        }
    }
}
