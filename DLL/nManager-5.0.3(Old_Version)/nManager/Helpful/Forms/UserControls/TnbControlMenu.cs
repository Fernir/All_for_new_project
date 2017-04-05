namespace nManager.Helpful.Forms.UserControls
{
    using nManager.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TnbControlMenu : Panel
    {
        private PictureBox _closeButton;
        private bool _flagClick;
        private Image _logoImage = Resources.logoImageG;
        private int _positionInitialeX;
        private int _positionInitialeY;
        private PictureBox _reduceButton;
        private Font _titleFont = new Font(new FontFamily("Microsoft Sans Serif"), 12f, GraphicsUnit.Point);
        private Color _titleForeColor = Color.FromArgb(0xde, 0xde, 0xde);
        private Label _titleLabel;
        private PictureBox _tnbLogo;

        public TnbControlMenu()
        {
            Label label = new Label {
                Text = "TheNoobBot"
            };
            this._titleLabel = label;
            base.Size = new Size(0x23f, 0x2b);
            this.InitializeContent();
        }

        private void InitializeContent()
        {
            base.SizeChanged += new EventHandler(this.OnSizeChanged);
            PictureBox box = new PictureBox {
                Visible = true,
                Location = new Point(base.Width - 0x34, 13),
                Size = new Size(13, 14),
                Image = Resources.reduce_button
            };
            this._reduceButton = box;
            this._reduceButton.Click += new EventHandler(this.OnReduce);
            PictureBox box2 = new PictureBox {
                Visible = true,
                Location = new Point(base.Width - 0x19, 13),
                Size = new Size(13, 14),
                Image = Resources.close_button
            };
            this._closeButton = box2;
            this._closeButton.Click += new EventHandler(this.OnClose);
            PictureBox box3 = new PictureBox {
                Visible = true,
                Location = new Point(13, 3),
                Size = new Size(30, 0x21),
                Image = this.LogoImage
            };
            this._tnbLogo = box3;
            Label label = new Label {
                Location = new Point(0x39, 4),
                Size = new Size(450, 0x23),
                Visible = true,
                AutoSize = false,
                BackColor = Color.FromArgb(0x41, 0x41, 0x41),
                Text = this.TitleText,
                Font = this.TitleFont,
                ForeColor = this.TitleForeColor,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this._titleLabel = label;
            base.AutoSize = false;
            base.BackgroundImage = Resources.controlbar;
            base.Controls.Add(this._closeButton);
            base.Controls.Add(this._titleLabel);
            base.Controls.Add(this._tnbLogo);
            base.Controls.Add(this._reduceButton);
            this._tnbLogo.MouseDown += new MouseEventHandler(this.MainFormMouseDown);
            this._tnbLogo.MouseMove += new MouseEventHandler(this.MainFormMouseMove);
            this._tnbLogo.MouseUp += new MouseEventHandler(this.MainFormMouseUp);
            this._titleLabel.MouseDown += new MouseEventHandler(this.MainFormMouseDown);
            this._titleLabel.MouseMove += new MouseEventHandler(this.MainFormMouseMove);
            this._titleLabel.MouseUp += new MouseEventHandler(this.MainFormMouseUp);
            base.MouseDown += new MouseEventHandler(this.MainFormMouseDown);
            base.MouseMove += new MouseEventHandler(this.MainFormMouseMove);
            base.MouseUp += new MouseEventHandler(this.MainFormMouseUp);
            this._closeButton.MouseEnter += new EventHandler(this.MouseEnterCloseButton);
            this._closeButton.MouseLeave += new EventHandler(this.MouseLeaveCloseButton);
            this._reduceButton.MouseEnter += new EventHandler(this.MouseEnterReduceButton);
            this._reduceButton.MouseLeave += new EventHandler(this.MouseLeaveReduceButton);
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
                base.Parent.Location = new Point(base.Parent.Left + (e.X - this._positionInitialeX), base.Parent.Top + (e.Y - this._positionInitialeY));
            }
        }

        private void MainFormMouseUp(object sender, MouseEventArgs e)
        {
            this._flagClick = false;
        }

        private void MouseEnterCloseButton(object sender, EventArgs eventArgs)
        {
            this._closeButton.Image = Resources.close_buttonG;
        }

        private void MouseEnterReduceButton(object sender, EventArgs eventArgs)
        {
            this._reduceButton.Image = Resources.reduce_buttonG;
        }

        private void MouseLeaveCloseButton(object sender, EventArgs eventArgs)
        {
            this._closeButton.Image = Resources.close_button;
        }

        private void MouseLeaveReduceButton(object sender, EventArgs eventArgs)
        {
            this._reduceButton.Image = Resources.reduce_button;
        }

        private void OnClose(object sender, EventArgs eventArgs)
        {
            if (Form.ActiveForm != null)
            {
                Form.ActiveForm.Close();
            }
        }

        private void OnReduce(object sender, EventArgs eventArgs)
        {
            if (Form.ActiveForm != null)
            {
                Form.ActiveForm.WindowState = FormWindowState.Minimized;
            }
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            this._closeButton.Location = new Point(base.Width - 0x19, 13);
            this._reduceButton.Location = new Point(base.Width - 0x34, 13);
            this._titleLabel.Size = new Size(base.Width - 0x73, 0x23);
        }

        [Category("AaTnbControls")]
        public Image LogoImage
        {
            get
            {
                return this._logoImage;
            }
            set
            {
                this._logoImage = value;
                base.Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Font TitleFont
        {
            get
            {
                return this._titleFont;
            }
            set
            {
                this._titleFont = value;
                base.Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Color TitleForeColor
        {
            get
            {
                return this._titleForeColor;
            }
            set
            {
                this._titleForeColor = value;
                base.Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public string TitleText
        {
            get
            {
                return this._titleLabel.Text;
            }
            set
            {
                this._titleLabel.Text = value;
                base.Invalidate();
            }
        }
    }
}

