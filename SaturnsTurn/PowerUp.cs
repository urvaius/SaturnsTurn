using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace SaturnsTurn
{
    class PowerUp
    {
        
        public int Shield { get; set; }
        public Texture2D powerUpTexture;
        public Vector2 Position;
        public bool Active;
        public int Damage { get; set; }


        Viewport viewport;
        //get the width of the projectile
        public int Width
        {
            get { return powerUpTexture.Width; }
        }

        //height of projectile
        public int Height
        {
            get { return powerUpTexture.Height; }
        }
        //how fast projectile moves
        float PowerUpMoveSpeed;


        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
        {
            powerUpTexture = texture;
            Position = position;
            this.viewport = viewport;
            Active = true;
            this.Damage = Damage;

            PowerUpMoveSpeed = 10f;
        }

        public void Update()
        {
            //projectiles move to the right always
            Position.X += PowerUpMoveSpeed;
            //deacivate the bullet if it goes out of screen
            if (Position.X + powerUpTexture.Width / 2 > viewport.Width)
                Active = false;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(powerUpTexture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);

        }



    }
}
