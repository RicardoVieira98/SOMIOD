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
    public partial class Main : Form
    {
        private HttpClient client;
        public Main()
        {
            client = WebClient.CreateHttpClient();
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = ApiCaller.GetAllApplicationNames(client);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            
            string selectedPersonName = comboBox1.SelectedItem.ToString();
            Chats chats = new Chats(selectedPersonName);
            chats.ShowDialog();
        }
    }
}
