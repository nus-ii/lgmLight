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
            

           

           
            LogitechGSDK.LogiLedInit();
            LogitechGSDK.LogiLedSetLightingForTargetZone(DeviceType.Mouse, 0, 100, 0, 0);

            ActiveColor = startColor;
        }

        //public void Flash(int redPercentage, int greenPercentage, int bluePercentage, int milliSecondsDuration, int milliSecondsInterval)
        //{            
        //    LogitechGSDK.LogiLedFlashLighting(redPercentage, greenPercentage, bluePercentage, milliSecondsDuration, milliSecondsInterval);

        //}


        public LightHolder() : this(new Color(0, 0, 0))
        {

        }

        public List<Color> GetColorListEight(bool withBlack=false)
        {
            ColorList = new List<Color>();

            ColorList.Add(new Color(0, 0, 99));
            ColorList.Add(new Color(0, 99, 0));
            ColorList.Add(new Color(0, 99, 99));

            ColorList.Add(new Color(99, 0, 0));
            ColorList.Add(new Color(99, 0, 99));
            ColorList.Add(new Color(99, 99, 0));

            ColorList.Add(new Color(99, 99, 99));

            if(withBlack)
                ColorList.Add(new Color(0, 0, 0));

            return ColorList;
        }

        public List<Color> GetColorListThousand(bool withBlack = false)
        {
            ColorList = new List<Color>();

            for (int i = 0; i <= 999; i++)
            {
                int han = i / 100;
                int dec = i / 10 - (han * 10);
                int d = i - (han * 100) - (dec * 10);

                ColorList.Add(new Color(han * 10, dec * 10, d * 10));
            }

            if (!withBlack)
                ColorList = ColorList.Where(c => c != new Color(0, 0, 0)).ToList();

            return ColorList;
        }


        public async void PulseAsync(Color targetColor, int milliSecondsInterval)
        {

            int milliSecondsDuration = 5000000;
            await Task.Run(() =>
         {

             for (;;)
             {
                 LogitechGSDK.LogiLedPulseLighting(targetColor.R, targetColor.G, targetColor.B, milliSecondsDuration, milliSecondsInterval);
                 Thread.Sleep(milliSecondsDuration);
             }

         });
        }


        public async Task<int> SetOtherColorAsync(List<Color> colorList)
        {
            int result = await Task.Run(() =>
            {
                List<Color> tempColorList = colorList.Where(c => c != _activeColor).ToList();

                int r = _rnd.Next(0, tempColorList.Count - 1);

                ActiveColor = tempColorList[r];

                return r;
            });
            return result;
        }

        public async void RandomShowAsync(int gap, List<Color> colorList)
        {
            for (;;)
            {
                await SetOtherColorAsync(colorList);
                Thread.Sleep(gap);
            }
        }

        public async void BlinkListAsync(List<Color> colorList, int gapMs)
        {

            for (;;)
            {
                foreach (Color c in colorList)
                {

                    int result = await Task.Run(() =>
                {
                    ActiveColor = c;
                    Thread.Sleep(gapMs);
                    return 0;
                });
                }
            }
        }

    }
}
