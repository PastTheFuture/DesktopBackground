using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;


namespace DesktopBackground
{
    public partial class MainForm : Form
    {
        #region Window_NoFocus
        //Makes it so when the form is loaded it doesn't have focus
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr handle, int flags);
        protected override void DefWndProc(ref Message m)
        {
            const int WM_MOUSEACTIVATE = 0x21;
            const int MA_NOACTIVATE = 0x0003;

            switch (m.Msg)
            {
                case WM_MOUSEACTIVATE:
                    m.Result = (IntPtr)MA_NOACTIVATE;
                    return;
            }
            base.DefWndProc(ref m);
        }
        //The official method that doesnt always work:
        protected override bool ShowWithoutActivation
        { get { return true; } }
        #endregion

        #region Window_BottomLayer
        //Moves the window to the bottom of the window stack
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOACTIVATE = 0x0010;
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        #endregion

        Point CenterScreen = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2);

        BackgroundStarfield Starfield;
        BackgroundWallpaper Wallpaper;
        BackgroundPong Pong;

        Rectangle ControlPanelOpenRect;
        Point[] ControlPanelOpenList;
        Point[] ControlPanelOpenListInvert;
        bool ControlPanelOpened;
        Rectangle ControlPanelFull;
        Rectangle ControlPanelStarfieldButton;
        Rectangle ControlPanelWallpaperButton;
        Rectangle ControlPanelPongButton;

        Rectangle ControlPanelExitButton;

        string State = "wall";

        public MainForm()
        {
            InitializeComponent();
            Gametimer.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Starfield = new BackgroundStarfield(this);
            Wallpaper = new BackgroundWallpaper(this);
            Pong = new BackgroundPong(this);
            GameNotification.ContextMenu = new ContextMenu();
            GameNotification.ContextMenu.MenuItems.Add("Wallpaper Mode", new EventHandler(CM_WallpaperModeClick));
            GameNotification.ContextMenu.MenuItems.Add("Dark Souls Fire Gif", new EventHandler(CM_DarksoulsFireClick));
            GameNotification.ContextMenu.MenuItems.Add("Starfield Mode", new EventHandler(CM_StarfieldModeClick));
            GameNotification.ContextMenu.MenuItems.Add("Pong Mode", new EventHandler(CM_PongModeClick));
            GameNotification.ContextMenu.MenuItems.Add("Exit", new EventHandler(CM_ExitClick));
            GameNotification.Icon = SystemIcons.Application;
            GameNotification.Visible = true;

            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            ControlPanelOpenRect = new Rectangle(CenterScreen.X - 10, Screen.PrimaryScreen.WorkingArea.Bottom - 20, 20, 10);
            ControlPanelOpenList = new Point[] { new Point(ControlPanelOpenRect.Left, ControlPanelOpenRect.Bottom), new Point(ControlPanelOpenRect.Left + ControlPanelOpenRect.Width / 2, ControlPanelOpenRect.Top), new Point(ControlPanelOpenRect.Right, ControlPanelOpenRect.Bottom), new Point(ControlPanelOpenRect.Left, ControlPanelOpenRect.Bottom) };
            ControlPanelOpenListInvert = new Point[] { new Point(ControlPanelOpenRect.Left, ControlPanelOpenRect.Top), new Point(ControlPanelOpenRect.Left + ControlPanelOpenRect.Width / 2, ControlPanelOpenRect.Bottom), new Point(ControlPanelOpenRect.Right, ControlPanelOpenRect.Top), new Point(ControlPanelOpenRect.Left, ControlPanelOpenRect.Top) };
            ControlPanelFull = new Rectangle(CenterScreen.X - 120, Screen.PrimaryScreen.WorkingArea.Bottom - 84, 240, 58);
            ControlPanelWallpaperButton = new Rectangle(ControlPanelFull.X + 4, ControlPanelFull.Y + 4, ControlPanelFull.Height - 8, ControlPanelFull.Height - 8);
            ControlPanelStarfieldButton = new Rectangle(ControlPanelWallpaperButton.Right + 8, ControlPanelFull.Y + 4, ControlPanelFull.Height - 8, ControlPanelFull.Height - 8);
            ControlPanelPongButton = new Rectangle(ControlPanelStarfieldButton.Right + 8, ControlPanelFull.Y + 4, ControlPanelFull.Height - 8, ControlPanelFull.Height - 8);

            ControlPanelExitButton = new Rectangle(ControlPanelFull.Right - 4 - (ControlPanelFull.Height - 8), ControlPanelFull.Y + 4, ControlPanelFull.Height - 8, ControlPanelFull.Height - 8);
        }

        private void Gametimer_Tick(object sender, EventArgs e)
        {
            //Moves the window to the bottom layer
            SetWindowPos(Handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Maximized;
            }

            switch (State)
            {
                case "default":
                case "wall":
                    Wallpaper.Update();
                    break;
                case "star":
                    Starfield.Update();
                    break;
                case "pong":
                    Pong.Update();
                    break;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                switch (State)
                {
                    default:
                    case "wall":
                        Wallpaper.Draw(e.Graphics);
                        break;
                    case "star":
                        Starfield.Draw(e.Graphics);
                        break;
                    case "pong":
                        Pong.Draw(e.Graphics);
                        break;
                }
                StringFormat sF = new StringFormat();
                sF.Alignment = StringAlignment.Center; sF.LineAlignment = StringAlignment.Center;
                //Arrow
                if (ControlPanelOpened) e.Graphics.DrawLines(Pens.White, ControlPanelOpenListInvert); else e.Graphics.DrawLines(Pens.White, ControlPanelOpenList);
                //Panel Full
                if (ControlPanelOpened) e.Graphics.FillRectangle(Brushes.DarkGray, ControlPanelFull);
                //Starfield
                if (ControlPanelOpened) e.Graphics.FillRectangle(Brushes.Gray, ControlPanelStarfieldButton);
                if (ControlPanelOpened) e.Graphics.DrawString("St", SystemFonts.MenuFont, Brushes.White, ControlPanelStarfieldButton, sF);
                //Wallpaper
                if (ControlPanelOpened) e.Graphics.FillRectangle(Brushes.Gray, ControlPanelWallpaperButton);
                if (ControlPanelOpened) e.Graphics.DrawString("Wl", SystemFonts.MenuFont, Brushes.White, ControlPanelWallpaperButton, sF);
                //Pong
                if (ControlPanelOpened) e.Graphics.FillRectangle(Brushes.Gray, ControlPanelPongButton);
                if (ControlPanelOpened) e.Graphics.DrawString("Pn", SystemFonts.MenuFont, Brushes.White, ControlPanelPongButton, sF);

                //Exit
                if (ControlPanelOpened) e.Graphics.FillRectangle(Brushes.Gray, ControlPanelExitButton);
                if (ControlPanelOpened) e.Graphics.DrawString("Ex", SystemFonts.MenuFont, Brushes.White, ControlPanelExitButton, sF);
            }
            catch 
            {
                e.Graphics.Clear(Color.Black);
                StringFormat sF = new StringFormat();
                sF.Alignment = StringAlignment.Center; sF.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString("Could not draw live background.", SystemFonts.MessageBoxFont, Brushes.White, CenterScreen, sF);
            }
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (ControlPanelOpenRect.ContainsPoint(this.PointToClient(e.Location)))
            {
                ControlPanelOpened = !ControlPanelOpened;
                this.Invalidate();
            }
            if (ControlPanelOpened)
            {
                if (ControlPanelWallpaperButton.ContainsPoint(this.PointToClient(e.Location)))
                {
                    State = "wall";
                    this.Invalidate();
                }
                if (ControlPanelStarfieldButton.ContainsPoint(this.PointToClient(e.Location)))
                {
                    State = "star";
                    this.Invalidate();
                }
                if (ControlPanelPongButton.ContainsPoint(this.PointToClient(e.Location)))
                {
                    State = "pong";
                    this.Invalidate();
                }
                if (ControlPanelExitButton.ContainsPoint(this.PointToClient(e.Location)))
                {
                    Application.Exit();
                }
            }
        }

        public string GetPathOfWallpaper()
        {
            string pathWallpaper = "";
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
            if (regKey != null)
            {
                pathWallpaper = regKey.GetValue("WallPaper").ToString();
                regKey.Close();
            }
            return pathWallpaper;
        }

        private void CM_ExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void CM_WallpaperModeClick(object sender, EventArgs e)
        {
            State = "wall";
            Wallpaper.DarkSoulsGif = false;
        }
        private void CM_DarksoulsFireClick(object sender, EventArgs e)
        {
            State = "wall";
            Wallpaper.DarkSoulsGif = true;
        }
        private void CM_StarfieldModeClick(object sender, EventArgs e)
        {
            State = "star";
        }
        private void CM_PongModeClick(object sender, EventArgs e)
        {
            State = "pong";
        }
    }

}
