using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kameleon3.ViewModel
{
    public class KameleonField : ViewModelBase
    {
        private bool _isLocked;
        private string _player;
        private Color _color;


        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Player
        {
            get { return _player; }
            set
            {
                if (_player != value)
                {
                    _player = value;
                    OnPropertyChanged();
                }


            }
        }


        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (_isLocked != value)
                {
                    _isLocked = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsEnabled
        {
            get { return !IsLocked; }
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Position { get; set; }

        public DelegateCommand? StepCommand { get; set; }
    }
}
