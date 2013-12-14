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

        float DesiredScrollOffset = 0;
        float ScrollOffset = 0;

        Vector2 BaseDrawPosition = Vector2.Zero;
        Rectangle BaseArea = new Rectangle(20, 85, 350, 500);

        int SelectedIndex = 0;

        bool bSelecting = true;
        int delayTimer = 5;

        float SelectorHeight;

        Rectangle SelectionHighlight = Rectangle.Empty;
        Rectangle DesiredHighlight = Rectangle.Empty;

        public EmployeeManager()
        {
            Employees = new List<Employee>();
            BaseDrawPosition = new Vector2(BaseArea.Left, BaseArea.Top);
        }

        public void AddEmployee()
        {
            string name = NameGenerator.GetName();
            int wps = Math.Abs(random.Next(100));

            Employees.Add(new Employee(wps, name));
            Console.WriteLine("Name: " + name + " WPS: " + wps);
            //Em
        }
        public void AddEmployee(int wps)
        {
            string name = NameGenerator.GetName();            

            Employees.Add(new Employee(wps, name));
            //Console.WriteLine("Name: " + name + " WPS: " + wps);
            //Em
        }

        public void Tick()
        {
            
            foreach (Employee employee in Employees)
            {
                employee.Work();
                if (employee.CurrentTask.IsFinished)
                    employee.Finish();
            }
            //Employees.RemoveAll(x => x.CompletedProjects > 3);
        }
        public void Update()
        {
            if (delayTimer > 0)
                delayTimer--;
            //ScrollOffset = Input.MouseWheelValue;
            if (Input.IsKeyPressed(Keys.Down) && bSelecting)
            {
                if (SelectedIndex < Employees.Count - 1)
                    SelectedIndex++;
                if (SelectorHeight > GameRoot.Instance.ScreenSize.Y - Art.Font.LineSpacing * 4)
                {
                    DesiredScrollOffset -= Art.Font.LineSpacing * 2;
                }
            }
            if (Input.IsKeyPressed(Keys.Up) && bSelecting)
            {
                if (SelectedIndex > 0)
                    SelectedIndex--;
                if (SelectorHeight < BaseDrawPosition.Y + Art.Font.LineSpacing * 2)
                {
                    DesiredScrollOffset += Art.Font.LineSpacing * 2;
                }
            }
            if (Input.IsKeyPressed(Keys.Enter))
            {
                if (bSelecting && delayTimer <= 0)
                {
                    bSelecting = false;
                    TaskManager.SelectTask(Employees[SelectedIndex]);                    
                }
            }
            if (Input.IsKeyPressed(Keys.E))
            {
                AddEmployee(0);
            }

            ScrollOffset = MathHelper.Lerp(ScrollOffset, DesiredScrollOffset, 0.3f);
            SelectionHighlight = SelectionHighlight.Lerp(DesiredHighlight, 0.5f);
        }

        private void DrawDivider(SpriteBatch spriteBatch, int YOffset)
        {
            spriteBatch.Draw(Art.Pixel, new Rectangle(BaseArea.Left, BaseArea.Top + YOffset, BaseArea.Width, 1), Color.Black);
        }

        public void DrawEmployees(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Art.Pixel, BaseArea, Color.Gray);
            int num3Liners = 0;
            for (int i = 0; i < Employees.Count; i++)
            {
                Employee emp = Employees[i];
                string text = "Name: " + emp.Name + " WPS: " + emp.WorkPerSecond +"\nTask: " + emp.CurrentTask.Name;
                Vector2 size = Art.Font.MeasureString(text);

                //float toAdd = 0;
                if (i > 0)
                {
                    Vector2 prevSize = Art.Font.MeasureString(Employees[i - 1].Name);
                    if (prevSize.Y > Art.Font.LineSpacing * 2)
                    {
                        num3Liners++;
                    }
                    if (size.Y > Art.Font.LineSpacing * 2)
                    {
                        //num3Liners++;
                    }
                }
                float YOffset = ScrollOffset + Art.Font.LineSpacing * 2 * i + num3Liners * Art.Font.LineSpacing;

                
                
                //if (i % 2 == 0)
                //    spriteBatch.Draw(Art.Pixel, SelectionHighlight, Color.Green);
                //else
                //    spriteBatch.Draw(Art.Pixel, SelectionHighlight, Color.LightGreen);

                if (i == SelectedIndex)
                {
                    //Rectangle rect = new Rectangle((int)BaseDrawPosition.X, (int)(BaseDrawPosition.Y + YOffset), (int)size.X, (int)size.Y);
                    //if (bSelecting)
                    DesiredHighlight = new Rectangle((int)BaseDrawPosition.X, (int)(BaseDrawPosition.Y + YOffset), (int)size.X, (int)size.Y);
                    DesiredHighlight.Inflate(-2, -2);
                    spriteBatch.Draw(Art.Pixel, SelectionHighlight, Color.LightGray);
                    SelectorHeight = DesiredHighlight.Center.ToVector().Y;
                }
                spriteBatch.DrawString(Art.Font, text, BaseDrawPosition + new Vector2(0, YOffset), Color.Black);
                DrawDivider(spriteBatch, (int)YOffset);
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
