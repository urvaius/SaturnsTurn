using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SaturnsTurn.Utility;
namespace SaturnsTurn
{
    class Player
    {

        #region fields
        //public Texture2D PlayerTexture3;switch to playeranimation
        public Animation PlayerAnimation;
        public Vector2 Position3;
        public bool Active;
        public int Health;
        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }
        #endregion
        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;
           // PlayerTexture3 = texture;
            Position3 = position;
            Active = true;
            Health = 100;


        }

        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = Position3;
            PlayerAnimation.Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }


    }
}
