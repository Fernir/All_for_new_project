namespace nManager.Helpful.Forms.UserControls
{
    using nManager.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class TnbButton : Label
    {
        private Image _bImage;
        private Image _hooverImage = Resources.greenB;
        public bool Hoovering;

        public TnbButton()
        {
            base.AutoSize = false;
            base.Size = new Size(0x6a, 0x1d);
            this.TextAlign = ContentAlignment.MiddleCenter;
            base.Image = Resources.blackB;
            base.AutoEllipsis = true;
            this.ForeColor = Color.Snow;
            this.Font = new Font(this.Font, FontStyle.Bold);
            base.MouseEnter += new EventHandler(this.OnMouseEnter);
            base.MouseLeave += new EventHandler(this.OnMouseLeave);
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            this.Hoovering = true;
            this._bImage = base.Image;
            base.Image = this.HooverImage;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            base.Image = this._bImage;
            this.Hoovering = false;
        }

        public override bool AutoSize
        {
            get
            {
                return false;
            }
        }

        [Category("Appearance")]
        public Image HooverImage
        {
            get
            {
                return this._hooverImage;
            }
            set
            {
                this._hooverImage = value;
                base.Invalidate();
            }
        }
    }
}

