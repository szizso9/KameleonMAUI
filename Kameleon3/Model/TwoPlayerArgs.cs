using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kameleon3.Model
{
    public class TwoPlayerArgs
    {
        private Point _player1;
        private Point _player2;

        public Point Player1 { get { return _player1; } }
        public Point Player2 { get { return _player2; } }

        public TwoPlayerArgs(Point player1, Point player2)
        {
            _player1 = player1;
            _player2 = player2;
        }
    }
}
