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

           
            if (answerA.isFirstKey('r'))
            {
                lh.RandomShow(450);

                for(;;)
                {
                    Console.WriteLine("random show");
                }

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
                lh.RandomShow(450);
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
                lh.ActiveColor = new Color(pm.GetInt("r"), pm.GetInt("g"), pm.GetInt("b"));
                Console.ReadLine();
            }

            if(answerA.Contains("f") || string.IsNullOrEmpty(answerA))
            {
                ParameterMaster pm = new ParameterMaster(new List<string> { "r", "g", "b", "dur","int"});
                LogitechGSDK.LogiLedSetLightingForTargetZone(DeviceType.Mouse, 0, 100, 0, 0);
                lh.Flash(pm.GetInt("r"), pm.GetInt("g"), pm.GetInt("b"), pm.GetInt("dur"), pm.GetInt("int"));
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

        public int GetInt(string key)
        {
            string result = "";
            result = parameters.FirstOrDefault(p=>p.Key==key).Value;

            return Int32.Parse(result);
        }

        
    }//Program

    public static class LetterMaster
    {
        private static string ruKeyMap = @"йцукенгшщзхъфывапролджэ\ячсмитьбю.";
        private static string enKeyMap = @"qwertyuiop[]asdfghjkl;'\zxcvbnm,./";

        public static bool isKey(char a, char b)
        {
            if (b == a || b == GetBrother(a))
                return true;

            return false;
        }

        public static bool isFirstKey(this string target, char f)
        {
            return isKey(target[0], f);
        }

        private static char GetBrother(char a)
        {
            if (ruKeyMap.Contains(a))
            {
                return enKeyMap[Position(a, ruKeyMap)];
            }
            else
            {
                return ruKeyMap[Position(a, enKeyMap)];
            }
        }

        private static int Position(char target, string map)
        {
            int result = -1;

            for (int i = 0; i <= map.Length - 1; i++)
            {
                if (map[i] == target)
                {
                    return i;
                }
            }

            return result;

        }
    }


}//Пространств имён
