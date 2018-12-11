using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightTestLib
{
    public class LightTask
    {
        public Color TargetColor;

        public int SleepTime;

        public LightTask(int r,int g, int b, int time)
        {
            TargetColor = new Color(r,g,b);
            SleepTime = time;
        }
    }
}
