namespace nManager.Helpful.Forms.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml.Linq;

    public sealed class UCXmlRichTextBox : RichTextBox
    {
        private ToolStripSeparator _aralikeqel;
        private ContextMenuStrip _asiabugor;
        private string _ejiecavoVacoamab = "";
        private IContainer _koiduferaluOwiakio;
        private ToolStripMenuItem _omiwuinelelEkaxiwivOwaubo;
        private ToolStripMenuItem _uheabiwegiu;
        private ToolStripMenuItem _uwiereutahamuo;

        public UCXmlRichTextBox()
        {
            this.Utaeriopasa();
            this.Font = new Font("Consolas", 9.5f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this._uheabiwegiu.Click += new EventHandler(this.Moenurecaixoa);
            this._uwiereutahamuo.Click += new EventHandler(this.Niginegina);
            this._omiwuinelelEkaxiwivOwaubo.Click += new EventHandler(this.XuoqoguserilSatua);
        }

        public static void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this._koiduferaluOwiakio != null))
            {
                this._koiduferaluOwiakio.Dispose();
            }
            base.Dispose(disposing);
        }

        private void LoitaokejiatVo(object moleileucucisUgofe, CancelEventArgs deisiko)
        {
            if (string.IsNullOrEmpty(this.SelectedText))
            {
                this._uwiereutahamuo.Enabled = false;
                this._uheabiwegiu.Enabled = false;
            }
            else
            {
                this._uwiereutahamuo.Enabled = true;
                this._uheabiwegiu.Enabled = true;
            }
        }

        private void Moenurecaixoa(object moleileucucisUgofe, EventArgs deisiko)
        {
            DataObject data = new DataObject();
            data.SetText(base.SelectedRtf, TextDataFormat.Rtf);
            data.SetText(this.SelectedText, TextDataFormat.UnicodeText);
            Clipboard.Clear();
            Clipboard.SetDataObject(data);
        }

        private void Niginegina(object moleileucucisUgofe, EventArgs deisiko)
        {
            string selectedText = this.SelectedText;
            try
            {
                selectedText = XDocument.Parse(selectedText).ToString();
            }
            catch
            {
            }
            Clipboard.SetText(selectedText);
        }

        private void Utaeriopasa()
        {
            this._koiduferaluOwiakio = new Container();
            this._asiabugor = new ContextMenuStrip(this._koiduferaluOwiakio);
            this._uwiereutahamuo = new ToolStripMenuItem();
            this._uheabiwegiu = new ToolStripMenuItem();
            this._aralikeqel = new ToolStripSeparator();
            this._omiwuinelelEkaxiwivOwaubo = new ToolStripMenuItem();
            this._asiabugor.SuspendLayout();
            base.SuspendLayout();
            this._asiabugor.Items.AddRange(new ToolStripItem[] { this._uwiereutahamuo, this._uheabiwegiu, this._aralikeqel, this._omiwuinelelEkaxiwivOwaubo });
            this._asiabugor.Name = "contextMenuStrip1";
            this._asiabugor.Size = new Size(0x9a, 0x4c);
            this._asiabugor.Opening += new CancelEventHandler(this.LoitaokejiatVo);
            this._uwiereutahamuo.Name = "miCopyText";
            this._uwiereutahamuo.Size = new Size(0x99, 0x16);
            this._uwiereutahamuo.Text = "Copy Text";
            this._uheabiwegiu.Name = "miCopyRtf";
            this._uheabiwegiu.Size = new Size(0x99, 0x16);
            this._uheabiwegiu.Text = "Copy Rich Text";
            this._aralikeqel.Name = "toolStripSeparator1";
            this._aralikeqel.Size = new Size(150, 6);
            this._omiwuinelelEkaxiwivOwaubo.Name = "miSelectAll";
            this._omiwuinelelEkaxiwivOwaubo.Size = new Size(0x99, 0x16);
            this._omiwuinelelEkaxiwivOwaubo.Text = "Select All";
            this.ContextMenuStrip = this._asiabugor;
            this._asiabugor.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void WoimueFuewo(string uleavacaom)
        {
            if (!string.IsNullOrEmpty(uleavacaom))
            {
                string str = XDocument.Parse(uleavacaom).ToString().Trim();
                if (!string.IsNullOrEmpty(str))
                {
                    XmlStateMachine machine = new XmlStateMachine();
                    if (uleavacaom.StartsWith("<?"))
                    {
                        string xmlDeclaration = machine.GetXmlDeclaration(uleavacaom);
                        if (xmlDeclaration != string.Empty)
                        {
                            str = xmlDeclaration + Environment.NewLine + str;
                        }
                    }
                    int location = 0;
                    int num2 = 0;
                    int num3 = 0;
                    while (location < str.Length)
                    {
                        XmlTokenType type;
                        string text = machine.GetNextToken(str, location, out type);
                        Color tokenColor = machine.GetTokenColor(type);
                        AppendText(this, text, tokenColor);
                        location += text.Length;
                        num3++;
                        if (text.Length == 0)
                        {
                            num2++;
                        }
                        if ((num2 > 10) || (num3 > str.Length))
                        {
                            string str4 = str.Substring(location, str.Length - location);
                            base.AppendText(str4);
                            return;
                        }
                    }
                }
            }
        }

        private void XuoqoguserilSatua(object moleileucucisUgofe, EventArgs deisiko)
        {
            base.SelectAll();
        }

        public string Xml
        {
            get
            {
                return this._ejiecavoVacoamab;
            }
            set
            {
                this.Text = "";
                this._ejiecavoVacoamab = value;
                this.WoimueFuewo(this._ejiecavoVacoamab);
            }
        }
    }
}

