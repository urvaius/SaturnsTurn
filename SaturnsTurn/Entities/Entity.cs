using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SaturnsTurn.Utility;


namespace SaturnsTurn.Entities
{
    public abstract class Entity
    {

        #region fields
        //animation for the enemy
        public Animation EntityAnimation;
        //the position of enemy of ship
        public Vector2 Position;
        //state of the shipo
        public bool Active;
        public int Health;
        //the damage the enemy inflicts
        public int Damage;
        public bool OnScreen;
        //the score you will get from killing the entity
        public int Value;
        //get the width of the enemy ship
        public int Width
        {
            get { return EntityAnimation.FrameWidth; }
        }

        //get the height of the enemy ship
        public int Height
        {
            get { return EntityAnimation.FrameHeight; }
        }
        //the speed of the enemy
        public float entityMoveSpeed;
        #endregion


        public abstract void Initialize(Animation animation, Vector2 position)
        {

            //abstract or virtual method lookup
            //load enemy ship texture
            this.EntityAnimation = animation;
            //set the position of enemy
            this.Position = position;
            // initizlize the enemy to be active
            Active = true;
            //set the helath
            Health = 0;
            //set damage it can do
            Damage = 0;
            // set how fast the enemy moves
            entityMoveSpeed = 0f;
            //set the score value
            Value = 100;
            OnScreen = true;
        }

    }
}
