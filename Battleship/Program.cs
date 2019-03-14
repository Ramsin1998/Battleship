using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Battleship
{
    static class Program
    {
        public static List<Player> Players = new List<Player>();

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

        static void script()
        {
            Console.Clear();

            for (int i = 0; i < 2; i++)
            {
                Utilities.TypeWrite($"Player {i + 1}: ");
                Players.Add(new Player(Utilities.GetText()));
                Console.Clear();
            }

            game();
        }

        static void game()
        {
            Players[0].Opponent = Players[1];
            Players[1].Opponent = Players[0];

            while (true)
                foreach (var player in Players)
                    player.StartTurn();
        }

        public static void WinningSequence(Player winner)
        {
            Console.Clear();

            Utilities.TypeWrite($"!!! {winner.Name} IS THE WINNER !!!");

            Utilities.Wait(1500);

            Random rng = new Random();

            for (int i = 0; i < 1000; i++)
            {
                Console.BackgroundColor = (ConsoleColor)rng.Next(0, 16);
                Console.ForegroundColor = (ConsoleColor)rng.Next(0, 16);

                Utilities.TypeWrite($"!!! {winner.Name} IS THE WINNER !!!", rng.Next(0, Console.BufferWidth - winner.Name.Length), rng.Next(0, Console.BufferHeight - winner.Name.Length), 1);
            }
        }

        static void Main(string[] args)
        {
            intro();

            script();

            WinningSequence(Players[0]);

            //secretScript();
        }
    }
}