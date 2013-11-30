using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GabenSimulator2013
{
    class Task
    {
        public int RequiredWork {get; private set;}
        public int CurrentWork { get; set; }
        List<Employee> CurrentWorkers;
        public string Name;
        public bool IsFinished { get { return (CurrentWork >= RequiredWork); } }

        public TaskManager Manager;
        public float PercentComplete
        {
            get
            {
                float result = ((float)CurrentWork / (float)RequiredWork);                
                return result;
            }
        }

        public Task()
        {
            CurrentWorkers = new List<Employee>();
        }

        public Task(int requiredWork, string name, TaskManager manager)
            : this()
        {
            RequiredWork = requiredWork;
            Name = name;
            CurrentWork = 0;
            Manager = manager;
        }

        public void AddWorker(Employee emp)
        {
            //emp.SetTask(this);
            if (!Manager.bUpdating)
                CurrentWorkers.Add(emp);
        }

        public void AddWork(Employee sender)
        {
            CurrentWork += sender.WorkPerSecond;
        }

        public void Finish()
        {
            for (int i = 0; i < CurrentWorkers.Count; i++)
            {
                CurrentWorkers[i].CurrentTask = TaskManager.HalfLife3;
                CurrentWorkers.RemoveAt(i);
                i--;
            }
        }
        

    }
}
