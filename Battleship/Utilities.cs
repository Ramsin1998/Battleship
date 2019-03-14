using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Battleship
{
    /// <summary>
    /// Static calss containing useful methods.
    /// </summary>
    [Serializable]
    static class Utilities
    {
        /// <summary>
        /// Writes text with color attributes.
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="backgroundColor">Color of the background of the text.</param>
        /// <param name="foregroundColor">Color of the foreground of the text.</param>
        public static void WriteWithColor(this object text, ConsoleColor backgroundColor, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Draws a rectangle with style and color attributes.
        /// </summary>
        /// <param name="texture">Texture of the rectangle.</param>
        /// <param name="length">Length of the rectangle.</param>
        /// <param name="backgroundColor">Background color of the rectangle.</param>
        /// <param name="foregroundColor">Forground color of the rectangle.</param>
        public static void DrawRect(this string texture, int length, ConsoleColor backgroundColor, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            int posX = Console.CursorLeft;
            int posY = Console.CursorTop;

            string[] chunks = wholeChunks(texture, length).ToArray();

            for (int i = 0; i < chunks.Count(); i++)
            {
                Console.SetCursorPosition(posX, posY + i);
                chunks[i].WriteWithColor(backgroundColor, foregroundColor);
            }
        }

        /// <summary>
        /// Splits a string into chunks. Faster than SubString().
        /// </summary>
        /// <param name="str">The string to be split.</param>
        /// <param name="chunkSize">Length of each chuck.</param>
        /// <returns>Chunks of the string.</returns>
        private static IEnumerable<string> wholeChunks(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, chunkSize);
        }

        /// <summary>
        /// Clones an object with no referance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object to be cloned.</param>
        /// <returns>A new object of the same value.</returns>
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

        /// <summary>
        /// Gets the the Y/N user input.
        /// </summary>
        /// <returns>True if user pressed Y.</returns>
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

        /// <summary>
        /// Paues the program for a while.
        /// </summary>
        /// <param name="ms">Milliseconds to pause.</param>
        public static void Wait(int ms)
        {
            DateTime start = DateTime.Now;

            while (true)
                if ((DateTime.Now - start).TotalMilliseconds > ms)
                    return;
        }

        /// <summary>
        /// Writes text with a typewriter theme.
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="left">Left location of the text in the console window.</param>
        /// <param name="top">Top location of the text in the console window.</param>
        /// <param name="WaitTime">The amount of time to wait after each iteration.</param>
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

        /// <summary>
        /// Writes text in a fancy way.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public static void FancyWrite(this string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Wait(10);
            }
        }

        /// <summary>
        /// Writes a paragraph of text using FancyWrtie.
        /// </summary>
        /// <param name="text">Text to write, containing where a newline begins with '\n'</param>
        /// <param name="left">Left location of the paragraph in the console.</param>
        /// <param name="top">Top location of the paragraph in the console.</param>
        public static void WriteParagraph(this string text, int left, int top)
        {
            string[] textBlocks = text.Split('\n');

            for (int i = 0; i < textBlocks.Length; i++)
            {
                Console.SetCursorPosition(left, top + i);

                FancyWrite(textBlocks[i]);
            }
        }

        /// <summary>
        /// Gets text from the user.
        /// </summary>
        /// <returns>The text from the user.</returns>
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