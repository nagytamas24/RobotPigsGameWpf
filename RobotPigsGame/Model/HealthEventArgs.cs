using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.Model
{
    /// <summary>
    /// Used to pass information along of a change in a player's health using an event.
    /// </summary>
    public class HealthEventArgs
    {
        public int Pid { get; private set; }
        public int NewHealth { get; private set; }

        public HealthEventArgs(int pid, int newHealth)
        {
            if (pid != 1 && pid != 2)
            {
                throw new ArgumentException("Player id must be either 1 or 2.");
            }
            Pid = pid;
            NewHealth = newHealth;
        }
    }
}
