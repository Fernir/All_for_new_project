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
        private readonly List<Logging.Log> _listLog = new List<Logging.Log>();
        private IContainer components;
        private TnbSwitchButton DebugLogSwitchButton;
        private Label DebugLogSwitchLabel;
        private System.Windows.Forms.Timer LoggingAreaTimer;
        private RichTextBox LoggingTextArea;
        private TnbSwitchButton NormalLogSwitchButton;
        private Label NormalLogSwitchLabel;

        public LoggingSchedulerUC()
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
                Logging.WriteError("LoggingSchedulerUC(): " + exception, true);
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
                }
                if (this.DebugLogSwitchButton.Value)
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

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(LoggingSchedulerUC));
            this.DebugLogSwitchLabel = new Label();
            this.NormalLogSwitchLabel = new Label();
            this.LoggingTextArea = new RichTextBox();
            this.LoggingAreaTimer = new System.Windows.Forms.Timer(this.components);
            this.NormalLogSwitchButton = new TnbSwitchButton();
            this.DebugLogSwitchButton = new TnbSwitchButton();
            base.SuspendLayout();
            this.DebugLogSwitchLabel.AutoEllipsis = true;
            this.DebugLogSwitchLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.DebugLogSwitchLabel.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this.DebugLogSwitchLabel.Location = new Point(720, 0x2d);
            this.DebugLogSwitchLabel.Name = "DebugLogSwitchLabel";
            this.DebugLogSwitchLabel.Size = new Size(70, 20);
            this.DebugLogSwitchLabel.TabIndex = 12;
            this.DebugLogSwitchLabel.Text = "Debug";
            this.DebugLogSwitchLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.NormalLogSwitchLabel.AutoEllipsis = true;
            this.NormalLogSwitchLabel.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.NormalLogSwitchLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.NormalLogSwitchLabel.ForeColor = Color.FromArgb(0x41, 0x41, 0x41);
            this.NormalLogSwitchLabel.Location = new Point(720, 13);
            this.NormalLogSwitchLabel.Name = "NormalLogSwitchLabel";
            this.NormalLogSwitchLabel.Size = new Size(70, 20);
            this.NormalLogSwitchLabel.TabIndex = 9;
            this.NormalLogSwitchLabel.Text = "Normal";
            this.NormalLogSwitchLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.LoggingTextArea.BackColor = Color.FromArgb(0xee, 0xee, 0xee);
            this.LoggingTextArea.BorderStyle = BorderStyle.None;
            this.LoggingTextArea.Font = new Font("Microsoft Sans Serif", 9.42f, FontStyle.Bold, GraphicsUnit.Pixel);
            this.LoggingTextArea.Location = new Point(14, 14);
            this.LoggingTextArea.Margin = new Padding(0);
            this.LoggingTextArea.MaximumSize = new Size(0x271, 0xb0);
            this.LoggingTextArea.MinimumSize = new Size(0x271, 0xb0);
            this.LoggingTextArea.Name = "LoggingTextArea";
            this.LoggingTextArea.ReadOnly = true;
            this.LoggingTextArea.RightMargin = 0x246;
            this.LoggingTextArea.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.LoggingTextArea.Size = new Size(0x271, 0xb0);
            this.LoggingTextArea.TabIndex = 0;
            this.LoggingTextArea.Text = "";
            this.LoggingTextArea.VisibleChanged += new EventHandler(this.LoggingTextArea_VisibleChanged);
            this.LoggingAreaTimer.Enabled = true;
            this.LoggingAreaTimer.Tick += new EventHandler(this.LoggingAreaTimer_Tick);
            this.NormalLogSwitchButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this.NormalLogSwitchButton.Location = new Point(0x28b, 13);
            this.NormalLogSwitchButton.MaximumSize = new Size(60, 20);
            this.NormalLogSwitchButton.MinimumSize = new Size(60, 20);
            this.NormalLogSwitchButton.Name = "NormalLogSwitchButton";
            this.NormalLogSwitchButton.OffText = "OFF";
            this.NormalLogSwitchButton.OnText = "ON";
            this.NormalLogSwitchButton.Size = new Size(60, 20);
            this.NormalLogSwitchButton.TabIndex = 13;
            this.NormalLogSwitchButton.Value = true;
            this.NormalLogSwitchButton.ValueChanged += new EventHandler(this.LoggingSwitchs_ValueChanged);
            this.DebugLogSwitchButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            this.DebugLogSwitchButton.Location = new Point(0x28b, 0x2d);
            this.DebugLogSwitchButton.MaximumSize = new Size(60, 20);
            this.DebugLogSwitchButton.MinimumSize = new Size(60, 20);
            this.DebugLogSwitchButton.Name = "DebugLogSwitchButton";
            this.DebugLogSwitchButton.OffText = "OFF";
            this.DebugLogSwitchButton.OnText = "ON";
            this.DebugLogSwitchButton.Size = new Size(60, 20);
            this.DebugLogSwitchButton.TabIndex = 0x10;
            this.DebugLogSwitchButton.Value = false;
            this.DebugLogSwitchButton.ValueChanged += new EventHandler(this.LoggingSwitchs_ValueChanged);
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.Controls.Add(this.NormalLogSwitchButton);
            base.Controls.Add(this.NormalLogSwitchLabel);
            base.Controls.Add(this.DebugLogSwitchButton);
            base.Controls.Add(this.DebugLogSwitchLabel);
            base.Controls.Add(this.LoggingTextArea);
            base.Margin = new Padding(0);
            this.MaximumSize = new Size(800, 0xca);
            this.MinimumSize = new Size(800, 0xca);
            base.Name = "LoggingSchedulerUC";
            base.Size = new Size(800, 0xca);
            base.ControlRemoved += new ControlEventHandler(this.LoggingSchedulerUC_ControlRemoved);
            base.ResumeLayout(false);
        }

        private void LoggingAreaTimer_Tick(object sender, EventArgs e)
        {
            this.AddLog();
        }

        private void LoggingSchedulerUC_ControlRemoved(object sender, ControlEventArgs e)
        {
            this.LoggingAreaTimer.Enabled = false;
            Logging.OnChanged -= new Logging.LoggingChangeEventHandler(this.SynchroniseLogging);
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
            this.DebugLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Debug);
        }
    }
}

