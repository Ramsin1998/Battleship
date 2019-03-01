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
        static public List<Player> Players = new List<Player>();

        static string getText()
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
            Console.Clear();

            List<Player> players = new List<Player>();

            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(104, 30);
                Console.Write($"Player {i+1}: ");
                Players.Add(new Player(getText()));
                Console.Clear();
            }

            game();
        }

        static void mainScript()
        {
            Console.WriteLine("cuntfacemcgee");
        }

        static void game()
        {
            foreach(var player in Players)
            {
                Console.SetCursorPosition(90, 30);

                Utilities.WriteInMiddle($"{player.Name}, would you like your ships to be placed randomly? Y/N");

                if (Utilities.YesOrNo())
                    player.PersonalBoard = Board.PlaceShipsRandomly();

                else
                    player.PersonalBoard = Board.PlaceShipsByPlayerInput();

                Console.Clear();
            }
        }

        static void Main(string[] args)
        {
            //intro();

            //Console.SetCursorPosition(104, 30);
            //Console.Write("Your name: ");

            //string input = getText();
            //if (input == "2")
            //    secretScript();

            //else
            //    mainScript();



            Console.ReadLine();
        }


    }
}