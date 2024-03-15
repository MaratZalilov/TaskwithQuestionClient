using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskwithQuestionClient
{
    internal class ClientLogicProgram
    {
        TcpClient tcpClient = new TcpClient();
        private string _nameClient = null, _surNameClient = null;

        public void ConnectToServer()
        {
            tcpClient.Connect("127.0.0.1", 8888);
            if (tcpClient.Connected)
            {
                MessageBox.Show("Подключено");
            }
            else
            {
                MessageBox.Show("Подключение не удалось...");
                return;
            }
        }

        public void GetQuestionsAndAnswer()
        {
            var stream = tcpClient.GetStream();
            MessageBox.Show($"Подключение с {tcpClient.Client.RemoteEndPoint} установлено");
            while (true)
            {
                byte[] buffer = new byte[1024];
                int b = stream.Read(buffer, 0, 1024);

                MessageBox.Show(Encoding.UTF8.GetString(buffer));
            }
        }

        public void CreateButton()
        {

            int W = 350, H = 250;
            for (int j = 0; j < 3; j++)
            {
                Button button = new Button();
                button.BackColor = Color.Orange;
                button.Name = $"Buttons" + $"{j}";
                button.Text = $"{j}";
                button.Size = new Size(200, 50);
                button.Location = new Point(W, H += 50);
                button.Click += Buttons_Click;
                MainForm.ActiveForm.Controls.Add(button);

            }
        }

        private void Buttons_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            MainForm.ActiveForm.Controls.Clear();
            CreateButton();
            MessageBox.Show(b.Name);
        }

    }
}
