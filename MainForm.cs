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
        
        public Screen ScreenSelection = Screen.PrimaryScreen;

        Dictionary<string, Bitmap> Gifs;

        string State = "wall";

        public MainForm()
        {
            InitializeComponent();
            Gametimer.Start();

            Gifs = new Dictionary<string, Bitmap>();
            Gifs.Add("CyberpunkBalcony", Properties.Resources.CyberpunkBalcony);
            Gifs.Add("CyberPunkCafe", Properties.Resources.CyberPunkCafe);
            Gifs.Add("CyberpunkElevator", Properties.Resources.CyberpunkElevator);
            Gifs.Add("CyberpunkNightRide", Properties.Resources.CyberpunkNightRide);
            Gifs.Add("CyberpunkStandoff", Properties.Resources.CyberpunkStandoff);
            Gifs.Add("CyberpunkSubway", Properties.Resources.CyberpunkSubway);
            Gifs.Add("CyberpunkTrollCave", Properties.Resources.CyberpunkTrollCave);
            Gifs.Add("DarkSoulsBalcony", Properties.Resources.DarkSoulsBalcony);
            Gifs.Add("DarkSoulsCave", Properties.Resources.DarkSoulsCave);
            Gifs.Add("DarkSoulsBorealValley", Properties.Resources.DarkSoulsBorealValley);
            Gifs.Add("DarkSoulsFire", Properties.Resources.DarkSoulsFire);
            Gifs.Add("DarkSoulsFire2", Properties.Resources.DarkSoulsFire2);
            Gifs.Add("Duck", Properties.Resources.Duck);
            Gifs.Add("Earth", Properties.Resources.Earth);
            Gifs.Add("GlobeDots", Properties.Resources.GlobeDots);
            Gifs.Add("LastStand", Properties.Resources.LastStand);
            Gifs.Add("LonelyFire", Properties.Resources.LonelyFire);
            Gifs.Add("OverwatchA", Properties.Resources.OverwatchA);
            Gifs.Add("OverwatchB", Properties.Resources.OverwatchB);
            Gifs.Add("SandCastle", Properties.Resources.SandCastle);
            Gifs.Add("SpaceBattle", Properties.Resources.SpaceBattle);
            Gifs.Add("SpaceBattle2", Properties.Resources.SpaceBattle2);
            Gifs.Add("SpaceBattle3", Properties.Resources.SpaceBattle3);
            Gifs.Add("SpaceshipCrash", Properties.Resources.SpaceshipCrash);
            Gifs.Add("Swamp", Properties.Resources.Swamp);
            Gifs.Add("Tavern", Properties.Resources.Tavern);
            Gifs.Add("Wilds", Properties.Resources.Wilds);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Starfield = new BackgroundStarfield(this);
            Wallpaper = new BackgroundWallpaper(this);
            Pong = new BackgroundPong(this);
            GameNotification.ContextMenu = new ContextMenu();
            MenuItem ScreenFolder = new MenuItem("Screen Select:");
            GameNotification.ContextMenu.MenuItems.Add(ScreenFolder);
            
            for(int x = 0; x < Screen.AllScreens.Length; x++)
            {
                ScreenFolder.MenuItems.Add(x.ToString(), CM_ScreenSelectionClick);
            }

            MenuItem GifFolder = new MenuItem("Gif Background Selection:");
            GameNotification.ContextMenu.MenuItems.Add(GifFolder);
            foreach(string key in Gifs.Keys)
            {
                GifFolder.MenuItems.Add(key, CM_GifClick);
            }


            GameNotification.ContextMenu.MenuItems.Add("Wallpaper Mode", new EventHandler(CM_WallpaperModeClick));
            GameNotification.ContextMenu.MenuItems.Add("Starfield Mode", new EventHandler(CM_StarfieldModeClick));
            GameNotification.ContextMenu.MenuItems.Add("Pong Mode", new EventHandler(CM_PongModeClick));
            GameNotification.ContextMenu.MenuItems.Add("Exit", new EventHandler(CM_ExitClick));
            GameNotification.Icon = SystemIcons.Application;
            GameNotification.Visible = true;

            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            SetWindowPos(Handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        }

        private void Gametimer_Tick(object sender, EventArgs e)
        {
            //Moves the window to the bottom layer
            //SetWindowPos(Handle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
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
            Reset();
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
            }
            catch 
            {
                e.Graphics.Clear(Color.Black);
                StringFormat sF = new StringFormat();
                sF.Alignment = StringAlignment.Center; sF.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString("Could not draw live background.", SystemFonts.MessageBoxFont, Brushes.White, CenterScreen, sF);
            }
        }

        public void Reset()
        {
            Location = ScreenSelection.Bounds.Location;
            Size = ScreenSelection.WorkingArea.Size;
            if (Wallpaper != null && State == "wall") Wallpaper.Reset();
            Invalidate();
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
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
            Wallpaper.currentGif = "";
            Wallpaper.GifState = false;
            DoubleBuffered = false;
            Gametimer.Interval = 1000;
            Reset();
        }
        private void CM_GifClick(object sender, EventArgs e)
        {
            State = "wall";
            Wallpaper.currentGif = ((MenuItem)sender).Text;
            Wallpaper.GifState = true;
            Wallpaper.ChangeGif(Gifs[((MenuItem)sender).Text]);
            DoubleBuffered = true;
            Gametimer.Interval = 10;
            Reset();
        }
        private void CM_ScreenSelectionClick(object sender, EventArgs e)
        {
            ScreenSelection = Screen.AllScreens[int.Parse(((MenuItem)sender).Text)];
            Reset();
        }
        private void CM_StarfieldModeClick(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            Gametimer.Interval = 10;
            State = "star";
            Reset();
        }
        private void CM_PongModeClick(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            Gametimer.Interval = 10;
            State = "pong";
            Reset();
        }
    }

}
