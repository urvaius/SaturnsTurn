using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;



namespace SaturnsTurn
{
    class PowerUp
    {
        
        public int Shield { get; set; }
        public Texture2D PowerUpTexture;
        public Vector2 Position;
        public bool Active;
        public List<String> PowerUpType;
        public int Damage { get; set; }
        public string ShieldPowerUp;
        public string DamagePowerUp;
        public string PowerUpName;
        //todo add move speed etc..


        Viewport viewport;
        //get the width of the projectile
        public int Width
        {
            get { return PowerUpTexture.Width; }
        }

        //height of projectile
        public int Height
        {
            get { return PowerUpTexture.Height; }
        }
        //how fast projectile moves
        float PowerUpMoveSpeed;


        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position,string powerUpName)
        {


            PowerUpTexture = texture;
            Position = position;
            this.viewport = viewport;
            Active = true;
            this.Damage = Damage;
            PowerUpName = powerUpName;
            PowerUpType = new List<String>();
            PowerUpType.Add(PowerUpName);

            
            
            

            
            
        }
        private void UpdateDamage()
        {
            PowerUpMoveSpeed = 10f;
            Position.X += PowerUpMoveSpeed;
            if (Position.X + PowerUpTexture.Width / 2 > viewport.Width)
                Active = false;
        
        }


        public void Update(GameTime gameTime)
        {

            if (PowerUpType.Equals(DamagePowerUp))
            {
                UpdateDamage();
            }

            //projectiles move to the right always
            //Position.X += PowerUpMoveSpeed;
            //deacivate the bullet if it goes out of screen
            if (Position.X + PowerUpTexture.Width / 2 > viewport.Width)
                Active = false;

        }
        public void Draw(SpriteBatch spriteBatch)
        {





            spriteBatch.Draw(PowerUpTexture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);

        }



    }
}
