using RobotPigsGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.WPF.ViewModel
{
    /// <summary>
    /// Allows to initialize the set commands window.
    /// </summary>
    internal class SetCommandArgs
    {
        /// <summary>
        /// Id of the player. Either 1 or 2.
        /// </summary>
        public byte Pid { get; set; }

        /// <summary>
        /// Commands previously set for the player.
        /// </summary>
        public Command[] InitialCommands { get; set; }

        public SetCommandArgs(byte pid, Command[] initialCommands)
        {
            Pid = pid;
            InitialCommands = initialCommands;
        }
    }
}
