using RobotPigsGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RobotPigsGame.WPF.ViewModel
{
    /// <summary>
    /// Represents one field of the map.
    /// </summary>
    internal class MapField : ViewModelBase
    {
        private string _text = String.Empty;

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }

        private int _x;

        public int X
        {
            get { return _x; }
            set { _x = value; OnPropertyChanged(); }
        }

        private int _y;

        public int Y
        {
            get { return _y; }
            set { _y = value; OnPropertyChanged(); }
        }


        public void RefreshText()
        {
            Text = String.Empty;
        }

        public void RefreshText(FacingDirection direction, int pid)
        {
            switch (direction)
            {
                case FacingDirection.Up:
                    Text = "↑\n";
                    break;
                case FacingDirection.Right:
                    Text = "→\n";
                    break;
                case FacingDirection.Down:
                    Text = "↓\n";
                    break;
                case FacingDirection.Left:
                    Text = "←\n";
                    break;
                default:
                    throw new InvalidOperationException("Direction doesn't exist.");
            }

            Text += pid == 1 ? "P1" : "P2";
        }
    }
}
