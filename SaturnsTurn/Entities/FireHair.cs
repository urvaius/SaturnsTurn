using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SaturnsTurn.Utility;

namespace SaturnsTurn.Entities
{
    class FireHair : Entity
    {
        public override void Initialize(Viewport viewport, Texture2D texture, Vector2 position, int damage)
        {
            base.Initialize(viewport, texture, position, damage);
            damage = 1;
            Health = 50;
            entityMoveSpeed = 3f;
        }


        public override void Update(GameTime gameTime)
        {
            //the enemy always move to the left so decrement its xposition
            Position.X -= entityMoveSpeed;


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
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }

        

    }
}
