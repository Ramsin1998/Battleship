using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

        static void secretScript()
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

        static void mainScript()
        {
            Console.WriteLine("cuntfacemcgee");
        }

        static void game()
        {
            Players[0].Opponent = Players[1];
            Players[1].Opponent = Players[0];

            while (true)
                foreach (var player in Players)
                {
                    player.StartTurn();

                    if (player.AttckBoard.Count(e => e == EBoard.Hit) == 17)
                        winningSequence(player);
                }
        }

        static void winningSequence(Player winner)
        {

        }

        static void Main(string[] args)
        {
            intro();

            Utilities.TypeWrite("Your name: ");

            string input = Utilities.GetText();
            if (input == "2")
                secretScript();

            else
                mainScript();
        }
    }
}