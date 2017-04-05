namespace nManager.Helpful.Forms.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TnbProgressBar : Panel
    {
        private const int _acuavolouno = 100;
        private Color _jifateakoAsa = Color.FromArgb(0x79, 0x79, 0x79);
        private Image _nakueneQoj = Resources.barImg;
        private Rectangle _opaosoguacibarEbotuwualOtifocue;
        private ButtonBorderStyle _rudiuxiUsiah = ButtonBorderStyle.Solid;
        private int _ubatiwomeujEtujeneu;
        private const int _ujebuaniJeuton = 15;

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
                this._opaosoguacibarEbotuwualOtifocue = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2);
                if (this.Value != 0)
                {
                    this._opaosoguacibarEbotuwualOtifocue.Width = (int) (this._opaosoguacibarEbotuwualOtifocue.Width * (((double) this.Value) / 100.0));
                    FillPattern(graphics, this.BarImage, this._opaosoguacibarEbotuwualOtifocue);
                }
                if (this.Value != 100)
                {
                    graphics.FillRectangle(new SolidBrush(this.BackColor), this._opaosoguacibarEbotuwualOtifocue.Width + this._opaosoguacibarEbotuwualOtifocue.X, this._opaosoguacibarEbotuwualOtifocue.Y, (base.Width - this._opaosoguacibarEbotuwualOtifocue.Width) - 2, this._opaosoguacibarEbotuwualOtifocue.Height);
                }
            }
        }

        [Category("Appearance")]
        public Image BarImage
        {
            get
            {
                return this._nakueneQoj;
            }
            set
            {
                if (value != null)
                {
                    this._nakueneQoj = value;
                }
                base.Invalidate();
            }
        }

        [Category("Appearance")]
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

        [Category("Appearance")]
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

        public System.Windows.Forms.DrawMode DrawMode { get; set; }

        [Category("Appearance")]
        public int Value
        {
            get
            {
                return this._ubatiwomeujEtujeneu;
            }
            set
            {
                if (value != this._ubatiwomeujEtujeneu)
                {
                    if (value < 0)
                    {
                        value = 0;
                    }
                    if (value > 100)
                    {
                        value = 100;
                    }
                    this._ubatiwomeujEtujeneu = value;
                    base.Invalidate(this._opaosoguacibarEbotuwualOtifocue);
                }
            }
        }
    }
}

