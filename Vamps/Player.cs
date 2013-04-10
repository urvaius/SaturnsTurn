using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vamps
{
    class Player
    {

        #region fields
        public Texture2D PlayerTexture3;
        public Vector2 Position3;
        public bool Active;
        public int Health;
        public int Width
        {
            get { return PlayerTexture3.Width; }
        }
        public int Height
        {
            get { return PlayerTexture3.Height; }
        }
        #endregion
        public void Initialize(Texture2D texture, Vector2 position)
        {
            PlayerTexture3 = texture;
            Position3 = position;
            Active = true;
            Health = 100;


        }

        public void Update()
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture3, Position3, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }


    }
}
