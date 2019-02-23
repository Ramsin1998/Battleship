using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    enum BoardState { Neutral, Hit, Miss, Ship, InvalidShip }

    static class Board
    {
        public static bool Any(this BoardState[,] board, Func<BoardState,bool> predicate)
        {
            int xLength = board.GetLength(0);
            int yLength = board.GetLength(1);

            for (int y = 0; y < yLength; y++)
                for (int x = 0; x < xLength; x++)
                    if (predicate(board[x, y]))
                        return true;

            return false;
        }

        public static void Print(this BoardState[,] board, int left, int top)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(i * 6 + left + 5, top);
                Console.WriteLine((char)(65 + i));
            }

            for (int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(left, i * 3 + top + 3);
                Console.Write(i + 1);
            }

            left += 3;
            top += 2;

            string ship = "/----\\" +
                          "| [] |" +
                          "\\----/";

            string hit = " \\  / " +
                         "  ><  " +
                         " /  \\ ";

            string neutral = "^^^^^^" +
                             "^^^^^^" +
                             "^^^^^^";

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Console.SetCursorPosition(x * 6 + left, y * 3 + top);

                    switch (board[x, y])
                    {
                        case (BoardState.Hit):
                            hit.DrawRect(6, ConsoleColor.DarkRed);
                            break;

                        case (BoardState.Ship):
                            ship.DrawRect(6, ConsoleColor.DarkBlue);
                            break;

                        case (BoardState.Miss):
                            neutral.DrawRect(6, ConsoleColor.DarkGray, ConsoleColor.Black);
                            break;

                        case (BoardState.Neutral):
                            neutral.DrawRect(6, ConsoleColor.Blue, ConsoleColor.Cyan);
                            break;

                        case (BoardState.InvalidShip):
                            ship.DrawRect(6, ConsoleColor.DarkRed);
                            break;
                    }
                }
            }
        }

        public static BoardState[,] PlaceShipsRandomly()
        {
            BoardState[,] boardToReturn = new BoardState[10, 10];
            BoardState[,] tmpBoard = new BoardState[10, 10];

            for (int i = 0; i < Ship.ShipLengths.Count(); i++)
            {
                bool done = false;
                while (!done)
                {
                    Ship ship = new Ship(Ship.ShipLengths[i], true);
                    tmpBoard = Utilities.DeepClone(boardToReturn);
                    tmpBoard.placeShip(ship);

                    if (!tmpBoard.Any(item => item == BoardState.InvalidShip))
                    {
                        boardToReturn = Utilities.DeepClone(tmpBoard);
                        break;
                    }
                }
            }

            return boardToReturn;
        }

        public static BoardState[,] PlaceShipsByPlayerInput()
        {
            BoardState[,] boardToReturn = new BoardState[10, 10]; 
            BoardState[,] tmpBoard = new BoardState[10, 10];

            for (int i = 0; i < Ship.ShipLengths.Count(); i++)
            {
                Ship ship = new Ship(Ship.ShipLengths[i], false);

                bool done = false;
                while (!done)
                {
                    tmpBoard = Utilities.DeepClone(boardToReturn);
                    tmpBoard.placeShip(ship).Print(85, 15);

                    switch (Console.ReadKey(true).Key)
                    {
                        case (ConsoleKey.UpArrow):
                            ship.StartPosY--;
                            break;

                        case (ConsoleKey.DownArrow):
                            ship.StartPosY++;
                            break;

                        case (ConsoleKey.RightArrow):
                            ship.StartPosX++;
                            break;

                        case (ConsoleKey.LeftArrow):
                            ship.StartPosX--;
                            break;

                        case (ConsoleKey.Spacebar):
                            ship.IsVertical = !ship.IsVertical;
                            break;

                        case (ConsoleKey.Enter):
                            if (!tmpBoard.Any(item => item == BoardState.InvalidShip))
                            {
                                boardToReturn = Utilities.DeepClone(tmpBoard);
                                done = true;
                            }
                            break;

                        default:
                            continue;
                    }
                }
            }

            return boardToReturn;
        }

        private static BoardState[,] placeShip(this BoardState[,] board, Ship ship)
        {
            bool isInvalid = checkIfShipWillBeInvalid(board, ship);

            int x = ship.StartPosX;
            int y = ship.StartPosY;

            for (int i = 0; i < ship.Length; i++)
            {
                try { board[ship.StartPosX, ship.StartPosY] = isInvalid ? BoardState.InvalidShip : BoardState.Ship; }
                catch (IndexOutOfRangeException) { continue; }

                if (ship.IsVertical)
                    ship.StartPosY++;
                else
                    ship.StartPosX++;
            }

            return board;
        }

        private static bool checkIfShipWillBeInvalid(BoardState[,] board, Ship ship)
        {
            for (int i = 0; i < ship.Length; i++)
            {
                if (blockAndSurroundingBlocksAreShip(board, ship, i == 0))
                    return true;

                if (ship.IsVertical)
                    ship.StartPosY++;
                else
                    ship.StartPosX++;
            }

            return false;
        }

        private static bool blockAndSurroundingBlocksAreShip(BoardState[,] board, Ship ship, bool firstBlock)
        {
            int posX = ship.StartPosX - 1;
            int posY = ship.StartPosY - 1;

            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 3; x++)
                {
                    try
                    {
                        if (!firstBlock)
                        {
                            if (ship.IsVertical)
                            {
                                if (y == 0 && x == 1)
                                    continue;
                            }

                            else
                            {
                                if (y == 1 && x == 0)
                                    continue;
                            }
                        }

                        if (board[posX + x, posY + y] == BoardState.Ship)
                            return true;
                    }
                    catch (IndexOutOfRangeException) { continue; }
                }

            return false;
        }
    }
}