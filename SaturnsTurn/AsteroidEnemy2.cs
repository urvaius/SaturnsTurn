using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SaturnsTurn
{
    class AsteroidEnemy2
    

        {
        //projectile image
        public Texture2D Texture;
        public Vector2 Position;
        public bool Active;
        public int Damage { get; set; }
       
        public int Health;
        //the damage the enemy inflicts
        
        public bool OnScreen;
        //the score you will get from killing the enemy
        public int Value;


        Viewport viewport;
        //get the width of the projectile
        public int Width
        {
            get { return Texture.Width; }
        }

        //height of projectile
        public int Height
        {
            get { return Texture.Height; }
        }
        //how fast projectile moves
        public float asteroidMoveSpeed;
        
        float RotationAngle;
        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
        {
            Texture = texture;
            this.Position = position;
            this.viewport = viewport;
            Active = true;
            Health = 20;
            Damage = 20;
            Value = 200;
            OnScreen = true;
           
            asteroidMoveSpeed = 1f;
        }

        public void Update(GameTime gameTime)
        {
            //projectiles move to the right always
            Position.X -= asteroidMoveSpeed;
            //deacivate the bullet if it goes out of screen
            if (Position.X < -Width)
            {
                //by setting the active flat to false the game will remove this object

                //active game list
                // Active = false;
                OnScreen = false;
            }
            if (Health <= 0)
            {
                Active = false;
                OnScreen = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);

        }
    }
    
}
