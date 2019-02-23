using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Player 
    {
        public string Name;
        public Player(string name)
        {
            Name = name;
        }

        public BoardState[,] AttckBoard = new BoardState[10, 10];
        public BoardState[,] PersonalBoard = new BoardState[10, 10];
    }
}
