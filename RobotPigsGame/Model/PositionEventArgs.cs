using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.Model
{
    /// <summary>
    /// Used to pass information about the change in a player's position using an event.
    /// </summary>
    public class PositionEventArgs
    {
        /// <summary>
        /// Id of the player. Either 1 or 2.
        /// </summary>
        public int Pid { get; private set; }

        /// <summary>
        /// Previous position.
        /// </summary>
        public Position? Prev { get; private set; }

        /// <summary>
        /// New position.
        /// </summary>
        public Position Changed { get; private set; }
        public FacingDirection FacingDirection { get; private set; }

        public PositionEventArgs(int pid, FacingDirection facingDirection, Position? prev, Position changed)
        {
            if (pid != 1 && pid != 2)
            {
                throw new ArgumentException("Player id must be either 1 or 2.");
            }
            Pid = pid;
            FacingDirection = facingDirection;
            Prev = prev;
            Changed = changed;
        }
    }
}
