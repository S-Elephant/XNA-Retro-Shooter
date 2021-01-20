using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;

namespace RetroShooter
{
    public class Rectangle2
    {
        public Rectangle Relative;
        public Rectangle Absolute;

        public Rectangle2(Vector2 location, Rectangle relative)
        {
            Relative = relative;
            Absolute = new Rectangle(location.Xi(), location.Yi(), relative.Width, relative.Height);
        }

        public void SetNewLocation(Point newLoc)
        {
            Absolute.Location = new Point(Relative.X + newLoc.X, Relative.Y + newLoc.Y);
        }
    }
}
