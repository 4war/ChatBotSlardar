using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToe;

namespace ChatBot
{
    public partial class MainForm : Form
    {
        private ChatController _chatController;
        
        private readonly TableLayoutPanel _chatPanel = new TableLayoutPanel()
        {
            CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            Dock = DockStyle.Top,
            AutoScroll = true,
            VerticalScroll = { Enabled = true, Visible = false},
            HorizontalScroll = { Enabled = false, Visible = false}
        };

        private readonly TextBox _inputTextBox = new TextBox()
        {
            BackColor = Defaults.DarkBlueDark,
            ForeColor = Defaults.White,
            Height = 50,
            Dock = DockStyle.Bottom,
            Font = Defaults.Font
        };

        public MainForm()
        {
            InitializeComponent();
            _chatPanel.RowStyles.Add(new RowStyle() { Height = 300, SizeType = SizeType.Absolute });
            _chatPanel.Controls.Add(new Control(), 0, 0);
            _chatPanel.Width = this.Width;
            _chatPanel.Height = this.Height - _inputTextBox.Height - 60;
            _chatController = new ChatController(_chatPanel);

            this.Resize += Resized;

            _inputTextBox.KeyDown += KeyPressed;
            Controls.Add(_inputTextBox);
            Controls.Add(_chatPanel);
            _chatController.Write(false, "Я бот Слардара и отвечаю на все вопросы за него. Что хочешь узнать?");
        }

        private void Resized(object sender, EventArgs e)
        {
            var newHeight = Height - _inputTextBox.Height - 60;
            var newWidth = Width - 30;
            _chatController.Resize(newHeight, newWidth);
        }

 
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var textBox = sender as TextBox;
                if (textBox.Text.Length == 0)
                {
                    return;
                }
                _chatController.Write(true, textBox.Text);
                textBox.Text = string.Empty;
            }
        }
    }
}