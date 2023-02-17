using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kameleon3.Model
{
    public enum Player { Green, Red }

    public class KameleonEventArgs
    {
        private Player _player;
        private bool _isWon;

        public Player Player { get { return _player; } }

        public bool IsWon { get { return _isWon; } }

        public KameleonEventArgs(Player player, bool isWon)
        {
            _player = player;
            _isWon = isWon;
        }


    }
}
