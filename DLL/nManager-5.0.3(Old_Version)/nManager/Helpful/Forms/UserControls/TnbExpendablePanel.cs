namespace nManager.Helpful.Forms.UserControls
{
    using nManager.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class TnbExpendablePanel : Panel
    {
        private Color _borderColor = Color.FromArgb(0x34, 0x34, 0x34);
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private Color _contentBackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
        private Size _contentSize = new Size(0x22c, 200);
        private bool _fold;
        private Image _folderImage = Resources.PanelExpendableMinusImg;
        private readonly Label _header = new Label();
        private Size _headerSize = new Size(0x22c, 0x24);
        private int _orderIndex = -1;
        private PictureBox _toggler;
        private Image _unfolderImage = Resources.PanelExpendablePlusImg;
        public EventHandler OnOrderChanged;
        public EventHandler OnStatusChanged;
        private const int WmPaint = 15;

        public TnbExpendablePanel()
        {
            Size size = new Size(this.HeaderSize.Width, this.HeaderSize.Height + this.ContentSize.Height);
            base.Size = size;
            if (this.HeaderImage == null)
            {
                this.HeaderImage = Resources.panelcontrolHeaderbottomborder;
            }
            if (this.TitleForeColor == new Color())
            {
                this.TitleForeColor = Color.FromArgb(0xff, 0xff, 0xff);
            }
            this.InitializeHeader();
            base.Padding = new Padding(0, 0, 0, 12);
            base.BackColor = this._contentBackColor;
        }

        private void InitializeHeader()
        {
            this._toggler = new PictureBox();
            base.Controls.Add(this._toggler);
            this._toggler.Image = !this.Fold ? this.FolderImage : this.UnfolderImage;
            this._toggler.Location = new Point(this.HeaderSize.Width - 40, 0x11);
            this._toggler.Visible = true;
            this._toggler.Size = new Size(7, 6);
            base.Controls.Add(this._header);
            this._header.Visible = true;
            this._header.AutoSize = false;
            this._header.Text = "Expendable Panel TitleText";
            this._header.Font = this.TitleFont;
            this._header.ForeColor = this.TitleForeColor;
            this._header.TextAlign = ContentAlignment.MiddleCenter;
            this._header.Size = this.HeaderSize;
            this._header.Location = new Point(0, 0);
            if (this.HeaderImage != null)
            {
                this._header.Image = this.HeaderImage;
            }
            else
            {
                this._header.BackColor = this.HeaderBackColor;
            }
            this._header.Invalidate();
            this._header.Click += new EventHandler(this.OnClickEvent);
            this.Fold = true;
        }

        private void OnClickEvent(object sender, EventArgs eventArgs)
        {
            this.Fold = !this.Fold;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            int msg = m.Msg;
        }

        public Color BackColor
        {
            get
            {
                return this._contentBackColor;
            }
            set
            {
                this._contentBackColor = value;
            }
        }

        [Category("AaTnbControls")]
        public Color BorderColor
        {
            get
            {
                return this._borderColor;
            }
            set
            {
                this._borderColor = value;
                base.Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public ButtonBorderStyle BorderStyle
        {
            get
            {
                return this._borderStyle;
            }
            set
            {
                this._borderStyle = value;
                base.Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Size ContentSize
        {
            get
            {
                return new Size(this.HeaderSize.Width, this._contentSize.Height);
            }
            set
            {
                this._contentSize = new Size(this.HeaderSize.Width, value.Height);
            }
        }

        [Category("AaTnbControls")]
        public bool Fold
        {
            get
            {
                return this._fold;
            }
            set
            {
                this._fold = value;
                if (value)
                {
                    this.AutoSize = false;
                    Size size = new Size(this.HeaderSize.Width, this.HeaderSize.Height);
                    this._toggler.Image = Resources.PanelExpendablePlusImg;
                    base.Size = size;
                }
                else
                {
                    this.AutoSize = true;
                    Size size2 = new Size(this.HeaderSize.Width, this.HeaderSize.Height + this.ContentSize.Height);
                    this._toggler.Image = Resources.PanelExpendableMinusImg;
                    base.Size = size2;
                    this.MaximumSize = new Size(this.HeaderSize.Width, 0);
                    this.MinimumSize = new Size(this.HeaderSize.Width, this.HeaderSize.Height);
                }
                if (this.OnStatusChanged != null)
                {
                    this.OnStatusChanged(this, EventArgs.Empty);
                }
            }
        }

        [Category("AaTnbControls")]
        public Image FolderImage
        {
            get
            {
                return this._folderImage;
            }
            set
            {
                if (value != null)
                {
                    this._folderImage = value;
                }
            }
        }

        [Category("AaTnbControls")]
        public Color HeaderBackColor
        {
            get
            {
                return this._header.BackColor;
            }
            set
            {
                this._header.BackColor = value;
            }
        }

        [Category("AaTnbControls")]
        public Image HeaderImage
        {
            get
            {
                return this._header.Image;
            }
            set
            {
                this._header.Image = value;
            }
        }

        [Category("AaTnbControls")]
        public Size HeaderSize
        {
            get
            {
                return this._headerSize;
            }
            set
            {
                this._headerSize = new Size(value.Width, 0x24);
            }
        }

        [Category("AaTnbControls")]
        public int OrderIndex
        {
            get
            {
                return this._orderIndex;
            }
            set
            {
                this._orderIndex = value;
                if (this.OnOrderChanged != null)
                {
                    this.OnOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        [Category("AaTnbControls")]
        public Font TitleFont
        {
            get
            {
                return this._header.Font;
            }
            set
            {
                this._header.Font = value;
            }
        }

        [Category("AaTnbControls")]
        public Color TitleForeColor
        {
            get
            {
                return this._header.ForeColor;
            }
            set
            {
                this._header.ForeColor = value;
            }
        }

        [Category("AaTnbControls")]
        public string TitleText
        {
            get
            {
                return this._header.Text;
            }
            set
            {
                this._header.Text = value;
            }
        }

        [Category("AaTnbControls")]
        public Image UnfolderImage
        {
            get
            {
                return this._unfolderImage;
            }
            set
            {
                if (value != null)
                {
                    this._unfolderImage = value;
                }
            }
        }
    }
}

