using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TicTacToe;

namespace ChatBot
{
    public sealed class Message : Label
    {
        public const int Offset = 20;
        public const int VerticalPadding = 7;
        public bool FromSelf { get; }
        private GraphicsPath GetRoundPath(RectangleF Rect, int radius)
        {
            var r2 = radius / 2f;
            var GraphPath = new GraphicsPath();
            GraphPath.AddArc(Rect.X, Rect.Y, radius, radius, 180, 90);
            GraphPath.AddLine(Rect.X + r2, Rect.Y, Rect.Width - r2, Rect.Y);
            GraphPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y, radius, radius, 270, 90);
            GraphPath.AddLine(Rect.Width, Rect.Y + r2, Rect.Width, Rect.Height - r2);
            GraphPath.AddArc(Rect.X + Rect.Width - radius,
                Rect.Y + Rect.Height - radius, radius, radius, 0, 90);
            GraphPath.AddLine(Rect.Width - r2, Rect.Height, Rect.X + r2, Rect.Height);
            GraphPath.AddArc(Rect.X, Rect.Y + Rect.Height - radius, radius, radius, 90, 90);
            GraphPath.AddLine(Rect.X, Rect.Height - r2, Rect.X, Rect.Y + r2);
            GraphPath.CloseFigure();
            return GraphPath;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var Rect = new RectangleF(0, 0, Width, Height);
            using (GraphicsPath GraphPath = GetRoundPath(Rect, Offset))
            {
                Region = new Region(GraphPath);
                using (Pen pen = new Pen(BackColor, 7.75f))
                {
                    pen.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawPath(pen, GraphPath);
                }
            }
        }


        public Message(bool fromUser)
        {
            FromSelf = fromUser;
            BackColor = fromUser ? Defaults.DarkGreenLight : Defaults.DarkBlueLight;
            Anchor = fromUser ? AnchorStyles.Right : AnchorStyles.Left;
            ForeColor = Defaults.White;
            TextAlign = fromUser ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
            Padding = new Padding(Offset, 0, Offset, 0);
            Margin = new Padding(Offset, 0, Offset, 0);
            Font = Defaults.Font;
        }
    }
}