using System;
using System.Windows.Forms;

namespace NetworkCheck.Service
{
    public class ColumnHeaderInfoAttribute : Attribute
    {
        public ColumnHeaderInfoAttribute(string text, int width, bool isShow = true, HorizontalAlignment textAlign = HorizontalAlignment.Left)
        {
            Text = text;
            Width = width;
            TextAlign = textAlign;
            IsShow = isShow;
        }

        public string Text { get; }
        public int Width { get; }

        public bool IsShow { get; }

        public HorizontalAlignment TextAlign { get; }
    }
}