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
        MainForm mainForm;
        private TcpClient _tcpClient = new TcpClient();
        private Label _mainLabel;

        private string _answerAndQuestion = "";
        private string _answer;

        private string[] _question;
        private string[] _curentIssue;

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

            CreateButtonAndLabel(_questionCount);

            MessageBox.Show(_answerAndQuestion);

        }

        public void CreateButtonAndLabel(int num)
        {
            _curentIssue = _question[_questionCount].Split('|');
            int W = 365, H = 200;
            if (_questionCount >= _curentIssue.Length)
            {
                Button buttonEnd = new Button
                {
                    BackColor = Color.Green,
                    Name = $"ButtonEnd",
                    Text = "Отправить",
                    Size = new Size(200, 50),
                    Location = new Point(W, H + 200)
                };
                buttonEnd.Click += SendAnswer_Click;
                MainForm.ActiveForm.Controls.Add(buttonEnd);
                
            }
            else
            {
                
                MessageBox.Show(_question[num]);
                _mainLabel = new Label();
                _mainLabel.Text = _curentIssue[0];
                _mainLabel.Location = new Point(200, 70);
                _mainLabel.Font = new Font("Microsoft Sans Serif", 15);
                MainForm.ActiveForm.Controls.Add(_mainLabel);
                
                _questionCount++;

                for (int j = 1; j < _curentIssue.Length - 1; j++)
                {
                    Button button = new Button
                    {
                        BackColor = Color.Orange,
                        Name = $"Buttons" + $"{j}",
                        Text = $"{_curentIssue[j]}",
                        Size = new Size(200, 50),
                        Location = new Point(W, H += 50),
                        Anchor = AnchorStyles.None
                    };
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
            CreateButtonAndLabel(_questionCount);
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
