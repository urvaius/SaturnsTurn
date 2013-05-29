using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SaturnsTurn.Utility;

namespace SaturnsTurn
{
    class AsteroidEnemy : Enemy
    {

        
        public virtual void Initialize(Animation animation,Vector2 position)
        {

            //load enemy ship texture
            EnemyAnimation = animation;
            //set the position of enemy
            Position = position;
            // initizlize the enemy to be active
            Active = true;
            //set the helath
            Health = 30;
            //set damage it can do
            Damage = 25;
            // set how fast the enemy moves
            enemyMoveSpeed = 3f;
            //set the score value
            Value = 200;
            OnScreen = true;

        }
   


    }
}
