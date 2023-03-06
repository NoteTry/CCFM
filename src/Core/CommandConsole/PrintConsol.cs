using System;

namespace CommandConsole
{
    public class PrintConsol
    {
        private int tableWidth;

        public PrintConsol(int Width)
        {
            tableWidth = Width;
        }

        public void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth) + "|");
        }

        public void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = " ";

            foreach (string column in columns)
            {
                row += AlignLeft(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        public string AlignLeft(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width).PadLeft(width);
            }
        }
    }
}
