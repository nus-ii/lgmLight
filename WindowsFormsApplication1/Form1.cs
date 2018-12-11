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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        LightHolder lh;
        lightcaller lc;
        public Form1()
        {
            InitializeComponent();
            lh = new LightHolder();
            lc = new lightcaller();
            //lc.ButtonC += lh.SetOtherColor;

            notifyIcon1.Visible = true;
            // добавляем Эвент или событие по 2му клику мышки, 
            //вызывая функцию  notifyIcon1_MouseDoubleClick
            this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);

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

        private void button3_Click(object sender, EventArgs e)
        {
            lc.ButtonC += lh.SetOtherColor;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lc.clickClick();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lc.ButtonC -= lh.SetOtherColor;
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

    }
}
