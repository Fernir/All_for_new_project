namespace nManager.Helpful.Forms.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TnbComboBox : ComboBox
    {
        private Color _borderColor = Color.FromArgb(0x79, 0x79, 0x79);
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private Color _hightlightColor = Color.Gainsboro;
        private Color _selectorBorderColor = Color.FromArgb(0x53, 0x6a, 0xc2);
        private Image _selectorImage = Resources.selectorBack_big;
        private const int WmPaint = 15;

        public TnbComboBox()
        {
            base.MouseWheel += new MouseEventHandler(this.DisableMouseWheel);
            base.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            base.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            base.ForeColor = Color.FromArgb(0x62, 160, 0xe5);
            base.ItemHeight = 20;
            base.DropDownStyle = ComboBoxStyle.DropDownList;
            base.FlatStyle = FlatStyle.Flat;
            base.DrawItem += new DrawItemEventHandler(this.AdvancedComboBox_DrawItem);
        }

        private void AdvancedComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                ComboBox box = sender as ComboBox;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(this.HighlightColor), e.Bounds);
                }
                else if (box != null)
                {
                    e.Graphics.FillRectangle(new SolidBrush(box.BackColor), e.Bounds);
                }
                if (box != null)
                {
                    e.Graphics.DrawString(box.Items[e.Index].ToString(), e.Font, new SolidBrush(box.ForeColor), (PointF) new Point(e.Bounds.X, e.Bounds.Y));
                }
                e.DrawFocusRectangle();
            }
        }

        private void DisableMouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs args = (HandledMouseEventArgs) e;
            args.Handled = true;
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
        public Color HighlightColor
        {
            get
            {
                return this._hightlightColor;
            }
            set
            {
                this._hightlightColor = value;
                base.Invalidate();
            }
        }

        [Category("Appearance")]
        public Color SelectorBorderColor
        {
            get
            {
                return this._selectorBorderColor;
            }
            set
            {
                this._selectorBorderColor = value;
                base.Invalidate();
            }
        }

        [Category("Appearance")]
        public Image SelectorImage
        {
            get
            {
                return this._selectorImage;
            }
            set
            {
                this._selectorImage = value;
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

