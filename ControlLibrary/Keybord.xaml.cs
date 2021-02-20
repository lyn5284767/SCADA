using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ControlLibrary
{
    /// <summary>
    /// Keybord.xaml 的交互逻辑
    /// </summary>
    public partial class Keybord : Window
    {
        [DllImport("User32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIdex);

        [DllImport("User32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("User32.dll")]
        public static extern void keybd_event(byte bVK, byte bScan, Int32 dwFlags, int dwExtraInfo);

        [DllImport("User32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);
        private static Keybord _instance = null;
        private static readonly object syncRoot = new object();

        public static Keybord Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Keybord();
                        }
                    }
                }
                return _instance;
            }
        }
        public Keybord()
        {
            InitializeComponent();
        }
        public Keybord(string visb)
        {
            InitializeComponent();
            if (visb == "1")
            {
                this.Width = 210;
                this.Height = 300;
            }
            else
            {
                this.Width = 10;
                this.Height = 10;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button keybtn = sender as System.Windows.Controls.Button;
            if (keybtn.Name == "cmd1")
            {
                addNumkeyINput(0x31);
            }
            else if (keybtn.Name == "cmd2")
            {
                addNumkeyINput(0x32);
            }
            else if (keybtn.Name == "cmd3")
            {
                addNumkeyINput(0x33);
            }
            else if (keybtn.Name == "cmd4")
            {
                addNumkeyINput(0x34);
            }
            else if (keybtn.Name == "cmd5")
            {
                addNumkeyINput(0x35);
            }
            else if (keybtn.Name == "cmd6")
            {
                addNumkeyINput(0x36);

            }
            else if (keybtn.Name == "cmd7")
            {
                addNumkeyINput(0x37);
            }
            else if (keybtn.Name == "cmd8")
            {
                addNumkeyINput(0x38);
            }
            else if (keybtn.Name == "cmd9")
            {
                addNumkeyINput(0x39);
            }
            else if (keybtn.Name == "cmd0")
            {
                addNumkeyINput(0x30);

            }
            else if (keybtn.Name == "cmdBackspace")//backspace
            {
                addNumkeyINput(0x08);
            }
            else if (keybtn.Name == "cmdpoint")
            {
                addNumkeyINput(0x6E);
            }
            else if (keybtn.Name == "CmdClose")//关闭键盘
            {
                this.Height = 10;
                this.Width = 10;
                this.WindowState = WindowState.Minimized;
            }
        }

        public const Int32 WM_SYSCOMMAND = 274;
        public const UInt32 SC_CLOSE = 61536;
        private const int SC_MINIMIZE = 0xF020;   //最小化   
        private const int SC_RESTORE = 0xF120;     //还原 

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private static void addNumkeyINput(byte input)
        {
            keybd_event(input, 0, 0, 0);
            keybd_event(input, 0, 0x02, 0);
        }
        Timer timer = new Timer();
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr a = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            int temp = GetWindowLong(a, -20);
            SetWindowLong(a, -20, temp | 0x08000000);
            HwndSource.FromHwnd(a).AddHook(new HwndSourceHook(WndProc));
            double height = Screen.PrimaryScreen.Bounds.Height;
            double width = Screen.PrimaryScreen.Bounds.Width;
            this.Left = (width - 960) / 2; this.Top = height - 400;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.ShowInTaskbar = false;
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - 210;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - 300;
            this.Left = x;
            this.Top = y;
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0216)
            {
                System.Drawing.Rectangle rect = (System.Drawing.Rectangle)Marshal.PtrToStructure(lParam, typeof(System.Drawing.Rectangle));
                this.Left = rect.Left;
                this.Top = rect.Top;
            }
            return IntPtr.Zero;
        }

        private void cmd1_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        public struct GUITHREADINFO
        {
            public int cbSize;
            public int flags;
            public int hwndActive;
            public int hwndFocus;
            public int hwndCapture;
            public int hwndMenuOwner;
            public int hwndMoveSize;
            public int hwndCaret;
            public System.Drawing.Rectangle rcCaret;
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetGUIThreadInfo(uint idThread, ref GUITHREADINFO lpgui);

        [DllImport("user32")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;

            IntPtr hWnd = GetForegroundWindow();
            uint processId;
            uint threadid = GetWindowThreadProcessId(hWnd, out processId);
            GUITHREADINFO lpgui = new GUITHREADINFO();
            lpgui.cbSize = Marshal.SizeOf(lpgui);

            if (GetGUIThreadInfo(threadid, ref lpgui))
            {
                if (lpgui.hwndCaret != 0)
                {
                    this.Opacity = 1;
                    //this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.Opacity = 0;
                    // this.WindowState = WindowState.Minimized;
                }

            }

            timer.Enabled = true;
        }



        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));
        }

        private void KeyBord_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
            }
            if (this.WindowState == WindowState.Maximized || this.WindowState == WindowState.Normal)
            {
                this.Height = 280;
                this.Width = 210;
                //this.Show();
            }
        }
    }
}
