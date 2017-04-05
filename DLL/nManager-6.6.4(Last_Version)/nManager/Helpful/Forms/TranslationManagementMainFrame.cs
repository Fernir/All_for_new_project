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
        private readonly Translate.Language _defaultLangage = XmlSerializer.Deserialize<Translate.Language>(Application.StartupPath + @"\Data\Lang\English.xml");
        private Label _egamiuxuinoruiQev;
        private Label _goapaIliaba;
        private IContainer _koiduferaluOwiakio;
        private Label _leixai;
        private DataGridView _rotaojeoliate;
        private readonly Translate.Language _translation = new Translate.Language();
        private TnbControlMenu _xuwoagPo;

        public TranslationManagementMainFrame()
        {
            try
            {
                this.Utaeriopasa();
                this.GeuxecAtajioxoa();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                {
                    base.TopMost = true;
                }
                this._rotaojeoliate.Columns.Add("Id", "Id");
                this._rotaojeoliate.Columns[0].ReadOnly = true;
                this._rotaojeoliate.Columns[0].Width = 0x48;
                this._rotaojeoliate.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this._rotaojeoliate.Columns.Add("YourTranslation", "Your translated text");
                this._rotaojeoliate.Columns[1].Width = 300;
                this._rotaojeoliate.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this._rotaojeoliate.Columns.Add("DefaultText", "Default English text");
                this._rotaojeoliate.Columns[2].ReadOnly = true;
                this._rotaojeoliate.Columns[2].Width = 300;
                this._rotaojeoliate.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                foreach (Translate.Translation translation in this._defaultLangage.Translations)
                {
                    this._rotaojeoliate.Rows.Add(new object[] { translation.Id.ToString(), "", translation.Text });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Translate_Tools > Translate_Tools(): " + exception, true);
            }
        }

        private void BixoexuceaciFugavo(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._egamiuxuinoruiQev.Image = Resources.blueB_260;
        }

        private void CeujavuiqTih()
        {
            try
            {
                string str = Others.DialogBoxSaveFile(Application.StartupPath + @"\Data\Lang\", "Langage files (*.xml)|*.xml");
                if (!string.IsNullOrWhiteSpace(str))
                {
                    this._translation.Translations.Clear();
                    for (int i = 0; i < (this._rotaojeoliate.Rows.Count - 1); i++)
                    {
                        DataGridViewRow row = this._rotaojeoliate.Rows[i];
                        foreach (Translate.Id id in Enum.GetValues(typeof(Translate.Id)))
                        {
                            if (!(id.ToString() != row.Cells[0].Value.ToString()))
                            {
                                string str2 = (((row.Cells[1].Value == null) || string.IsNullOrEmpty(row.Cells[1].Value.ToString())) || ((row.Cells[1].Value.ToString() == row.Cells[0].Value.ToString()) || row.Cells[1].Value.ToString().Contains("_"))) ? row.Cells[2].Value.ToString() : row.Cells[1].Value.ToString();
                                Translate.Translation item = new Translate.Translation {
                                    Id = id,
                                    Text = str2
                                };
                                this._translation.Translations.Add(item);
                                break;
                            }
                        }
                    }
                    XmlSerializer.Serialize(str, this._translation);
                    this.IraeraheolIki(str);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Translate_Tools > SaveGrid(): " + exception, true);
            }
        }

        private void Diabigaipoc(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._leixai.Image = Resources.blueB_260;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this._koiduferaluOwiakio != null))
            {
                this._koiduferaluOwiakio.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EneniavoveabTop(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._leixai.Image = Resources.greenB_260;
        }

        private void GeuxecAtajioxoa()
        {
            this._egamiuxuinoruiQev.Text = Translate.Get(Translate.Id.SaveAndClose).ToUpper();
            this._leixai.Text = Translate.Get(Translate.Id.LoadTranslationFile).ToUpper();
            this._goapaIliaba.Text = Translate.Get(Translate.Id.CloseWithoutSaving).ToUpper();
            this._xuwoagPo.TitleText = Translate.Get(Translate.Id.Translate_Tools) + " - " + Information.MainTitle;
            this.Text = this._xuwoagPo.TitleText;
        }

        private void IraeraheolIki(string egiavotaIjoijiu = "")
        {
            try
            {
                if (string.IsNullOrEmpty(egiavotaIjoijiu))
                {
                    egiavotaIjoijiu = Others.DialogBoxOpenFile(Application.StartupPath + @"\Data\Lang\", "Langage files (*.xml)|*.xml");
                }
                if (File.Exists(egiavotaIjoijiu))
                {
                    Translate.Language laecadoqeohaEsusame = XmlSerializer.Deserialize<Translate.Language>(egiavotaIjoijiu);
                    this.OgiajiloUxi(ref laecadoqeohaEsusame);
                    for (int i = 0; i <= (this._defaultLangage.Translations.Count - 1); i++)
                    {
                        if ((string.IsNullOrEmpty(laecadoqeohaEsusame.Translations[i].Text) || (laecadoqeohaEsusame.Translations[i].Text == laecadoqeohaEsusame.Translations[i].Id.ToString())) || laecadoqeohaEsusame.Translations[i].Text.Contains("_"))
                        {
                            laecadoqeohaEsusame.Translations[i].Text = this._defaultLangage.Translations[i].Text;
                        }
                    }
                    this._rotaojeoliate.Rows.Clear();
                    for (int j = 0; j < this._defaultLangage.Translations.Count; j++)
                    {
                        Translate.Translation translation = this._defaultLangage.Translations[j];
                        this._rotaojeoliate.Rows.Add(new object[] { translation.Id.ToString(), laecadoqeohaEsusame.Translations[j].Text, translation.Text });
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Translate_Tools > LoadGrid(string filePath): " + exception, true);
            }
        }

        private void Loasifoujiba(object moleileucucisUgofe, EventArgs deisiko)
        {
            base.Close();
        }

        private void Miela(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._goapaIliaba.Image = Resources.greenB_260;
        }

        private void OgiajiloUxi(ref Translate.Language laecadoqeohaEsusame)
        {
            for (int i = 0; i <= (this._defaultLangage.Translations.Count - 1); i++)
            {
                if ((laecadoqeohaEsusame.Translations.Count - 1) > i)
                {
                    if (this._defaultLangage.Translations[i].Id == laecadoqeohaEsusame.Translations[i].Id)
                    {
                        continue;
                    }
                    bool flag = false;
                    int index = 0;
                    string text = "";
                    for (int j = 0; j < (laecadoqeohaEsusame.Translations.Count - 1); j++)
                    {
                        if (laecadoqeohaEsusame.Translations[j].Id == this._defaultLangage.Translations[i].Id)
                        {
                            flag = true;
                            index = j;
                            text = laecadoqeohaEsusame.Translations[j].Text;
                            break;
                        }
                    }
                    if (flag)
                    {
                        laecadoqeohaEsusame.Translations.RemoveAt(index);
                        if (string.IsNullOrEmpty(text))
                        {
                            text = this._defaultLangage.Translations[i].Text;
                        }
                        Translate.Translation item = new Translate.Translation {
                            Id = this._defaultLangage.Translations[i].Id,
                            Text = text
                        };
                        laecadoqeohaEsusame.Translations.Insert(i, item);
                    }
                    else
                    {
                        laecadoqeohaEsusame.Translations.Insert(i, this._defaultLangage.Translations[i]);
                    }
                    continue;
                }
                if (((laecadoqeohaEsusame.Translations.Count - 1) < i) && (i != this._defaultLangage.Translations.Count))
                {
                    laecadoqeohaEsusame.Translations.Insert(i, this._defaultLangage.Translations[i]);
                }
            }
            while (this._defaultLangage.Translations.Count < laecadoqeohaEsusame.Translations.Count)
            {
                laecadoqeohaEsusame.Translations.RemoveAt(laecadoqeohaEsusame.Translations.Count - 1);
            }
        }

        private void OveireufeOcoamu(object moleileucucisUgofe, EventArgs deisiko)
        {
            this.CeujavuiqTih();
            base.Close();
        }

        private void QoevoidBaelici(object moleileucucisUgofe, EventArgs deisiko)
        {
            this.IraeraheolIki("");
        }

        private void Riufaeta(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._goapaIliaba.Image = Resources.blackB_260;
        }

        private void Roigie(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._egamiuxuinoruiQev.Image = Resources.greenB_260;
        }

        private void Utaeriopasa()
        {
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(TranslationManagementMainFrame));
            this._leixai = new Label();
            this._egamiuxuinoruiQev = new Label();
            this._goapaIliaba = new Label();
            this._rotaojeoliate = new DataGridView();
            this._xuwoagPo = new TnbControlMenu();
            ((ISupportInitialize) this._rotaojeoliate).BeginInit();
            base.SuspendLayout();
            this._leixai.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._leixai.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._leixai.ForeColor = Color.White;
            this._leixai.Image = Resources.blueB_260;
            this._leixai.Location = new Point(0x1fb, 0x36);
            this._leixai.Margin = new Padding(0);
            this._leixai.MaximumSize = new Size(260, 0x1d);
            this._leixai.MinimumSize = new Size(260, 0x1d);
            this._leixai.Name = "LoadButton";
            this._leixai.Size = new Size(260, 0x1d);
            this._leixai.TabIndex = 13;
            this._leixai.Text = "LOAD A TRANSLATION FILE";
            this._leixai.TextAlign = ContentAlignment.MiddleCenter;
            this._leixai.Click += new EventHandler(this.QoevoidBaelici);
            this._leixai.MouseEnter += new EventHandler(this.EneniavoveabTop);
            this._leixai.MouseLeave += new EventHandler(this.Diabigaipoc);
            this._egamiuxuinoruiQev.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._egamiuxuinoruiQev.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._egamiuxuinoruiQev.ForeColor = Color.White;
            this._egamiuxuinoruiQev.Image = Resources.blueB_260;
            this._egamiuxuinoruiQev.Location = new Point(0x1fb, 0x22f);
            this._egamiuxuinoruiQev.Margin = new Padding(0);
            this._egamiuxuinoruiQev.MaximumSize = new Size(260, 0x1d);
            this._egamiuxuinoruiQev.MinimumSize = new Size(260, 0x1d);
            this._egamiuxuinoruiQev.Name = "SaveButton";
            this._egamiuxuinoruiQev.Size = new Size(260, 0x1d);
            this._egamiuxuinoruiQev.TabIndex = 14;
            this._egamiuxuinoruiQev.Text = "SAVE AND CLOSE";
            this._egamiuxuinoruiQev.TextAlign = ContentAlignment.MiddleCenter;
            this._egamiuxuinoruiQev.Click += new EventHandler(this.OveireufeOcoamu);
            this._egamiuxuinoruiQev.MouseEnter += new EventHandler(this.Roigie);
            this._egamiuxuinoruiQev.MouseLeave += new EventHandler(this.BixoexuceaciFugavo);
            this._goapaIliaba.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._goapaIliaba.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._goapaIliaba.ForeColor = Color.White;
            this._goapaIliaba.Image = Resources.blackB_260;
            this._goapaIliaba.Location = new Point(0xed, 0x22f);
            this._goapaIliaba.Margin = new Padding(0);
            this._goapaIliaba.MaximumSize = new Size(260, 0x1d);
            this._goapaIliaba.MinimumSize = new Size(260, 0x1d);
            this._goapaIliaba.Name = "QuitButton";
            this._goapaIliaba.Size = new Size(260, 0x1d);
            this._goapaIliaba.TabIndex = 15;
            this._goapaIliaba.Text = "CLOSE WITHOUT SAVING";
            this._goapaIliaba.TextAlign = ContentAlignment.MiddleCenter;
            this._goapaIliaba.Click += new EventHandler(this.Loasifoujiba);
            this._goapaIliaba.MouseEnter += new EventHandler(this.Miela);
            this._goapaIliaba.MouseLeave += new EventHandler(this.Riufaeta);
            style.BackColor = Color.FromArgb(250, 250, 250);
            style.ForeColor = Color.FromArgb(0x76, 0x76, 0x76);
            style.SelectionBackColor = Color.FromArgb(0x93, 0xb5, 0x16);
            style.SelectionForeColor = Color.FromArgb(250, 250, 250);
            this._rotaojeoliate.AlternatingRowsDefaultCellStyle = style;
            this._rotaojeoliate.BackgroundColor = Color.FromArgb(250, 250, 250);
            this._rotaojeoliate.BorderStyle = BorderStyle.None;
            this._rotaojeoliate.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._rotaojeoliate.GridColor = Color.FromArgb(0x76, 0x76, 0x76);
            this._rotaojeoliate.Location = new Point(0x23, 0x5f);
            this._rotaojeoliate.Name = "TranslationTable";
            this._rotaojeoliate.Size = new Size(730, 0x1c4);
            this._rotaojeoliate.TabIndex = 0x15;
            this._xuwoagPo.BackgroundImage = Resources._800x43_controlbar;
            this._xuwoagPo.Location = new Point(0, 0);
            this._xuwoagPo.LogoImage = (Image) manager.GetObject("MainHeader.LogoImage");
            this._xuwoagPo.Name = "MainHeader";
            this._xuwoagPo.Size = new Size(800, 0x2b);
            this._xuwoagPo.TabIndex = 0x16;
            this._xuwoagPo.TitleFont = new Font("Microsoft Sans Serif", 12f);
            this._xuwoagPo.TitleForeColor = Color.FromArgb(0xde, 0xde, 0xde);
            this._xuwoagPo.TitleText = "Translate Manager - TheNoobBot - DevVersionRestrict";
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = SystemColors.ActiveCaptionText;
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.ClientSize = new Size(800, 600);
            base.Controls.Add(this._xuwoagPo);
            base.Controls.Add(this._rotaojeoliate);
            base.Controls.Add(this._goapaIliaba);
            base.Controls.Add(this._egamiuxuinoruiQev);
            base.Controls.Add(this._leixai);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            this.MaximumSize = new Size(800, 600);
            this.MinimumSize = new Size(800, 600);
            base.Name = "TranslationManagementMainFrame";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Translate Tools";
            ((ISupportInitialize) this._rotaojeoliate).EndInit();
            base.ResumeLayout(false);
        }
    }
}

