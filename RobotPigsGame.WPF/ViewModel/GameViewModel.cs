using RobotPigsGame.Model;
using RobotPigsGame.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.WPF.ViewModel
{

    internal class GameViewModel : ViewModelBase, ICommandRequesterViewModel
    {
        #region Fields

        private const byte _maxCommandCount = 5;

        private GameModel _model;
        private IRobotPigsDataAccess _dataAccess;
        private Command[] _p1Commands = null!;
        private Command[] _p2Commands = null!;
        private MapSize _setMapSize = MapSize.Medium;

        #endregion

        #region Properties

        /// <summary>
        /// Responsible for handling the logic of the game.
        /// </summary>
        public GameModel Model { get { return _model; } }

        #endregion

        #region Binding properties

        public int MapSizeValue { get { return _model.MapSizeValue; } }

        private int _p1Health;

        /// <summary>
        /// Player one's health
        /// </summary>
        public int P1Health
        {
            get { return _p1Health; }
            set { _p1Health = value; OnPropertyChanged(); }
        }

        private int _p2Health;

        /// <summary>
        /// Player two's health
        /// </summary>
        public int P2Health
        {
            get { return _p2Health; }
            set { _p2Health = value; OnPropertyChanged(); }
        }

        private bool _canPlayTurn;

        public bool CanPlayTurn
        {
            get { return _canPlayTurn; }
            set { _canPlayTurn = value; OnPropertyChanged(); }
        }

        public ObservableCollection<MapField> Fields { get; set; }

        #endregion

        #region Events

        public event EventHandler<SetCommandArgs>? SetCommands;

        #endregion

        #region Commands

        public DelegateCommand SetPlayerCommandsCommand { get; private set; }
        public DelegateCommand PlayTurnCommand { get; private set; }
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand SetMapSizeCommand { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes everything required to play the game.
        /// Connects events to the model layer.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public GameViewModel()
        {
            _dataAccess = new RobotPigsFileDataAccess();
            _model = new GameModel(_dataAccess);
            _model.PlayerHealthChanged += SetHealthValues;
            _model.PlayerPositionChanged += PositionChanged;
            Fields = new ObservableCollection<MapField>();

            InitCommands();
            InitMap(_model.MapSizeValue);

            SetPlayerCommandsCommand = new DelegateCommand((param) =>
                {
                    if (param is string text && Byte.TryParse(text, out byte id))
                    {
                        switch (id)
                        {
                            case 1:
                                SetCommands?.Invoke(this, new SetCommandArgs(id, _p1Commands));
                                break;
                            case 2:
                                SetCommands?.Invoke(this, new SetCommandArgs(id, _p2Commands));
                                break;
                            default:
                                throw new NotImplementedException("Player id must be either 1 or 2");
                        }
                    }
                }
            );

            PlayTurnCommand = new DelegateCommand((param) =>
            {
                Model.ExecuteCommands(_p1Commands, _p2Commands);
                CanPlayTurn = !Model.IsGameWon;
                InitCommands();
            });

            NewGameCommand = new DelegateCommand((param) =>
            {
                Model.MapSize = _setMapSize;
                Model.NewGame();
                OnPropertyChanged(nameof(MapSizeValue));
                InitMap(Model.MapSizeValue);
            });

            SetMapSizeCommand = new DelegateCommand((param) =>
            {
                if (param is MapSize mapSize)
                {
                    _setMapSize = mapSize;
                }
            });
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Called when commands are accepted in the setter window.
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="commands"></param>
        /// <exception cref="ArgumentException"></exception>
        public void CommandsAssembled(byte pid, Command[] commands)
        {
            if (commands.Length != _maxCommandCount)
            {
                throw new ArgumentException($"{nameof(commands)} did not return the expected number of commands.");
            }
            switch (pid)
            {
                case 1:
                    _p1Commands = commands;
                    break;
                case 2:
                    _p2Commands = commands;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Handles the PlayerPositionChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionChanged(object? sender, PositionEventArgs e)
        {
            if (e.Prev != null)
            {
                Fields.First(x => x.X == e.Prev.X && x.Y == e.Prev.Y).RefreshText(); 
            }
            MapField debug = Fields.First(x => x.X == e.Changed.X && x.Y == e.Changed.Y); // .RefreshText(e.FacingDirection, e.Pid);
            debug.RefreshText(e.FacingDirection, e.Pid);
        }

        /// <summary>
        /// Handles the PlayerHealthChanged event and sets the new health on the UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Information about which player is affected and how.</param>
        private void SetHealthValues(object? sender, HealthEventArgs e)
        {
            switch (e.Pid)
            {
                case 1:
                    P1Health = e.NewHealth;
                    break;
                case 2:
                    P2Health = e.NewHealth;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Add the fields that make up the map to the observable collections.
        /// </summary>
        /// <param name="mapSize"></param>
        private void InitMap(int mapSize)
        {
            Fields.Clear();
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Fields.Add(new MapField
                    {
                        Y = i,
                        X = j
                    });
                }
            }

            _model.FirePlayerEventsWithCurrentValues();
        }

        /// <summary>
        /// Defaults all commands to skip.
        /// </summary>
        private void InitCommands()
        {
            _p1Commands = new Command[_maxCommandCount];
            _p2Commands = new Command[_maxCommandCount];
            for (byte i = 0; i < _maxCommandCount; i++)
            {
                _p1Commands[i] = new Command(CommandType.Skip);
                _p2Commands[i] = new Command(CommandType.Skip);
            }
        }

        #endregion
    }
}
