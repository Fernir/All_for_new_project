namespace nManager.Helpful.Forms.UserControls
{
    using nManager;
    using nManager.Helpful;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class LoggingSchedulerUC : UserControl
    {
        private RichTextBox _fuepoamuawatifXo;
        private IContainer _koiduferaluOwiakio;
        private readonly List<Logging.Log> _listLog = new List<Logging.Log>();
        private TnbSwitchButton _niakaihaono;
        private Label _qegewaulabamea;
        private TnbSwitchButton _ridog;
        private Label _uroqaecoaroviqQoav;
        private System.Windows.Forms.Timer _uxuhueqiajuoweRedonah;

        public LoggingSchedulerUC()
        {
            try
            {
                this.Utaeriopasa();
                this.GeuxecAtajioxoa();
                Logging.OnChanged += new Logging.LoggingChangeEventHandler(this.Tubuloe);
                this.FeleubeomeaVube(null, null);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoggingSchedulerUC(): " + exception, true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this._koiduferaluOwiakio != null))
            {
                this._koiduferaluOwiakio.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Ejomum(object moleileucucisUgofe, EventArgs deisiko)
        {
            this.Muajeuti();
        }

        private void FeleubeomeaVube(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                lock (this)
                {
                    this._listLog.Clear();
                    this._listLog.AddRange(Logging.ReadList(this.Fuojievumiaw(), false));
                    this._fuepoamuawatifXo.Clear();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("CbCheckedChanged(object sender, EventArgs e): " + exception, true);
            }
        }

        private Logging.LogType Fuojievumiaw()
        {
            try
            {
                Logging.LogType none = Logging.LogType.None;
                if (this._niakaihaono.Value)
                {
                    none |= Logging.LogType.S;
                }
                if (this._ridog.Value)
                {
                    none |= Logging.LogType.D;
                    none |= Logging.LogType.E;
                }
                return none;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetFlag(): " + exception, true);
            }
            return Logging.LogType.None;
        }

        private void GeuxecAtajioxoa()
        {
            this._uroqaecoaroviqQoav.Text = Translate.Get(Translate.Id.Normal);
            this._qegewaulabamea.Text = Translate.Get(Translate.Id.Debug);
        }

        private void Iqafamikeani(object moleileucucisUgofe, ControlEventArgs deisiko)
        {
            this._uxuhueqiajuoweRedonah.Enabled = false;
            Logging.OnChanged -= new Logging.LoggingChangeEventHandler(this.Tubuloe);
        }

        private void Muajeuti()
        {
            try
            {
                lock (this)
                {
                    if (this._listLog.Count > 0)
                    {
                        this._fuepoamuawatifXo.AppendText(this._listLog[0].ToString());
                        int start = this._fuepoamuawatifXo.Text.Length - this._listLog[0].ToString().Length;
                        if (start < 0)
                        {
                            start = 0;
                        }
                        this._fuepoamuawatifXo.Select(start, this._listLog[0].ToString().Length);
                        this._fuepoamuawatifXo.SelectionColor = this._listLog[0].Color;
                        this._fuepoamuawatifXo.AppendText(Environment.NewLine);
                        this._listLog.RemoveAt(0);
                        this._fuepoamuawatifXo.ScrollToCaret();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("AddLog(): " + exception, true);
            }
        }

        private void PuvureacRewaxu(object moleileucucisUgofe, EventArgs deisiko)
        {
            lock (this)
            {
                this._listLog.Clear();
                this._listLog.AddRange(Logging.ReadList(this.Fuojievumiaw(), true));
                this._fuepoamuawatifXo.Clear();
            }
        }

        private void Tubuloe(object moleileucucisUgofe, Logging.LoggingChangeEventArgs deisiko)
        {
            try
            {
                lock (this)
                {
                    if ((deisiko.Log.LogType & this.Fuojievumiaw()) == deisiko.Log.LogType)
                    {
                        this._listLog.Add(deisiko.Log);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("SynchroniseLoggin(object sender, Logging.LoggingChangeEventArgs e): " + exception, true);
            }
        }

        private void Utaeriopasa()
        {
            this._koiduferaluOwiakio = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(LoggingSchedulerUC));
            this._qegewaulabamea = new Label();
            this._uroqaecoaroviqQoav = new Label();
            this._fuepoamuawatifXo = new RichTextBox();
            this._uxuhueqiajuoweRedonah = new System.Windows.Forms.Timer(this._koiduferaluOwiakio);
            this._niakaihaono = new TnbSwitchButton();
            this._ridog = new TnbSwitchButton();
            base.SuspendLayout();
            this._qegewaulabamea.AutoEllipsis = true;
            this._qegewaulabamea.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._qegewaulabamea.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this._qegewaulabamea.Location = new Point(720, 0x2d);
            this._qegewaulabamea.Name = "DebugLogSwitchLabel";
            this._qegewaulabamea.Size = new Size(70, 20);
            this._qegewaulabamea.TabIndex = 12;
            this._qegewaulabamea.Text = "Debug";
            this._qegewaulabamea.TextAlign = ContentAlignment.MiddleLeft;
            this._uroqaecoaroviqQoav.AutoEllipsis = true;
            this._uroqaecoaroviqQoav.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._uroqaecoaroviqQoav.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._uroqaecoaroviqQoav.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this._uroqaecoaroviqQoav.Location = new Point(720, 13);
            this._uroqaecoaroviqQoav.Name = "NormalLogSwitchLabel";
            this._uroqaecoaroviqQoav.Size = new Size(70, 20);
            this._uroqaecoaroviqQoav.TabIndex = 9;
            this._uroqaecoaroviqQoav.Text = "Normal";
            this._uroqaecoaroviqQoav.TextAlign = ContentAlignment.MiddleLeft;
            this._fuepoamuawatifXo.BackColor = Color.FromArgb(0xee, 0xee, 0xee);
            this._fuepoamuawatifXo.BorderStyle = BorderStyle.None;
            this._fuepoamuawatifXo.Font = new Font("Microsoft Sans Serif", 9.42f, FontStyle.Bold, GraphicsUnit.Pixel);
            this._fuepoamuawatifXo.Location = new Point(14, 14);
            this._fuepoamuawatifXo.Margin = new Padding(0);
            this._fuepoamuawatifXo.MaximumSize = new Size(0x271, 0xb0);
            this._fuepoamuawatifXo.MinimumSize = new Size(0x271, 0xb0);
            this._fuepoamuawatifXo.Name = "LoggingTextArea";
            this._fuepoamuawatifXo.ReadOnly = true;
            this._fuepoamuawatifXo.RightMargin = 0x246;
            this._fuepoamuawatifXo.ScrollBars = RichTextBoxScrollBars.Vertical;
            this._fuepoamuawatifXo.Size = new Size(0x271, 0xb0);
            this._fuepoamuawatifXo.TabIndex = 0;
            this._fuepoamuawatifXo.Text = "";
            this._fuepoamuawatifXo.VisibleChanged += new EventHandler(this.WiriximuqeuOvomo);
            this._uxuhueqiajuoweRedonah.Enabled = true;
            this._uxuhueqiajuoweRedonah.Tick += new EventHandler(this.Ejomum);
            this._niakaihaono.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this._niakaihaono.Location = new Point(0x28b, 13);
            this._niakaihaono.MaximumSize = new Size(60, 20);
            this._niakaihaono.MinimumSize = new Size(60, 20);
            this._niakaihaono.Name = "NormalLogSwitchButton";
            this._niakaihaono.OffText = "OFF";
            this._niakaihaono.OnText = "ON";
            this._niakaihaono.Size = new Size(60, 20);
            this._niakaihaono.TabIndex = 13;
            this._niakaihaono.Value = true;
            this._niakaihaono.ValueChanged += new EventHandler(this.PuvureacRewaxu);
            this._ridog.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this._ridog.Location = new Point(0x28b, 0x2d);
            this._ridog.MaximumSize = new Size(60, 20);
            this._ridog.MinimumSize = new Size(60, 20);
            this._ridog.Name = "DebugLogSwitchButton";
            this._ridog.OffText = "OFF";
            this._ridog.OnText = "ON";
            this._ridog.Size = new Size(60, 20);
            this._ridog.TabIndex = 0x10;
            this._ridog.Value = false;
            this._ridog.ValueChanged += new EventHandler(this.PuvureacRewaxu);
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.Controls.Add(this._niakaihaono);
            base.Controls.Add(this._uroqaecoaroviqQoav);
            base.Controls.Add(this._ridog);
            base.Controls.Add(this._qegewaulabamea);
            base.Controls.Add(this._fuepoamuawatifXo);
            base.Margin = new Padding(0);
            this.MaximumSize = new Size(800, 0xca);
            this.MinimumSize = new Size(800, 0xca);
            base.Name = "LoggingSchedulerUC";
            base.Size = new Size(800, 0xca);
            base.ControlRemoved += new ControlEventHandler(this.Iqafamikeani);
            base.ResumeLayout(false);
        }

        private void WiriximuqeuOvomo(object moleileucucisUgofe, EventArgs deisiko)
        {
            if (this._fuepoamuawatifXo.Visible)
            {
                this._fuepoamuawatifXo.SelectionStart = this._fuepoamuawatifXo.TextLength;
                this._fuepoamuawatifXo.ScrollToCaret();
            }
        }
    }
}

