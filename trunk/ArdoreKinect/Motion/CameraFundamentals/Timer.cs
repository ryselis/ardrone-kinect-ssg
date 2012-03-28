using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraFundamentals
{
    public class Timer
    {
        private float totaltime;
        public float currenttime;
        public bool finnish;


        public Timer(float timems)
        {
            finnish = false;
            currenttime = 0f;
            totaltime = timems;
        }

        public void update(float dt)
        {
            if (finnish)
                return;
            currenttime += dt;
            if (totaltime <= currenttime)
                finnish = true;
        }
    }
}
