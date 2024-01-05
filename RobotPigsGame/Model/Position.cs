using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.Model
{
    public class Position
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Checks if the position is outside of a square that has sides of size.
        /// Useful to check if a position would be outside of the map.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool IsOutsideOfSquare(int size)
        {
            if (X < 0 || X >= size || Y < 0 || Y >= size)
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Position)
            {
                return false;
            }

            Position other = (Position)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }
    }
}
