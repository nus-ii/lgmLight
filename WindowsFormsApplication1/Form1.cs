using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LightTestLib;
using LedCSharp;
using System.Management;
using System.Device;
using Utilities;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        LightHolder lh;
        lightcaller lc;
        globalKeyboardHook gkh;
        private DateTime laskKeyPress;
        public bool HitchFlag { get; set; }
        public TimeSpan HitchSpan { get; set; }
        public bool HitchIndicator
        {
            get { return checkBox1.Checked;}
            set { checkBox1.Checked = value; }
        }

        public Form1()
        {
            InitializeComponent();
            lh = new LightHolder();
            lc = new lightcaller();
            gkh = new globalKeyboardHook();
            lh.ActiveColor = new LightTestLib.Color(10, 50, 80);

            notifyIcon1.Visible = true;
            // добавляем Эвент или событие по 2му клику мышки, 
            //вызывая функцию  notifyIcon1_MouseDoubleClick
            this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            List<MenuItem> i = new List<MenuItem>();
            i.Add(new MenuItem("Hook",Hook_Click));
            i.Add(new MenuItem("Blink", button3_Click));
            this.notifyIcon1.ContextMenu = new ContextMenu(i.ToArray());

            // добавляем событие на изменение окна
            this.Resize += new System.EventHandler(this.Form1_Resize);

            //Время последнего нажатия на клавишу
            laskKeyPress=DateTime.Now;

            //Время ожидания нажатия на клавишу
            HitchSpan = new TimeSpan(10*10000000);
            label4.Text = "10 sec.";

            //Признак отслеживания простоя
            checkBox1.Checked = true;

            //сначала выключено
            radioButton3.Checked = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // проверяем наше окно, и если оно было свернуто, делаем событие        
            if (WindowState == FormWindowState.Minimized)
            {
                // прячем наше окно из панели
                this.ShowInTaskbar = false;
                // делаем нашу иконку в трее активной
                notifyIcon1.Visible = true;
            }
        }


        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // делаем нашу иконку скрытой
            notifyIcon1.Visible = true;
            // возвращаем отображение окна в панели
            this.ShowInTaskbar = true;
            //разворачиваем окно
            WindowState = FormWindowState.Normal;
        }


        private async void button3_Click(object sender, EventArgs e)
        {
            lc.ButtonC += lh.SetOtherColor;
            timer1.Interval = 1000;
            timer1.Start();
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<LightTestLib.Color> cList = new List<LightTestLib.Color>
            {
                new LightTestLib.Color(40,50,0),
                new LightTestLib.Color(90,0,0),
                new LightTestLib.Color(90,0,90),
                new LightTestLib.Color(90,90,0)
            };

            lc.clickClick(cList);
        }



        private void button7_Click(object sender, EventArgs e)
        {
            List<USBDeviceInfo> d = GetUSBDevices();  

            List<USBDeviceInfo> result = new List<USBDeviceInfo>();

            foreach(var i in d)
            {
                if(!string.IsNullOrEmpty(i.Description) && i.Description[0]=='L')
                {                   
                    result.Add(i);
                }
            }


            if(result.Any(i=>i.Description.Contains("Mouse")))
            {
                button7.Text = "Mouse=true ";
            }

            if (result.Any(i => i.Description.Contains("300") && i.Description.Contains("Mouse")))
            {
                button7.Text = button7.Text+"300=true";
            }
        }

        static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            System.Management.ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
        }

        private void Hook_Click(object sender, EventArgs e)
        {
            //for(int? c=0;c<254;c++)
            //{ gkh.HookedKeys.Add((Keys)c); }
            //gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
        }


        async void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            laskKeyPress=DateTime.Now;
            HitchFlag = false;
            lock (lh)
            {
                lh.SetOtherColor();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                if (lh != null)
                {
                    lh.Kill();
                    lh = null;
                }
                lh = new LightHolder();
                radioButton2.Checked = false;
                radioButton3.Checked = false;

                List<LightTestLib.Color> cList = new List<LightTestLib.Color>
                {
                    new LightTestLib.Color(90,90,90),
                    new LightTestLib.Color(0,90,90),
                    new LightTestLib.Color(0,0,90),

                    new LightTestLib.Color(90,0,0),
                    new LightTestLib.Color(90,90,0),
                    new LightTestLib.Color(90,0,90),
                    new LightTestLib.Color(0,90,0),

                    new LightTestLib.Color(90,90,90),
                };
                lh.BlinkListAsync(cList,500);
            }
   
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                if (lh != null)
                {
                    lh.Kill();
                    lh = null;
                }
           
                lh = new LightHolder();
                radioButton1.Checked = false;
                radioButton3.Checked = false;

                for (int? c = 0; c < 254; c++)
                { gkh.HookedKeys.Add((Keys)c); }
                gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
                timer2.Interval = 1000;
                timer2.Start();
            }

     
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                if (lh != null)
                {
                    lh.Kill();
                    lh = null;
                }
                radioButton1.Checked = false;
                radioButton2.Checked = false;
            }
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            var dnow = DateTime.Now;
            var dif = dnow - laskKeyPress;
            if ((dif > HitchSpan)&&HitchFlag==false&&HitchIndicator)
            {
                HitchFlag = true;
                List<LightTestLib.Color> cList = new List<LightTestLib.Color>
                {
                    new LightTestLib.Color(90,0,0),
                    new LightTestLib.Color(0,0,0)
                };
                lh = new LightHolder();
                lh.BlinkListAsync(cList, 500);
            }
        }

        

        private void timer3_Tick(object sender, EventArgs e)
        {
           

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            HitchSpan =new TimeSpan(trackBar1.Value * 1000);
            label4.Text = trackBar1.Value.ToString() + " sec.";
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }

    public class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
        }
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }     


        public override string ToString()
        {
            return Description;
        }
    }
}
