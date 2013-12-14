using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GabenSimulator2013
{
    class TaskManager
    {
        public EmployeeManager EmployeeManager;

        public static Task HalfLife3;
        public static Task None;

        public List<Task> Tasks;
        public int MaxTasks;
        public int TaskCount;

        public List<GameName> GameNames;
        bool bAreGameNamesExhausted = false;
        public List<NumericalGameName> NumericalGameNames;
        List<GameName> ExhaustedNames;

        private Random random = new Random();

        private int SelectedIndex = -1;
        private bool bSelecting = false;

        
        Rectangle BaseArea = new Rectangle(400, 85, 402, 500);
        Vector2 BaseDrawPosition = new Vector2();
        Color BaseColour = new Color(83, 83, 83);

        Employee SelectedEmployee = null;

        int delayTimer = 5;
        public bool bUpdating = false;

        Rectangle AddTaskRectangle = new Rectangle(0, 40, 150, 40);
        Rectangle RoundRectArea = new Rectangle(0, 30, 400, 30);
        Rectangle FrameArea = new Rectangle(0, 0, 400, 30);
        Rectangle BorderArea = new Rectangle(0, 60, 402, 32);

        float ScrollOffset = 0;
        float DesiredScrollOffset = 0;
        float SelectorTop, SelectorBottom = 0;

        public struct GameName
        {
            public string Name;
            public bool Used;
            public GameName(string name, bool used)
            {
                Name = name;
                Used = used;
            }
        }
        public struct NumericalGameName
        {
            public string Name;
            public int Number;
            public NumericalGameName(string name, int number)
            {
                Name = name;
                Number = number;
            }
        }

        public TaskManager()
        {
            HalfLife3 = new Task(10000, "Half-Life 3", this);
            None = new Task(-1, "No Task", this);
            Tasks = new List<Task>();
            GameNames = new List<GameName>();
            NumericalGameNames = new List<NumericalGameName>();
            ExhaustedNames = new List<GameName>();
            LoadNames();
            MaxTasks = 3;
            BaseDrawPosition = new Vector2(BaseArea.Left, BaseArea.Top);
        }

        public void LoadNames()
        {
            using (StreamReader reader = new StreamReader("GameNames.txt"))
            {
                while (!reader.EndOfStream)
                {
                    //Console.WriteLine(reader.ReadLine());
                    GameNames.Add(new GameName(reader.ReadLine(), false));
                    
                }
            }
            using (StreamReader reader = new StreamReader("NumericalGameNames.txt"))
            {
                while (!reader.EndOfStream)
                {
                    NumericalGameNames.Add(new NumericalGameName(reader.ReadLine(), 2));
                }
            }
        }

        public void AddTask()
        {
            //if (TaskCount < MaxTasks)
            {
                int requiredWork = (int)random.NextFloat(1000, 2000);
                if (!bAreGameNamesExhausted)
                {
                    GameName name = new GameName();

                    //do 
                    int i;
                    do
                    {
                        i = random.Next(GameNames.Count);
                        name = GameNames[i];
                        if (name.Used)
                        {
                            //if (ExhaustedNames.Contains(name))
                            ExhaustedNames.Add(name);
                        }
                        else
                        {
                            break;
                        }
                    } while (true);
                    //if (name.Used)

                    if (ExhaustedNames.Count >= GameNames.Count)
                        bAreGameNamesExhausted = true;

                    name.Used = true;
                    GameNames[i] = name;
                    Tasks.Add(new Task(requiredWork, name.Name, this));
                    //Hypeometer.Instance.AddHype(requiredWork);
                }
                else
                {
                    int i = random.Next(NumericalGameNames.Count);
                    NumericalGameName name = NumericalGameNames[i];

                    Tasks.Add(new Task(requiredWork, name.Name + " " + name.Number, this));
                    name.Number++;
                    NumericalGameNames[i] = name;
                }
                TaskCount++;
            }
        }

        public void Update()
        {
            if (delayTimer > 0)
                delayTimer--;
            if (Input.IsKeyPressed(Keys.Down))
            {
                if (SelectedIndex < Tasks.Count - 1 && bSelecting)
                    SelectedIndex++;
                if (SelectorBottom + BaseDrawPosition.Y > GameRoot.Instance.ScreenSize.Y)
                {
                    DesiredScrollOffset -= 30;
                }                
                Console.WriteLine(SelectedIndex);
            }
            if (Input.IsKeyPressed(Keys.Up))
            {
                if (SelectedIndex > -1 && bSelecting)
                {
                    SelectedIndex--;
                    if (SelectorTop + BaseDrawPosition.Y < BaseDrawPosition.Y)
                    {
                        DesiredScrollOffset += 30;
                    }
                }
                Console.WriteLine(SelectedIndex);
            }
            if (Input.IsKeyPressed(Keys.Enter))
            {
                if (bSelecting && delayTimer <= 0 && !bUpdating)
                {
                    if (SelectedIndex < 0)
                    {
                        AddTask();
                        return;
                    }
                    else
                    {
                        if (Tasks.Count <= 0)
                        {
                            SelectedIndex = -1;
                            return;
                        }
                        if (SelectedIndex < Tasks.Count)
                            SelectedEmployee.SetTask(Tasks[SelectedIndex]);
                        bSelecting = false;
                        SelectedEmployee = null;
                        EmployeeManager.FinishSelecting();
                    }
                }
            }

            if (SelectedIndex == -1)
            {
                if (bSelecting)
                {
                    if (TaskCount < MaxTasks)
                        AddTaskRectangle.Y = 0;
                    else
                        AddTaskRectangle.Y = 120;
                }                
            }
            else
            {
                if (TaskCount < MaxTasks)
                    AddTaskRectangle.Y = 40;
                else
                    AddTaskRectangle.Y = 80;
            }

            ScrollOffset = MathHelper.Lerp(ScrollOffset, DesiredScrollOffset, 0.15f);
        }

        public void Tick()
        {
            bUpdating = true;
            foreach (Task task in Tasks)
            {
                if (task.IsFinished)
                {
                    task.Finish();
                    TaskCount--;
                    //Tasks.Remove(task);
                }
            }
            Tasks.RemoveAll(x => x.IsFinished);
            bUpdating = false;
            //AddTask();
            //Console.WriteLine(Tasks[Tasks.Count - 1].Name);
        }

        public void SelectTask(Employee emp)
        {
            bSelecting = true;
            SelectedEmployee = emp;
            delayTimer = 5;
        }

        public void DrawDivider(SpriteBatch spriteBatch, int yOffset)
        {
            spriteBatch.Draw(Art.Pixel, new Rectangle(BaseArea.X, yOffset, BaseArea.Width, 1), Color.Black);
        }

        public void DrawTask(SpriteBatch spriteBatch, Task task)
        {

        }

        float LastY = 0;

        public Rectangle CalcCompletionRect(Task task)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Height = 30;
            rectangle.Width = (int)(task.DecimalComplete * BaseArea.Width);
            return rectangle;
        }

        public void DrawTasks(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Art.Pixel, BaseArea, BaseColour);
            spriteBatch.Draw(Art.AddTask, new Vector2(400, 40), AddTaskRectangle, Color.White);
            spriteBatch.DrawString(Art.Font, TaskCount + " of " + MaxTasks +" tasks currently running", new Vector2(BaseDrawPosition.X, 20), Color.Black);
            for (int i = 0; i < Tasks.Count; i++)
            {
                Task task = Tasks[i];

                float YOffset = LastY + ScrollOffset;
                //if (LastY != 0)
                    //YOffset = LastY + Art.Font.LineSpacing;                

                string text = task.Name + " " + (int)task.PercentComplete + "% Complete";
                //Vector2 size = Art.Font.MeasureString(text);
                
                Rectangle completion = CalcCompletionRect(task);
                completion.X = BaseArea.X;
                completion.Y = (int)(BaseDrawPosition.Y +YOffset);

                Color toDraw = Color.White;
                if (i == SelectedIndex)
                {
                    if (bSelecting)
                        toDraw = Color.Yellow;
                    SelectorTop = YOffset;
                    SelectorBottom = YOffset + 30;
                }
                spriteBatch.Draw(Art.TaskFrame, BaseDrawPosition + new Vector2(1, YOffset), RoundRectArea, toDraw);
                spriteBatch.Draw(Art.Pixel, completion, Color.ForestGreen);
                spriteBatch.Draw(Art.TaskFrame, BaseDrawPosition + new Vector2(1, YOffset), FrameArea, BaseColour);
                spriteBatch.Draw(Art.TaskFrame, BaseDrawPosition + new Vector2(0, YOffset - 1), BorderArea, Color.Black);

                spriteBatch.DrawString(Art.Font, text, BaseDrawPosition + new Vector2(2, YOffset + 5), Color.Black);
                LastY += Art.Font.LineSpacing + 11;
            }
            LastY = 1;
            spriteBatch.Draw(Art.Pixel, new Rectangle((int)BaseDrawPosition.X, 0, BaseArea.Width, 85), Color.White);
            spriteBatch.Draw(Art.AddTask, new Vector2(400, 40), AddTaskRectangle, Color.White);
            spriteBatch.DrawString(Art.Font, TaskCount + " of " + MaxTasks + " tasks currently running", new Vector2(BaseDrawPosition.X, 20), Color.Black);
        }
    }
}
