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
        private readonly List<Logging.Log> _listLog = new List<Logging.Log>();
        private IContainer components;
        private TnbSwitchButton DebugLogSwitchButton;
        private Label DebugLogSwitchLabel;
        private TnbSwitchButton FightLogSwitchButton;
        private Label FightLogSwitchLabel;
        private System.Windows.Forms.Timer LoggingAreaTimer;
        private RichTextBox LoggingTextArea;
        private TnbSwitchButton NavigationLogSwitchButton;
        private Label NavigationLogSwitchLabel;
        private TnbSwitchButton NormalLogSwitchButton;
        private Label NormalLogSwitchLabel;
        private TnbSwitchButton WhispersLogSwitchButton;
        private Label WhispersLogSwitchLabel;

        public LoggingUC()
        {
            try
            {
                this.InitializeComponent();
                this.Translate();
                Logging.OnChanged += new Logging.LoggingChangeEventHandler(this.SynchroniseLogging);
                this.CbCheckedChanged(null, null);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoggingUC(): " + exception, true);
            }
        }

        private void AddLog()
        {
            try
            {
                lock (this)
                {
                    if (this._listLog.Count > 0)
                    {
                        this.LoggingTextArea.AppendText(this._listLog[0].ToString());
                        int start = this.LoggingTextArea.Text.Length - this._listLog[0].ToString().Length;
                        if (start < 0)
                        {
                            start = 0;
                        }
                        this.LoggingTextArea.Select(start, this._listLog[0].ToString().Length);
                        this.LoggingTextArea.SelectionColor = this._listLog[0].Color;
                        this.LoggingTextArea.AppendText(Environment.NewLine);
                        this._listLog.RemoveAt(0);
                        this.LoggingTextArea.ScrollToCaret();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("AddLog(): " + exception, true);
            }
        }

        private void CbCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lock (this)
                {
                    this._listLog.Clear();
                    this._listLog.AddRange(Logging.ReadList(this.GetFlag(), false));
                    this.LoggingTextArea.Clear();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("CbCheckedChanged(object sender, EventArgs e): " + exception, true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Logging.LogType GetFlag()
        {
            try
            {
                Logging.LogType none = Logging.LogType.None;
                if (this.NormalLogSwitchButton.Value)
                {
                    none |= Logging.LogType.S;
                    none |= Logging.LogType.P;
                }
                if (this.DebugLogSwitchButton.Value)
                {
                    none |= Logging.LogType.D;
                    none |= Logging.LogType.E;
                    none |= Logging.LogType.EP;
                    none |= Logging.LogType.DP;
                }
                if (this.FightLogSwitchButton.Value)
                {
                    none |= Logging.LogType.F;
                }
                if (this.NavigationLogSwitchButton.Value)
                {
                    none |= Logging.LogType.N;
                }
                if (this.WhispersLogSwitchButton.Value)
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

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(LoggingUC));
            this.DebugLogSwitchLabel = new Label();
            this.NavigationLogSwitchLabel = new Label();
            this.FightLogSwitchLabel = new Label();
            this.NormalLogSwitchLabel = new Label();
            this.LoggingTextArea = new RichTextBox();
            this.DebugLogSwitchButton = new TnbSwitchButton();
            this.NavigationLogSwitchButton = new TnbSwitchButton();
            this.FightLogSwitchButton = new TnbSwitchButton();
            this.NormalLogSwitchButton = new TnbSwitchButton();
            this.LoggingAreaTimer = new System.Windows.Forms.Timer(this.components);
            this.WhispersLogSwitchButton = new TnbSwitchButton();
            this.WhispersLogSwitchLabel = new Label();
            base.SuspendLayout();
            this.DebugLogSwitchLabel.AutoEllipsis = true;
            this.DebugLogSwitchLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.DebugLogSwitchLabel.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this.DebugLogSwitchLabel.Location = new Point(0x1ed, 0x6d);
            this.DebugLogSwitchLabel.Name = "DebugLogSwitchLabel";
            this.DebugLogSwitchLabel.Size = new Size(70, 20);
            this.DebugLogSwitchLabel.TabIndex = 12;
            this.DebugLogSwitchLabel.Text = "Debug";
            this.DebugLogSwitchLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.NavigationLogSwitchLabel.AutoEllipsis = true;
            this.NavigationLogSwitchLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.NavigationLogSwitchLabel.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this.NavigationLogSwitchLabel.Location = new Point(0x1ed, 0x4d);
            this.NavigationLogSwitchLabel.Name = "NavigationLogSwitchLabel";
            this.NavigationLogSwitchLabel.Size = new Size(70, 20);
            this.NavigationLogSwitchLabel.TabIndex = 11;
            this.NavigationLogSwitchLabel.Text = "Navigation";
            this.NavigationLogSwitchLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.FightLogSwitchLabel.AutoEllipsis = true;
            this.FightLogSwitchLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.FightLogSwitchLabel.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this.FightLogSwitchLabel.Location = new Point(0x1ed, 0x2d);
            this.FightLogSwitchLabel.Name = "FightLogSwitchLabel";
            this.FightLogSwitchLabel.Size = new Size(70, 20);
            this.FightLogSwitchLabel.TabIndex = 10;
            this.FightLogSwitchLabel.Text = "Fight";
            this.FightLogSwitchLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.NormalLogSwitchLabel.AutoEllipsis = true;
            this.NormalLogSwitchLabel.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.NormalLogSwitchLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.NormalLogSwitchLabel.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this.NormalLogSwitchLabel.Location = new Point(0x1ed, 13);
            this.NormalLogSwitchLabel.Name = "NormalLogSwitchLabel";
            this.NormalLogSwitchLabel.Size = new Size(70, 20);
            this.NormalLogSwitchLabel.TabIndex = 9;
            this.NormalLogSwitchLabel.Text = "Normal";
            this.NormalLogSwitchLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.LoggingTextArea.BackColor = Color.FromArgb(0xee, 0xee, 0xee);
            this.LoggingTextArea.BorderStyle = BorderStyle.None;
            this.LoggingTextArea.Font = new Font("Microsoft Sans Serif", 9.42f, FontStyle.Bold, GraphicsUnit.Pixel);
            this.LoggingTextArea.Location = new Point(13, 13);
            this.LoggingTextArea.Margin = new Padding(0);
            this.LoggingTextArea.MaximumSize = new Size(0x18e, 0xb0);
            this.LoggingTextArea.MinimumSize = new Size(0x18e, 0xb0);
            this.LoggingTextArea.Name = "LoggingTextArea";
            this.LoggingTextArea.ReadOnly = true;
            this.LoggingTextArea.RightMargin = 0x163;
            this.LoggingTextArea.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.LoggingTextArea.Size = new Size(0x18e, 0xb0);
            this.LoggingTextArea.TabIndex = 0;
            this.LoggingTextArea.Text = "";
            this.LoggingTextArea.VisibleChanged += new EventHandler(this.LoggingTextArea_VisibleChanged);
            this.DebugLogSwitchButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this.DebugLogSwitchButton.Location = new Point(0x1a8, 0x6d);
            this.DebugLogSwitchButton.MaximumSize = new Size(60, 20);
            this.DebugLogSwitchButton.MinimumSize = new Size(60, 20);
            this.DebugLogSwitchButton.Name = "DebugLogSwitchButton";
            this.DebugLogSwitchButton.OffText = "OFF";
            this.DebugLogSwitchButton.OnText = "ON";
            this.DebugLogSwitchButton.Size = new Size(60, 20);
            this.DebugLogSwitchButton.TabIndex = 0x10;
            this.DebugLogSwitchButton.Value = false;
            this.DebugLogSwitchButton.ValueChanged += new EventHandler(this.LoggingSwitchs_ValueChanged);
            this.NavigationLogSwitchButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this.NavigationLogSwitchButton.Location = new Point(0x1a8, 0x4d);
            this.NavigationLogSwitchButton.MaximumSize = new Size(60, 20);
            this.NavigationLogSwitchButton.MinimumSize = new Size(60, 20);
            this.NavigationLogSwitchButton.Name = "NavigationLogSwitchButton";
            this.NavigationLogSwitchButton.OffText = "OFF";
            this.NavigationLogSwitchButton.OnText = "ON";
            this.NavigationLogSwitchButton.Size = new Size(60, 20);
            this.NavigationLogSwitchButton.TabIndex = 15;
            this.NavigationLogSwitchButton.Value = false;
            this.NavigationLogSwitchButton.ValueChanged += new EventHandler(this.LoggingSwitchs_ValueChanged);
            this.FightLogSwitchButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this.FightLogSwitchButton.Location = new Point(0x1a8, 0x2d);
            this.FightLogSwitchButton.MaximumSize = new Size(60, 20);
            this.FightLogSwitchButton.MinimumSize = new Size(60, 20);
            this.FightLogSwitchButton.Name = "FightLogSwitchButton";
            this.FightLogSwitchButton.OffText = "OFF";
            this.FightLogSwitchButton.OnText = "ON";
            this.FightLogSwitchButton.Size = new Size(60, 20);
            this.FightLogSwitchButton.TabIndex = 14;
            this.FightLogSwitchButton.Value = true;
            this.FightLogSwitchButton.ValueChanged += new EventHandler(this.LoggingSwitchs_ValueChanged);
            this.NormalLogSwitchButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this.NormalLogSwitchButton.Location = new Point(0x1a8, 13);
            this.NormalLogSwitchButton.MaximumSize = new Size(60, 20);
            this.NormalLogSwitchButton.MinimumSize = new Size(60, 20);
            this.NormalLogSwitchButton.Name = "NormalLogSwitchButton";
            this.NormalLogSwitchButton.OffText = "OFF";
            this.NormalLogSwitchButton.OnText = "ON";
            this.NormalLogSwitchButton.Size = new Size(60, 20);
            this.NormalLogSwitchButton.TabIndex = 13;
            this.NormalLogSwitchButton.Value = true;
            this.NormalLogSwitchButton.ValueChanged += new EventHandler(this.LoggingSwitchs_ValueChanged);
            this.LoggingAreaTimer.Enabled = true;
            this.LoggingAreaTimer.Tick += new EventHandler(this.LoggingAreaTimer_Tick);
            this.WhispersLogSwitchButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this.WhispersLogSwitchButton.Location = new Point(0x1a8, 0x8d);
            this.WhispersLogSwitchButton.MaximumSize = new Size(60, 20);
            this.WhispersLogSwitchButton.MinimumSize = new Size(60, 20);
            this.WhispersLogSwitchButton.Name = "WhispersLogSwitchButton";
            this.WhispersLogSwitchButton.OffText = "OFF";
            this.WhispersLogSwitchButton.OnText = "ON";
            this.WhispersLogSwitchButton.Size = new Size(60, 20);
            this.WhispersLogSwitchButton.TabIndex = 0x12;
            this.WhispersLogSwitchButton.Value = true;
            this.WhispersLogSwitchButton.ValueChanged += new EventHandler(this.LoggingSwitchs_ValueChanged);
            this.WhispersLogSwitchLabel.AutoEllipsis = true;
            this.WhispersLogSwitchLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.WhispersLogSwitchLabel.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this.WhispersLogSwitchLabel.Location = new Point(0x1ed, 0x8d);
            this.WhispersLogSwitchLabel.Name = "WhispersLogSwitchLabel";
            this.WhispersLogSwitchLabel.Size = new Size(70, 20);
            this.WhispersLogSwitchLabel.TabIndex = 0x11;
            this.WhispersLogSwitchLabel.Text = "Whispers";
            this.WhispersLogSwitchLabel.TextAlign = ContentAlignment.MiddleLeft;
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.Controls.Add(this.WhispersLogSwitchButton);
            base.Controls.Add(this.WhispersLogSwitchLabel);
            base.Controls.Add(this.NormalLogSwitchButton);
            base.Controls.Add(this.NormalLogSwitchLabel);
            base.Controls.Add(this.DebugLogSwitchButton);
            base.Controls.Add(this.DebugLogSwitchLabel);
            base.Controls.Add(this.NavigationLogSwitchButton);
            base.Controls.Add(this.NavigationLogSwitchLabel);
            base.Controls.Add(this.FightLogSwitchButton);
            base.Controls.Add(this.FightLogSwitchLabel);
            base.Controls.Add(this.LoggingTextArea);
            base.Margin = new Padding(0);
            this.MaximumSize = new Size(0x23d, 0xcb);
            this.MinimumSize = new Size(0x23d, 0xcb);
            base.Name = "LoggingUC";
            base.Size = new Size(0x23d, 0xcb);
            base.ControlRemoved += new ControlEventHandler(this.LoggingUC_ControlRemoved);
            base.ResumeLayout(false);
        }

        private void LoggingAreaTimer_Tick(object sender, EventArgs e)
        {
            this.AddLog();
        }

        private void LoggingSwitchs_ValueChanged(object sender, EventArgs e)
        {
            lock (this)
            {
                this._listLog.Clear();
                this._listLog.AddRange(Logging.ReadList(this.GetFlag(), true));
                this.LoggingTextArea.Clear();
            }
        }

        private void LoggingTextArea_VisibleChanged(object sender, EventArgs e)
        {
            if (this.LoggingTextArea.Visible)
            {
                this.LoggingTextArea.SelectionStart = this.LoggingTextArea.TextLength;
                this.LoggingTextArea.ScrollToCaret();
            }
        }

        private void LoggingUC_ControlRemoved(object sender, ControlEventArgs e)
        {
            this.LoggingAreaTimer.Enabled = false;
            Logging.OnChanged -= new Logging.LoggingChangeEventHandler(this.SynchroniseLogging);
        }

        private void SynchroniseLogging(object sender, Logging.LoggingChangeEventArgs e)
        {
            try
            {
                lock (this)
                {
                    if ((e.Log.LogType & this.GetFlag()) == e.Log.LogType)
                    {
                        this._listLog.Add(e.Log);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("SynchroniseLoggin(object sender, Logging.LoggingChangeEventArgs e): " + exception, true);
            }
        }

        private void Translate()
        {
            this.NormalLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Normal);
            this.FightLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Fight);
            this.NavigationLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Navigator);
            this.DebugLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Debug);
            this.WhispersLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Whisper);
        }
    }
}

