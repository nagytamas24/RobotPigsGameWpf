using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.Persistence
{
    /// <summary>
    /// Exception to be used for any problems while saving/loading a game state.
    /// </summary>
    public class RobotPigsDataException : Exception
    {
        public RobotPigsDataException() { }
        public RobotPigsDataException(string message) : base(message) { }
        public RobotPigsDataException (string message, Exception innerException) : base(message, innerException) { }
    }
}
