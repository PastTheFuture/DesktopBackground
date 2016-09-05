namespace DesktopBackground
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Gametimer = new System.Windows.Forms.Timer(this.components);
            this.GameNotification = new System.Windows.Forms.NotifyIcon(this.components);
            this.DarkSoulsFire = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DarkSoulsFire)).BeginInit();
            this.SuspendLayout();
            // 
            // Gametimer
            // 
            this.Gametimer.Interval = 10;
            this.Gametimer.Tick += new System.EventHandler(this.Gametimer_Tick);
            // 
            // GameNotification
            // 
            this.GameNotification.Text = "ImmortalWall";
            this.GameNotification.Visible = true;
            // 
            // DarkSoulsFire
            // 
            this.DarkSoulsFire.Image = global::DesktopBackground.Properties.Resources.DarkSoulsFire;
            this.DarkSoulsFire.Location = new System.Drawing.Point(69, 70);
            this.DarkSoulsFire.Name = "DarkSoulsFire";
            this.DarkSoulsFire.Size = new System.Drawing.Size(100, 50);
            this.DarkSoulsFire.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.DarkSoulsFire.TabIndex = 0;
            this.DarkSoulsFire.TabStop = false;
            this.DarkSoulsFire.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.ControlBox = false;
            this.Controls.Add(this.DarkSoulsFire);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseClick);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.DarkSoulsFire)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer Gametimer;
        private System.Windows.Forms.NotifyIcon GameNotification;
        public System.Windows.Forms.PictureBox DarkSoulsFire;
    }
}

