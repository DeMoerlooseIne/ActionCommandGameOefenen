using System;
using System.Collections.Generic;
using System.Linq;

namespace ActionCommandGame.Ui.ConsoleApp.ConsoleWriters
{
    public static class ConsoleBlockWriter
    {
        public static void Write(string text, int padding = 1, ConsoleColor color = ConsoleColor.White)
        {
            Write(new List<string> { text }, padding, color);
        }

        public static void Write(IList<string> textLines, int padding, ConsoleColor color)
        {
            var maxTextLength = textLines.Max(t => t.Length);

            Console.ForegroundColor = color;
            //Top Line
            WriteTopLine(maxTextLength, padding);

            //Empty Line
            WriteEmptyLine(maxTextLength, padding);

            foreach (var text in textLines)
            {
                //Text Line
                WriteTextLine(text, maxTextLength, padding);
            }

            //Empty Line
            WriteEmptyLine(maxTextLength, padding);

            //Bottom Line
            WriteBottomLine(maxTextLength, padding);

            Console.ResetColor();
        }

        private static void WriteTopLine(int contentLength, int padding)
        {
            Console.Write("\t");
            Console.Write((char)0x2554);
            for (int i = 0; i < contentLength + padding * 2; i++)
            {
                Console.Write((char)0x2550);
            }
            Console.Write((char)0x2557);
            Console.WriteLine();
        }

        private static void WriteEmptyLine(int contentLength, int padding)
        {
            Console.Write("\t");
            Console.Write((char)0x2551);
            for (int i = 0; i < contentLength + padding * 2; i++)
            {
                Console.Write(" ");
            }
            Console.Write((char)0x2551);
            Console.WriteLine();
        }

        private static void WriteTextLine(string text, int contentLength, int padding)
        {
            Console.Write("\t");
            Console.Write((char)0x2551);

            var maxWidth = contentLength + padding * 2;
            var linePadding = (maxWidth - text.Length) / 2;


            
            for (int i = 0; i < linePadding; i++)
            {
                Console.Write(" ");
            }

            Console.Write(text);

            for (int i = 0; i < linePadding; i++)
            {
                Console.Write(" ");
            }

            //Fix for uneven text length
            if (((linePadding * 2) + text.Length) < maxWidth)
            {
                Console.Write(" ");
            }

            Console.Write((char)0x2551);
            Console.WriteLine();
        }

        private static void WriteBottomLine(int contentLength, int padding)
        {
            Console.Write("\t");
            Console.Write((char)0x255A);
            for (int i = 0; i < contentLength + padding * 2; i++)
            {
                Console.Write((char)0x2550);
            }
            Console.Write((char)0x255D);
            Console.WriteLine();
        }
    }
}
