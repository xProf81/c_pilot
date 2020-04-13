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
        Bot Pilot1;

        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Pilot1 = new Bot(comboBox1.Text,1);
            if (Pilot1.isStartOk) textBox2.Text = "Window ok";
            /*            EveWindow= Window.FindWindowByWindowTitle(comboBox1.Text);
                        if (EveWindow.ToInt64() == 0) textBox2.Text = "Error";
                        else
                        {
                            textBox2.Text = "Window ok";
                            Window.ShowWindow(EveWindow,3);
                            Window.SetFrontWindow(comboBox1.Text);
                        }
            */
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
            Pilot1.Start();            
        }


        private void button3_Click(object sender, EventArgs e)
        {
            DateTime st, end;
            st = DateTime.Now;
            Point rz = Pilot1.FindAllWindow(1);
            end = DateTime.Now;

            textBox1.Text = st.ToString()+"}}} x= " + rz.X.ToString() + " y= " + rz.Y.ToString()+" {{{"+end.ToString();

            st = DateTime.Now;
            rz = Pilot1.FindWindow(1);
            end = DateTime.Now;

            textBox1.Text = st.ToString() + "}}} x= " + rz.X.ToString() + " y= " + rz.Y.ToString() + " {{{" + end.ToString();
        }
    }
}
