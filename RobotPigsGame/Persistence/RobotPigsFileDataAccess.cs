using RobotPigsGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.Persistence
{
    /// <summary>
    /// Implements loading/saving the game state from/to a text file.
    /// </summary>
    public class RobotPigsFileDataAccess : IRobotPigsDataAccess
    {
        public async Task<GameData> LoadAsync(string path)
        {
            try
            {
                int mapSize;
                using StreamReader reader = new(path);

                string line = await reader.ReadLineAsync() ?? String.Empty;
                mapSize = int.Parse(line);

                line = await reader.ReadLineAsync() ?? String.Empty;
                string[] props = line.Split(' ');
                Player p1 = new(new Position(int.Parse(props[1]), int.Parse(props[2])), mapSize, (FacingDirection)int.Parse(props[3]), int.Parse(props[0]));
                
                line = await reader.ReadLineAsync() ?? String.Empty;
                props = line.Split(' ');
                Player p2 = new(new Position(int.Parse(props[1]), int.Parse(props[2])), mapSize, (FacingDirection)int.Parse(props[3]), int.Parse(props[0]));

                return new GameData(mapSize, p1, p2);
            }
            catch (Exception ex)
            {
                throw new RobotPigsDataException("An error occourd while loading the save.", ex);
            }
        }

        public async Task SaveAsync(string path, GameData gameData)
        {
            try
            {
                using StreamWriter writer = new(path);
                await writer.WriteLineAsync(((int)gameData.MapSize).ToString());
                await writer.WriteLineAsync(gameData.P1.Health.ToString() + " " + gameData.P1.Position.ToString() + " " + (int)gameData.P1.FacingDirection);
                await writer.WriteLineAsync(gameData.P2.Health.ToString() + " " + gameData.P2.Position.ToString() + " " + (int)gameData.P2.FacingDirection);
            }
            catch (Exception ex)
            {
                throw new RobotPigsDataException("An error occourd while saving the save.", ex);
            }
        }
    }
}
