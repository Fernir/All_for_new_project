namespace nManager.Helpful.Forms.UserControls
{
    using nManager.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class TnbExpendablePanel : Panel
    {
        private int _agagiaki = -1;
        private Size _erapeaSeuxoq = new Size(0x22c, 0x24);
        private bool _geavifeaciu;
        private readonly Label _header = new Label();
        private Image _jeapowu = Resources.PanelExpendablePlusImg;
        private Color _jifateakoAsa = Color.FromArgb(0x34, 0x34, 0x34);
        private Image _ojojiejaufije = Resources.PanelExpendableMinusImg;
        private ButtonBorderStyle _rudiuxiUsiah = ButtonBorderStyle.Solid;
        private const int _ujebuaniJeuton = 15;
        private Size _vacesiceviumuEkiuhiuji = new Size(0x22c, 200);
        private PictureBox _xeoqaetIgehean;
        private Color _xievixirQojuaqe = Color.FromArgb(0xe8, 0xe8, 0xe8);
        public EventHandler OnOrderChanged;
        public EventHandler OnStatusChanged;

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
            this.AtoepaedoefuUdah();
            base.Padding = new Padding(0, 0, 0, 12);
            base.BackColor = this._xievixirQojuaqe;
        }

        private void AtoepaedoefuUdah()
        {
            this._xeoqaetIgehean = new PictureBox();
            base.Controls.Add(this._xeoqaetIgehean);
            this._xeoqaetIgehean.Image = !this.Fold ? this.FolderImage : this.UnfolderImage;
            this._xeoqaetIgehean.Location = new Point(this.HeaderSize.Width - 40, 0x11);
            this._xeoqaetIgehean.Visible = true;
            this._xeoqaetIgehean.Size = new Size(7, 6);
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
            this._header.Click += new EventHandler(this.Ipaineoho);
            this.Fold = true;
        }

        private void Ipaineoho(object moleileucucisUgofe, EventArgs saoxevi)
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
                return this._xievixirQojuaqe;
            }
            set
            {
                this._xievixirQojuaqe = value;
            }
        }

        [Category("AaTnbControls")]
        public Color BorderColor
        {
            get
            {
                return this._jifateakoAsa;
            }
            set
            {
                this._jifateakoAsa = value;
                base.Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public ButtonBorderStyle BorderStyle
        {
            get
            {
                return this._rudiuxiUsiah;
            }
            set
            {
                this._rudiuxiUsiah = value;
                base.Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Size ContentSize
        {
            get
            {
                return new Size(this.HeaderSize.Width, this._vacesiceviumuEkiuhiuji.Height);
            }
            set
            {
                this._vacesiceviumuEkiuhiuji = new Size(this.HeaderSize.Width, value.Height);
            }
        }

        [Category("AaTnbControls")]
        public bool Fold
        {
            get
            {
                return this._geavifeaciu;
            }
            set
            {
                this._geavifeaciu = value;
                if (value)
                {
                    this.AutoSize = false;
                    Size size = new Size(this.HeaderSize.Width, this.HeaderSize.Height);
                    this._xeoqaetIgehean.Image = Resources.PanelExpendablePlusImg;
                    base.Size = size;
                }
                else
                {
                    this.AutoSize = true;
                    Size size2 = new Size(this.HeaderSize.Width, this.HeaderSize.Height + this.ContentSize.Height);
                    this._xeoqaetIgehean.Image = Resources.PanelExpendableMinusImg;
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
                return this._ojojiejaufije;
            }
            set
            {
                if (value != null)
                {
                    this._ojojiejaufije = value;
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
                return this._erapeaSeuxoq;
            }
            set
            {
                this._erapeaSeuxoq = new Size(value.Width, 0x24);
            }
        }

        [Category("AaTnbControls")]
        public int OrderIndex
        {
            get
            {
                return this._agagiaki;
            }
            set
            {
                this._agagiaki = value;
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
                return this._jeapowu;
            }
            set
            {
                if (value != null)
                {
                    this._jeapowu = value;
                }
            }
        }
    }
}

