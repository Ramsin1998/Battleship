using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public struct Ship
    {
        private bool isVertical;
        private int startPosY;
        private int startPosX;

        public int Length{ get; set; }

        public bool IsVertical
        {
            get { return isVertical; }

            set
            {
                if (value)
                {
                    if (startPosY + Length <= 10)
                        isVertical = value;
                }

                else
                {
                    if (startPosX + Length <= 10)
                        isVertical = value;
                }
            }
        }

        public int StartPosX
        {
            get { return startPosX; }

            set
            {
                int limit = !isVertical ? 10 - Length : 9;

                if (value < 0)
                    startPosX = 0;

                else if (value > limit)
                    startPosX = limit;

                else
                    startPosX = value;
            }
        }

        public int StartPosY
        {
            get { return startPosY; }

            set
            {
                int limit = isVertical ? 10 - Length : 9;

                if (value < 0)
                    startPosY = 0;

                else if (value > limit)
                    startPosY = limit;

                else
                    startPosY = value;
            }
        }

        public Ship(int length, bool randomize, bool isVertical = false, int startPosX = 0, int startPosY = 0)
        {
            Length = length;

            if (randomize)
            {
                Random rng = new Random();
                this.isVertical = rng.Next(0, 2) == 1;
                this.startPosX = this.isVertical ? rng.Next(0, 10) : rng.Next(0, 11 - length);
                this.startPosY = this.isVertical ? rng.Next(0, 11 - length) : rng.Next(0, 10);
            }

            else
            {
                this.isVertical = isVertical;
                this.startPosX = startPosX;
                this.startPosY = startPosY;
            }
        }

        public static int[] ShipLengths = { 2, 3, 3, 4, 5 };
    }
}