using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.Model
{
    public enum CommandType
    {
        Move,
        Turn,
        Fire,
        Punch,
        Skip
    }

    public enum MapSize
    {
        Small,
        Medium,
        Large
    }

    // Directions are defined using enums to make movement cases easier. Allows simple arithmetic to be used.

    public enum MoveDirection
    {
        Forward = 0,
        Right,
        Backward,
        Left
    }

    public enum FacingDirection
    {
        Up = 0,
        Right,
        Down,
        Left
    }
}
