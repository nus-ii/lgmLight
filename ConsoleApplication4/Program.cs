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

            for (;;)
            {
                Console.Clear();
                BasicFunc();

                Console.ReadLine();
            }
        }




        static void BasicFunc()
        {
            var lh = new LightHolder();
            List<string> menu = new List<string>
            {
                "r - random show",
                "l - later start random show",
                "b - blink",
                "s - set color",
                "p - pulse"
            };


            foreach (string s in menu)
            {
                Console.WriteLine(s);
            }


            string answerA = Console.ReadLine();


            if (answerA.isFirstKey('r'))
            {

                lh.RandomShowAsync(300, lh.GetColorListThousand());
            }

            if (answerA.isFirstKey('l'))
            {
                Console.WriteLine("When start?");

                ParameterMaster pm = new ParameterMaster(new List<string> { "Hour", "Minute" });

                PauseTo(pm.GetInt("Hour"), pm.GetInt("Minute"));
                lh.RandomShowAsync(300, lh.GetColorListEight());
                Thread.Sleep(15000);
            }

            if (answerA.isFirstKey('b'))
            {
                List<Color> c = new List<Color>();
                c.Add(ParameterMaster.GetColor("Insert color!"));
                c.Add(new Color(0, 0, 0));

                int gap = 400;

                lh.BlinkListAsync(c, gap);
            }

            if (answerA.isFirstKey('s'))
            {                
                lh.ActiveColor = ParameterMaster.GetColor("Insert color!");
            }
           
            if (answerA.isFirstKey('p'))
            {
                
                ParameterMaster pm = new ParameterMaster(new List<string> { "int" });
                lh.PulseAsync(ParameterMaster.GetColor("Insert color!"), pm.GetInt("int"));
                Console.ReadLine();
            }

            if (answerA.isFirstKey('m'))
            {

                for (;;)
                {
                    lh.ActiveColor = new Color(90, 40, 20);
                    Thread.Sleep(10);                 
                    lh.ActiveColor = new Color(0, 0, 0);
                    Thread.Sleep(20);
                }
            }
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

        public static Color GetColor(string Header)
        {
            Console.WriteLine(Header);
            ParameterMaster pm = new ParameterMaster(new List<string> { "r", "g", "b" });
            return new Color(pm.GetInt("r"), pm.GetInt("g"), pm.GetInt("b"));
        }

        public ParameterMaster(List<string> inList)
        {
            parameters = new Dictionary<string, string>();
            foreach (string s in inList)
            {
                string val;
                Console.Write(s + " - ");
                val = Console.ReadLine();

                parameters.Add(s, val);
            }
        }

        public int GetInt(string key)
        {
            string result = "";
            result = parameters.FirstOrDefault(p => p.Key == key).Value;

            return Int32.Parse(result);
        }


    }//Program




}//Пространств имён
