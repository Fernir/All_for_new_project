namespace nManager.Helpful.Forms
{
    using nManager;
    using nManager.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ErrorPopup : Form
    {
        private bool _biwugureareqiOkauqio;
        private int _dutaobEpe;
        private Label _esenesahiefoc;
        private PictureBox _haocopafoOxe;
        private int _ijewuenupujuoUkiw;
        private IContainer _koiduferaluOwiakio;
        private PictureBox _qeawoifuofoHeoxu;
        private PictureBox _ulacineukovaju;

        public ErrorPopup(string errorMessage)
        {
            this.Utaeriopasa();
            this._esenesahiefoc.Text = errorMessage;
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
            {
                base.TopMost = true;
            }
        }

        private void Aqainujiexo(object moleileucucisUgofe, MouseEventArgs deisiko)
        {
            this._biwugureareqiOkauqio = true;
            this._dutaobEpe = deisiko.X;
            this._ijewuenupujuoUkiw = deisiko.Y;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this._koiduferaluOwiakio != null))
            {
                this._koiduferaluOwiakio.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Imeodeafoaj(object moleileucucisUgofe, MouseEventArgs deisiko)
        {
            this._biwugureareqiOkauqio = false;
        }

        private void Loasifoujiba(object moleileucucisUgofe, EventArgs deisiko)
        {
            base.Close();
        }

        private void Loavou(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._ulacineukovaju.Image = Resources.close_buttonG;
        }

        private void NaxujokupoOwi(object moleileucucisUgofe, MouseEventArgs deisiko)
        {
            if (this._biwugureareqiOkauqio)
            {
                base.Location = new Point(base.Left + (deisiko.X - this._dutaobEpe), base.Top + (deisiko.Y - this._ijewuenupujuoUkiw));
            }
        }

        private void Omiudu(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._ulacineukovaju.Image = Resources.close_button;
        }

        private void Utaeriopasa()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ErrorPopup));
            this._esenesahiefoc = new Label();
            this._haocopafoOxe = new PictureBox();
            this._qeawoifuofoHeoxu = new PictureBox();
            this._ulacineukovaju = new PictureBox();
            ((ISupportInitialize) this._haocopafoOxe).BeginInit();
            ((ISupportInitialize) this._qeawoifuofoHeoxu).BeginInit();
            ((ISupportInitialize) this._ulacineukovaju).BeginInit();
            base.SuspendLayout();
            this._esenesahiefoc.BackColor = Color.White;
            this._esenesahiefoc.Font = new Font("Segoe UI", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._esenesahiefoc.ForeColor = Color.FromArgb(0x34, 0x34, 0x34);
            this._esenesahiefoc.Location = new Point(7, 0x1c);
            this._esenesahiefoc.MaximumSize = new Size(0x132, 0xa5);
            this._esenesahiefoc.MinimumSize = new Size(0x132, 0xa5);
            this._esenesahiefoc.Name = "ErrorDescription";
            this._esenesahiefoc.Size = new Size(0x132, 0xa5);
            this._esenesahiefoc.TabIndex = 3;
            this._esenesahiefoc.Text = "Error description";
            this._esenesahiefoc.TextAlign = ContentAlignment.MiddleLeft;
            this._haocopafoOxe.ErrorImage = null;
            this._haocopafoOxe.Image = (Image) manager.GetObject("Controlbar.Image");
            this._haocopafoOxe.InitialImage = null;
            this._haocopafoOxe.Location = new Point(0, 0);
            this._haocopafoOxe.Name = "Controlbar";
            this._haocopafoOxe.Size = new Size(320, 0x16);
            this._haocopafoOxe.TabIndex = 4;
            this._haocopafoOxe.TabStop = false;
            this._haocopafoOxe.MouseDown += new MouseEventHandler(this.Aqainujiexo);
            this._haocopafoOxe.MouseMove += new MouseEventHandler(this.NaxujokupoOwi);
            this._haocopafoOxe.MouseUp += new MouseEventHandler(this.Imeodeafoaj);
            this._qeawoifuofoHeoxu.BackColor = Color.Transparent;
            this._qeawoifuofoHeoxu.Image = (Image) manager.GetObject("Logo.Image");
            this._qeawoifuofoHeoxu.Location = new Point(3, 3);
            this._qeawoifuofoHeoxu.Name = "Logo";
            this._qeawoifuofoHeoxu.Size = new Size(15, 0x10);
            this._qeawoifuofoHeoxu.TabIndex = 5;
            this._qeawoifuofoHeoxu.TabStop = false;
            this._qeawoifuofoHeoxu.MouseDown += new MouseEventHandler(this.Aqainujiexo);
            this._qeawoifuofoHeoxu.MouseMove += new MouseEventHandler(this.NaxujokupoOwi);
            this._qeawoifuofoHeoxu.MouseUp += new MouseEventHandler(this.Imeodeafoaj);
            this._ulacineukovaju.ErrorImage = null;
            this._ulacineukovaju.Image = Resources.close_button;
            this._ulacineukovaju.InitialImage = null;
            this._ulacineukovaju.Location = new Point(0x12e, 3);
            this._ulacineukovaju.Name = "CloseButton";
            this._ulacineukovaju.Size = new Size(13, 14);
            this._ulacineukovaju.TabIndex = 6;
            this._ulacineukovaju.TabStop = false;
            this._ulacineukovaju.Click += new EventHandler(this.Loasifoujiba);
            this._ulacineukovaju.MouseEnter += new EventHandler(this.Loavou);
            this._ulacineukovaju.MouseLeave += new EventHandler(this.Omiudu);
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.ClientSize = new Size(320, 200);
            base.ControlBox = false;
            base.Controls.Add(this._ulacineukovaju);
            base.Controls.Add(this._qeawoifuofoHeoxu);
            base.Controls.Add(this._haocopafoOxe);
            base.Controls.Add(this._esenesahiefoc);
            this.DoubleBuffered = true;
            this.Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ErrorPopup";
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Error";
            base.TopMost = true;
            ((ISupportInitialize) this._haocopafoOxe).EndInit();
            ((ISupportInitialize) this._qeawoifuofoHeoxu).EndInit();
            ((ISupportInitialize) this._ulacineukovaju).EndInit();
            base.ResumeLayout(false);
        }
    }
}

