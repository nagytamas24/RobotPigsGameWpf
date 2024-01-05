using RobotPigsGame.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.Model
{
    public class GameModel
    {

        #region Fields

        private readonly IRobotPigsDataAccess _robotPigsDataAccess;
        private GameData _gameData = null!;

        #endregion

        #region Properties

        public MapSize MapSize { get; set; }

        /// <summary>
        /// Represents how many fields are in a single row/column of the map.
        /// </summary>
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

        public bool IsGameWon { get { return Winner != 0; } }

        /// <summary>
        /// Returns the id of the winning player, or -1 if the game ended with a tie, or 0 if the game has not yet finished.
        /// </summary>
        public int Winner 
        { 
            get 
            {
                if (_gameData.P1.Health <= 0 && _gameData.P2.Health <= 0)
                {
                    return -1;
                }
                else if (_gameData.P1.Health <= 0)
                {
                    return 2;
                }
                else if (_gameData.P2.Health <= 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            } 
        }

        /// <summary>
        /// Returns the position of player one.
        /// </summary>
        public Position GetP1Pos { get { return _gameData.P1.Position; } }

        /// <summary>
        /// Returns the position of player two.
        /// </summary>
        public Position GetP2Pos { get { return _gameData.P2.Position; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of game model and initializes a new game.
        /// </summary>
        /// <param name="dataAccess">Dependency injection for data access</param>
        /// <param name="mapSize"></param>
        public GameModel(IRobotPigsDataAccess dataAccess, MapSize mapSize = MapSize.Medium)
        {
            _robotPigsDataAccess = dataAccess;
            MapSize = mapSize;
            NewGame();

        }

        #endregion

        #region Events

        public event EventHandler<HealthEventArgs>? PlayerHealthChanged;
        public event EventHandler<PositionEventArgs>? PlayerPositionChanged;
        public event EventHandler<int>? GameWon;

        #endregion

        #region Private Game Methods

        /// <summary>
        /// Called in case both players need to move. 
        /// Checks if the players want to move into the same field.
        /// If so the turn is skipped. If not the players are moved individually.
        /// </summary>
        /// <param name="p1Direction">Movement direction of player 1</param>
        /// <param name="p2Direction">Movement direction of player 2</param>
        private void MoveBoth(MoveDirection p1Direction, MoveDirection p2Direction)
        {
            Position? simPosP1 = _gameData.P1.SimulateMove(p1Direction);
            Position? simPosP2 = _gameData.P2.SimulateMove(p2Direction);
            if (simPosP1 != null && simPosP2 != null)
            {
                if (!simPosP1.Equals(simPosP2))
                {
                    Position prev = _gameData.P1.Position;
                    _gameData.P1.Move(p1Direction);
                    OnPlayerPositionChanged(1, _gameData.P1.FacingDirection, prev, _gameData.P1.Position);

                    prev = _gameData.P2.Position;
                    _gameData.P2.Move(p2Direction);
                    OnPlayerPositionChanged(2, _gameData.P2.FacingDirection, prev, _gameData.P2.Position);
                }
            }
            else if (_gameData.P1.SimulateMove(p1Direction) != null)
            {
                MoveP1(p1Direction);
            }
            else if (_gameData.P2.SimulateMove(p2Direction) != null)
            {
                MoveP2(p2Direction);
            }
        }

        /// <summary>
        /// Checks if move is valid, if it is moves player 1 on the map.
        /// </summary>
        /// <param name="direction"></param>
        private void MoveP1(MoveDirection direction)
        {
            Position? simPos = _gameData.P1.SimulateMove(direction);
            if (simPos != null && !simPos.Equals(_gameData.P2.Position))
            {
                Position prev = _gameData.P1.Position;
                _gameData.P1.Move(direction);
                OnPlayerPositionChanged(1, _gameData.P1.FacingDirection, prev, _gameData.P1.Position);
            }
        }

        /// <summary>
        /// Checks if move is valid, if it is moves player 2 on the map.
        /// </summary>
        /// <param name="direction"></param>
        private void MoveP2(MoveDirection direction)
        {
            Position? simPos = _gameData.P2.SimulateMove(direction);
            if (simPos != null && !simPos.Equals(_gameData.P1.Position))
            {
                Position prev = _gameData.P2.Position;
                _gameData.P2.Move(direction);
                OnPlayerPositionChanged(1, _gameData.P2.FacingDirection, prev, _gameData.P2.Position);
            }
        }

        #endregion

        #region Public Game Methods

        /// <summary>
        /// Initializes the players in their starting positions.
        /// </summary>
        public void NewGame()
        {
            Player p1 = new(new Position((MapSizeValue - 1) / 2, 0), MapSizeValue, FacingDirection.Down);

            Player p2 = new(new Position((MapSizeValue - 1) / 2 + 1, MapSizeValue - 1), MapSizeValue, FacingDirection.Up);

            _gameData = new GameData(MapSizeValue, p1, p2);
        }

        /// <summary>
        /// Loads a saved game using the data access dependency.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if no data access was provided.</exception>
        public async Task LoadGameAsync(string path)
        {
            if (_robotPigsDataAccess == null)
            {
                throw new InvalidOperationException("No data access was provided.");
            }
            _gameData = await _robotPigsDataAccess.LoadAsync(path);
            MapSize = _gameData.MapSize;
        }

        /// <summary>
        /// Saves the current game state using the data access dependency.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if no data access was provided.</exception>
        public async Task SaveGameAsync(string path)
        {
            if (_robotPigsDataAccess == null)
            {
                throw new InvalidOperationException("No data access was provided.");
            }
            await _robotPigsDataAccess.SaveAsync(path, _gameData);
        }

        /// <summary>
        /// Fires all events in relation to the players. Call to manually refresh the map.
        /// </summary>
        public void FirePlayerEventsWithCurrentValues()
        {
            OnPlayerHealthChanged(1, _gameData.P1.Health);
            OnPlayerHealthChanged(2, _gameData.P2.Health);

            OnPlayerPositionChanged(1, _gameData.P1.FacingDirection, null, _gameData.P1.Position);
            OnPlayerPositionChanged(2, _gameData.P2.FacingDirection, null, _gameData.P2.Position);
        }

        /// <summary>
        /// Handles the execution of a turn.
        /// </summary>
        /// <param name="p1">Player one's commands</param>
        /// <param name="p2">Player two's commands</param>
        /// <exception cref="RobotPigsDataException"></exception>
        public void ExecuteCommands(Command[] p1, Command[] p2)
        {
            if (p1.Length != p2.Length)
            {
                throw new RobotPigsDataException("The length of the command arrays do not match.");
            }

            for (int i = 0; i < p1.Length; i++)
            {
                if (p1[i].CommandType == CommandType.Move && p2[i].CommandType == CommandType.Move)
                {
                    MoveBoth(p1[i].Direction, p2[i].Direction);
                }
                else
                {
                    HashSet<Position>? p1AffectedPositions = null;
                    HashSet<Position>? p2AffectedPositions = null;

                    switch (p1[i].CommandType)
                    {
                        case CommandType.Move:
                            MoveP1(p1[i].Direction);
                            break;
                        case CommandType.Turn:
                            _gameData.P1.Turn(p1[i].Direction);
                            OnPlayerPositionChanged(1, _gameData.P1.FacingDirection, null, _gameData.P1.Position);
                            break;
                        case CommandType.Fire:
                            p1AffectedPositions = _gameData.P1.Fire();
                            break;
                        case CommandType.Punch:
                            p1AffectedPositions = _gameData.P1.Punch();
                            break;
                        case CommandType.Skip:
                            break;
                        default:
                            break;
                    }

                    switch (p2[i].CommandType)
                    {
                        case CommandType.Move:
                            MoveP2(p2[i].Direction);
                            break;
                        case CommandType.Turn:
                            _gameData.P2.Turn(p2[i].Direction);
                            OnPlayerPositionChanged(2, _gameData.P2.FacingDirection, null, _gameData.P2.Position);
                            break;
                        case CommandType.Fire:
                            p2AffectedPositions = _gameData.P2.Fire();
                            break;
                        case CommandType.Punch:
                            p2AffectedPositions = _gameData.P2.Punch();
                            break;
                        case CommandType.Skip:
                            break;
                        default:
                            break;
                    }

                    if (p1AffectedPositions != null && p1AffectedPositions.Any(x => x.Equals(_gameData.P2.Position)))
                    {
                        _gameData.P2.Hurt();
                        OnPlayerHealthChanged(2, _gameData.P2.Health);
                    }

                    if (p2AffectedPositions != null && p2AffectedPositions.Any(x => x.Equals(_gameData.P1.Position)))
                    {
                        _gameData.P1.Hurt();
                        OnPlayerHealthChanged(1, _gameData.P1.Health);
                    }

                    if (IsGameWon)
                    {
                        OnGameWon();
                        break;
                    }
                }

            }
        }

        #endregion

        #region Private Event Methods

        private void OnPlayerPositionChanged(int pid, FacingDirection facingDirection, Position? prev, Position changed)
        {
            PlayerPositionChanged?.Invoke(this, new PositionEventArgs(pid, facingDirection, prev, changed));
        }

        private void OnPlayerHealthChanged(int pid, int health)
        {
            PlayerHealthChanged?.Invoke(this, new HealthEventArgs(pid, health));
        }

        private void OnGameWon()
        {
            GameWon?.Invoke(this, Winner);
        }

        #endregion
    }
}
