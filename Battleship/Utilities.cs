using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Battleship
{
    [Serializable]
    static class Utilities
    {
        public static void WriteWithColor(this object text, ConsoleColor backgroundColor, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void DrawRect(this string style, int length, ConsoleColor backgroundColor, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            int posX = Console.CursorLeft;
            int posY = Console.CursorTop;

            string[] chunks = wholeChunks(style, length).ToArray();

            for (int i = 0; i < chunks.Count(); i++)
            {
                Console.SetCursorPosition(posX, posY + i);
                chunks[i].WriteWithColor(backgroundColor, foregroundColor);
            }
        }

        private static IEnumerable<string> wholeChunks(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, chunkSize);
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static bool YesOrNo()
        {
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Y:
                        return true;

                    case ConsoleKey.N:
                        return false;

                    default:
                        continue;
                }
            }
        }

        public static void Wait(int ms)
        {
            DateTime start = DateTime.Now;

            while (true)
                if ((DateTime.Now - start).TotalMilliseconds > ms)
                    return;
        }

        public static void TypeWrite(this string text, int left = 117, int top = 33, int WaitTime = 10)
        {
            int newLeft = 0;

            for (int i = 0; i < text.Length; i++)
            {
                newLeft = left - i / 2;

                if (newLeft < 0)
                    newLeft = 0;

                Console.SetCursorPosition(newLeft, top);
                Console.Write(text.Substring(0, i + 1));

                Wait(WaitTime);
            }
        }

        public static void FancyWrite(this string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Wait(10);
            }
        }

        public static void WriteParagraph(this string text, int left, int top)
        {
            string[] textBlocks = text.Split('\n');

            for (int i = 0; i < textBlocks.Length; i++)
            {
                Console.SetCursorPosition(left, top + i);

                FancyWrite(textBlocks[i]);
            }
        }

        public static string GetText()
        {
            string text;
            int y = Console.CursorTop;
            int x = Console.CursorLeft;

            while (true)
            {
                Console.SetCursorPosition(x, y);
                text = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(text))
                    break;
            }

            return text;
        }
    }
}
