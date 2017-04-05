namespace nManager.Helpful.Forms
{
    using nManager;
    using nManager.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ErrorPopup : Form
    {
        private bool _flagClick;
        private int _positionInitialeX;
        private int _positionInitialeY;
        private PictureBox CloseButton;
        private IContainer components;
        private PictureBox Controlbar;
        private Label ErrorDescription;
        private PictureBox Logo;

        public ErrorPopup(string errorMessage)
        {
            this.InitializeComponent();
            this.ErrorDescription.Text = errorMessage;
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
            {
                base.TopMost = true;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            this.CloseButton.Image = Resources.close_buttonG;
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            this.CloseButton.Image = Resources.close_button;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ErrorPopup));
            this.ErrorDescription = new Label();
            this.Controlbar = new PictureBox();
            this.Logo = new PictureBox();
            this.CloseButton = new PictureBox();
            ((ISupportInitialize) this.Controlbar).BeginInit();
            ((ISupportInitialize) this.Logo).BeginInit();
            ((ISupportInitialize) this.CloseButton).BeginInit();
            base.SuspendLayout();
            this.ErrorDescription.BackColor = Color.White;
            this.ErrorDescription.Font = new Font("Segoe UI", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.ErrorDescription.ForeColor = Color.FromArgb(0x34, 0x34, 0x34);
            this.ErrorDescription.Location = new Point(7, 0x1c);
            this.ErrorDescription.MaximumSize = new Size(0x132, 0xa5);
            this.ErrorDescription.MinimumSize = new Size(0x132, 0xa5);
            this.ErrorDescription.Name = "ErrorDescription";
            this.ErrorDescription.Size = new Size(0x132, 0xa5);
            this.ErrorDescription.TabIndex = 3;
            this.ErrorDescription.Text = "Error description";
            this.ErrorDescription.TextAlign = ContentAlignment.MiddleLeft;
            this.Controlbar.ErrorImage = null;
            this.Controlbar.Image = (Image) manager.GetObject("Controlbar.Image");
            this.Controlbar.InitialImage = null;
            this.Controlbar.Location = new Point(0, 0);
            this.Controlbar.Name = "Controlbar";
            this.Controlbar.Size = new Size(320, 0x16);
            this.Controlbar.TabIndex = 4;
            this.Controlbar.TabStop = false;
            this.Controlbar.MouseDown += new MouseEventHandler(this.MainFormMouseDown);
            this.Controlbar.MouseMove += new MouseEventHandler(this.MainFormMouseMove);
            this.Controlbar.MouseUp += new MouseEventHandler(this.MainFormMouseUp);
            this.Logo.BackColor = Color.Transparent;
            this.Logo.Image = (Image) manager.GetObject("Logo.Image");
            this.Logo.Location = new Point(3, 3);
            this.Logo.Name = "Logo";
            this.Logo.Size = new Size(15, 0x10);
            this.Logo.TabIndex = 5;
            this.Logo.TabStop = false;
            this.Logo.MouseDown += new MouseEventHandler(this.MainFormMouseDown);
            this.Logo.MouseMove += new MouseEventHandler(this.MainFormMouseMove);
            this.Logo.MouseUp += new MouseEventHandler(this.MainFormMouseUp);
            this.CloseButton.ErrorImage = null;
            this.CloseButton.Image = Resources.close_button;
            this.CloseButton.InitialImage = null;
            this.CloseButton.Location = new Point(0x12e, 3);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new Size(13, 14);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new EventHandler(this.CloseButton_Click);
            this.CloseButton.MouseEnter += new EventHandler(this.CloseButton_MouseEnter);
            this.CloseButton.MouseLeave += new EventHandler(this.CloseButton_MouseLeave);
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.ClientSize = new Size(320, 200);
            base.ControlBox = false;
            base.Controls.Add(this.CloseButton);
            base.Controls.Add(this.Logo);
            base.Controls.Add(this.Controlbar);
            base.Controls.Add(this.ErrorDescription);
            this.DoubleBuffered = true;
            this.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ErrorPopup";
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Error";
            base.TopMost = true;
            ((ISupportInitialize) this.Controlbar).EndInit();
            ((ISupportInitialize) this.Logo).EndInit();
            ((ISupportInitialize) this.CloseButton).EndInit();
            base.ResumeLayout(false);
        }

        private void MainFormMouseDown(object sender, MouseEventArgs e)
        {
            this._flagClick = true;
            this._positionInitialeX = e.X;
            this._positionInitialeY = e.Y;
        }

        private void MainFormMouseMove(object sender, MouseEventArgs e)
        {
            if (this._flagClick)
            {
                base.Location = new Point(base.Left + (e.X - this._positionInitialeX), base.Top + (e.Y - this._positionInitialeY));
            }
        }

        private void MainFormMouseUp(object sender, MouseEventArgs e)
        {
            this._flagClick = false;
        }
    }
}

