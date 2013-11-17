using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace GabenSimulator2013
{
    class TimingManager
    {
        public delegate void TickDelegate();

        /// <summary>
        /// Called every second to update the various managers.
        /// </summary>
        public event TickDelegate Second;
        //Ela
        public event TickDelegate NewEmployee;

        private Timer UpdateTimer;
        private Timer EmployeeTimer;
        //public ElapsedEventHandler UpdateTimer_Elapsed;

        public TimingManager()
        {
            UpdateTimer = new Timer(1000); //1 second            
            EmployeeTimer = new Timer(3000); // 30 seconds

            UpdateTimer.Elapsed += Update;
            EmployeeTimer.Elapsed += EmployeeUpdate;
        }

        public void StartTimers()
        {
            UpdateTimer.Start();
            EmployeeTimer.Start();
        }

        public void StopTimers()
        {
            UpdateTimer.Stop();
            EmployeeTimer.Stop();
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            if (Second != null)
                Second();            
        }

        private void EmployeeUpdate(object sender, ElapsedEventArgs e)
        {
            if (NewEmployee != null)
                NewEmployee();
        }
    }
}
