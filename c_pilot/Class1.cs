using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BotSuite;
using BotSuite.ImageLibrary;
using System.Xml;
using c_pilot.Properties;

namespace c_pilot
{
    class Bot
    {
        public struct RECT
        {
            public int left;        // x position of upper-left corner
            public int top;         // y position of upper-left corner
            public int right;       // x position of lower-right corner
            public int bottom;      // y position of lower-right corner
        }

        private struct CONFIG 
        {
            public int maxMouseSpeed;
            public int minMouseSpeed;
            public int maxClickSpeed;
            public int minClickSpeed;
            public int maxDrones;
            public int maxDroneDistance;
            public int maxTargets;
        }

        private CONFIG cfg;

        private string windowName;

        float scaleFactor { get {
                return (float)PhysicalScreenHeight / (float)LogicalScreenHeight;} }
        int LogicalScreenHeight{get
            {
                Graphics g = Graphics.FromHwnd(IntPtr.Zero);
                IntPtr desktop = g.GetHdc();
                return GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            }
        }
        int PhysicalScreenHeight {get
            {
                Graphics g = Graphics.FromHwnd(IntPtr.Zero);
                IntPtr desktop = g.GetHdc();
                return GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);
            }
        }

        private IntPtr hWnd;
        const int klvBmp = 16;
        private Bitmap[] templates;
        private RECT overViewIco;
        private RECT overViewNames;
        private RECT mainWindow;
        private bool _isStartOk = true;
        private int status = 0;
        private string iniFile = "cfg_pilot";
        private int number;
        public int GetStatus { get { return status; } }
        private string logFile = "c_pilot";
        StreamWriter swLog;

        public bool isStartOk { get { return _isStartOk; } }

        public Bot(string winName,int numberOfBot)
        {
            windowName = winName;
            number = numberOfBot;
            logFile = logFile + number + ".log";
            templates = new Bitmap[klvBmp];
        }

        public Point FindAllWindow(int imageNum)
        {
            
            Window.ShowWindow(hWnd, 9);
            Window.SetFrontWindow(windowName);
            Utility.Delay(200,300);
            Bitmap screen = ScreenShot.Create(hWnd);
            if (screen.Height < PhysicalScreenHeight - 70)
            {
                Window.ShowWindow(hWnd, 3);
                Utility.Delay(200, 300);
                screen = ScreenShot.Create(hWnd);
            }
            ImageData screenshot = new ImageData(screen);
            screen.Save("screen_full.bmp");

            Rectangle rez;
            for (int i = imageNum; i < imageNum + 4;i++)
            {
                rez = Template.Image(screenshot, new ImageData(templates[i]), 25);
                if (rez.X != 0 && rez.Y != 0)
                {
                    overViewIco.top = 0;
                    overViewIco.bottom = screen.Height;
                    overViewIco.left = rez.X - 10;
                    overViewIco.right = rez.Y + templates[imageNum].Width + 10;
                    return new Point(rez.X, rez.Y);
                }
            }
            return new Point(0, 0);
        }

        public Point FindWindow(int imageNum)
        {
            Window.ShowWindow(hWnd, 3);
            Window.SetFrontWindow(windowName);
            Utility.Delay(1000, 3000);
            Bitmap screen = ScreenShot.Create(overViewIco.left, overViewIco.top, overViewIco.right-overViewIco.left,overViewIco.bottom-overViewIco.top);
            ImageData screenshot = new ImageData(screen);
            screen.Save("screenshot.bmp");
            Rectangle rez;
            rez = Template.Image(screenshot, new ImageData(templates[imageNum]), 25);
            if (rez.X != 0 && rez.Y != 0)
            {
                overViewIco.top = 0;
                overViewIco.bottom = screen.Height;
                overViewIco.left = rez.X - 10;
                overViewIco.right = rez.Y + templates[imageNum].Width + 10;
            }
            return new Point(rez.X, rez.Y);
        }

        public bool Start()
        {
                hWnd = Window.FindWindowByWindowTitle(windowName);
                if (hWnd.ToInt64() != 0)
                {
                    Log("Окно євки найдено: " + windowName);
                    if (LoadIni())
                    {
                        _isStartOk = LoadImageTemplates();
                    }
                    else _isStartOk = false;
                }
                else
                {
                    _isStartOk = false;
                    Log("Окно эвки не найдено: " + windowName);
                }

            return _isStartOk;
        }


        void Log(string Text)
        {
            using (swLog = new StreamWriter(logFile, true, System.Text.Encoding.Default))
            {
                DateTime date;
                if (swLog != null)
                    swLog.WriteLine("["+DateTime.Now.ToString()+"] - "+ Text);
            }
        }

        bool LoadIni()
        {
            bool rez = true;
            XmlNode attr;
            int tmp;
            Log("Попытка загрузки файла конфигурации");
            iniFile = iniFile + number.ToString() + ".xml";
            FileInfo file = new FileInfo(iniFile);
            if (file.Exists)
            {
                XmlDocument xmlFile = new XmlDocument();
                xmlFile.Load(iniFile);
                XmlElement xRoot = xmlFile.DocumentElement;
                for (int i = 0; i < xRoot.ChildNodes.Count; i++)
                {
                    attr = xRoot.ChildNodes[i];
                    switch (xRoot.ChildNodes[i].Name)
                    {
                        case "maxMouseSpeed":
                            if (int.TryParse(attr.InnerText.ToString(), out tmp))
                            {
                                cfg.maxMouseSpeed = tmp;
                                Log("maxMouseSpeed=" + tmp.ToString());
                            }
                            else
                            {
                                Log("Ошибка загрузки конфигурации. maxMouseSpeed не задан");
                                rez = false;
                            }
                            break;
                        case "minMouseSpeed":
                            if (int.TryParse(attr.InnerText.ToString(), out tmp))
                            {
                                cfg.minMouseSpeed = tmp;
                                Log("minMouseSpeed=" + tmp.ToString());
                            }
                            else
                            {
                                Log("Ошибка загрузки конфигурации. minMouseSpeed не задан");
                                rez = false;
                            }
                            break;
                        case "maxClickSpeed":
                            if (int.TryParse(attr.InnerText.ToString(), out tmp))
                            {
                                cfg.maxClickSpeed = tmp;
                                Log("maxClickSpeed=" + tmp.ToString());
                            }
                            else
                            {
                                Log("Ошибка загрузки конфигурации. maxClickSpeed не задан");
                                rez = false;
                            }
                            break;
                        case "minClickSpeed":
                            if (int.TryParse(attr.InnerText.ToString(), out tmp))
                            {
                                cfg.minClickSpeed = tmp;
                                Log("minClickSpeed=" + tmp.ToString());
                            }
                            else
                            {
                                Log("Ошибка загрузки конфигурации. minClickSpeed не задан");
                                rez = false;
                            }
                            break;
                        case "maxDrones":
                            if (int.TryParse(attr.InnerText.ToString(), out tmp))
                            {
                                cfg.maxDrones = tmp;
                                Log("maxDrones=" + tmp.ToString());
                            }
                            else
                            {
                                Log("Ошибка загрузки конфигурации. maxDrones не задан");
                                rez = false;
                            }
                            break;
                        case "maxDroneDistance":
                            if (int.TryParse(attr.InnerText.ToString(), out tmp))
                            {
                                cfg.maxDroneDistance = tmp;
                                Log("maxDroneDistance=" + tmp.ToString());
                            }
                            else
                            {
                                Log("Ошибка загрузки конфигурации. maxDroneDistance не задан");
                                rez = false;
                            }
                            break;
                        case "maxTargets":
                            if (int.TryParse(attr.InnerText.ToString(), out tmp))
                            {
                                cfg.maxTargets = tmp;
                                Log("maxTargets=" + tmp.ToString());
                            }
                            else
                            {
                                Log("Ошибка загрузки конфигурации. maxTargets не задан");
                                rez = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                rez = false;
                Log("Файл конфига не найден");
            }
            return rez;
        }
        bool LoadImageTemplates()
        {
            bool rez = true;
            for (int i = 0; i < klvBmp; i++)
            {
                templates[i] = GetImageByName("_"+i.ToString());
                Log("Подгружена картинка "+i.ToString());
            }
            return rez;
        }

    [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,
        }

        public static Bitmap GetImageByName(string imageName)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            string resourceName = asm.GetName().Name + ".Properties.Resources";
            var rm = new System.Resources.ResourceManager(resourceName, asm);
            return (Bitmap)rm.GetObject(imageName);

        }
    }
}
