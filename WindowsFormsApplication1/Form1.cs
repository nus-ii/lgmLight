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
        public Form1()
        {
            InitializeComponent();
            lh = new LightHolderSparta();
            lc = new lightcaller();
            gkh = new globalKeyboardHook();
            lh.ActiveColor = new LightTestLib.Color(10, 50, 80);
            //lc.ButtonC += lh.SetOtherColor;

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

        private async void button1_Click(object sender, EventArgs e)
        {
            
            //var k = await lh.cum();
            //var t = 0;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            //var d=await lh.Bbb();
            //var dh = 0;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            lc.ButtonC += lh.SetOtherColor;
            timer1.Interval = 1000;
            timer1.Start();
        }

        private int Lc_ButtonC()
        {
            throw new NotImplementedException();
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

        private void button4_Click(object sender, EventArgs e)
        {
            //lc.ButtonC -= lh.SetOtherColor;
            timer1.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Interval = timer1.Interval + 60;
            label1.Text = timer1.Interval.ToString() + " ms";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Interval = timer1.Interval - 60;
            label1.Text = timer1.Interval.ToString() + " ms";
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
            for(int? c=0;c<254;c++)
            { gkh.HookedKeys.Add((Keys)c); }
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
        }


        async void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            lock(lh)
            {
                lh.SetOtherColor();
            }
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
