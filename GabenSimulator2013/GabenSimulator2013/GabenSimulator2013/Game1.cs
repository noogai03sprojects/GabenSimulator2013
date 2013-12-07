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

        Hypeometer Hypeometer;

        public static Game1 Instance;

        public Vector2 ScreenSize;

        public Color Background = Color.White;

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
            ScreenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            TimingManager = new TimingManager();
            TaskManager = new TaskManager();
            EmployeeManager = new EmployeeManager();
            TaskManager.EmployeeManager = EmployeeManager;
            EmployeeManager.TaskManager = TaskManager;

            TimingManager.Second += UpdateManagers;
            TimingManager.NewEmployee += NewEmployee;
            TimingManager.StartTimers();

            NameGenerator.SetNicknameMode(NameGenerator.NicknameMode.Half);

            //TimingManager.UpdateTimer_Elapsed
            EmployeeManager.AddEmployee();

            Hypeometer = new Hypeometer(TaskManager, (int)ScreenSize.X - 40, 100, 40, 500, 10000);
            Hypeometer.AddEvent(new Hypeometer.HypeEvent("Shit", "Shit hits the fan", 3000, TestHypeEvent));

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

            if (State == GameState.Gameplay)
            {
                EmployeeManager.Update();
                TaskManager.Update();
                Hypeometer.Update();
            }

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
                Hypeometer.Tick();
            }
            //Console.WriteLine(NameGenerator.GetName());
        }

        public void NewEmployee()
        {
            if (State == GameState.Gameplay)
                EmployeeManager.AddEmployee();
        }

        public void TestHypeEvent()
        {
            Background = Color.Red;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Background);
            if (State == GameState.Intro)
            {
                

                spriteBatch.Begin();

                spriteBatch.DrawString(Art.Font, @"Mr Newell! I have a task for you!
You must delay the launch of Half-Life 3 at all costs.

You will do this by managing your employees. Give them other things to work on. Anything, but not Half-Life 3!
Unfortunately, Valve is buying entire student dev teams so fast that another employee will join every 30 seconds.

However, the bright light of indie development is your friend: when an employee has completed more than 3 projects,
he/she will abandon the mighty battleship that is Valve Software and proceed to try their luck in a rowing boat
which they constructed themself out of spare parts. They will then proceed to try and either make a Cave Story clone
or Minecraft 2.

Good luck, Mr Newell. You'll need it.

Press enter to start.", new Vector2(20, 20), Color.Black);

                spriteBatch.End();
            }
            else if (State == GameState.Gameplay)
            {
                Vector2 size = Art.Font.MeasureString("Half-Life 3 is " + TaskManager.HalfLife3.PercentComplete + "% complete.");
                Rectangle HL3 = new Rectangle(20, 20, (int)size.X, (int)size.Y);

                spriteBatch.Begin();               

                EmployeeManager.DrawEmployees(spriteBatch);
                TaskManager.DrawTasks(spriteBatch);
                Hypeometer.Draw(spriteBatch);

                spriteBatch.Draw(Art.Pixel, HL3, Color.ForestGreen);
                spriteBatch.DrawString(Art.Font, "Half-Life 3 is " + TaskManager.HalfLife3.PercentComplete + "% complete.", new Vector2(20, 20), Color.Black);

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
