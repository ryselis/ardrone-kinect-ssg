using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ARDroneKinect
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

        public void update(GameTime dt)
        {
            if (finnish)
                return;
            currenttime += (float)dt.ElapsedGameTime.Milliseconds;
            if (totaltime <= currenttime)
                finnish = true;
        }
    }
}
