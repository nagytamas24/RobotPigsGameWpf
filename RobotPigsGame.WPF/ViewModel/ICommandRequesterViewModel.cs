using RobotPigsGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.WPF.ViewModel
{
    /// <summary>
    /// Provides a way to notify the calling window of the command setter window that the commands were chosen.
    /// </summary>
    internal interface ICommandRequesterViewModel
    {
        void CommandsAssembled(byte pid, Command[] commands);
    }
}
