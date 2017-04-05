namespace nManager.Helpful.Forms.UserControls
{
    using nManager.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class TnbButton : Label
    {
        private Image _jaekodohanoiqDa;
        private Image _juxoxibovipipe = Resources.greenB;
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
            base.MouseEnter += new EventHandler(this.FoqiopGoese);
            base.MouseLeave += new EventHandler(this.Joana);
        }

        private void FoqiopGoese(object moleileucucisUgofe, EventArgs deisiko)
        {
            this.Hoovering = true;
            this._jaekodohanoiqDa = base.Image;
            base.Image = this.HooverImage;
        }

        private void Joana(object moleileucucisUgofe, EventArgs deisiko)
        {
            base.Image = this._jaekodohanoiqDa;
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
                return this._juxoxibovipipe;
            }
            set
            {
                this._juxoxibovipipe = value;
                base.Invalidate();
            }
        }
    }
}

