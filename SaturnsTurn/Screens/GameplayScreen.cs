#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;

using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SaturnsTurn;
using SaturnsTurn.Utility;
using System.Collections.Generic;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont scoreFont;
        SpriteFont gameFont;
        //Texture2D backgroundStart;
        float playerMoveSpeed;
        Player player;
        Texture2D mainBackground;
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;
        ScrollingBackground star1;
        ScrollingBackground star2;


        //enemies
        Texture2D enemyTexture;
        Texture2D balloonEnemyTexture;
        List<Enemy> enemies;
        List<GreenMineEnemy> balloonEnemies;
        Texture2D projectileTexture;
        List<Projectile> projectiles;
        TimeSpan fireTime;
        //TimeSpan previousFireTime;
        //the rate for enemies to appear
        TimeSpan enemySpawnTime;
        TimeSpan balloonEnemySpawnTime;
        TimeSpan previousSpawnTime;
        TimeSpan previousBalloonSpawnTime;
        TimeSpan powerUpSpawnTime;
        TimeSpan previousPowerUpSpawnTime;
        //explosions
        Texture2D explosion1Texture;
        List<Animation> explosions;
        Texture2D powerupDamageTexture;
        List<PowerUp> damagePowerUps;
        List<PowerUp> shieldPowerUps;
        //string damagePowerUp;

        MouseState currentMouseState;
        MouseState previousMouseState;
        Random randomPowerUp;
        //Vector2 powerUpPosition = new Vector2(100, 100); randonm
        Random randomEnemy;
        //Vector2 enemyPosition = new Vector2(100, 100);  not needed doit randomly

        Random random = new Random();

        float pauseAlpha;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);


        }



        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");


            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();
            star1 = new ScrollingBackground();
            star2 = new ScrollingBackground();
            gameFont = content.Load<SpriteFont>(@"Graphics\gamefont");

            //load paralzxing background
            bgLayer1.Initialize(content, @"Graphics\bgLayer1", ScreenManager.GraphicsDevice.Viewport.Width, -1);
            bgLayer2.Initialize(content, @"Graphics\bglayer2", ScreenManager.GraphicsDevice.Viewport.Width, -2);
           // try scrolling
            star1.Initialize(content, @"Graphics\Backgrounds\star1", ScreenManager.GraphicsDevice.Viewport.Width, -1);
           // star2.Initialize(content, @"Graphics\Backgrounds\star6", ScreenManager.GraphicsDevice.Viewport.Width, -1);

            
            
            //load enemies textures
            enemyTexture = content.Load<Texture2D>(@"Graphics\mineAnimation");
            balloonEnemyTexture = content.Load<Texture2D>(@"Graphics\mineGreenAnimation");
            powerupDamageTexture = content.Load<Texture2D>(@"Graphics\powerup");
            mainBackground = content.Load<Texture2D>(@"Graphics\mainbackground");
            scoreFont = content.Load<SpriteFont>(@"Graphics\gameFont");
            //initialize projectile
            projectiles = new List<Projectile>();




            damagePowerUps = new List<PowerUp>();
            //set the laser to fie every quarter second
            fireTime = TimeSpan.FromSeconds(.15f);

            //load projectile
            projectileTexture = content.Load<Texture2D>(@"Graphics\laser");

            // explosions
            explosions = new List<Animation>();
            explosion1Texture = content.Load<Texture2D>(@"Graphics\explosion");


            //initialize enemies list etc..

            enemies = new List<Enemy>();
            //seperate enemies to balloon to add other ones.
            balloonEnemies = new List<GreenMineEnemy>();
            //set enemy spawn time keepers to zero
            previousSpawnTime = TimeSpan.Zero;
            previousBalloonSpawnTime = TimeSpan.Zero;
            //used to determine how fast enemy respawns
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            balloonEnemySpawnTime = TimeSpan.FromSeconds(5.0f);
            powerUpSpawnTime = TimeSpan.FromSeconds(5.0f);
            previousPowerUpSpawnTime = TimeSpan.Zero;
            //initialize random number for enemies
            randomEnemy = new Random();
            randomPowerUp = new Random();
            //initialize a new player. not sure why have to do it here. 
            player = new Player();

            //try projectile hee



            //score = 0;

            //use animation now.

            Animation playerAnimation = new Animation();
            Texture2D playerTexture = content.Load<Texture2D>(@"Graphics\shipAnimation");

            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);
            Vector2 playerPosition3 = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y + ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerAnimation, playerPosition3);


            //----animation section ^

            // player.Initialize(content.Load<Texture2D>(@"Graphics\player"), playerPosition3);
            playerMoveSpeed = 8.0f;
            Thread.Sleep(1000);


            //playmusic maybe here
            AudioManager.PlayMusic("gamemusic");
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {


                previousMouseState = currentMouseState;
                currentMouseState = Mouse.GetState();




                UpdatePlayer(gameTime);
                UpdateProjectiles();
                bgLayer1.Update();
                bgLayer2.Update();
                //udate scrolling background w \\todo
                star1.Update();
               // star2.Update();
                //update the enemies
                UpdateEnemies(gameTime);
                UpdateCollision();
                UpdateExplosions(gameTime);

                UpdatePowerUp(gameTime);

            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];
            KeyboardState lastKeyboardState = input.LastKeyboardStates[playerIndex];
            GamePadState lastGamePadState = input.LastGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                Vector2 movement = Vector2.Zero;
                //windows 8 gestures monogame
                while (TouchPanel.IsGestureAvailable)
                {
                    GestureSample gesture = TouchPanel.ReadGesture();

                    if (gesture.GestureType == GestureType.FreeDrag)
                    {
                        player.Position3 += gesture.Delta;


                    }
                }
                //mouse

                Vector2 mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);


                if (currentMouseState.RightButton == ButtonState.Pressed)
                {
                    Vector2 posDelta = mousePosition - player.Position3;
                    posDelta.Normalize();
                    posDelta = posDelta * playerMoveSpeed;
                    player.Position3 = player.Position3 + posDelta;

                }
                // Otherwise move the player position.


                if (keyboardState.IsKeyDown(Keys.A))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.D))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.W))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.S))
                    movement.Y++;

                Vector2 thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;
                // Make sure that the player does not go out of bounds
                player.Position3.X = MathHelper.Clamp(player.Position3.X, 0, ScreenManager.GraphicsDevice.Viewport.Width - player.Width);
                player.Position3.Y = MathHelper.Clamp(player.Position3.Y, 0, ScreenManager.GraphicsDevice.Viewport.Height - player.Height);






                //fire weapon normal

                if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released
                    || keyboardState.IsKeyDown(Keys.Space) && lastKeyboardState.IsKeyUp(Keys.Space))
                {
                    AddProjectile(player.Position3 + new Vector2(player.Width / 2, 0));
                    AudioManager.PlaySound("laserSound");
                }


                if (movement.Length() > 1)
                    movement.Normalize();



                //new player move
                player.Position3 += movement * playerMoveSpeed;
            }
        }

       


        private void UpdatePlayer(GameTime gameTime)
        {

            player.Update(gameTime);





            if (player.Health <= 0)
            {
                player.Health = 100;
                player.Score = 0;
            }
        }
        private void UpdateProjectiles()
        {
            //update projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {

               

                projectiles[i].Update();
                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }

            }
        }
                

        private void UpdateCollision()
        {
            //use the rectangles built in intersect funtion to determine if two objects are overlapping
            Rectangle playerRectangle;
            Rectangle enemyRectangle2;
            Rectangle projectileRectangle;
            Rectangle damagePowerUpRectangle;
            //only create the rectangle once for the player
            playerRectangle = new Rectangle((int)player.Position3.X, (int)player.Position3.Y, player.Width, player.Height);

            //damage powerup collisiong
            for (int i = 0; i < damagePowerUps.Count; i++)
            {
                damagePowerUpRectangle = new Rectangle((int)damagePowerUps[i].Position.X, (int)damagePowerUps[i].Position.Y, damagePowerUps[i].Width, damagePowerUps[i].Height);
                if (playerRectangle.Intersects(damagePowerUpRectangle))
                {

                    //todo add powerup to do stuff

                    // projectiles[i].Damage = 10;

                    //add damage 
                    if (player.DamageMod <= 20)
                    {
                        player.DamageMod += 5;
                    }
                    


                    damagePowerUps[i].Active = false;
                }

            }
            //do collision with balloonenemy and player
            for (int i = 0; i < balloonEnemies.Count; i++)
            {
                enemyRectangle2 = new Rectangle((int)balloonEnemies[i].Position.X, (int)balloonEnemies[i].Position.Y, balloonEnemies[i].Width, balloonEnemies[i].Height);
                if (playerRectangle.Intersects(enemyRectangle2))
                {


                    player.Health -= balloonEnemies[i].Damage;
                    balloonEnemies[i].Health -= player.Damage;
                    if (player.Health <= 0)
                        player.Active = false;
                }
            }
            //do the collision between the player and the enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemyRectangle2 = new Rectangle((int)enemies[i].Position.X, (int)enemies[i].Position.Y, enemies[i].Width, enemies[i].Height);
                //determine if the thwo objects collided with each other
                if (playerRectangle.Intersects(enemyRectangle2))
                {
                    //subtracth the health from the player based on enemy damage
                    player.Health -= enemies[i].Damage;
                    //destroy the enemy
                    enemies[i].Health -= player.Damage;

                    //if the player health is less than zero we died
                    if (player.Health <= 0)
                        player.Active = false;
                }
            }

            //projectile vs balloon enemy collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < balloonEnemies.Count; j++)
                {
                    //create the rectanles we need to determine if we collided with each other
                    projectileRectangle = new Rectangle((int)projectiles[i].Position.X - projectiles[i].Width / 2, (int)projectiles[i].Position.Y - projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);

                    enemyRectangle2 = new Rectangle((int)balloonEnemies[j].Position.X - balloonEnemies[j].Width / 2, (int)balloonEnemies[j].Position.Y - balloonEnemies[j].Height / 2, balloonEnemies[j].Width, balloonEnemies[j].Height);
                    //determine if the two objects collide with each other
                    if (projectileRectangle.Intersects(enemyRectangle2))
                    {
                        balloonEnemies[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }




            //projectile vs enemy collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    //create the rectanles we need to determine if we collided with each other
                    projectileRectangle = new Rectangle((int)projectiles[i].Position.X - projectiles[i].Width / 2, (int)projectiles[i].Position.Y - projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);

                    enemyRectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2, (int)enemies[j].Position.Y - enemies[j].Height / 2, enemies[j].Width, enemies[j].Height);
                    //determine if the two objects collide with each other
                    if (projectileRectangle.Intersects(enemyRectangle2))
                    {
                        enemies[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }

        }

        
        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                {
                    explosions.RemoveAt(i);
                }
            }
        }
        
        //addding powerup
        private void UpdatePowerUp(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousPowerUpSpawnTime > powerUpSpawnTime)
            {
                previousPowerUpSpawnTime = gameTime.TotalGameTime;
                //todo
                AddDamagePowerUp();

            }

            for (int i = damagePowerUps.Count - 1; i >= 0; i--)
            {
                damagePowerUps[i].Update(gameTime);
                if (damagePowerUps[i].Active == false)
                {
                    damagePowerUps.RemoveAt(i);
                    //todo do something
                }
            }
        }
        private void UpdateEnemies(GameTime gameTime)
        {
            //spawn a new enemy every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;
                //add the enemy
                AddEnemy();

            }
            //spawn ballon enemies every 5 sec
            if (gameTime.TotalGameTime - previousBalloonSpawnTime > balloonEnemySpawnTime)
            {
                previousBalloonSpawnTime = gameTime.TotalGameTime;
                //add abllon enemies
                AddBalloonEnemy();
            }

            //update balloon enemies
            for (int i = balloonEnemies.Count - 1; i >= 0; i--)
            {
                balloonEnemies[i].Update(gameTime);
                if (balloonEnemies[i].Active == false)
                {
                    AddExplosion(balloonEnemies[i].Position);
                    AudioManager.PlaySound("explosionSound");
                    player.Score += balloonEnemies[i].Value;

                    balloonEnemies.RemoveAt(i);

                }

                else if (balloonEnemies[i].Active == true && balloonEnemies[i].OnScreen == false)
                {
                    balloonEnemies.RemoveAt(i);
                }


            }
            //update the enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);
                if (enemies[i].Active == false)
                {
                    AddExplosion(enemies[i].Position);
                    AudioManager.PlaySound("explosionSound");
                    player.Score += enemies[i].Value;
                    enemies.RemoveAt(i);
                }
                else if (enemies[i].Active == true && enemies[i].OnScreen == false)
                {
                    enemies.RemoveAt(i);
                }
            }
        }


        private void AddProjectile(Vector2 position)
        {

            //moving to top
            int projDamage = player.DamageMod + 3;
            Projectile projectile = new Projectile();
            projectile.Initialize(ScreenManager.GraphicsDevice.Viewport, projectileTexture, position, projDamage);




            projectiles.Add(projectile);
        }
        //this addballoonenemy probably be taken out. but can add more later
        private void AddBalloonEnemy()
        {
            //create the animation object
            Animation balloonEnemyAnimation = new Animation();
            //initizlize theanimation with the correct ahimation information
            balloonEnemyAnimation.Initialize(balloonEnemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);
            //randomly generate the position of the enemy or later change this to a specific spot
            Vector2 position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, randomEnemy.Next(100, ScreenManager.GraphicsDevice.Viewport.Height - 100));
            //create an enemy
            GreenMineEnemy balloonEnemy = new GreenMineEnemy();
            //initizlize the enemy
            balloonEnemy.Initialize(balloonEnemyAnimation, position);
            // add the enemy to the active enemies list
            balloonEnemies.Add(balloonEnemy);

        }
        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosion1Texture, position, 134, 134, 12, 45, Color.White, 1f, false);
            explosions.Add(explosion);
        }

        private void AddDamagePowerUp()
        {

            //todo

            PowerUp damagePowerUp = new PowerUp();
            Vector2 position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + powerupDamageTexture.Width / 2, randomPowerUp.Next(100, ScreenManager.GraphicsDevice.Viewport.Height - 75));
            damagePowerUp.Initialize(ScreenManager.GraphicsDevice.Viewport, powerupDamageTexture, position, "DamagePowerUp");

            damagePowerUps.Add(damagePowerUp);

        }
        private void AddEnemy()
        {
            //create the animation object
            Animation enemyAnimation = new Animation();
            //Animation balloonEnemyAnimation = new Animation();
            //initizlize theanimation with the correct ahimation information
            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);
            // balloonEnemyAnimation.Initialize(balloonEnemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);
            //randomly generate the position of the enemy or later change this to a specific spot
            Vector2 position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, randomEnemy.Next(100, ScreenManager.GraphicsDevice.Viewport.Height - 100));
            // Vector2 balloonPosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + balloonEnemyTexture.Width / 2, randomEnemy.Next(100, ScreenManager.GraphicsDevice.Viewport.Height - 100));

            //create an enemy
            Enemy enemy = new Enemy();
            //Enemy balloonEnemy = new Enemy();
            //initizlize the enemy
            enemy.Initialize(enemyAnimation, position);
            //balloonEnemy.Initialize(balloonEnemyAnimation, balloonPosition);
            // add the enemy to the active enemies list
            enemies.Add(enemy);
            //balloonEnemies.Add(balloonEnemy);
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();



            //spriteBatch.Draw(backgroundStart, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            //spriteBatch.DrawString(gameFont, "// TODO", playerPosition, Color.Green);
            //draw new paralaxing background
            spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

            //draw sroller
            star1.Draw(spriteBatch);
          //  star2.Draw(spriteBatch);
            //dont draw these for now
           // bgLayer1.Draw(spriteBatch);
           // bgLayer2.Draw(spriteBatch);
            //draw the score
            spriteBatch.DrawString(scoreFont, "score: " + player.Score, new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);

            //draw teh player health
            spriteBatch.DrawString(scoreFont, "Health: " + player.Health, new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y + 35), Color.White);


            //draw the enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);

            }

            //draw balloon enemies
            for (int i = 0; i < balloonEnemies.Count; i++)
            {
                balloonEnemies[i].Draw(spriteBatch);
            }
            //working now draw player from player class.
            player.Draw(spriteBatch);
            //draw projectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }

            //draw explosions
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(spriteBatch);
            }

            //draw damage powerup 
            for (int i = 0; i < damagePowerUps.Count; i++)
            {
                damagePowerUps[i].Draw(spriteBatch);
            }


            //spriteBatch.Draw(playerTexture, playerPosition, Color.White);
            // spriteBatch.DrawString(gameFont, "Insert Gameplay Here",
            //                     enemyPosition, Color.DarkRed);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
