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
            if(sender.ToString() == "LBDOWN")
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

            if (sender.ToString() == "MMOVE")
            {
                if (isActive)
                {
                    Point cursor = new Point();
                    GetCursorPos(ref cursor);

                    c = GetColorAt(cursor);

                    string hex = "#" + (c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2"));
                    string material = isMaterialColor(hex);
                    if (material != null) material = " - " + material;
                    color_lbl.Text = c.R.ToString() + ", " + c.G.ToString() + ", " + c.B.ToString() + " - " + hex + material;

                    colorPanel_pnl.BackColor = c;

                    if (!previewF.Visible) { previewF.Show(); previewFshadow.Show(); }

                    previewF.BackColor = c;
                    previewF.Location = new Point(cursor.X + 15, cursor.Y + 20);


                    previewFshadow.Location = new Point(cursor.X + 17, cursor.Y + 21);
                }
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
            previewFshadow.TopMost = true;
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
            //if(isActive)
            //{
            //    Point cursor = new Point();
            //    GetCursorPos(ref cursor);

            //    c = GetColorAt(cursor);

            //    color_lbl.Text = c.R.ToString() + ", " + c.G.ToString() + ", " + c.B.ToString() + " - #" + (c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2"));

            //    colorPanel_pnl.BackColor = c;

            //    if (!previewF.Visible) { previewF.Show(); previewFshadow.Show(); }

            //    previewF.BackColor = c;
            //    previewF.Location = new Point(cursor.X + 15, cursor.Y + 20);


            //    previewFshadow.Location = new Point(cursor.X + 17, cursor.Y + 21);
            //}

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
                Update_timer.Enabled = false;
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

        private void colorPanel_pnl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void colorPanel_pnl_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("#" + (c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")));
            notifyIcon1.ShowBalloonTip(2000, "cPick", "#" + (c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")) + " \nColor copied to clipboard.", ToolTipIcon.Info);
        }

        private string isMaterialColor(string color)
        {
            switch(color)
            {
                case "#FFEBEE":
                    return "Red 50";
                case "#FFCDD2":
                    return "Red 100";
                case "#EF9A9A":
                    return "Red 200";
                case "#E57373":
                    return "Red 300";
                case "#EF5350":
                    return "Red 400";
                case "#F44336":
                    return "Red 500";
                case "#E53935":
                    return "Red 600";
                case "#D32F2F":
                    return "Red 700";
                case "#C62828":
                    return "Red 800";
                case "#B71C1C":
                    return "Red 900";

                case "#FCE4EC":
                    return "Pink 50";
                case "#F8BBD0":
                    return "Pink 100";
                case "#F48FB1":
                    return "Pink 200";
                case "#F06292":
                    return "Pink 300";
                case "#EC407A":
                    return "Pink 400";
                case "#E91E63":
                    return "Pink 500";
                case "#D81B60":
                    return "Pink 600";
                case "#C2185B":
                    return "Pink 700";
                case "#AD1457":
                    return "Pink 800";
                case "#880E4F":
                    return "Pink 900";

                case "#F3E5F5":
                    return "Purple 50";
                case "#E1BEE7":
                    return "Purple 100";
                case "#CE93D8":
                    return "Purple 200";
                case "#BA68C8":
                    return "Purple 300";
                case "#AB47BC":
                    return "Purple 400";
                case "#9C27B0":
                    return "Purple 500";
                case "#8E24AA":
                    return "Purple 600";
                case "#7B1FA2":
                    return "Purple 700";
                case "#6A1B9A":
                    return "Purple 800";
                case "#4A148C":
                    return "Purple 900";

                case "#EDE7F6":
                    return "Deep Purple 50";
                case "#D1C4E9":
                    return "Deep Purple 100";
                case "#B39DDB":
                    return "Deep Purple 200";
                case "#9575CD":
                    return "Deep Purple 300";
                case "#7E57C2":
                    return "Deep Purple 400";
                case "#673AB7":
                    return "Deep Purple 500";
                case "#5E35B1":
                    return "Deep Purple 600";
                case "#512DA8":
                    return "Deep Purple 700";
                case "#4527A0":
                    return "Deep Purple 800";
                case "#311B92":
                    return "Deep Purple 900";

                case "#E8EAF6":
                    return "Indigo 50";
                case "#C5CAE9":
                    return "Indigo 100";
                case "#9FA8DA":
                    return "Indigo 200";
                case "#7986CB":
                    return "Indigo 300";
                case "#5C6BC0":
                    return "Indigo 400";
                case "#3F51B5":
                    return "Indigo 500";
                case "#3949AB":
                    return "Indigo 600";
                case "#303F9F":
                    return "Indigo 700";
                case "#283593":
                    return "Indigo 800";
                case "#1A237E":
                    return "Indigo 900";

                case "#E3F2FD":
                    return "Blue 50";
                case "#BBDEFB":
                    return "Blue 100";
                case "#90CAF9":
                    return "Blue 200";
                case "#64B5F6":
                    return "Blue 300";
                case "#42A5F5":
                    return "Blue 400";
                case "#2196F3":
                    return "Blue 500";
                case "#1E88E5":
                    return "Blue 600";
                case "#1976D2":
                    return "Blue 700";
                case "#1565C0":
                    return "Blue 800";
                case "#0D47A1":
                    return "Blue 900";

                case "#E1F5FE":
                    return "Light Blue 50";
                case "#B3E5FC":
                    return "Light Blue 100";
                case "#81D4FA":
                    return "Light Blue 200";
                case "#4FC3F7":
                    return "Light Blue 300";
                case "#29B6F6":
                    return "Light Blue 400";
                case "#03A9F4":
                    return "Light Blue 500";
                case "#039BE5":
                    return "Light Blue 600";
                case "#0288D1":
                    return "Light Blue 700";
                case "#0277BD":
                    return "Light Blue 800";
                case "#01579B":
                    return "Light Blue 900";

                case "#E0F7FA":
                    return "Cyan 50";
                case "#B2EBF2":
                    return "Cyan 100";
                case "#80DEEA":
                    return "Cyan 200";
                case "#4DD0E1":
                    return "Cyan 300";
                case "#26C6DA":
                    return "Cyan 400";
                case "#00BCD4":
                    return "Cyan 500";
                case "#00ACC1":
                    return "Cyan 600";
                case "#0097A7":
                    return "Cyan 700";
                case "#00838F":
                    return "Cyan 800";
                case "#006064":
                    return "Cyan 900";

                case "#E0F2F1":
                    return "Teal 50";
                case "#B2DFDB":
                    return "Teal 100";
                case "#80CBC4":
                    return "Teal 200";
                case "#4DB6AC":
                    return "Teal 300";
                case "#26A69A":
                    return "Teal 400";
                case "#009688":
                    return "Teal 500";
                case "#00897B":
                    return "Teal 600";
                case "#00796B":
                    return "Teal 700";
                case "#00695C":
                    return "Teal 800";
                case "#004D40":
                    return "Teal 900";

                case "#E8F5E9":
                    return "Green 50";
                case "#C8E6C9":
                    return "Green 100";
                case "#A5D6A7":
                    return "Green 200";
                case "#81C784":
                    return "Green 300";
                case "#66BB6A":
                    return "Green 400";
                case "#4CAF50":
                    return "Green 500";
                case "#43A047":
                    return "Green 600";
                case "#388E3C":
                    return "Green 700";
                case "#2E7D32":
                    return "Green 800";
                case "#1B5E20":
                    return "Green 900";

                case "#F1F8E9":
                    return "Light Green 50";
                case "#DCEDC8":
                    return "Light Green 100";
                case "#C5E1A5":
                    return "Light Green 200";
                case "#AED581":
                    return "Light Green 300";
                case "#9CCC65":
                    return "Light Green 400";
                case "#8BC34A":
                    return "Light Green 500";
                case "#7CB342":
                    return "Light Green 600";
                case "#689F38":
                    return "Light Green 700";
                case "#558B2F":
                    return "Light Green 800";
                case "#33691E":
                    return "Light Green 900";

                case "#F9FBE7":
                    return "Lime 50";
                case "#F0F4C3":
                    return "Lime 100";
                case "#E6EE9C":
                    return "Lime 200";
                case "#DCE775":
                    return "Lime 300";
                case "#D4E157":
                    return "Lime 400";
                case "#CDDC39":
                    return "Lime 500";
                case "#C0CA33":
                    return "Lime 600";
                case "#AFB42B":
                    return "Lime 700";
                case "#9E9D24":
                    return "Lime 800";
                case "#827717":
                    return "Lime 900";

                case "#FFFDE7":
                    return "Yellow 50";
                case "#FFF9C4":
                    return "Yellow 100";
                case "#FFF59D":
                    return "Yellow 200";
                case "#FFF176":
                    return "Yellow 300";
                case "#FFEE58":
                    return "Yellow 400";
                case "#FFEB3B":
                    return "Yellow 500";
                case "#FDD835":
                    return "Yellow 600";
                case "#FBC02D":
                    return "Yellow 700";
                case "#F9A825":
                    return "Yellow 800";
                case "#F57F17":
                    return "Yellow 900";

                case "#FFF8E1":
                    return "Amber 50";
                case "#FFECB3":
                    return "Amber 100";
                case "#FFE082":
                    return "Amber 200";
                case "#FFD54F":
                    return "Amber 300";
                case "#FFCA28":
                    return "Amber 400";
                case "#FFC107":
                    return "Amber 500";
                case "#FFB300":
                    return "Amber 600";
                case "#FFA000":
                    return "Amber 700";
                case "#FF8F00":
                    return "Amber 800";
                case "#FF6F00":
                    return "Amber 900";

                case "#FFF3E0":
                    return "Orange 50";
                case "#FFE0B2":
                    return "Orange 100";
                case "#FFCC80":
                    return "Orange 200";
                case "#FFB74D":
                    return "Orange 300";
                case "#FFA726":
                    return "Orange 400";
                case "#FF9800":
                    return "Orange 500";
                case "#FB8C00":
                    return "Orange 600";
                case "#F57C00":
                    return "Orange 700";
                case "#EF6C00":
                    return "Orange 800";
                case "#E65100":
                    return "Orange 900";

                case "#FBE9E7":
                    return "Deep Orange 50";
                case "#FFCCBC":
                    return "Deep Orange 100";
                case "#FFAB91":
                    return "Deep Orange 200";
                case "#FF8A65":
                    return "Deep Orange 300";
                case "#FF7043":
                    return "Deep Orange 400";
                case "#FF5722":
                    return "Deep Orange 500";
                case "#F4511E":
                    return "Deep Orange 600";
                case "#E64A19":
                    return "Deep Orange 700";
                case "#D84315":
                    return "Deep Orange 800";
                case "#BF360C":
                    return "Deep Orange 900";

                case "#EFEBE9":
                    return "Brown 50";
                case "#D7CCC8":
                    return "Brown 100";
                case "#BCAAA4":
                    return "Brown 200";
                case "#A1887F":
                    return "Brown 300";
                case "#8D6E63":
                    return "Brown 400";
                case "#795548":
                    return "Brown 500";
                case "#6D4C41":
                    return "Brown 600";
                case "#5D4037":
                    return "Brown 700";
                case "#4E342E":
                    return "Brown 800";
                case "#3E2723":
                    return "Brown 900";

                case "#FAFAFA":
                    return "Grey 50";
                case "#F5F5F5":
                    return "Grey 100";
                case "#EEEEEE":
                    return "Grey 200";
                case "#E0E0E0":
                    return "Grey 300";
                case "#BDBDBD":
                    return "Grey 400";
                case "#9E9E9E":
                    return "Grey 500";
                case "#757575":
                    return "Grey 600";
                case "#616161":
                    return "Grey 700";
                case "#424242":
                    return "Grey 800";
                case "#212121":
                    return "Grey 900";

                case "#ECEFF1":
                    return "Blue Grey 50";
                case "#CFD8DC":
                    return "Blue Grey 100";
                case "#B0BEC5":
                    return "Blue Grey 200";
                case "#90A4AE":
                    return "Blue Grey 300";
                case "#78909C":
                    return "Blue Grey 400";
                case "#607D8B":
                    return "Blue Grey 500";
                case "#546E7A":
                    return "Blue Grey 600";
                case "#455A64":
                    return "Blue Grey 700";
                case "#37474F":
                    return "Blue Grey 800";
                case "#263238":
                    return "Blue Grey 900";

                
                case "#FF8A80":
                    return "Red A100";
                case "#FF5252":
                    return "Red A200";
                case "#FF1744":
                    return "Red A400";
                case "#D50000":
                    return "Red A700";

                case "#FF80AB":
                    return "Pink A100";
                case "#FF4081":
                    return "Pink A200";
                case "#F50057":
                    return "Pink A400";
                case "#C51162":
                    return "Pink A700";

                case "#EA80FC":
                    return "Purple A100";
                case "#E040FB":
                    return "Purple A200";
                case "#D500F9":
                    return "Purple A400";
                case "#AA00FF":
                    return "Purple A700";

                case "#B388FF":
                    return "Deep Purple A100";
                case "#7C4DFF":
                    return "Deep Purple A200";
                case "#651FFF":
                    return "Deep Purple A400";
                case "#6200EA":
                    return "Deep Purple A700";

                case "#8C9EFF":
                    return "Indigo A100";
                case "#536DFE":
                    return "Indigo A200";
                case "#3D5AFE":
                    return "Indigo A400";
                case "#304FFE":
                    return "Indigo A700";

                case "#82B1FF":
                    return "Blue A100";
                case "#448AFF":
                    return "Blue A200";
                case "#2979FF":
                    return "Blue A400";
                case "#2962FF":
                    return "Blue A700";

                case "#80D8FF":
                    return "Light Blue A100";
                case "#40C4FF":
                    return "Light Blue A200";
                case "#00B0FF":
                    return "Light Blue A400";
                case "#0091EA":
                    return "Light Blue A700";

                case "#84FFFF":
                    return "Cyan A100";
                case "#18FFFF":
                    return "Cyan A200";
                case "#00E5FF":
                    return "Cyan A400";
                case "#00B8D4":
                    return "Cyan A700";

                case "#A7FFEB":
                    return "Teal A100";
                case "#64FFDA":
                    return "Teal A200";
                case "#1DE9B6":
                    return "Teal A400";
                case "#00BFA5":
                    return "Teal A700";

                case "#B9F6CA":
                    return "Green A100";
                case "#69F0AE":
                    return "Green A200";
                case "#00E676":
                    return "Green A400";
                case "#00C853":
                    return "Green A700";

                case "#CCFF90":
                    return "Light Green A100";
                case "#B2FF59":
                    return "Light Green A200";
                case "#76FF03":
                    return "Light Green A400";
                case "#64DD17":
                    return "Light Green A700";

                case "#F4FF81":
                    return "Lime A100";
                case "#EEFF41":
                    return "Lime A200";
                case "#C6FF00":
                    return "Lime A400";
                case "#AEEA00":
                    return "Lime A700";

                case "#FFFF8D":
                    return "Yellow A100";
                case "#FFFF00":
                    return "Yellow A200";
                case "#FFEA00":
                    return "Yellow A400";
                case "#FFD600":
                    return "Yellow A700";

                case "#FFE57F":
                    return "Amber A100";
                case "#FFD740":
                    return "Amber A200";
                case "#FFC400":
                    return "Amber A400";
                case "#FFAB00":
                    return "Amber A700";

                case "#FFD180":
                    return "Orange A100";
                case "#FFAB40":
                    return "Orange A200";
                case "#FF9100":
                    return "Orange A400";
                case "#FF6D00":
                    return "Orange A700";

                case "#FF9E80":
                    return "Deep Orange A100";
                case "#FF6E40":
                    return "Deep Orange A200";
                case "#FF3D00":
                    return "Deep Orange A400";
                case "#DD2C00":
                    return "Deep Orange A700";

                


                case "#000000":
                    return "Black";

                case "#FFFFFF":
                    return "White";

                default:
                    return null;
            }
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
                MouseAction("LBDOWN", new EventArgs());
            }
            if (nCode >= 0 && MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseAction("MMOVE", new EventArgs());
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
