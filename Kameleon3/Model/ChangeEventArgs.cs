using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kameleon3.Model
{
    public class ChangeEventArgs
    {
        private Point _p;

        public Point p { get { return _p; } }

        public ChangeEventArgs(Point p)
        {
            _p = p;
        }
    }
}
