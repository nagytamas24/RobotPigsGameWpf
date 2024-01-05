using RobotPigsGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.Persistence
{
    /// <summary>
    /// Contains all the necessary data to save or load a game.
    /// </summary>
    public class GameData
    {
        /// <summary>
        /// Player 1
        /// </summary>
        public Player P1 { get; set; }

        /// <summary>
        /// Player 2
        /// </summary>
        public Player P2 { get; set; }
        public MapSize MapSize { get; private set; }
        public int MapSizeValue
        {
            get
            {
                return MapSize switch
                {
                    MapSize.Small => 4,
                    MapSize.Medium => 6,
                    MapSize.Large => 8,
                    _ => throw new NotImplementedException("This map size is not implemented."),
                };
            }
        }

        public GameData(int mapSize, Player p1, Player p2)
        {
            switch (mapSize)
            {
                case 4:
                    MapSize = MapSize.Small;
                    break;
                default:
                case 6:
                    MapSize = MapSize.Medium;
                    break;
                case 8:
                    MapSize = MapSize.Large;
                    break;
            }
            P1 = p1;
            P2 = p2;
        }
    }
}
