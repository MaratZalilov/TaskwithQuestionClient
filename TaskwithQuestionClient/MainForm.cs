using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskwithQuestionClient
{
    public partial class MainForm : Form
    {
        ClientLogicProgram _clientLogicProgram = new ClientLogicProgram();
        public MainForm()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            _clientLogicProgram.ConnectToServer();
            Controls.Remove(ConnectButton);
            _clientLogicProgram.GetQuestionsAndAnswer();
        }
    }
}
