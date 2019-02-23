using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Battleship
{
    class Program
    {
        static string getText()
        {
            string text;
            int y = Console.CursorTop;
            int x = Console.CursorLeft;

            while (true)
            {
                Console.SetCursorPosition(x, y);
                text = Console.ReadLine();
                if (!String.IsNullOrWhiteSpace(text))
                    break;
            }

            return text;
        }

        static void intro()
        {
            Console.WriteLine("psst! ...press Alt+Enter!");

            while (true)
                if (Console.WindowHeight == Console.LargestWindowHeight)
                    break;

            Console.Clear();
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.CursorVisible = false;
            Random rng = new Random();
            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (timer.ElapsedMilliseconds < 2000)
            {
                Console.ForegroundColor = (ConsoleColor)rng.Next(1, 16);
                Console.WriteLine(Properties.Resources.Intro);
                System.Threading.Thread.Sleep(250);
                Console.ResetColor();
                Console.Clear();
            }
        }

        static void secretScript()
        {
            Console.SetCursorPosition(5,5);
            Console.Write("Player 1: ");
            Player player1 = new Player(getText());
            Console.Clear();

            Console.Write("Player 2: ");
            Player player2 = new Player(getText());

        }

        static void mainScript()
        {
            Console.WriteLine("cuntfacemcgee");
        }

        static void Main(string[] args)
        {
            intro();
            //Console.SetCursorPosition(104, 30);
            //Console.Write("Your name: ");

            //string input = getText();
            //if (input == "2")
            //    secretScript();

            //else
            //    mainScript();

            Player player = new Player("");

            while (true)
            {
                player.PersonalBoard = Board.PlaceShipsRandomly();

                player.PersonalBoard.Print(5, 5);

                Console.ReadLine();
            }

            //player.PersonalBoard.PlaceShipsByPlayerInput();

            Console.ReadLine();
        }
    }
}