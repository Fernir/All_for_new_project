namespace nManager.Helpful.Forms
{
    using nManager;
    using nManager.Helpful;
    using nManager.Helpful.Forms.UserControls;
    using nManager.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class TranslationManagementMainFrame : Form
    {
        private readonly nManager.Translate.Language _defaultLangage = XmlSerializer.Deserialize<nManager.Translate.Language>(Application.StartupPath + @"\Data\Lang\English.xml");
        private readonly nManager.Translate.Language _translation = new nManager.Translate.Language();
        private IContainer components;
        private Label LoadButton;
        private TnbControlMenu MainHeader;
        private Label QuitButton;
        private Label SaveButton;
        private DataGridView TranslationTable;

        public TranslationManagementMainFrame()
        {
            try
            {
                this.InitializeComponent();
                this.Translate();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                {
                    base.TopMost = true;
                }
                this.TranslationTable.Columns.Add("Id", "Id");
                this.TranslationTable.Columns[0].ReadOnly = true;
                this.TranslationTable.Columns[0].Width = 0x48;
                this.TranslationTable.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.TranslationTable.Columns.Add("YourTranslation", "Your translated text");
                this.TranslationTable.Columns[1].Width = 300;
                this.TranslationTable.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.TranslationTable.Columns.Add("DefaultText", "Default English text");
                this.TranslationTable.Columns[2].ReadOnly = true;
                this.TranslationTable.Columns[2].Width = 300;
                this.TranslationTable.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                foreach (nManager.Translate.Translation translation in this._defaultLangage.Translations)
                {
                    this.TranslationTable.Rows.Add(new object[] { translation.Id.ToString(), "", translation.Text });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Translate_Tools > Translate_Tools(): " + exception, true);
            }
        }

        private void CheckIdListAndReOrganize(ref nManager.Translate.Language lang)
        {
            for (int i = 0; i <= (this._defaultLangage.Translations.Count - 1); i++)
            {
                if ((lang.Translations.Count - 1) > i)
                {
                    if (this._defaultLangage.Translations[i].Id == lang.Translations[i].Id)
                    {
                        continue;
                    }
                    bool flag = false;
                    int index = 0;
                    string text = "";
                    for (int j = 0; j < (lang.Translations.Count - 1); j++)
                    {
                        if (lang.Translations[j].Id == this._defaultLangage.Translations[i].Id)
                        {
                            flag = true;
                            index = j;
                            text = lang.Translations[j].Text;
                            break;
                        }
                    }
                    if (flag)
                    {
                        lang.Translations.RemoveAt(index);
                        if (string.IsNullOrEmpty(text))
                        {
                            text = this._defaultLangage.Translations[i].Text;
                        }
                        nManager.Translate.Translation item = new nManager.Translate.Translation {
                            Id = this._defaultLangage.Translations[i].Id,
                            Text = text
                        };
                        lang.Translations.Insert(i, item);
                    }
                    else
                    {
                        lang.Translations.Insert(i, this._defaultLangage.Translations[i]);
                    }
                    continue;
                }
                if (((lang.Translations.Count - 1) < i) && (i != this._defaultLangage.Translations.Count))
                {
                    lang.Translations.Insert(i, this._defaultLangage.Translations[i]);
                }
            }
            while (this._defaultLangage.Translations.Count < lang.Translations.Count)
            {
                lang.Translations.RemoveAt(lang.Translations.Count - 1);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(TranslationManagementMainFrame));
            this.LoadButton = new Label();
            this.SaveButton = new Label();
            this.QuitButton = new Label();
            this.TranslationTable = new DataGridView();
            this.MainHeader = new TnbControlMenu();
            ((ISupportInitialize) this.TranslationTable).BeginInit();
            base.SuspendLayout();
            this.LoadButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.LoadButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.LoadButton.ForeColor = Color.White;
            this.LoadButton.Image = Resources.blueB_260;
            this.LoadButton.Location = new Point(0x1fb, 0x36);
            this.LoadButton.Margin = new Padding(0);
            this.LoadButton.MaximumSize = new Size(260, 0x1d);
            this.LoadButton.MinimumSize = new Size(260, 0x1d);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new Size(260, 0x1d);
            this.LoadButton.TabIndex = 13;
            this.LoadButton.Text = "LOAD A TRANSLATION FILE";
            this.LoadButton.TextAlign = ContentAlignment.MiddleCenter;
            this.LoadButton.Click += new EventHandler(this.LoadButton_Click);
            this.LoadButton.MouseEnter += new EventHandler(this.LoadButton_MouseEnter);
            this.LoadButton.MouseLeave += new EventHandler(this.LoadButton_MouseLeave);
            this.SaveButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.SaveButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.SaveButton.ForeColor = Color.White;
            this.SaveButton.Image = Resources.blueB_260;
            this.SaveButton.Location = new Point(0x1fb, 0x22f);
            this.SaveButton.Margin = new Padding(0);
            this.SaveButton.MaximumSize = new Size(260, 0x1d);
            this.SaveButton.MinimumSize = new Size(260, 0x1d);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new Size(260, 0x1d);
            this.SaveButton.TabIndex = 14;
            this.SaveButton.Text = "SAVE AND CLOSE";
            this.SaveButton.TextAlign = ContentAlignment.MiddleCenter;
            this.SaveButton.Click += new EventHandler(this.SaveButton_Click);
            this.SaveButton.MouseEnter += new EventHandler(this.SaveButton_MouseEnter);
            this.SaveButton.MouseLeave += new EventHandler(this.SaveButton_MouseLeave);
            this.QuitButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.QuitButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.QuitButton.ForeColor = Color.White;
            this.QuitButton.Image = Resources.blackB_260;
            this.QuitButton.Location = new Point(0xed, 0x22f);
            this.QuitButton.Margin = new Padding(0);
            this.QuitButton.MaximumSize = new Size(260, 0x1d);
            this.QuitButton.MinimumSize = new Size(260, 0x1d);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new Size(260, 0x1d);
            this.QuitButton.TabIndex = 15;
            this.QuitButton.Text = "CLOSE WITHOUT SAVING";
            this.QuitButton.TextAlign = ContentAlignment.MiddleCenter;
            this.QuitButton.Click += new EventHandler(this.CloseButton_Click);
            this.QuitButton.MouseEnter += new EventHandler(this.QuitButton_MouseEnter);
            this.QuitButton.MouseLeave += new EventHandler(this.QuitButton_MouseLeave);
            style.BackColor = Color.FromArgb(250, 250, 250);
            style.ForeColor = Color.FromArgb(0x76, 0x76, 0x76);
            style.SelectionBackColor = Color.FromArgb(0x93, 0xb5, 0x16);
            style.SelectionForeColor = Color.FromArgb(250, 250, 250);
            this.TranslationTable.AlternatingRowsDefaultCellStyle = style;
            this.TranslationTable.BackgroundColor = Color.FromArgb(250, 250, 250);
            this.TranslationTable.BorderStyle = BorderStyle.None;
            this.TranslationTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TranslationTable.GridColor = Color.FromArgb(0x76, 0x76, 0x76);
            this.TranslationTable.Location = new Point(0x23, 0x5f);
            this.TranslationTable.Name = "TranslationTable";
            this.TranslationTable.Size = new Size(730, 0x1c4);
            this.TranslationTable.TabIndex = 0x15;
            this.MainHeader.BackgroundImage = Resources._800x43_controlbar;
            this.MainHeader.Location = new Point(0, 0);
            this.MainHeader.LogoImage = (Image) manager.GetObject("MainHeader.LogoImage");
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new Size(800, 0x2b);
            this.MainHeader.TabIndex = 0x16;
            this.MainHeader.TitleFont = new Font("Microsoft Sans Serif", 12f);
            this.MainHeader.TitleForeColor = Color.FromArgb(0xde, 0xde, 0xde);
            this.MainHeader.TitleText = "Translate Manager - TheNoobBot - DevVersionRestrict";
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = SystemColors.ActiveCaptionText;
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.ClientSize = new Size(800, 600);
            base.Controls.Add(this.MainHeader);
            base.Controls.Add(this.TranslationTable);
            base.Controls.Add(this.QuitButton);
            base.Controls.Add(this.SaveButton);
            base.Controls.Add(this.LoadButton);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            this.MaximumSize = new Size(800, 600);
            this.MinimumSize = new Size(800, 600);
            base.Name = "TranslationManagementMainFrame";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Translate Tools";
            ((ISupportInitialize) this.TranslationTable).EndInit();
            base.ResumeLayout(false);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            this.LoadGrid("");
        }

        private void LoadButton_MouseEnter(object sender, EventArgs e)
        {
            this.LoadButton.Image = Resources.greenB_260;
        }

        private void LoadButton_MouseLeave(object sender, EventArgs e)
        {
            this.LoadButton.Image = Resources.blueB_260;
        }

        private void LoadGrid(string filePath = "")
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = Others.DialogBoxOpenFile(Application.StartupPath + @"\Data\Lang\", "Langage files (*.xml)|*.xml");
                }
                if (File.Exists(filePath))
                {
                    nManager.Translate.Language lang = XmlSerializer.Deserialize<nManager.Translate.Language>(filePath);
                    this.CheckIdListAndReOrganize(ref lang);
                    for (int i = 0; i <= (this._defaultLangage.Translations.Count - 1); i++)
                    {
                        if ((string.IsNullOrEmpty(lang.Translations[i].Text) || (lang.Translations[i].Text == lang.Translations[i].Id.ToString())) || lang.Translations[i].Text.Contains("_"))
                        {
                            lang.Translations[i].Text = this._defaultLangage.Translations[i].Text;
                        }
                    }
                    this.TranslationTable.Rows.Clear();
                    for (int j = 0; j < this._defaultLangage.Translations.Count; j++)
                    {
                        nManager.Translate.Translation translation = this._defaultLangage.Translations[j];
                        this.TranslationTable.Rows.Add(new object[] { translation.Id.ToString(), lang.Translations[j].Text, translation.Text });
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Translate_Tools > LoadGrid(string filePath): " + exception, true);
            }
        }

        private void QuitButton_MouseEnter(object sender, EventArgs e)
        {
            this.QuitButton.Image = Resources.greenB_260;
        }

        private void QuitButton_MouseLeave(object sender, EventArgs e)
        {
            this.QuitButton.Image = Resources.blackB_260;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.SaveGrid();
            base.Close();
        }

        private void SaveButton_MouseEnter(object sender, EventArgs e)
        {
            this.SaveButton.Image = Resources.greenB_260;
        }

        private void SaveButton_MouseLeave(object sender, EventArgs e)
        {
            this.SaveButton.Image = Resources.blueB_260;
        }

        private void SaveGrid()
        {
            try
            {
                string str = Others.DialogBoxSaveFile(Application.StartupPath + @"\Data\Lang\", "Langage files (*.xml)|*.xml");
                if (!string.IsNullOrWhiteSpace(str))
                {
                    this._translation.Translations.Clear();
                    for (int i = 0; i < (this.TranslationTable.Rows.Count - 1); i++)
                    {
                        DataGridViewRow row = this.TranslationTable.Rows[i];
                        foreach (nManager.Translate.Id id in Enum.GetValues(typeof(nManager.Translate.Id)))
                        {
                            if (!(id.ToString() != row.Cells[0].Value.ToString()))
                            {
                                string str2 = (((row.Cells[1].Value == null) || string.IsNullOrEmpty(row.Cells[1].Value.ToString())) || ((row.Cells[1].Value.ToString() == row.Cells[0].Value.ToString()) || row.Cells[1].Value.ToString().Contains("_"))) ? row.Cells[2].Value.ToString() : row.Cells[1].Value.ToString();
                                nManager.Translate.Translation item = new nManager.Translate.Translation {
                                    Id = id,
                                    Text = str2
                                };
                                this._translation.Translations.Add(item);
                                break;
                            }
                        }
                    }
                    XmlSerializer.Serialize(str, this._translation);
                    this.LoadGrid(str);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Translate_Tools > SaveGrid(): " + exception, true);
            }
        }

        private void Translate()
        {
            this.SaveButton.Text = nManager.Translate.Get(nManager.Translate.Id.SaveAndClose).ToUpper();
            this.LoadButton.Text = nManager.Translate.Get(nManager.Translate.Id.LoadTranslationFile).ToUpper();
            this.QuitButton.Text = nManager.Translate.Get(nManager.Translate.Id.CloseWithoutSaving).ToUpper();
            this.MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Translate_Tools) + " - " + Information.MainTitle;
            this.Text = this.MainHeader.TitleText;
        }
    }
}

