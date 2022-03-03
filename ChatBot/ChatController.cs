using System;
using System.Drawing;
using System.Windows.Forms;
using TicTacToe;

namespace ChatBot
{
    public class ChatController
    {
        private TableLayoutPanel _chatPanel;
        private IAiLogic _logic;

        private const int Height = 40;
        private const int MaxLettersInRow = 30;
        private static int _maxWidth;
        private const int LetterWidth = 15;
        private static readonly int HorizontalOffset = Message.Offset * 2 + 30;
        
        public ChatController(TableLayoutPanel chatPanel)
        {
            _logic = new SimpleAiLogic();
            _chatPanel = chatPanel;
            _maxWidth = (int)(chatPanel.Width * 0.6);
        }

        public void Resize(int newHeight, int newWidth)
        {
            _chatPanel.Height = newHeight;
            _chatPanel.Width = newWidth;
            _maxWidth = (int)(_chatPanel.Width * 0.6);
        }

        public void Write(bool fromUser, string message)
        {
            var height = (int)Math.Ceiling(message.Length / (double)MaxLettersInRow) * Height + 2 * Message.VerticalPadding;

            var rowStyle = new RowStyle()
            {
                Height = height,
                SizeType = SizeType.Absolute,
            };
            _chatPanel.RowStyles.Add(rowStyle);

            var messageControl = new Message(fromUser)
            {
                Width = Math.Max(Math.Min(LetterWidth * message.Length + HorizontalOffset, _maxWidth), HorizontalOffset),
                Text = message,
                Height = (int)(height * 0.7),
                ForeColor = Defaults.White
            };
            _chatPanel.Controls.Add(messageControl);
            
            _chatPanel.VerticalScroll.Value = _chatPanel.VerticalScroll.Maximum;
            if (fromUser)
            {
                WaitForAiToAnswer(message);
            }
        }

        private void WaitForAiToAnswer(string userMessage)
        {
            var aiMessage = _logic.DecideMessage(userMessage.ToLower());
            Write(false, aiMessage);
        }
    }
}