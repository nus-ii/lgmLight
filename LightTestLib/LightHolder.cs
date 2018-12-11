using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedCSharp;
using System.Threading;

namespace LightTestLib
{
    public class LightHolder
    {
        private Color _activeColor;

        private Random _rnd;

        public bool stop;

        public LightTask nTask;

        public Color ActiveColor
        {
            set
            {
                _activeColor = value;
                LogitechGSDK.LogiLedSetLighting(value.R, value.G, value.B);
            }

            get
            {
                return _activeColor;
            }
        }

        private List<Color> ColorList;

        public LightHolder(Color startColor)
        {
            _rnd = new Random();

            ColorList = new List<Color>();

            ColorList.Add(new Color(0, 0, 99));
            ColorList.Add(new Color(0, 99, 0));
            ColorList.Add(new Color(0, 99, 99));

            ColorList.Add(new Color(99, 0, 0));
            ColorList.Add(new Color(99, 0, 99));
            ColorList.Add(new Color(99, 99, 0));

            ColorList.Add(new Color(99, 99, 99));

            LogitechGSDK.LogiLedInit();
            ActiveColor = startColor;
        }

        public void Flash(int redPercentage, int greenPercentage, int bluePercentage, int milliSecondsDuration, int milliSecondsInterval)
        {
            LogitechGSDK.LogiLedSetTargetDevice(LogitechGSDK.LOGI_DEVICETYPE_RGB);
            LogitechGSDK.LogiLedFlashLighting(redPercentage,greenPercentage,bluePercentage, milliSecondsDuration, milliSecondsDuration);

        }

        public LightHolder() : this(new Color(0, 0, 0))
        {

        }

        public async Task<bool> Bbb()
        {
                //ActiveColor = new Color(0,0,0);                
                var d = DateTime.Now.AddMinutes(20);

            return await Bbb(d,DateTime.Now.AddSeconds(20));
        }

        public async Task<bool> Bbb(DateTime d,DateTime t)
        {
           // var t = DateTime.Now.AddSeconds(20);
            for (;;)
            {
               
                if (d < DateTime.Now)
                {
                    break;
                }
                else
                {
                    if (DateTime.Now > t)
                    {
                        SetOtherColor();
                        t = t.AddSeconds(5);
                        return await Bbb(d,t);
                    }
                }
            }  
            return true;
        }

        public async Task<bool> cum()
        {
            stop = false;

            SetOtherColor();
            //Thread.Sleep(300);

            return true;
        }

        public void SetOtherColor()
        {
            List<Color> temp = new List<Color>();

            foreach (var c in ColorList)
            {
                if (c != _activeColor)
                {
                    temp.Add(c);
                }
            }

            var r = _rnd.Next(0, temp.Count - 1);

            ActiveColor = temp[r];
        }

        public void RandomShow(int gap, int count)
        {
            for (int i = count; count > 0; count--)
            {
                SetOtherColor();
                Thread.Sleep(gap);
            }
        }

        public void RandomShowByMin(int gap, int min)
        {
            RandomShowBySec(gap, min * 60);
        }

        public void RandomShowBySec(int gap, int secunds)
        {
            int count = (secunds * 1000) / gap;
            for (int i = count; count > 0; count--)
            {
                SetOtherColor();
                Thread.Sleep(gap);
            }
        }


        public void BlinkByListmin(List<Color> colors, int gapMs, int min)
        {
            BlinkByListSec(colors, gapMs, min * 60);
        }

        public void BlinkByListSec(List<Color> colors, int gapMs, int timeinsecund)
        {
            int repeat = (timeinsecund * 1000) / (colors.Count * gapMs);

            for (int i = 0; i < repeat; i++)
            {
                foreach (Color c in colors)
                {
                    ActiveColor = c;
                    Thread.Sleep(gapMs);
                }
            }
        }

    }
}
