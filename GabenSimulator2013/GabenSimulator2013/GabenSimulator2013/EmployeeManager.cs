using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GabenSimulator2013
{
    class EmployeeManager
    {
        List<Employee> Employees;
        Random random = new Random();

        float ScrollOffset = 0;

        Vector2 BaseDrawPosition = new Vector2(20, 50);

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

        public void Update()
        {
            ScrollOffset = Input.MouseWheelValue;
            foreach (Employee employee in Employees)
            {
                employee.Work();
                if (employee.CurrentTask.IsFinished)
                    employee.CurrentTask = TaskManager.HalfLife3;
            }
        }

        public void DrawEmployees(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Employees.Count; i++)
            {
                Employee emp = Employees[i];
                string text = "Name: " + emp.Name + " WPS: " + emp.WorkPerSecond;
                Vector2 size = Art.Font.MeasureString(text);
                float YOffset = ScrollOffset + Art.Font.LineSpacing * i;
                
                spriteBatch.DrawString(Art.Font, text, BaseDrawPosition + new Vector2(0, YOffset), Color.Black);
            }
            //foreach (Employee emp in Employees.Values)
            {
                
            }
        }
    }
}
