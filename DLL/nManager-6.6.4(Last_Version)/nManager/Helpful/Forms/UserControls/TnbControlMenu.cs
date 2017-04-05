namespace nManager.Helpful.Forms.UserControls
{
    using nManager.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TnbControlMenu : Panel
    {
        private PictureBox _agoibuAmoajil;
        private bool _biwugureareqiOkauqio;
        private PictureBox _buruaquwougLueb;
        private int _dutaobEpe;
        private Image _gaigiabuwisi = Resources.logoImageG;
        private Label _gilaviogag;
        private int _ijewuenupujuoUkiw;
        private PictureBox _joupiqewoasejo;
        private Color _ofeupu = Color.FromArgb(0xde, 0xde, 0xde);
        private Font _roikusudVufos = new Font(new FontFamily("Microsoft Sans Serif"), 12f, GraphicsUnit.Point);

        public TnbControlMenu()
        {
            Label label = new Label {
                Text = "TheNoobBot"
            };
            this._gilaviogag = label;
            base.Size = new Size(0x23f, 0x2b);
            this.Lieweakepo();
        }

        private void Aqainujiexo(object moleileucucisUgofe, MouseEventArgs deisiko)
        {
            this._biwugureareqiOkauqio = true;
            this._dutaobEpe = deisiko.X;
            this._ijewuenupujuoUkiw = deisiko.Y;
        }

        private void EfikeoOfu(object moleileucucisUgofe, EventArgs saoxevi)
        {
            if (Form.ActiveForm != null)
            {
                Form.ActiveForm.WindowState = FormWindowState.Minimized;
            }
        }

        private void Ejoxepouqo(object moleileucucisUgofe, EventArgs saoxevi)
        {
            this._agoibuAmoajil.Image = Resources.reduce_button;
        }

        private void Imeodeafoaj(object moleileucucisUgofe, MouseEventArgs deisiko)
        {
            this._biwugureareqiOkauqio = false;
        }

        private void Kidaifa(object moleileucucisUgofe, EventArgs saoxevi)
        {
            this._joupiqewoasejo.Image = Resources.close_buttonG;
        }

        private void Lieweakepo()
        {
            base.SizeChanged += new EventHandler(this.Ojakuiv);
            PictureBox box = new PictureBox {
                Visible = true,
                Location = new Point(base.Width - 0x34, 13),
                Size = new Size(13, 14),
                Image = Resources.reduce_button
            };
            this._agoibuAmoajil = box;
            this._agoibuAmoajil.Click += new EventHandler(this.EfikeoOfu);
            PictureBox box2 = new PictureBox {
                Visible = true,
                Location = new Point(base.Width - 0x19, 13),
                Size = new Size(13, 14),
                Image = Resources.close_button
            };
            this._joupiqewoasejo = box2;
            this._joupiqewoasejo.Click += new EventHandler(this.OwoaqaedateuSop);
            PictureBox box3 = new PictureBox {
                Visible = true,
                Location = new Point(13, 3),
                Size = new Size(30, 0x21),
                Image = this.LogoImage
            };
            this._buruaquwougLueb = box3;
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
            this._gilaviogag = label;
            base.AutoSize = false;
            base.BackgroundImage = Resources.controlbar;
            base.Controls.Add(this._joupiqewoasejo);
            base.Controls.Add(this._gilaviogag);
            base.Controls.Add(this._buruaquwougLueb);
            base.Controls.Add(this._agoibuAmoajil);
            this._buruaquwougLueb.MouseDown += new MouseEventHandler(this.Aqainujiexo);
            this._buruaquwougLueb.MouseMove += new MouseEventHandler(this.NaxujokupoOwi);
            this._buruaquwougLueb.MouseUp += new MouseEventHandler(this.Imeodeafoaj);
            this._gilaviogag.MouseDown += new MouseEventHandler(this.Aqainujiexo);
            this._gilaviogag.MouseMove += new MouseEventHandler(this.NaxujokupoOwi);
            this._gilaviogag.MouseUp += new MouseEventHandler(this.Imeodeafoaj);
            base.MouseDown += new MouseEventHandler(this.Aqainujiexo);
            base.MouseMove += new MouseEventHandler(this.NaxujokupoOwi);
            base.MouseUp += new MouseEventHandler(this.Imeodeafoaj);
            this._joupiqewoasejo.MouseEnter += new EventHandler(this.Kidaifa);
            this._joupiqewoasejo.MouseLeave += new EventHandler(this.WopeqauxoeqGok);
            this._agoibuAmoajil.MouseEnter += new EventHandler(this.MeidoateuririfSaujaemiq);
            this._agoibuAmoajil.MouseLeave += new EventHandler(this.Ejoxepouqo);
        }

        private void MeidoateuririfSaujaemiq(object moleileucucisUgofe, EventArgs saoxevi)
        {
            this._agoibuAmoajil.Image = Resources.reduce_buttonG;
        }

        private void NaxujokupoOwi(object moleileucucisUgofe, MouseEventArgs deisiko)
        {
            if (this._biwugureareqiOkauqio)
            {
                base.Parent.Location = new Point(base.Parent.Left + (deisiko.X - this._dutaobEpe), base.Parent.Top + (deisiko.Y - this._ijewuenupujuoUkiw));
            }
        }

        private void Ojakuiv(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._joupiqewoasejo.Location = new Point(base.Width - 0x19, 13);
            this._agoibuAmoajil.Location = new Point(base.Width - 0x34, 13);
            this._gilaviogag.Size = new Size(base.Width - 0x73, 0x23);
        }

        private void OwoaqaedateuSop(object moleileucucisUgofe, EventArgs saoxevi)
        {
            if (Form.ActiveForm != null)
            {
                Form.ActiveForm.Close();
            }
        }

        private void WopeqauxoeqGok(object moleileucucisUgofe, EventArgs saoxevi)
        {
            this._joupiqewoasejo.Image = Resources.close_button;
        }

        [Category("AaTnbControls")]
        public Image LogoImage
        {
            get
            {
                return this._gaigiabuwisi;
            }
            set
            {
                this._gaigiabuwisi = value;
                base.Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Font TitleFont
        {
            get
            {
                return this._roikusudVufos;
            }
            set
            {
                this._roikusudVufos = value;
                base.Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Color TitleForeColor
        {
            get
            {
                return this._ofeupu;
            }
            set
            {
                this._ofeupu = value;
                base.Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public string TitleText
        {
            get
            {
                return this._gilaviogag.Text;
            }
            set
            {
                this._gilaviogag.Text = value;
                base.Invalidate();
            }
        }
    }
}

