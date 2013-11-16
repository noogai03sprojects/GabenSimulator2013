using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GabenSimulator2013
{
    class Employee
    {
        Task CurrentTask;

        int WorkPerDay;

        string Name;

        public Employee(int wpd, string name)
        {
            WorkPerDay = wpd;
            Name = name;
            CurrentTask = Task.None;
        }
    }
}
