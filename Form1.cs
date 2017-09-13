using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using MaterialSkin;
using MaterialSkin.Controls;

namespace CPickX
{
    public partial class Form1 : MaterialForm
    {
        Color c = new Color();
        Form previewF;
        Form previewFshadow;

        bool hoverForm = false;

        bool isActive = false;

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        public Form1()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.LightBlue800, Primary.LightBlue900, Primary.LightBlue500, Accent.Red200, TextShade.WHITE);

            MouseHook.Start();
            MouseHook.MouseAction += new EventHandler(MouseLeftClick);
        }

        private void MouseLeftClick(object sender, EventArgs e)
        {
            if(isActive && !hoverForm)
            {
                Clipboard.SetText("#" + (c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")));
                isActive = false;
                pick_btn.Text = "Pick";
                pick_btn.Location = new Point(366, 176);
                pick_btn.Enabled = true;
                Update_timer.Enabled = false;
                previewF.Hide();
                previewFshadow.Hide();
                notifyIcon1.ShowBalloonTip(2000, "cPick", "#" + (c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")) + " \nColor copied to clipboard.", ToolTipIcon.Info);
                
            }
        }

        private void NotifyIcon1_BalloonTipShown(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipShown += NotifyIcon1_BalloonTipShown;
            notifyIcon1.BalloonTipClicked += NotifyIcon1_BalloonTipClicked;

            Point cursor = new Point();
            GetCursorPos(ref cursor);

            previewFshadow = new Form();
            previewFshadow.BackColor = Color.Black;
            previewFshadow.Opacity = 0.2;
            previewFshadow.FormBorderStyle = FormBorderStyle.None;
            previewFshadow.Bounds = Screen.PrimaryScreen.Bounds;
            previewFshadow.TopMost = false;
            previewFshadow.Size = new Size(42, 42);


            previewF = new Form();
            previewF.BackColor = Color.White;
            previewF.FormBorderStyle = FormBorderStyle.None;
            previewF.Bounds = Screen.PrimaryScreen.Bounds;
            previewF.TopMost = true;
            previewF.Size = new Size(40, 40);

            Application.EnableVisualStyles();
            previewFshadow.ShowInTaskbar = false;

            previewF.ShowInTaskbar = false;
        }

        private void NotifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Show();
            this.Focus();
        }

        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }
        
        private void Update_timer_Tick(object sender, EventArgs e)
        {
            if(isActive)
            {
                Point cursor = new Point();
                GetCursorPos(ref cursor);

                c = GetColorAt(cursor);

                color_lbl.Text = c.R.ToString() + ", " + c.G.ToString() + ", " + c.B.ToString() + " - #" + (c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2"));

                colorPanel_pnl.BackColor = c;

                if (!previewF.Visible) { previewF.Show(); previewFshadow.Show(); }

                previewF.BackColor = c;
                previewF.Location = new Point(cursor.X + 15, cursor.Y + 20);


                previewFshadow.Location = new Point(cursor.X + 17, cursor.Y + 21);
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void pick_btn_Click(object sender, EventArgs e)
        {
            if(isActive)
            {
                Update_timer.Enabled = false;
                isActive = false;
                pick_btn.Text = "Pick";
                pick_btn.Location = new Point(366, 176);
                pick_btn.Enabled = true;
                previewF.Hide(); previewFshadow.Hide();
            }
            else
            {
                Update_timer.Enabled = true;
                isActive = true;
                pick_btn.Enabled = false;
                pick_btn.Text = "Picking...";
                pick_btn.Location = new Point(333, 176);
                previewFshadow.Show();

                previewF.Show();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.Focus();
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                this.ShowInTaskbar = true;
            }
            else
            {
                this.ShowInTaskbar = false;
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            hoverForm = true;
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            hoverForm = false;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape && isActive)
            {
                Update_timer.Enabled = false;
                isActive = false;
                pick_btn.Text = "Pick";
                pick_btn.Location = new Point(366, 176);
                pick_btn.Enabled = true;
                previewF.Hide(); previewFshadow.Hide();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://material.io/guidelines/style/color.html#color-color-palette");
        }
    }

    public static class MouseHook
    {
        public static event EventHandler MouseAction = delegate { };

        public static void Start()
        {
            _hookID = SetHook(_proc);


        }
        public static void stop()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                  GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
          int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseAction(null, new EventArgs());
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
          LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
          IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


    }
}
