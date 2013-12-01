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

        public List<GameName> GameNames;
        bool bAreGameNamesExhausted = false;
        public List<NumericalGameName> NumericalGameNames;
        List<GameName> ExhaustedNames;

        private Random random = new Random();

        private int SelectedIndex = -1;
        private bool bSelecting = false;

        Vector2 BaseDrawPosition = new Vector2(400, 50);

        Employee SelectedEmployee = null;

        int delayTimer = 5;
        public bool bUpdating = false;

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
            }
            else
            {
                int i = random.Next(NumericalGameNames.Count);
                NumericalGameName name = NumericalGameNames[i];
                
                Tasks.Add(new Task(requiredWork, name.Name + " " + name.Number, this));
                name.Number++;
                NumericalGameNames[i] = name;
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
            }
            if (Input.IsKeyPressed(Keys.Up))
            {
                if (SelectedIndex > -1 && bSelecting)
                    SelectedIndex--;
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
                        SelectedEmployee.SetTask(Tasks[SelectedIndex]);
                        bSelecting = false;
                        SelectedEmployee = null;
                        EmployeeManager.FinishSelecting();
                    }
                }
            }
        }

        public void Tick()
        {
            bUpdating = true;
            foreach (Task task in Tasks)
            {
                if (task.IsFinished)
                {
                    task.Finish();
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

        public void DrawTasks(SpriteBatch spriteBatch)
        {
            if (SelectedIndex == -1)
            {
                if (bSelecting)
                {
                    Vector2 size = Art.Font.MeasureString("Add new task");
                    Rectangle rect = new Rectangle((int)BaseDrawPosition.X, (int)(BaseDrawPosition.Y - Art.Font.LineSpacing), (int)size.X, (int)size.Y);
                    spriteBatch.Draw(Art.Pixel, rect, Color.Yellow);
                }
            }
            spriteBatch.DrawString(Art.Font, "Add new task", BaseDrawPosition + new Vector2(0, -Art.Font.LineSpacing), Color.Black);
            for (int i = 0; i < Tasks.Count; i++)
            {
                Task task = Tasks[i];

                float YOffset = Art.Font.LineSpacing * i;

                string text = "Name: " + task.Name + " Percent Complete: " + task.PercentComplete + "%";
                Vector2 size = Art.Font.MeasureString(text);                

                if (i == SelectedIndex)
                {
                    Rectangle rect = new Rectangle((int)BaseDrawPosition.X, (int)(BaseDrawPosition.Y + YOffset), (int)size.X, (int)size.Y);
                    if (bSelecting)
                        spriteBatch.Draw(Art.Pixel, rect, Color.Yellow);
                }                

                spriteBatch.DrawString(Art.Font, text, BaseDrawPosition + new Vector2(0, YOffset), Color.Black);
            }
        }
    }
}
