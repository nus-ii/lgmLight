using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedCSharp;
using System.Threading;
using LightTestLib;
using System.Threading.Tasks;
using System.Linq;

namespace ConsoleApplication4
{
    public class Program
    {


        static void Main(string[] args)
        {

            var lh = new LightHolder();
            List<string> menu = new List<string>
            {
                "r - random show",
                "l - later start random show",
                "b - blink",
                "s - set color LogitechGSDK.LogiLedSetLighting",
                "f - flash LogiLedFlashLighting"
            };


            foreach(string s in menu)
            {
                Console.WriteLine(s);
            }
            
            
            string answerA = Console.ReadLine();

           
            if (answerA.Contains("r"))
            {
                lh.RandomShow(450, 500000);

            }

            if (answerA.Contains("l"))
            {
                Console.WriteLine("When start?");

                Console.WriteLine("Hour?");
                string h = Console.ReadLine();

                Console.WriteLine("Minute?");
                string m = Console.ReadLine();

                int hh = Convert.ToInt32(h);
                int mm = Convert.ToInt32(m);

                PauseTo(hh, mm);
                lh.RandomShow(450, 500000);
                Thread.Sleep(15000);
            }

            if (answerA.Contains("b"))
            {
                List<Color> c = new List<Color>();
                c.Add(new Color(99, 99, 0));
                c.Add(new Color(0, 0, 0));

                int gap = 800;
                for (int u = 0; u <= 4; u++)
                {
                    lh.BlinkByListSec(c, gap, 10);
                    gap = gap / 2;
                }

            }

            if (answerA.Contains("s"))
            {

                ParameterMaster pm = new ParameterMaster(new List<string> {"r","g","b" });
                lh.ActiveColor = new Color(pm.GetValueInt("r"), pm.GetValueInt("g"), pm.GetValueInt("b"));
                Console.ReadLine();
            }

            if(answerA.Contains("f") || string.IsNullOrEmpty(answerA))
            {
                ParameterMaster pm = new ParameterMaster(new List<string> { "r", "g", "b", "dur","int"});
                lh.Flash(pm.GetValueInt("r"), pm.GetValueInt("g"), pm.GetValueInt("b"), pm.GetValueInt("dur"), pm.GetValueInt("int"));
                Console.ReadLine();
            }

            //if (answerA.Contains("p"))
            //{
            //    List<Color> c = new List<Color>();
            //    c.Add(new Color(99, 0, 0));
            //    int gap = 200;
            //    lh.BlinkByListmin(c, gap, 1);

            //    c.Add(new Color(0, 0, 0));
            //    lh.BlinkByListmin(c, gap, 1);

            //    lh.RandomShowByMin(150, 1);

            //}
        }








        static void PauseTo(int hours, int min)
        {
            for (;;)
            {
                DateTime now = new DateTime();
                now = DateTime.Now;

                if (hours == now.Hour && min == now.Minute)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(15000);
                }
            }
        }


    }

    public class ParameterMaster
    {
        public Dictionary<string, string> parameters;

        public ParameterMaster(List<string> inList)
        {
            parameters = new Dictionary<string, string>();
            foreach (string s in inList)
            {
                string val;
                Console.Write(s +" - ");
                val = Console.ReadLine();

                parameters.Add(s, val);
            }
        }

        public int GetValueInt(string key)
        {
            string result = "";
            result = parameters.FirstOrDefault(p=>p.Key==key).Value;

            return Int32.Parse(result);
        }
    }
}
