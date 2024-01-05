using RobotPigsGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPigsGame.WPF.ViewModel
{
    internal class SetCommandsViewModel : ViewModelBase
    {


        #region Translation constants

        private const string skip = "Kihagy";
        private const string turn = "Fordul";
        private const string move = "Mozog";
        private const string fire = "Tűz";
        private const string punch = "Ütés";

        private const string forward = "Előre";
        private const string right = "Jobbra";
        private const string backwards = "Hátra";
        private const string left = "Balra";

        #endregion

        #region Translation dictionaries

        private readonly Dictionary<string, CommandType> _displayCommandTypes;
        private readonly Dictionary<string, MoveDirection> _displayMoveDirectionTypes;
        private readonly Dictionary<string, MoveDirection> _displayTurnDirectionTypes;

        #endregion

        #region ComboBox properties

        public List<string> DisplayCommandTypes
        {
            get { return _displayCommandTypes.Keys.ToList(); }
        }

        public List<string> DisplayMoveDirectionTypes
        {
            get { return _displayMoveDirectionTypes.Keys.ToList(); }
        }

        public List<string> DisplayTurnDirectionTypes
        {
            get { return _displayTurnDirectionTypes.Keys.ToList(); }
        }

        #endregion

        #region Command box bindings

        private string _selectedCommand1;

        public string SelectedCommand1
        {
            get { return _selectedCommand1; }
            set 
            { 
                _selectedCommand1 = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(Direction1Visibility));
                OnPropertyChanged(nameof(Direction1Display)); 
            }
        }

        private string _selectedCommand2;

        public string SelectedCommand2
        {
            get { return _selectedCommand2; }
            set
            {
                _selectedCommand2 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Direction2Visibility));
                OnPropertyChanged(nameof(Direction2Display));
            }
        }

        private string _selectedCommand3;

        public string SelectedCommand3
        {
            get { return _selectedCommand3; }
            set
            {
                _selectedCommand3 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Direction3Visibility));
                OnPropertyChanged(nameof(Direction3Display));
            }
        }

        private string _selectedCommand4;

        public string SelectedCommand4
        {
            get { return _selectedCommand4; }
            set
            {
                _selectedCommand4 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Direction4Visibility));
                OnPropertyChanged(nameof(Direction4Display));
            }
        }

        private string _selectedCommand5;

        public string SelectedCommand5
        {
            get { return _selectedCommand5; }
            set
            {
                _selectedCommand5 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Direction5Visibility));
                OnPropertyChanged(nameof(Direction5Display));
            }
        }

        #endregion

        #region Direction visibility, box bindings

        public bool Direction1Visibility { get { return _selectedCommand1 == turn || _selectedCommand1 == move; } }
        public bool Direction2Visibility { get { return _selectedCommand2 == turn || _selectedCommand2 == move; } }
        public bool Direction3Visibility { get { return _selectedCommand3 == turn || _selectedCommand3 == move; } }
        public bool Direction4Visibility { get { return _selectedCommand4 == turn || _selectedCommand4 == move; } }
        public bool Direction5Visibility { get { return _selectedCommand5 == turn || _selectedCommand5 == move; } }

        public List<string> Direction1Display
        {
            get { return _displayCommandTypes[SelectedCommand1] == CommandType.Move ? DisplayMoveDirectionTypes : DisplayTurnDirectionTypes; }
        }
        public List<string> Direction2Display
        {
            get { return _displayCommandTypes[SelectedCommand2] == CommandType.Move ? DisplayMoveDirectionTypes : DisplayTurnDirectionTypes; }
        }
        public List<string> Direction3Display
        {
            get { return _displayCommandTypes[SelectedCommand3] == CommandType.Move ? DisplayMoveDirectionTypes : DisplayTurnDirectionTypes; }
        }
        public List<string> Direction4Display
        {
            get { return _displayCommandTypes[SelectedCommand4] == CommandType.Move ? DisplayMoveDirectionTypes : DisplayTurnDirectionTypes; }
        }
        public List<string> Direction5Display
        {
            get { return _displayCommandTypes[SelectedCommand5] == CommandType.Move ? DisplayMoveDirectionTypes : DisplayTurnDirectionTypes; }
        }

        private string _selectedDirection1;

        public string SelectedDirection1
        {
            get { return _selectedDirection1; }
            set { _selectedDirection1 = value; OnPropertyChanged(); }
        }

        private string _selectedDirection2;

        public string SelectedDirection2
        {
            get { return _selectedDirection2; }
            set { _selectedDirection2 = value; OnPropertyChanged(); }
        }

        private string _selectedDirection3;

        public string SelectedDirection3
        {
            get { return _selectedDirection3; }
            set { _selectedDirection3 = value; OnPropertyChanged(); }
        }

        private string _selectedDirection4;

        public string SelectedDirection4
        {
            get { return _selectedDirection4; }
            set { _selectedDirection4 = value; OnPropertyChanged(); }
        }

        private string _selectedDirection5;

        public string SelectedDirection5
        {
            get { return _selectedDirection5; }
            set { _selectedDirection5 = value; OnPropertyChanged(); }
        }

        #endregion

        #region Fields

        private ICommandRequesterViewModel _caller;
        private byte _pid;
        private int _maxCommandCount;

        #endregion

        #region Events

        public event EventHandler? CommandWindowClose;

        #endregion

        #region Delegate commands

        public DelegateCommand AcceptCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        #endregion

        #region Constructors

        public SetCommandsViewModel(ICommandRequesterViewModel caller, byte pid, Command[] commands)
        {
            _caller = caller;
            if (pid != 1 && pid != 2)
            {
                throw new ArgumentException("Player id must be either 1 or 2");
            }
            _pid = pid;
            _maxCommandCount = commands.Length;

            _displayCommandTypes = new Dictionary<string, CommandType>
            {
                { skip, CommandType.Skip },
                { turn, CommandType.Turn },
                { move, CommandType.Move },
                { fire, CommandType.Fire },
                { punch, CommandType.Punch }
            };

            _displayTurnDirectionTypes = new Dictionary<string, MoveDirection>
            {
                { right, MoveDirection.Right },
                { left, MoveDirection.Left }
            };

            _displayMoveDirectionTypes = new Dictionary<string, MoveDirection>(_displayTurnDirectionTypes)
            {
                { forward, MoveDirection.Forward },
                { backwards, MoveDirection.Backward }
            };

            _selectedCommand1 = _displayCommandTypes.Where(x => x.Value == commands[0].CommandType).First().Key;
            _selectedDirection1 = _displayMoveDirectionTypes.Where(x => x.Value == commands[0].Direction).First().Key;

            _selectedCommand2 = _displayCommandTypes.Where(x => x.Value == commands[1].CommandType).First().Key;
            _selectedDirection2 = _displayMoveDirectionTypes.Where(x => x.Value == commands[1].Direction).First().Key;

            _selectedCommand3 = _displayCommandTypes.Where(x => x.Value == commands[2].CommandType).First().Key;
            _selectedDirection3 = _displayMoveDirectionTypes.Where(x => x.Value == commands[2].Direction).First().Key;

            _selectedCommand4 = _displayCommandTypes.Where(x => x.Value == commands[3].CommandType).First().Key;
            _selectedDirection4 = _displayMoveDirectionTypes.Where(x => x.Value == commands[3].Direction).First().Key;

            _selectedCommand5 = _displayCommandTypes.Where(x => x.Value == commands[4].CommandType).First().Key;
            _selectedDirection5 = _displayMoveDirectionTypes.Where(x => x.Value == commands[4].Direction).First().Key;

            AcceptCommand = new DelegateCommand((param) =>
            {
                AssembleCommands();
            });

            CancelCommand = new DelegateCommand((param) =>
            {
                CloseSelf();
            });
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Collects the input from the comboboxes and assembles the commands.
        /// </summary>
        private void AssembleCommands()
        {
            Command[] commands = new Command[_maxCommandCount];

            commands[0] = GetCommandFromProperty(SelectedCommand1, SelectedDirection1);
            commands[1] = GetCommandFromProperty(SelectedCommand2, SelectedDirection2);
            commands[2] = GetCommandFromProperty(SelectedCommand3, SelectedDirection3);
            commands[3] = GetCommandFromProperty(SelectedCommand4, SelectedDirection4);
            commands[4] = GetCommandFromProperty(SelectedCommand5, SelectedDirection5);

            _caller.CommandsAssembled(_pid, commands);
            CloseSelf();
        }

        private Command GetCommandFromProperty(string selectedCommand, string selectedDirection)
        {
            Command result;

            CommandType commandType = _displayCommandTypes[selectedCommand];
            if (commandType == CommandType.Turn || commandType == CommandType.Move)
            {
                result = new Command(commandType, _displayMoveDirectionTypes[selectedDirection]);
            }
            else
            {
                result = new Command(commandType);
            }

            return result;
        }
        
        private void CloseSelf()
        {
            CommandWindowClose?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
