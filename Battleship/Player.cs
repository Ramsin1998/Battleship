using System;
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

                if (value < 0)
                    cursorPosX = 0;
            }
        }

        public int CursorPosY
        {
            get { return cursorPosY; }

            set
            {
                if (value > 9)
                    cursorPosY = 9;

                if (value < 0)
                    cursorPosY = 0;
            }
        }

        public string Name;
        public Player(string name)
        {
            Name = name;
        }

        public void StartTurn()
        {

        }

        public BoardState[,] AttckBoard = new BoardState[10, 10];
        public BoardState[,] PersonalBoard = new BoardState[10, 10];
    }
}
