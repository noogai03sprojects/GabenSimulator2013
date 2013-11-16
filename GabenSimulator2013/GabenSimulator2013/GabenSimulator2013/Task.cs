using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GabenSimulator2013
{
    class Task
    {
        public float RequiredWork {get; private set;}
        public float CurrentWork { get; private set; }
        List<Employee> CurrentWorkers;
        public string Name;
        public bool IsFinished;

        public Task()
        {
            CurrentWorkers = new List<Employee>();
        }

        public Task(float requiredWork, string name)
            : this()
        {
            RequiredWork = requiredWork;
            Name = name;
        }

        public void AddWorker(Employee emp)
        {

        }
        public static Task None = new Task(-1, "No Task");

    }
}
