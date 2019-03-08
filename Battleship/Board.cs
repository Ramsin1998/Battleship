using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    enum Grid { Neutral, Hit, Miss, Ship, InvalidShip, DeadShip, DeadZone }

    static class Board
    {
        private static string ship = "/----\\" +
                                     "| [] |" +
                                    "\\----/";

        private static string hit = " \\  / " +
                                    "  ><  " +
                                    " /  \\ ";

        private static string neutral = "^^^^^^" +
                                        "^^^^^^" +
                                        "^^^^^^";

        public static bool Any(this Grid[,] board, Func<Grid,bool> predicate)
        {
            int xLength = board.GetLength(0);
            int yLength = board.GetLength(1);

            for (int y = 0; y < yLength; y++)
                for (int x = 0; x < xLength; x++)
                    if (predicate(board[x, y]))
                        return true;

            return false;
        }

        public static void Print(this Grid[,] board, int left, int top)
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

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Console.SetCursorPosition(x * 6 + left, y * 3 + top);

                    switch (board[x, y])
                    {
                        case (Grid.Hit):
                            hit.DrawRect(6, ConsoleColor.DarkRed);
                            break;

                        case (Grid.Ship):
                            ship.DrawRect(6, ConsoleColor.DarkBlue);
                            break;

                        case (Grid.Miss):
                            hit.DrawRect(6, ConsoleColor.DarkGray);
                            break;

                        case (Grid.Neutral):
                            neutral.DrawRect(6, ConsoleColor.Cyan, ConsoleColor.Blue);
                            break;

                        case (Grid.DeadZone):
                            neutral.DrawRect(6, ConsoleColor.DarkCyan, ConsoleColor.Black);
                            break;

                        case (Grid.InvalidShip):
                            ship.DrawRect(6, ConsoleColor.DarkRed);
                            break;

                        case (Grid.DeadShip):
                            ship.DrawRect(6, ConsoleColor.DarkGray, ConsoleColor.Black);
                            break;
                    }
                }
            }
        }

        public static Grid[,] PlaceShipsRandomly()
        {
            Console.Clear();

            Utilities.WriteParagraph(
                "SPACE: Randomize again\n" +
                "ENTER: Done", 153, 22);

            while (true)
            {
                Grid[,] boardToReturn = new Grid[10, 10];
                Grid[,] tmpBoard = new Grid[10, 10];

                for (int i = 0; i < Ship.ShipLengths.Count(); i++)
                {
                    while (true)
                    {
                        Ship ship = new Ship(Ship.ShipLengths[i], true);
                        tmpBoard = Utilities.DeepClone(boardToReturn);
                        tmpBoard.placeShip(ship);

                        if (!tmpBoard.Any(item => item == Grid.InvalidShip))
                        {
                            boardToReturn = Utilities.DeepClone(tmpBoard);
                            break;
                        }
                    }
                }

                boardToReturn.Print(85, 15);

                input:
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        return boardToReturn;

                    case ConsoleKey.Spacebar:
                        continue;

                    default:
                        goto input; //plz dont kill me.
                }
            }
        }

        public static Grid[,] PlaceShipsByPlayerInput()
        {
            Console.Clear();

            Console.OutputEncoding = Encoding.Unicode;
            Utilities.WriteParagraph(
                "↑: Up\n" +
                "↓: Down\n" +
                "→: Right\n" +
                "←: Left\n" +
                "SPACE: Direction\n" +
                "ENTER: Done", 153, 22);

            Grid[,] boardToReturn = new Grid[10, 10]; 
            Grid[,] tmpBoard = new Grid[10, 10];

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
                        case ConsoleKey.UpArrow:
                            ship.StartPosY--;
                            break;

                        case ConsoleKey.DownArrow:
                            ship.StartPosY++;
                            break;

                        case ConsoleKey.LeftArrow:
                            ship.StartPosX--;
                            break;

                        case ConsoleKey.RightArrow:
                            ship.StartPosX++;
                            break;

                        case ConsoleKey.Spacebar:
                            ship.IsVertical = !ship.IsVertical;
                            break;

                        case ConsoleKey.Enter:
                            if (!tmpBoard.Any(item => item == Grid.InvalidShip))
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

        private static Grid[,] placeShip(this Grid[,] board, Ship ship)
        {
            bool isInvalid = ifShipPlacementWillBeInvalid(board, ship);

            int x = ship.StartPosX;
            int y = ship.StartPosY;

            for (int i = 0; i < ship.Length; i++)
            {
                try { board[x, y] = isInvalid ? Grid.InvalidShip : Grid.Ship; }
                catch (IndexOutOfRangeException) { continue; }

                if (ship.IsVertical)
                    y++;
                else
                    x++;
            }

            return board;
        }

        private static bool ifShipPlacementWillBeInvalid(Grid[,] board, Ship ship)
        {
            int x = ship.StartPosX;
            int y = ship.StartPosY;

            for (int i = 0; i < ship.Length; i++)
            {
                if (ifShipBlockIsInvalid(board, ship, x, y, i == 0))
                    return true;

                if (ship.IsVertical)
                    y++;
                else
                    x++;
            }

            return false;
        }

        private static bool ifShipBlockIsInvalid(Grid[,] board, Ship ship, int blockX, int blockY, bool firstBlock)
        {
            blockX--;
            blockY--;

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

                        if (board[blockX + x, blockY + y] == Grid.Ship)
                            return true;
                    }
                    catch (IndexOutOfRangeException) { continue; }
                }

            return false;
        }
    }
}