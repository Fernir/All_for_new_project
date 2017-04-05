namespace nManager.Helpful.Forms.UserControls
{
    using nManager.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public sealed class TnbSwitchButton : Panel
    {
        private string _etato = "OFF";
        private bool _guakocolOgoipero;
        private readonly Label _leftSide = new Label();
        private string _meiqel = "ON";
        private readonly PictureBox _rightSide = new PictureBox();

        public event EventHandler ValueChanged;

        public TnbSwitchButton()
        {
            base.SuspendLayout();
            this.Font = new Font(this.Font, FontStyle.Bold);
            this.MaximumSize = new Size(60, 20);
            this.MinimumSize = new Size(60, 20);
            base.Size = new Size(60, 20);
            base.Controls.Add(this._leftSide);
            base.Controls.Add(this._rightSide);
            this._leftSide.AutoSize = false;
            this._rightSide.AutoSize = false;
            this._leftSide.MinimumSize = new Size(0x2f, 20);
            this._rightSide.MinimumSize = new Size(13, 20);
            this._leftSide.MaximumSize = new Size(0x2f, 20);
            this._rightSide.MaximumSize = new Size(13, 20);
            this._leftSide.Size = new Size(0x2f, 20);
            this._rightSide.Size = new Size(13, 20);
            this._leftSide.Location = new Point(0, 0);
            this._rightSide.Location = new Point(0x2f, 0);
            this._leftSide.TextAlign = ContentAlignment.MiddleCenter;
            this._leftSide.ForeColor = Color.Snow;
            this._leftSide.Font = this.Font;
            this._leftSide.MouseClick += new MouseEventHandler(this.Ukerijugatum);
            this._rightSide.MouseClick += new MouseEventHandler(this.Ukerijugatum);
            if (this.Value)
            {
                this._leftSide.Text = this.OnText;
                this._leftSide.Image = Resources.switchonl;
                this._rightSide.Image = Resources.switchonr;
            }
            else
            {
                this._leftSide.Text = this.OffText;
                this._leftSide.Image = Resources.switchoffl;
                this._rightSide.Image = Resources.switchoffr;
            }
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LabinidimeonEgi(bool xeoxuIpecDapanik)
        {
            if (xeoxuIpecDapanik)
            {
                this._leftSide.Text = this.OnText;
                this._leftSide.Image = Resources.switchonl;
                this._rightSide.Image = Resources.switchonr;
            }
            else
            {
                this._leftSide.Text = this.OffText;
                this._leftSide.Image = Resources.switchoffl;
                this._rightSide.Image = Resources.switchoffr;
            }
        }

        private void Ukerijugatum(object moleileucucisUgofe, MouseEventArgs deisiko)
        {
            this.Value = !this.Value;
            this.LabinidimeonEgi(this.Value);
        }

        [Category("Appearance")]
        public string OffText
        {
            get
            {
                return this._etato;
            }
            set
            {
                this._etato = value;
                if (!this.Value)
                {
                    this._leftSide.Text = value;
                }
                base.Invalidate();
            }
        }

        [Category("Appearance")]
        public string OnText
        {
            get
            {
                return this._meiqel;
            }
            set
            {
                this._meiqel = value;
                if (this.Value)
                {
                    this._leftSide.Text = value;
                }
                base.Invalidate();
            }
        }

        [Category("Appearance")]
        public bool Value
        {
            get
            {
                return this._guakocolOgoipero;
            }
            set
            {
                this._guakocolOgoipero = value;
                this.LabinidimeonEgi(value);
                if (this._ebaikuoquEja != null)
                {
                    this._ebaikuoquEja(this, EventArgs.Empty);
                }
                base.Invalidate();
            }
        }
    }
}

