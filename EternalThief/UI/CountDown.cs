using Microsoft.Xna.Framework;
using System;

namespace EternalThief.UI
{
    class CountDown
    {
        private long startTimerMS;
        private TimeSpan currentTime;
        private bool start = false;

        public CountDown(long startTimer)
        {
            startTimerMS = startTimer;
            currentTime =  TimeSpan.FromMilliseconds(startTimerMS);
        }

        public void Update(GameTime gameTime)
        {
            if(!start)
            {
                currentTime += gameTime.ElapsedGameTime;
                start = true;
            } else
            {
                currentTime -= gameTime.ElapsedGameTime;
            }
        }

        public String getCurrentTimeString()
        {
            if(currentTime < TimeSpan.Zero)
            {
                return "00:00";
            } 
            else
            { 
                return currentTime.ToString(@"mm\:ss"); ;
            }
        }
        public float getCurrentTime()
        {
            return (float)currentTime.TotalMilliseconds;
        }

        public void reset()
        {
            currentTime = new TimeSpan(startTimerMS);
        }
    }
}
