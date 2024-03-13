using System;
using System.Collections.Generic;
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
    }
}
