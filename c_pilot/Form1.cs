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

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

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
                 screen.Save("screenshot.bmp");
            }
            timer1.Stop();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Window.ShowWindow(EveWindow,3);
            Window.SetFrontWindow(comboBox1.Text);
            timer1.Start();
        }
    }
}
