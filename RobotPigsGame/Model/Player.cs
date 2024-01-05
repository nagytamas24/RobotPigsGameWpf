using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.Model
{
    public class Player
    {
        // The default health value all players will start with
        private const int StartingHealth = 3;

        // The map size acquired from the instanciated game model
        private readonly int _mapSize;

        // Position of the player
        private Position _position;
        
        public Position Position
        {
            get { return _position; }
            set
            {
                if (value.IsOutsideOfSquare(_mapSize))
                {
                    throw new ArgumentException("Position would be ouside of the play area.");
                }
                _position = value;
            }
        }

        // Current health value
        public int Health { get; private set; }
        public FacingDirection FacingDirection { get; private set; }


        /// <summary>
        /// Initializes a new instance of the player class.
        /// </summary>
        /// <param name="position">Starting position.</param>
        /// <param name="mapSize">Size of the map on which the player will be placed.</param>
        /// <param name="facingDirection">Starting direction towards which a player will face.</param>
        /// <param name="health">Amount of health the player will start with. Has default value.</param>
        public Player(Position position, int mapSize, FacingDirection facingDirection, int health = StartingHealth)
        {
            _mapSize = mapSize;
            _position = position;
            FacingDirection = facingDirection;
            Health = health;
        }

        /// <summary>
        /// Turns the player in a direction.
        /// </summary>
        /// <param name="direction">Direction in which the player will turn. Can only be left or right.</param>
        /// <exception cref="InvalidOperationException">Thrown if provided direction wasn't either left or right.</exception>
        public void Turn(MoveDirection direction)
        {
            FacingDirection = direction switch
            {
                MoveDirection.Left => (FacingDirection)((int)(FacingDirection + 3) % 4),
                MoveDirection.Right => (FacingDirection)((int)(FacingDirection + 1) % 4),
                _ => throw new InvalidOperationException("Can only turn left and right"),
            };
        }

        /// <summary>
        /// Does not actually move the player, just calculates which position the move would result.
        /// </summary>
        /// <param name="direction">Direction towards which to calculate the simulated move.</param>
        /// <returns>The position where the move would result, null if the move would fall outside the play area.</returns>
        public Position? SimulateMove(MoveDirection direction)
        {
            Position? result = null;
            switch (((int)direction + (int)FacingDirection) % 4)
            {
                case 0:
                    result = new Position(Position.X, Position.Y - 1);
                    break;
                case 1:
                    result = new Position(Position.X + 1, Position.Y);
                    break;
                case 2:
                    result = new Position(Position.X, Position.Y + 1);
                    break;
                case 3:
                    result = new Position(Position.X - 1, Position.Y);
                    break;
                default:
                    break;
            }

            if (result != null && result.IsOutsideOfSquare(_mapSize))
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// Moves a player in a given direction.
        /// </summary>
        /// <param name="direction">The direction the player will move in on the play area.</param>
        /// <exception cref="InvalidOperationException">Thrown if the move results in a position outside of the play area.</exception>
        public void Move(MoveDirection direction)
        {
            Position? simulated = SimulateMove(direction);
            if (simulated != null)
            {
                Position = simulated;
            }
            else
            {
                throw new InvalidOperationException("Cannot move to position because it is outside of the playable area.");
            }
        }

        /// <summary>
        /// Performs the laser fire action of a player.
        /// </summary>
        /// <returns>Affected areas where other players will be damaged.</returns>
        public HashSet<Position> Fire()
        {
            HashSet<Position> affectedPositions = new();

            switch (FacingDirection)
            {
                case FacingDirection.Up:
                    for (int i = Position.Y - 1; i >= 0; i--)
                    {
                        affectedPositions.Add(new Position(Position.X, i));
                    }
                    break;
                case FacingDirection.Right:
                    for (int i = Position.X + 1; i < _mapSize; i++)
                    {
                        affectedPositions.Add(new Position(i, Position.Y));
                    }
                    break;
                case FacingDirection.Down:
                    for (int i = Position.Y + 1; i < _mapSize; i++)
                    {
                        affectedPositions.Add(new Position(Position.X, i));
                    }
                    break;
                case FacingDirection.Left:
                    for (int i = Position.X - 1; i >= 0; i--)
                    {
                        affectedPositions.Add(new Position(i, Position.Y));
                    }
                    break;
                default:
                    break;
            }

            return affectedPositions;
        }

        /// <summary>
        /// Performs the punch action of a player.
        /// </summary>
        /// <returns>Affected areas where other players will be damaged.</returns>
        public HashSet<Position> Punch()
        {
            HashSet<Position> affectedPositions = new();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (!new Position(Position.X + i, Position.Y + j).IsOutsideOfSquare(_mapSize) && !new Position(Position.X + i, Position.Y + j).Equals(Position))
                    {
                        affectedPositions.Add(new Position(Position.X + i, Position.Y + j));
                    }
                }
            }

            return affectedPositions;
        }

        /// <summary>
        /// Decreases the player's health.
        /// </summary>
        /// <param name="amount">Integer amount with which to decrease the players health. Default is 1.</param>
        public void Hurt(int amount = 1)
        {
            if (Health - amount < 0)
            {
                Health = 0;
            }
            else
            {
                Health -= amount;
            }
        }

    }
}
