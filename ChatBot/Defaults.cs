using System.Collections.Generic;
using System.Drawing;

namespace TicTacToe
{
    public static class Defaults
    {
        public static readonly Color DarkBlueDark = ColorTranslator.FromHtml("#0e1722"); 
        public static readonly Color DarkBlueLight = ColorTranslator.FromHtml("#070e1a"); 
        public static readonly Color DarkGreenLight = ColorTranslator.FromHtml("#3a8a17"); 
        public static readonly Color White = ColorTranslator.FromHtml("#e1e3e6");

        public static readonly Font Font = new Font("Century Gothic", 16, FontStyle.Bold);
    }
}