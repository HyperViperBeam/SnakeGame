using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SnakeGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D snakeTexture;
        Snake snake1;
        public Rectangle Pellet;
        Song ME;
        SoundEffect crunch;
        Random rand = new Random();
        float timeRemaining = 0.0f;
        float timeTotal = 0.2f;
        Color[] WhiteScheme = { Color.Black, Color.Purple, Color.Black, Color.Purple, Color.Black, Color.Purple}; 
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            snake1 = new Snake(600, 200, 22, WhiteScheme.ToList());
            crunch = Content.Load<SoundEffect>("Apple_Bite-Simon_Craggs-1683647397");
            ME = Content.Load<Song>("Guilty Gear Xrd -SIGN- OST Magnolia Éclair");
            MediaPlayer.Play(ME);
            snake1.effect = crunch;
            NewPellet();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            snakeTexture = Content.Load<Texture2D>(@"SQUARE");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();
            KeyboardState keyState = Keyboard.GetState();


            if (keyState.IsKeyDown(Keys.Down) && snake1.Facing != 2) snake1.Facing = 0;
            else if (keyState.IsKeyDown(Keys.Right) && snake1.Facing != 3) snake1.Facing = 1;
            else if (keyState.IsKeyDown(Keys.Up) && snake1.Facing != 0) snake1.Facing = 2;
            else if (keyState.IsKeyDown(Keys.Left) && snake1.Facing != 1) snake1.Facing = 3;

             
            if (snake1.CheckCollisions(this.Window)) snake1.isAlive = false;
           

            if (timeRemaining == 0.0f)
            {
                snake1.Update();
               

                if (snake1.DidEatPellet(Pellet)) NewPellet();

                timeRemaining = timeTotal;
            }
            timeRemaining = MathHelper.Max(0, timeRemaining -
           (float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }
        public void NewPellet()
        {
            Pellet = new Rectangle(
               rand.Next(25, this.Window.ClientBounds.Width - 25),
               rand.Next(25, this.Window.ClientBounds.Height - 25),
               16, 16);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();
            for (int i = 0; i < snake1.Count; i++)
            {

                spriteBatch.Draw(
                   snakeTexture,
                   new Rectangle((int)snake1.GetInstance(i).X, (int)snake1.GetInstance(i).Y, snake1.DrawSize, snake1.DrawSize),
                   new Rectangle(0, 0, 16, 16),
                   snake1.getDrawColor(i));
            }
            
            spriteBatch.Draw(
               snakeTexture,
               Pellet,
               Color.Purple);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
