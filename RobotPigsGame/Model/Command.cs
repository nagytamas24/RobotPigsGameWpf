using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.Model
{
    public class Command
    {
        private readonly CommandType _commandType;
        private readonly MoveDirection _direction;

        public CommandType CommandType { get { return _commandType; } }
        public MoveDirection Direction { get { return _direction; } }

        public Command(CommandType commandType)
        {
            if (commandType == CommandType.Move || commandType == CommandType.Turn)
            {
                throw new ArgumentException("Cannot construct move or turn command without direction parameter");
            }
            _commandType = commandType;
            _direction = MoveDirection.Right;
        }

        public Command(CommandType commandType, MoveDirection direction)
        {
            _commandType = commandType;
            _direction = direction;
        }
    }
}
