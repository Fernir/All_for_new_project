namespace nManager.Helpful.Forms.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TnbComboBox : ComboBox
    {
        private Image _acevuapuaPilib = Resources.selectorBack_big;
        private Color _faikamiRemuraem = Color.Gainsboro;
        private Color _feuwu = Color.FromArgb(0x53, 0x6a, 0xc2);
        private Color _jifateakoAsa = Color.FromArgb(0x79, 0x79, 0x79);
        private ButtonBorderStyle _rudiuxiUsiah = ButtonBorderStyle.Solid;
        private const int _ujebuaniJeuton = 15;

        public TnbComboBox()
        {
            base.MouseWheel += new MouseEventHandler(this.EnomenoUteig);
            base.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            base.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            base.ForeColor = Color.FromArgb(0x62, 160, 0xe5);
            base.ItemHeight = 20;
            base.DropDownStyle = ComboBoxStyle.DropDownList;
            base.FlatStyle = FlatStyle.Flat;
            base.DrawItem += new DrawItemEventHandler(this.HuohuedapirenuMa);
        }

        private void EnomenoUteig(object moleileucucisUgofe, MouseEventArgs deisiko)
        {
            HandledMouseEventArgs args = (HandledMouseEventArgs) deisiko;
            args.Handled = true;
        }

        private void HuohuedapirenuMa(object moleileucucisUgofe, DrawItemEventArgs deisiko)
        {
            if (deisiko.Index >= 0)
            {
                ComboBox box = moleileucucisUgofe as ComboBox;
                if ((deisiko.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    deisiko.Graphics.FillRectangle(new SolidBrush(this.HighlightColor), deisiko.Bounds);
                }
                else if (box != null)
                {
                    deisiko.Graphics.FillRectangle(new SolidBrush(box.BackColor), deisiko.Bounds);
                }
                if (box != null)
                {
                    deisiko.Graphics.DrawString(box.Items[deisiko.Index].ToString(), deisiko.Font, new SolidBrush(box.ForeColor), (PointF) new Point(deisiko.Bounds.X, deisiko.Bounds.Y));
                }
                deisiko.DrawFocusRectangle();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 15)
            {
                Graphics graphics = Graphics.FromHwnd(base.Handle);
                Rectangle bounds = new Rectangle(0, 0, base.Width - 0x11, base.Height);
                ControlPaint.DrawBorder(graphics, bounds, this.BorderColor, this.BorderStyle);
                bounds = new Rectangle(base.Width - 0x12, 0, 0x12, base.Height);
                ControlPaint.DrawBorder(graphics, bounds, this.SelectorBorderColor, this.BorderStyle);
                if (this.SelectorImage != null)
                {
                    bounds = new Rectangle(base.Width - 0x11, 1, 0x10, base.Height - 2);
                    graphics.DrawImage(this.SelectorImage, bounds);
                }
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
        public Color HighlightColor
        {
            get
            {
                return this._faikamiRemuraem;
            }
            set
            {
                this._faikamiRemuraem = value;
                base.Invalidate();
            }
        }

        [Category("Appearance")]
        public Color SelectorBorderColor
        {
            get
            {
                return this._feuwu;
            }
            set
            {
                this._feuwu = value;
                base.Invalidate();
            }
        }

        [Category("Appearance")]
        public Image SelectorImage
        {
            get
            {
                return this._acevuapuaPilib;
            }
            set
            {
                this._acevuapuaPilib = value;
                base.Invalidate();
            }
        }

        [Category("Appearance")]
        public string Text
        {
            get
            {
                if ((base.Items.Count > 0) && (this.SelectedIndex != -1))
                {
                    return base.Items[this.SelectedIndex].ToString();
                }
                return "";
            }
            set
            {
                if (base.Items.Count > 0)
                {
                    for (int i = 0; i < base.Items.Count; i++)
                    {
                        object obj2 = base.Items[i];
                        if (!(obj2.ToString() != value))
                        {
                            this.SelectedIndex = i;
                            return;
                        }
                    }
                }
            }
        }
    }
}

