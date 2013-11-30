using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GabenSimulator2013
{
    class EmployeeManager
    {
        List<Employee> Employees;
        Random random = new Random();
        public TaskManager TaskManager;

        float ScrollOffset = 0;

        Vector2 BaseDrawPosition = new Vector2(20, 50);

        int SelectedIndex = 0;

        bool bSelecting = true;
        int delayTimer = 5;

        public EmployeeManager()
        {
            Employees = new List<Employee>();
        }

        public void AddEmployee()
        {
            string name = NameGenerator.GetName();
            int wps = Math.Abs(random.Next(100));

            Employees.Add(new Employee(wps, name));
            Console.WriteLine("Name: " + name + " WPS: " + wps);
            //Em
        }

        public void Tick()
        {
            
            foreach (Employee employee in Employees)
            {
                employee.Work();
                if (employee.CurrentTask.IsFinished)
                    employee.CurrentTask = TaskManager.HalfLife3;
            }
        }
        public void Update()
        {
            if (delayTimer > 0)
                delayTimer--;
            ScrollOffset = Input.MouseWheelValue;
            if (Input.IsKeyPressed(Keys.Down) && bSelecting)
            {
                if (SelectedIndex < Employees.Count - 1)
                    SelectedIndex++;
            }
            if (Input.IsKeyPressed(Keys.Up) && bSelecting)
            {
                if (SelectedIndex > 0)
                    SelectedIndex--;
            }
            if (Input.IsKeyPressed(Keys.Enter))
            {
                if (bSelecting && delayTimer <= 0)
                {
                    bSelecting = false;
                    TaskManager.SelectTask(Employees[SelectedIndex]);                    
                }
            }            
        }

        public void DrawEmployees(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Employees.Count; i++)
            {
                Employee emp = Employees[i];
                string text = "Name: " + emp.Name + " WPS: " + emp.WorkPerSecond +"\nTask: " + emp.CurrentTask.Name;
                Vector2 size = Art.Font.MeasureString(text);
                
                float YOffset = ScrollOffset + Art.Font.LineSpacing * 2 * i;

                if (i == SelectedIndex)
                {
                    Rectangle rect = new Rectangle((int)BaseDrawPosition.X, (int)(BaseDrawPosition.Y + YOffset), (int)size.X, (int)size.Y);
                    //if (bSelecting)
                        spriteBatch.Draw(Art.Pixel, rect, Color.Yellow);
                }

                spriteBatch.DrawString(Art.Font, text, BaseDrawPosition + new Vector2(0, YOffset), Color.Black);
            }
            //foreach (Employee emp in Employees.Values)
            {
                
            }
        }

        public  void FinishSelecting()
        {
            bSelecting = true;
            delayTimer = 5;
        }
    }
}
