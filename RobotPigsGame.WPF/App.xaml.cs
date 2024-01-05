using Microsoft.Win32;
using RobotPigsGame.Model;
using RobotPigsGame.Persistence;
using RobotPigsGame.WPF.View;
using RobotPigsGame.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RobotPigsGame.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GameView _mainWindow = null!;
        private GameViewModel _gameViewModel = null!;
        private SetCommandsView? _setCommandsView = null;
        private SetCommandsViewModel? _setCommandsViewModel = null;

        public App()
        {
            Startup += AppStartup;
        }

        /// <summary>
        /// Initializes everything on the startup of the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppStartup(object sender, StartupEventArgs e)
        {
            _gameViewModel = new GameViewModel();
            _mainWindow = new GameView();
            _mainWindow.DataContext = _gameViewModel;
            _gameViewModel.SetCommands += GetCommands;
            _gameViewModel.Model.GameWon += GameOver;
            _mainWindow._exit.Click += AppClose;
            _mainWindow.Show();
            _mainWindow._saveGame.Click += SaveGame;
            _mainWindow._loadGame.Click += LoadGame;
            _mainWindow._saveGame.IsEnabled = true;
            _gameViewModel.CanPlayTurn = true;
        }

        /// <summary>
        /// Prompts the user with the set commands window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetCommands(object? sender, SetCommandArgs e)
        {
            if (_setCommandsView != null || _setCommandsViewModel != null)
            {
                return;
            }
            _setCommandsViewModel = new SetCommandsViewModel(_gameViewModel, e.Pid, e.InitialCommands);
            _setCommandsView = new SetCommandsView();
            _setCommandsView.Owner = _mainWindow;
            _setCommandsViewModel.CommandWindowClose += CloseCommandWindow;
            _setCommandsView.DataContext = _setCommandsViewModel;
            _setCommandsView.Show();
        }

        private void CloseCommandWindow(object? sender, EventArgs e)
        {
            if (_setCommandsView == null || _setCommandsViewModel == null)
            {
                return;
            }
            _setCommandsView.Close();
            _setCommandsView = null;
            _setCommandsViewModel = null;

        }

        /// <summary>
        /// Notifies the players that the game has finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Describes the winner</param>
        private void GameOver(object? sender, int e)
        {
            string text = String.Empty;
            switch (e)
            {
                case -1:
                    text = "A játék döntetlen.";
                    break;
                case 1:
                    text = "Az 1. játékos nyert.";
                    break;
                case 2:
                    text = "A 2. játékos nyert.";
                    break;
                default:
                    break;
            }
            MessageBox.Show(text, "Játék vége.", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void SaveGame(object? sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Robot malcok játék mentés  (*.rpg)|*.rpg"

            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    await _gameViewModel.Model.SaveGameAsync(saveDialog.FileName);
                }
                catch (RobotPigsDataException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Sikertelen mentés!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void LoadGame(object? sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "Robot malcok játék mentés  (*.rpg)|*.rpg"
            };

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    await _gameViewModel.Model.LoadGameAsync(openDialog.FileName);
                    // InitializeMap(null, _gameViewModel.Model.MapSizeValue);
                    _mainWindow._saveGame.IsEnabled = true;
                    _gameViewModel.CanPlayTurn = true;
                }
                catch (RobotPigsDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Sikertelen betöltés!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AppClose(object? sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
