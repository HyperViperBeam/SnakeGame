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
        Texture2D snakeTexture, titlescreen, GameOver;
        Snake snake1;
        public Rectangle Pellet;
        Song ME;
        SoundEffect crunch;
        Random rand = new Random();
        float timeRemaining = 0.0f;
        float timeTotal = 0.2f;
        Color[] WhiteScheme = { Color.Black, Color.Purple, Color.Black, Color.Purple, Color.Black, Color.Purple };
        enum GameStates { TitleScreen, Playing, GameOver };
        GameStates gameState = GameStates.TitleScreen;
        private float titleScreenTimer = 0f;
        private float titleScreenDelayTime = 1f;
        private Vector2 scoreLocation = new Vector2(20, 10);
        int score;
        SpriteFont Font1;
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
            ME = Content.Load<Song>("Guilty Gear Xrd -SIGN- OST Magnolia �clair");
            MediaPlayer.Play(ME);
            snake1.effect = crunch;
            NewPellet();
            titlescreen = Content.Load<Texture2D>("snakepic");
            Font1 = Content.Load<SpriteFont>(@"Pericles14");
            GameOver = Content.Load<Texture2D>("fail");

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

            switch (gameState)
            {
                case GameStates.TitleScreen:
                    titleScreenTimer +=
                        (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (titleScreenTimer >= titleScreenDelayTime)
                    {
                        if ((Keyboard.GetState().IsKeyDown(Keys.Space)) ||
                            (GamePad.GetState(PlayerIndex.One).Buttons.A ==
                            ButtonState.Pressed))
                        {

                            gameState = GameStates.Playing;
                        }
                    }
                    break;

                case GameStates.Playing:

                    if (keyState.IsKeyDown(Keys.Down) && snake1.Facing != 2) snake1.Facing = 0;
                    else if (keyState.IsKeyDown(Keys.Right) && snake1.Facing != 3) snake1.Facing = 1;
                    else if (keyState.IsKeyDown(Keys.Up) && snake1.Facing != 0) snake1.Facing = 2;
                    else if (keyState.IsKeyDown(Keys.Left) && snake1.Facing != 1) snake1.Facing = 3;


                    if (snake1.CheckCollisions(this.Window)|| snake1.isAlive == false)
                    {

                        snake1.isAlive = false;
                        gameState = GameStates.GameOver;

                    }
                    if (timeRemaining == 0.0f)
                    {
                        snake1.Update();


                        if (snake1.DidEatPellet(Pellet))
                        {
                            NewPellet();
                            score++;
                        }
                        timeRemaining = timeTotal;
                    }
                    timeRemaining = MathHelper.Max(0, timeRemaining -
                   (float)gameTime.ElapsedGameTime.TotalSeconds);
                    break;

                case GameStates.GameOver:
                    snake1 = new Snake(600, 200, 22, WhiteScheme.ToList());

                    break;

            }
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
            GraphicsDevice.Clear(Color.Blue);
            
            spriteBatch.Begin();
            if (gameState == GameStates.TitleScreen)
            {
                spriteBatch.Draw(titlescreen,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height), Color.White);
            }
        if (gameState == GameStates.Playing)
        {

            for (int i = 0; i < snake1.Count; i++)
            {

                spriteBatch.Draw(
                   snakeTexture,
                   new Rectangle((int)snake1.GetInstance(i).X, (int)snake1.GetInstance(i).Y, snake1.DrawSize, snake1.DrawSize),
                   new Rectangle(0, 0, 16, 16),
                   snake1.getDrawColor(i));
                spriteBatch.DrawString(
              Font1,
              "Score: " + score.ToString(),
              scoreLocation,
              Color.Cyan);
            }

            spriteBatch.Draw(
               snakeTexture,
               Pellet,
               Color.Purple);
        }
                if (gameState == GameStates.GameOver)
                {
                    spriteBatch.Draw(GameOver,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height), Color.White);

                    spriteBatch.DrawString(
                        Font1,
                        "Score: " + score.ToString(),
                        scoreLocation,
                        Color.Cyan);
                    

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        gameState = GameStates.TitleScreen;
                        score = 0;
                   }
                }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}