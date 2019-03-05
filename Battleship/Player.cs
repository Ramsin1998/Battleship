﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Player 
    {
        private int cursorPosX = 0;
        private int cursorPosY = 0;

        public int CursorPosX
        {
            get { return cursorPosX; }

            set
            {
                if (value > 9)
                    cursorPosX = 9;

                else if (value < 0)
                    cursorPosX = 0;

                else
                    cursorPosX = value;
            }
        }

        public int CursorPosY
        {
            get { return cursorPosY; }

            set
            {
                if (value > 9)
                    cursorPosY = 9;

                else if (value < 0)
                    cursorPosY = 0;

                else
                    cursorPosY = value;
            }
        }

        public string Name;
        public Player(string name)
        {
            Name = name;

            Utilities.TypeWrite($"Player: {Name}, would you like your ships to be placed randomly? Y/N");

            if (Utilities.YesOrNo())
                PersonalBoard = Board.PlaceShipsRandomly();

            else
                PersonalBoard = Board.PlaceShipsByPlayerInput();

            Console.Clear();
        }

        public void StartTurn()
        {
            Utilities.TypeWrite($"It's player {Name}'s turn", 117, 4);

            string cursor = "  ||  " +
                            "==}{==" +
                            "  ||  ";

            PersonalBoard.Print(15, 17);

            Console.OutputEncoding = Encoding.Unicode;
            Utilities.WriteParagraph(
            "↑: Up\n" +
            "↓: Down\n" +
            "→: Right\n" +
            "←: Left\n" +
            "ENTER: Attack", 190, 53);

            Console.SetCursorPosition(42, 11);
            Utilities.FancyWrite("Your board");

            Console.SetCursorPosition(185, 11);
            Utilities.FancyWrite("Your attack board");

            while (true)
            {
                AttckBoard.Print(160, 17);

                Console.SetCursorPosition(163 + cursorPosX * 6, 19 + cursorPosY * 3);

                cursor.DrawRect(6, ConsoleColor.Gray, ConsoleColor.Black);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        CursorPosY--;
                        break;

                    case ConsoleKey.DownArrow:
                        CursorPosY++;
                        break;

                    case ConsoleKey.RightArrow:
                        CursorPosX++;
                        break;

                    case ConsoleKey.LeftArrow:
                        CursorPosX--;
                        break;

                    case ConsoleKey.Enter:

                        if (AttckBoard[cursorPosX, cursorPosY] == EBoard.Miss)
                            continue;

                        switch (Opponent.PersonalBoard[cursorPosX, cursorPosY])
                        {
                            case EBoard.Ship:

                                Utilities.TypeWrite("Hit!");

                                AttckBoard[cursorPosX, cursorPosY] = EBoard.Hit;

                                int newCursorPosX = cursorPosX - 1;
                                int newCursorPosY = cursorPosY - 1;

                                for (int y = 0; y < 2; y++)
                                    for (int x = 0; x < 2; x++)
                                        try { AttckBoard[newCursorPosX + x * 2, newCursorPosY + y * 2] = EBoard.Miss; }

                                        catch { continue; }

                                break;

                            case EBoard.Neutral:

                                Console.Clear();
                                Utilities.TypeWrite("Miss!");
                                Utilities.Wait(1000);
                                Console.Clear();

                                AttckBoard[cursorPosX, cursorPosY] = EBoard.Miss;
                                return;
                        }

                        break;
                }
            }
        }

        public EBoard[,] AttckBoard = new EBoard[10, 10];
        public EBoard[,] PersonalBoard = new EBoard[10, 10];

        public Player Opponent;
    }
}
