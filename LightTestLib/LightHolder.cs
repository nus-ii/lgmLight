using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedCSharp;
using System.Threading;

namespace LightTestLib
{
    public class LightHolder : ILightHolder
    {
        protected Color _activeColor { get; set; }

        private Random _rnd;

        public bool stop;

        public LightTask nTask;

        public List<Color> BaseColors
        {
            get
            {
                if (_baseColor == null || _baseColor.Count == 0)
                {
                    _baseColor = GetBaseColors();
                }
                return _baseColor;
            }

        }

        private bool _readyToSetColor;

        private List<Color> _baseColor;

        /// <summary>
        /// Текущий цвет
        /// </summary>
        public Color ActiveColor
        {
            set
            {
                if (stop == false)
                {
                    _activeColor = value;
                    LogitechGSDK.LogiLedSetLighting(value.R, value.G, value.B);
                    LogitechGSDK.LogiLedSetLightingForTargetZone(DeviceType.Mouse, 0, value.R, value.G, value.B);
                    LogitechGSDK.LogiLedSetLightingForTargetZone(DeviceType.Mouse, 1, value.R, value.G, value.B);
                    LogitechGSDK.LogiLedSetLighting(value.R, value.G, value.B);
                }
         
            }

            get
            {
                return _activeColor;
            }
        }


        public LightHolder(Color startColor)
        {
            stop = false;
            _rnd = new Random();
            LogitechGSDK.LogiLedInitWithName(Guid.NewGuid().ToString().Split('-')[0]);
            _readyToSetColor = true;
            ActiveColor = startColor;
            
        }

        public LightHolder() : this(new Color(0, 0, 0))
        {

        }



        /// <summary>
        /// Получение списка базовых цветов для данной версии мыши
        /// </summary>
        /// <returns></returns>
        public virtual List<Color> GetBaseColors(bool withBlack = false)
        {
            //List<Color> result = new List<Color>();
            //for (int i = 0; i <= 2; i++)
            //{
            //    for (int s = 0; s <= 100; s++)
            //    {
            //        if (i == 0)
            //        {
            //            result.Add(new Color(s, 0, 100));
            //        }
            //        if (i == 1)
            //        {
            //            result.Add(new Color(0, s, 100));
            //        }
            //        if (i == 2)
            //        {
            //            result.Add(new Color(0, 100, s));
            //        }
            //    }
            //}
            //return result;


            var ColorList = new List<Color>();

            ColorList.Add(new Color(0, 0, 99));
            ColorList.Add(new Color(0, 99, 0));
            ColorList.Add(new Color(0, 99, 99));

            ColorList.Add(new Color(99, 0, 0));
            ColorList.Add(new Color(99, 0, 99));
            ColorList.Add(new Color(99, 99, 0));

            ColorList.Add(new Color(99, 99, 99));

            if (withBlack)
                ColorList.Add(new Color(0, 0, 0));

            return ColorList;
        }

        /// <summary>
        /// Пульсация (Эффект дыхания) только для RGB моделей
        /// </summary>
        /// <param name="targetColor"></param>
        /// <param name="milliSecondsInterval"></param>
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


        /// <summary>
        /// Установить любой цвет из списка не равный текущему
        /// </summary>
        /// <param name="gapMs"></param>
        /// <param name="colorList">Список цветов для выбора</param>
        public async void SetOtherColorAsync(int gapMs,List<Color> colorList)
        {
            await Task.Run(() =>
            {
               SetOtherColor(colorList);
               Thread.Sleep(gapMs);
            });
            
        }

        public void Kill()
        {
            stop = true;
            LogitechGSDK.LogiLedRestoreLighting();
            LogitechGSDK.LogiLedStopEffects();
            LogitechGSDK.LogiLedShutdown();
        }

        public int SetOtherColor(List<Color> colorList)
        {
            if (_readyToSetColor)
            {
                _readyToSetColor = false;

                int r = 0;
                for (;;)
                {
                    r = _rnd.Next(0, colorList.Count - 1);

                    if (colorList[r] != ActiveColor)
                    {
                        ActiveColor = colorList[r];
                        break;
                    }
                }
                _readyToSetColor = true;
                return r;
            }
            return 0;
        }

        public int SetOtherColor()
        {
            return SetOtherColor(this.BaseColors);
        }

        /// <summary>
        /// Случайный перебор цветов по списку с заданным промежутком
        /// </summary>
        /// <param name="gapMs">Промежуток</param>
        /// <param name="colorList">Список цветов</param>
        public void RandomShowAsync(int gapMs, List<Color> colorList)
        {
            for (;;)
            {
               SetOtherColorAsync(gapMs, colorList);
            }
        }

        /// <summary>
        /// Последовательный перебор цветов 
        /// </summary>
        /// <param name="colorList">Список цветов</param>
        /// <param name="gapMs">Промежуток</param>
        public async void BlinkListAsync(List<Color> colorList, int gapMs)
        {
            for (;;)
            {
                foreach (Color c in colorList)
                {
                   await Task.Run(() =>
                {
                   
                        ActiveColor = c;
                  
                    Thread.Sleep(gapMs);
                    return 0;
                });
                }
            }
        }

        Task<int> ILightHolder.SetOtherColorAsync(int gapMs, List<Color> colorList)
        {
            throw new NotImplementedException();
        }
    }
}
