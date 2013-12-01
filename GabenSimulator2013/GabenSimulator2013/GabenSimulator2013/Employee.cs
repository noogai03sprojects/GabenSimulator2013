using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GabenSimulator2013
{
    class Employee
    {
        public Task CurrentTask;

        public int WorkPerSecond;
        public int Multiplier = 1;

        public string Name;

        public int Age = 1;

        public int CompletedProjects = 0;

        public Employee(int wps, string name, Task task = null)
        {
            WorkPerSecond = wps;
            Name = name;

            CurrentTask = TaskManager.HalfLife3;
        }

        public void SetTask(Task task)
        {
            CurrentTask = task;
            task.AddWorker(this);
        }

        public void Work()
        {
            CurrentTask.AddWork(this);
            Age++;
            if (Age % 100 == 0)
            {
                Multiplier++;
            }
        }

        public void Finish()
        {
            CurrentTask = TaskManager.HalfLife3;
            CompletedProjects++;
        }
    }
}
