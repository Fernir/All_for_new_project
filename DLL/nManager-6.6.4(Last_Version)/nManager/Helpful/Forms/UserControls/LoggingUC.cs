namespace nManager.Helpful.Forms.UserControls
{
    using nManager;
    using nManager.Helpful;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class LoggingUC : UserControl
    {
        private TnbSwitchButton _caeluem;
        private RichTextBox _fuepoamuawatifXo;
        private IContainer _koiduferaluOwiakio;
        private readonly List<Logging.Log> _listLog = new List<Logging.Log>();
        private TnbSwitchButton _nelakoetifufLe;
        private TnbSwitchButton _niakaihaono;
        private Label _oquivofuravut;
        private Label _qaegihiti;
        private Label _qegewaulabamea;
        private TnbSwitchButton _ridog;
        private TnbSwitchButton _rigoweabepe;
        private Label _ulimoiqiatufVai;
        private Label _uroqaecoaroviqQoav;
        private System.Windows.Forms.Timer _uxuhueqiajuoweRedonah;

        public LoggingUC()
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
                Logging.WriteError("LoggingUC(): " + exception, true);
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

        private void Etimafa(object moleileucucisUgofe, ControlEventArgs deisiko)
        {
            this._uxuhueqiajuoweRedonah.Enabled = false;
            Logging.OnChanged -= new Logging.LoggingChangeEventHandler(this.Tubuloe);
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
                    none |= Logging.LogType.P;
                }
                if (this._ridog.Value)
                {
                    none |= Logging.LogType.D;
                    none |= Logging.LogType.E;
                    none |= Logging.LogType.EP;
                    none |= Logging.LogType.DP;
                }
                if (this._nelakoetifufLe.Value)
                {
                    none |= Logging.LogType.F;
                }
                if (this._caeluem.Value)
                {
                    none |= Logging.LogType.N;
                }
                if (this._rigoweabepe.Value)
                {
                    none |= Logging.LogType.W;
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
            this._qaegihiti.Text = Translate.Get(Translate.Id.Fight);
            this._oquivofuravut.Text = Translate.Get(Translate.Id.Navigator);
            this._qegewaulabamea.Text = Translate.Get(Translate.Id.Debug);
            this._ulimoiqiatufVai.Text = Translate.Get(Translate.Id.Whisper);
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(LoggingUC));
            this._qegewaulabamea = new Label();
            this._oquivofuravut = new Label();
            this._qaegihiti = new Label();
            this._uroqaecoaroviqQoav = new Label();
            this._fuepoamuawatifXo = new RichTextBox();
            this._ridog = new TnbSwitchButton();
            this._caeluem = new TnbSwitchButton();
            this._nelakoetifufLe = new TnbSwitchButton();
            this._niakaihaono = new TnbSwitchButton();
            this._uxuhueqiajuoweRedonah = new System.Windows.Forms.Timer(this._koiduferaluOwiakio);
            this._rigoweabepe = new TnbSwitchButton();
            this._ulimoiqiatufVai = new Label();
            base.SuspendLayout();
            this._qegewaulabamea.AutoEllipsis = true;
            this._qegewaulabamea.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._qegewaulabamea.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this._qegewaulabamea.Location = new Point(0x1ed, 0x6d);
            this._qegewaulabamea.Name = "DebugLogSwitchLabel";
            this._qegewaulabamea.Size = new Size(70, 20);
            this._qegewaulabamea.TabIndex = 12;
            this._qegewaulabamea.Text = "Debug";
            this._qegewaulabamea.TextAlign = ContentAlignment.MiddleLeft;
            this._oquivofuravut.AutoEllipsis = true;
            this._oquivofuravut.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._oquivofuravut.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this._oquivofuravut.Location = new Point(0x1ed, 0x4d);
            this._oquivofuravut.Name = "NavigationLogSwitchLabel";
            this._oquivofuravut.Size = new Size(70, 20);
            this._oquivofuravut.TabIndex = 11;
            this._oquivofuravut.Text = "Navigation";
            this._oquivofuravut.TextAlign = ContentAlignment.MiddleLeft;
            this._qaegihiti.AutoEllipsis = true;
            this._qaegihiti.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._qaegihiti.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this._qaegihiti.Location = new Point(0x1ed, 0x2d);
            this._qaegihiti.Name = "FightLogSwitchLabel";
            this._qaegihiti.Size = new Size(70, 20);
            this._qaegihiti.TabIndex = 10;
            this._qaegihiti.Text = "Fight";
            this._qaegihiti.TextAlign = ContentAlignment.MiddleLeft;
            this._uroqaecoaroviqQoav.AutoEllipsis = true;
            this._uroqaecoaroviqQoav.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._uroqaecoaroviqQoav.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._uroqaecoaroviqQoav.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this._uroqaecoaroviqQoav.Location = new Point(0x1ed, 13);
            this._uroqaecoaroviqQoav.Name = "NormalLogSwitchLabel";
            this._uroqaecoaroviqQoav.Size = new Size(70, 20);
            this._uroqaecoaroviqQoav.TabIndex = 9;
            this._uroqaecoaroviqQoav.Text = "Normal";
            this._uroqaecoaroviqQoav.TextAlign = ContentAlignment.MiddleLeft;
            this._fuepoamuawatifXo.BackColor = Color.FromArgb(0xee, 0xee, 0xee);
            this._fuepoamuawatifXo.BorderStyle = BorderStyle.None;
            this._fuepoamuawatifXo.Font = new Font("Microsoft Sans Serif", 9.42f, FontStyle.Bold, GraphicsUnit.Pixel);
            this._fuepoamuawatifXo.Location = new Point(13, 13);
            this._fuepoamuawatifXo.Margin = new Padding(0);
            this._fuepoamuawatifXo.MaximumSize = new Size(0x18e, 0xb0);
            this._fuepoamuawatifXo.MinimumSize = new Size(0x18e, 0xb0);
            this._fuepoamuawatifXo.Name = "LoggingTextArea";
            this._fuepoamuawatifXo.ReadOnly = true;
            this._fuepoamuawatifXo.RightMargin = 0x163;
            this._fuepoamuawatifXo.ScrollBars = RichTextBoxScrollBars.Vertical;
            this._fuepoamuawatifXo.Size = new Size(0x18e, 0xb0);
            this._fuepoamuawatifXo.TabIndex = 0;
            this._fuepoamuawatifXo.Text = "";
            this._fuepoamuawatifXo.VisibleChanged += new EventHandler(this.WiriximuqeuOvomo);
            this._ridog.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this._ridog.Location = new Point(0x1a8, 0x6d);
            this._ridog.MaximumSize = new Size(60, 20);
            this._ridog.MinimumSize = new Size(60, 20);
            this._ridog.Name = "DebugLogSwitchButton";
            this._ridog.OffText = "OFF";
            this._ridog.OnText = "ON";
            this._ridog.Size = new Size(60, 20);
            this._ridog.TabIndex = 0x10;
            this._ridog.Value = false;
            this._ridog.ValueChanged += new EventHandler(this.PuvureacRewaxu);
            this._caeluem.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this._caeluem.Location = new Point(0x1a8, 0x4d);
            this._caeluem.MaximumSize = new Size(60, 20);
            this._caeluem.MinimumSize = new Size(60, 20);
            this._caeluem.Name = "NavigationLogSwitchButton";
            this._caeluem.OffText = "OFF";
            this._caeluem.OnText = "ON";
            this._caeluem.Size = new Size(60, 20);
            this._caeluem.TabIndex = 15;
            this._caeluem.Value = false;
            this._caeluem.ValueChanged += new EventHandler(this.PuvureacRewaxu);
            this._nelakoetifufLe.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this._nelakoetifufLe.Location = new Point(0x1a8, 0x2d);
            this._nelakoetifufLe.MaximumSize = new Size(60, 20);
            this._nelakoetifufLe.MinimumSize = new Size(60, 20);
            this._nelakoetifufLe.Name = "FightLogSwitchButton";
            this._nelakoetifufLe.OffText = "OFF";
            this._nelakoetifufLe.OnText = "ON";
            this._nelakoetifufLe.Size = new Size(60, 20);
            this._nelakoetifufLe.TabIndex = 14;
            this._nelakoetifufLe.Value = true;
            this._nelakoetifufLe.ValueChanged += new EventHandler(this.PuvureacRewaxu);
            this._niakaihaono.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this._niakaihaono.Location = new Point(0x1a8, 13);
            this._niakaihaono.MaximumSize = new Size(60, 20);
            this._niakaihaono.MinimumSize = new Size(60, 20);
            this._niakaihaono.Name = "NormalLogSwitchButton";
            this._niakaihaono.OffText = "OFF";
            this._niakaihaono.OnText = "ON";
            this._niakaihaono.Size = new Size(60, 20);
            this._niakaihaono.TabIndex = 13;
            this._niakaihaono.Value = true;
            this._niakaihaono.ValueChanged += new EventHandler(this.PuvureacRewaxu);
            this._uxuhueqiajuoweRedonah.Enabled = true;
            this._uxuhueqiajuoweRedonah.Tick += new EventHandler(this.Ejomum);
            this._rigoweabepe.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this._rigoweabepe.Location = new Point(0x1a8, 0x8d);
            this._rigoweabepe.MaximumSize = new Size(60, 20);
            this._rigoweabepe.MinimumSize = new Size(60, 20);
            this._rigoweabepe.Name = "WhispersLogSwitchButton";
            this._rigoweabepe.OffText = "OFF";
            this._rigoweabepe.OnText = "ON";
            this._rigoweabepe.Size = new Size(60, 20);
            this._rigoweabepe.TabIndex = 0x12;
            this._rigoweabepe.Value = true;
            this._rigoweabepe.ValueChanged += new EventHandler(this.PuvureacRewaxu);
            this._ulimoiqiatufVai.AutoEllipsis = true;
            this._ulimoiqiatufVai.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._ulimoiqiatufVai.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this._ulimoiqiatufVai.Location = new Point(0x1ed, 0x8d);
            this._ulimoiqiatufVai.Name = "WhispersLogSwitchLabel";
            this._ulimoiqiatufVai.Size = new Size(70, 20);
            this._ulimoiqiatufVai.TabIndex = 0x11;
            this._ulimoiqiatufVai.Text = "Whispers";
            this._ulimoiqiatufVai.TextAlign = ContentAlignment.MiddleLeft;
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.Controls.Add(this._rigoweabepe);
            base.Controls.Add(this._ulimoiqiatufVai);
            base.Controls.Add(this._niakaihaono);
            base.Controls.Add(this._uroqaecoaroviqQoav);
            base.Controls.Add(this._ridog);
            base.Controls.Add(this._qegewaulabamea);
            base.Controls.Add(this._caeluem);
            base.Controls.Add(this._oquivofuravut);
            base.Controls.Add(this._nelakoetifufLe);
            base.Controls.Add(this._qaegihiti);
            base.Controls.Add(this._fuepoamuawatifXo);
            base.Margin = new Padding(0);
            this.MaximumSize = new Size(0x23d, 0xcb);
            this.MinimumSize = new Size(0x23d, 0xcb);
            base.Name = "LoggingUC";
            base.Size = new Size(0x23d, 0xcb);
            base.ControlRemoved += new ControlEventHandler(this.Etimafa);
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

