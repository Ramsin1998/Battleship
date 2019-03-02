﻿using System;
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

        public static void WriteInMiddle(this string text)
        {
            string newText = "";
            int left = 0;

            foreach (char c in text)
            {
                newText += c;

                left = (Console.BufferWidth - newText.Length) / 2;
                Console.SetCursorPosition(left, 30);

                Console.Write(newText);

                System.Threading.Thread.Sleep(1);
            }
        }

        public static void FancyWrite(this string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);

                System.Threading.Thread.Sleep(1);
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
