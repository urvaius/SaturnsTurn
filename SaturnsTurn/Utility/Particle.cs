using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace SaturnsTurn.Utility
{
    class Particle
    {

        public Vector2 Position { get; set; }
        public Vector2 Destination { get; set; }

        //add in some velocity color and rotation as well

        public void Update()
        {
            //move 1/60th of the way to your destination
            Position += (Destination - Position / 60f);
        }
    }
}
