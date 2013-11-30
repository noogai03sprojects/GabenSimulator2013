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
using System.Diagnostics;
using System.Timers;

namespace GabenSimulator2013
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GameState
        {
            Intro,
            Gameplay,
            GameOver
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;        

        TaskManager TaskManager;
        EmployeeManager EmployeeManager;
        TimingManager TimingManager;

        GameState State = GameState.Intro;

        Random random = new Random();

        public static Game1 Instance;

        public Game1()
        {
            Instance = this;
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
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            TimingManager = new TimingManager();
            TaskManager = new TaskManager();
            EmployeeManager = new EmployeeManager();
            TaskManager.EmployeeManager = EmployeeManager;
            EmployeeManager.TaskManager = TaskManager;

            TimingManager.Second += UpdateManagers;
            TimingManager.NewEmployee += NewEmployee;
            TimingManager.StartTimers();

            NameGenerator.SetNicknameMode(NameGenerator.NicknameMode.Always);

            //TimingManager.UpdateTimer_Elapsed
            EmployeeManager.AddEmployee();

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

            Art.LoadContent(Content);         
            //timer.Elapsed += Tick;

            

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
            Input.Update();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (TaskManager.HalfLife3.IsFinished)
                GameOver();

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Enter))
                State = GameState.Gameplay;

            EmployeeManager.Update();
            TaskManager.Update();

            // TODO: Add your update logic here

            //color = Color.Lerp(color, newColor, 0.01f);

            base.Update(gameTime);
        }

        public void GameOver()
        {
            State = GameState.GameOver;
        }

        public void UpdateManagers()
        {
            if (State == GameState.Gameplay)
            {
                EmployeeManager.Tick();
                TaskManager.Tick();
            }
            Console.WriteLine(NameGenerator.GetName());
        }

        public void NewEmployee()
        {
            if (State == GameState.Gameplay)
                EmployeeManager.AddEmployee();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (State == GameState.Intro)
            {
                

                spriteBatch.Begin();

                spriteBatch.DrawString(Art.Font, @"You are Gaben. You must delay the launch of Half-Life 3 at all costs.
Manage your employees by giving them other things to work on.
Unfortunately, Valve is growing so fast that another employee joins every 30 seconds.
Good luck.

Press enter to start.", new Vector2(20, 20), Color.Black);

                spriteBatch.End();
            }
            else if (State == GameState.Gameplay)
            {
                

                spriteBatch.Begin();

                spriteBatch.DrawString(Art.Font, "Half-Life 3 is " + TaskManager.HalfLife3.PercentComplete + "% complete.", new Vector2(20, 20), Color.Black);

                EmployeeManager.DrawEmployees(spriteBatch);
                TaskManager.DrawTasks(spriteBatch);

                spriteBatch.End();
            }
            else if (State == GameState.GameOver)
            {
                spriteBatch.Begin();

                spriteBatch.DrawString(Art.Font, @"GAME OVER
Half-Life 3 is released! Disaster!
You procrastinated for " + TimingManager.GameOver() + " seconds.", new Vector2(20, 20), Color.Black);

                spriteBatch.End();
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }        
    }
}
