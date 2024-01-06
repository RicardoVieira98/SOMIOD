using SOMIOD.AppGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace SOMIOD.WebChatZ
{
    public partial class ChatRoom : Form
    {
        private string Username;
        private string Chatname;
        public ChatRoom(string username, string chat)
        {
            InitializeComponent();
            Username = "Ricardo";
            Chatname = chat;
            LoadMessages(username, chat);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadMessages(string username, string chat)
        {
            int x = 10;
            int y = 10;
            
            //var messages = ApiCaller.GetAllDatasByContainer();
            var messages = new List<KeyValuePair<string, string>>();
            messages.Add(new KeyValuePair<string,string>("Ricardo", "Ola Bruna"));
            messages.Add(new KeyValuePair<string, string>("Bruna", "Ola Ricardo"));
            messages.Add(new KeyValuePair<string, string>("Ricardo", "Como estás"));
            messages.Add(new KeyValuePair<string, string>("Bruna", "Bem e tu?"));
            messages.Add(new KeyValuePair<string, string>("Ricardo", " Bem obrigadoBemm obrigadoBem obrigadoBem obem obrigadoBem obrigado"));
            foreach(var message in messages)
            {
                Panel panel = new Panel();
                panel.Height = 30;
                panel.Width = Right;
                panel.Location = new Point(x,y);
                panel.AutoSize = true;

                Label usernameLabel = new Label();
                usernameLabel.Height = 15;
                usernameLabel.Text = message.Key;
                usernameLabel.ForeColor = IsCurrentUserMessage(message.Key) ? Color.Chocolate : Color.Orange;


                Label messageLabel = new Label();
                messageLabel.Height = 15;
                messageLabel.Text = message.Value;
                messageLabel.Top = 15;
                messageLabel.Width = Right - 20;
                messageLabel.AutoSize = true;

                panel.Controls.Add(usernameLabel);
                panel.Controls.Add(messageLabel);
                panel1.Controls.Add(panel);

                y += messages.Count * 10;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newMessage = textBox1.Text;
            //Insert new data
            LoadMessages(Username, Chatname);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadMessages(Username, Chatname);
        }

        private bool IsCurrentUserMessage(string currentUserMessage)
        {
            return string.Equals(currentUserMessage, Username);
        }
    }
}
