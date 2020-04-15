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
using c_pilot.Properties;

namespace c_pilot
{
    public partial class Form1 : Form
    {
        Bot Pilot1;
       
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Pilot1 = new Bot(comboBox1.Text, 1);
            if (Pilot1.Start()) textBox2.Text = "Window ok";
            button2.Enabled = Pilot1.isStartOk;
            button3.Enabled = Pilot1.isStartOk;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*           if (EveWindow.ToInt64() != 0)
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
           */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime st, end;
            bool rz;
            st = DateTime.Now;
            rz = Pilot1.MouseClickOverViewIco(0,Keys.LControlKey);
            end = DateTime.Now;

            textBox1.Text = st.ToString() + "}}}  " + rz.ToString()  + " {{{" + end.ToString();
            Utility.Delay(2000);
            st = DateTime.Now;
            rz = Pilot1.MouseClickOverViewIco(4, Keys.S);
            end = DateTime.Now;
            textBox2.Text = st.ToString() + "}}}  " + rz.ToString() + " {{{" + end.ToString();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Pilot1.MouseClickOverViewIco(0, Keys.D);
        }
    }
}
