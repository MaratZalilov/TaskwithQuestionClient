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
        private TcpClient _tcpClient = new TcpClient();
        private Label _mainLabel;

        private string _answerAndQuestion = "";
        private string[] _question;
        private string _answer;

        private int _questionCount = 0;


        public void ConnectToServer()
        {
            _tcpClient.Connect("127.0.0.1", 8888);
            if (_tcpClient.Connected)
            {
                MessageBox.Show("Вы уже подключены");
                return;
            }
            else
            {
                MessageBox.Show("Подключено");

            }
        }

        public void GetQuestionsAndAnswer()
        {
            var stream = _tcpClient.GetStream();
            MessageBox.Show($"Подключение с {_tcpClient.Client.RemoteEndPoint} установлено");

            byte[] buffer = new byte[1024];
            int b = stream.Read(buffer, 0, 1024);
            _answerAndQuestion = Encoding.UTF8.GetString(buffer);
            _question = _answerAndQuestion.Split('\n');

            CreateButton(_questionCount);

            MessageBox.Show(_answerAndQuestion);

        }

        public void CreateButton(int num)
        {

            int W = 350, H = 250;
            if (_questionCount >= 5)
            {
                Button button = new Button();
                button.BackColor = Color.Green;
                button.Name = $"ButtonEnd";
                button.Text = "Отправить";
                button.Size = new Size(200, 50);
                button.Location = new Point(W, H += 50);
                button.Click += SendAnswer_Click;
                MainForm.ActiveForm.Controls.Add(button);
            }
            else
            {
                string[] str = _question[num].Split('|');
                MessageBox.Show(_question[num]);
                _mainLabel = new Label();
                _mainLabel.Text = str[0];
                _mainLabel.Location = new Point(W, H);
                MainForm.ActiveForm.Controls.Add(_mainLabel);

                for (int j = 1; j < str.Length - 1; j++)
                {
                    Button button = new Button();
                    button.BackColor = Color.Orange;
                    button.Name = $"Buttons" + $"{j}";
                    button.Text = $"{str[j]}";
                    button.Size = new Size(200, 50);
                    button.Location = new Point(W, H += 50);
                    button.Click += Buttons_Click;
                    MainForm.ActiveForm.Controls.Add(button);

                }
            }
        }

        private void Buttons_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            _answer += _mainLabel.Text + '|' + b.Text + '|';
            MainForm.ActiveForm.Controls.Clear();
            _questionCount++;
            CreateButton(_questionCount);
            //MessageBox.Show(b.Name);
        }

        public void SendAnswer_Click(object sender, EventArgs e)
        {
            var stream = _tcpClient.GetStream();
            stream.Write(Encoding.UTF8.GetBytes(_answer), 0, _answer.Length);
            MessageBox.Show("Отправлено");
        }

        public void Disconnected()
        {
            _tcpClient.Close();
        }
    }
}
