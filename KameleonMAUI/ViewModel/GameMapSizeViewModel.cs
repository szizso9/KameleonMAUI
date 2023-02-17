using Kameleon3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kameleon3.ViewModel
{
    public class GameMapSizeViewModel : ViewModelBase
    {
        private MapType _maptype;

        public MapType MapType
        {
            get => _maptype;
            set
            {
                _maptype = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MapTypeText));
            }
        }

        public string MapTypeText { get {
                switch (_maptype)
                {
                    case MapType.Little:
                        return "Kicsi";
                    case MapType.Medium:
                        return "Közepes";
                    case MapType.Large:
                        return "Nagy";
                    default:
                        break;
                }
                return "Közepes";

            } }
    }
}
