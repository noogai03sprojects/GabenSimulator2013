using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GabenSimulator2013
{
    class TaskManager
    {
        public Task HalfLife3;
        public Dictionary<string, Task> Tasks;

        public TaskManager()
        {
            HalfLife3 = new Task(1000, "Half-Life 3");
            Tasks = new Dictionary<string, Task>();
        }
    }
}
