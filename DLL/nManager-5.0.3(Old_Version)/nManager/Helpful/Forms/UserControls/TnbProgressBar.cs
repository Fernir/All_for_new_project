namespace nManager.Helpful.Forms.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TnbProgressBar : Panel
    {
        private Image _barImage = Resources.barImg;
        private Rectangle _barRect;
        private Color _borderColor = Color.FromArgb(0x79, 0x79, 0x79);
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private int _value;
        private const int Maximum = 100;
        private const int WmPaint = 15;

        public TnbProgressBar()
        {
            base.Size = new Size(base.Size.Width, 10);
        }

        public static void FillPattern(Graphics g, Image image, Rectangle rect)
        {
            for (int i = rect.X; i < rect.Right; i += image.Width)
            {
                for (int j = rect.Y; j < rect.Bottom; j += image.Height)
                {
                    Rectangle destRect = new Rectangle(i, j, Math.Min(image.Width, rect.Right - i), Math.Min(image.Height, rect.Bottom - j));
                    Rectangle srcRect = new Rectangle(0, 0, destRect.Width, destRect.Height);
                    g.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 15)
            {
                Graphics graphics = Graphics.FromHwnd(base.Handle);
                Rectangle bounds = new Rectangle(0, 0, base.Width, base.Height);
                ControlPaint.DrawBorder(graphics, bounds, this.BorderColor, this.BorderStyle);
                this._barRect = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2);
                if (this.Value != 0)
                {
                    this._barRect.Width = (int) (this._barRect.Width * (((double) this.Value) / 100.0));
                    FillPattern(graphics, this.BarImage, this._barRect);
                }
                if (this.Value != 100)
                {
                    graphics.FillRectangle(new SolidBrush(this.BackColor), this._barRect.Width + this._barRect.X, this._barRect.Y, (base.Width - this._barRect.Width) - 2, this._barRect.Height);
                }
            }
        }

        [Category("Appearance")]
        public Image BarImage
        {
            get
            {
                return this._barImage;
            }
            set
            {
                if (value != null)
                {
                    this._barImage = value;
                }
                base.Invalidate();
            }
        }

        [Category("Appearance")]
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

        [Category("Appearance")]
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

        public System.Windows.Forms.DrawMode DrawMode { get; set; }

        [Category("Appearance")]
        public int Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (value != this._value)
                {
                    if (value < 0)
                    {
                        value = 0;
                    }
                    if (value > 100)
                    {
                        value = 100;
                    }
                    this._value = value;
                    base.Invalidate(this._barRect);
                }
            }
        }
    }
}

