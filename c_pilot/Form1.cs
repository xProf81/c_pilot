using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BotSuite;
using BotSuite.ImageLibrary;

namespace c_pilot
{
    public partial class Form1 : Form
    {
        IntPtr EveWindow;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EveWindow= Window.FindWindowByWindowTitle(comboBox1.Text);
            if (EveWindow.ToInt64() == 0) textBox2.Text = "Error";
            else
            {
                textBox2.Text = "Window ok";
                Window.ShowWindow(EveWindow,3);
                Window.SetFrontWindow(comboBox1.Text);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (EveWindow.ToInt64() != 0)
            {
                Bitmap bmpGate = new Bitmap(Image.FromFile("gate1.bmp"));
                ImageData gate = new ImageData(bmpGate);
                Bitmap screen = ScreenShot.Create(EveWindow);
                ImageData screenshot = new ImageData(screen);
                screen.Save("screenshot.bmp");
                Rectangle rez;
                rez=Template.Image(screenshot,gate,25);
                int x;
                int y;
                float scale = getScalingFactor();
                x = rez.Location.X+7;
                y = rez.Location.Y+7;
                textBox3.Text = Mouse.Move(new Point((int)(x/scale), (int)(y/scale)), true, 100).ToString();
                //Cursor.Position = new Point(1483, 281);
                //Mouse.LeftDown(EveWindow, new Point(x, y));
                Utility.Delay(100, 300);
               // Mouse.LeftUp(EveWindow, new Point(x, y));
                Mouse.LeftClick(EveWindow, new Point((int)(x / scale), (int)(y / scale)));
            }
            timer1.Stop();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Window.ShowWindow(EveWindow,3);
            Window.SetFrontWindow(comboBox1.Text);
            timer1.Start();
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

        }


        private float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Mouse.DoubleClick(EveWindow, new Point(1490, 288));
        }
    }
}
