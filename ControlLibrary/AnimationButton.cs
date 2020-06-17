using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ControlLibrary
{
    public class AnimationButton : Button
    {
        private Point _point;
        public Point DiffDistance
        {
            get { return _point; }
            set { _point = value; }
        }
    }
}
