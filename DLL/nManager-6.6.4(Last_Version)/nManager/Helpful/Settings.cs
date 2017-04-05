namespace nManager.Helpful
{
    using nManager;
    using nManager.Helpful.Forms.UserControls;
    using nManager.Properties;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public abstract class Settings
    {
        private string _fataixoe = "Settings";
        private TnbRibbonManager _iseviaqesigen;
        private readonly List<TukiuJae> _listFormSetting = new List<TukiuJae>();
        private const int _mawuakiati = 12;
        private readonly ComponentResourceManager _resources = new ComponentResourceManager(typeof(DeveloperToolsMainFrame));
        private Form _ruruadeufoewSamo;
        private const int _ucomauhaomuheToepohete = 5;
        private TnbControlMenu _xamiob;

        protected Settings()
        {
        }

        protected void AddControlInWinForm(string description, string fieldName, string category = "Main", string settingsType = "", string ressourceType = "")
        {
            try
            {
                if ((description != string.Empty) && (category != string.Empty))
                {
                    this._listFormSetting.Add(new TukiuJae(description, fieldName, category, settingsType, ressourceType));
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("AddControlInWinForm(string description, string fieldName, string category = \"Main\"): " + exception, true);
            }
        }

        public static string AdviserFilePathAndName(string productName)
        {
            string str;
            nManager.Helpful.Timer timer = new nManager.Helpful.Timer(10000.0);
            try
            {
                timer.Reset();
                while (nManager.Wow.ObjectManager.ObjectManager.Me.WowClass.ToString() == "None")
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                    if (timer.IsReady)
                    {
                        break;
                    }
                }
                str = string.Concat(new object[] { productName, "-", Others.DelSpecialChar(nManager.Wow.ObjectManager.ObjectManager.Me.Name), ".", nManager.Wow.ObjectManager.ObjectManager.Me.WowClass, ".", Others.DelSpecialChar(Usefuls.RealmName), ".xml" });
            }
            catch (Exception exception)
            {
                Logging.WriteError("AdviserFileName(string productName): " + exception, true);
                str = productName + "-null.xml";
            }
            return (Application.StartupPath + @"\Settings\" + str);
        }

        public void ConfigWinForm(string windowName = "Settings")
        {
            this._fataixoe = windowName;
        }

        public static T Load<T>(string settingsPath)
        {
            try
            {
                if (File.Exists(settingsPath))
                {
                    return XmlSerializer.Deserialize<T>(settingsPath);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Load<T>(string settingsPath): " + exception, true);
            }
            return default(T);
        }

        public bool Save(string settingsPath)
        {
            try
            {
                XmlSerializer.Serialize(settingsPath, this);
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Save(string settingsPath): " + exception, true);
            }
            return false;
        }

        private void Soijeaw(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._iseviaqesigen.Focus();
        }

        public void ToForm()
        {
            try
            {
                Form form = new Form {
                    BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8),
                    Size = new Size(0x23f, 0x193),
                    FormBorderStyle = FormBorderStyle.None,
                    Name = "MainForm",
                    ShowIcon = false,
                    StartPosition = FormStartPosition.CenterParent,
                    Text = this._fataixoe,
                    Icon = (Icon) this._resources.GetObject("$this.Icon"),
                    BackgroundImage = Resources.backgroundCustomSettings,
                    Font = new Font("Segoe UI", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0)
                };
                this._ruruadeufoewSamo = form;
                this._ruruadeufoewSamo.Load += new EventHandler(this.Soijeaw);
                this._ruruadeufoewSamo.Click += new EventHandler(this.Soijeaw);
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                {
                    this._ruruadeufoewSamo.TopMost = true;
                }
                TnbControlMenu menu = new TnbControlMenu {
                    BackgroundImageLayout = ImageLayout.None,
                    Location = new Point(0, 0),
                    Margin = new Padding(0),
                    Name = "MainHeader",
                    Size = new Size(0x23f, 0x2b),
                    TitleFont = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0),
                    TitleForeColor = Color.FromArgb(250, 250, 250),
                    TitleText = this._fataixoe
                };
                this._xamiob = menu;
                this._ruruadeufoewSamo.Controls.Add(this._xamiob);
                TnbRibbonManager manager = new TnbRibbonManager {
                    Anchor = AnchorStyles.None,
                    AutoScroll = true,
                    AutoScrollMinSize = new Size(0, 0x169),
                    BackColor = Color.Transparent,
                    ForeColor = Color.FromArgb(0x34, 0x34, 0x34),
                    Location = new Point(-7, 0x18),
                    TabIndex = 0,
                    Name = "MainPanel",
                    Size = new Size(0x23d, 0x167)
                };
                this._iseviaqesigen = manager;
                this._ruruadeufoewSamo.Controls.Add(this._iseviaqesigen);
                List<TnbExpendablePanel> list = new List<TnbExpendablePanel>();
                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                foreach (TukiuJae jae in this._listFormSetting)
                {
                    int num3;
                    NumericUpDown down;
                    Label label6;
                    Label label11;
                    Label label = null;
                    int num = -1;
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        if (!(list[i].TitleText != jae.get_Category()))
                        {
                            num = i;
                            break;
                        }
                    }
                    if (num < 0)
                    {
                        TnbExpendablePanel item = new TnbExpendablePanel {
                            BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8),
                            BorderColor = Color.FromArgb(0x34, 0x34, 0x34),
                            BorderStyle = ButtonBorderStyle.Solid,
                            ContentSize = new Size(0x22c, 0),
                            Fold = false,
                            AutoSize = true,
                            HeaderBackColor = Color.FromArgb(250, 250, 250),
                            HeaderSize = new Size(0x22c, 0x24),
                            Location = new Point(0, 0),
                            Margin = new Padding(0),
                            Padding = new Padding(0, 0, 0, 12),
                            Size = new Size(0x22c, 0x24),
                            TitleFont = new Font("Segoe UI", 7.65f, FontStyle.Bold),
                            TitleForeColor = Color.FromArgb(0xe8, 0xe8, 0xe8),
                            TitleText = jae.get_Category()
                        };
                        item.Click += new EventHandler(this.Soijeaw);
                        list.Add(item);
                        dictionary.Add(jae.get_Category(), item.HeaderSize.Height + 12);
                        num = list.Count - 1;
                    }
                    dictionary.TryGetValue(jae.get_Category(), out num3);
                    if (jae.get_FieldName() == string.Empty)
                    {
                        label = new Label {
                            Text = jae.get_Description(),
                            Location = new Point(12, num3),
                            Size = new Size(80, 0x11),
                            AutoSize = true,
                            BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8),
                            ForeColor = Color.FromArgb(0x34, 0x34, 0x34)
                        };
                        list[num].Controls.Add(label);
                        dictionary.Remove(jae.get_Category());
                        dictionary.Add(jae.get_Category(), num3 + 12);
                        continue;
                    }
                    FieldInfo field = base.GetType().GetField(jae.get_FieldName());
                    if (field == null)
                    {
                        continue;
                    }
                    switch (System.Type.GetTypeCode(field.FieldType))
                    {
                        case TypeCode.Boolean:
                        {
                            TnbSwitchButton button = new TnbSwitchButton {
                                Value = (bool) field.GetValue(this),
                                ForeColor = Color.FromArgb(0x34, 0x34, 0x34),
                                Location = new Point(12, num3),
                                Name = jae.get_FieldName(),
                                OffText = Translate.Get(Translate.Id.NO),
                                OnText = Translate.Get(Translate.Id.YES),
                                Size = new Size(60, 20)
                            };
                            label = new Label {
                                Text = jae.get_Description(),
                                Location = new Point((12 + button.Size.Width) + 12, num3 + 4),
                                Size = new Size(80, 0x11),
                                AutoSize = true,
                                BackColor = Color.Transparent
                            };
                            list[num].Controls.Add(label);
                            dictionary.Remove(jae.get_Category());
                            dictionary.Add(jae.get_Category(), (num3 + 12) + button.Size.Height);
                            list[num].Controls.Add(button);
                            goto Label_0E2F;
                        }
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        {
                            if (jae.get_SettingsType().Contains("Percentage") || !(jae.get_SettingsType() != "Percentage"))
                            {
                                break;
                            }
                            NumericUpDown down2 = new NumericUpDown {
                                Location = new Point(12, num3)
                            };
                            int[] bits = new int[4];
                            bits[0] = -1592738368;
                            bits[1] = 7;
                            down2.Maximum = new decimal(bits);
                            down2.Minimum = new decimal(new int[] { -1592738368, 7, 0, -2147483648 });
                            down2.Name = jae.get_FieldName();
                            down2.Size = new Size(120, 20);
                            down2.Value = Convert.ToInt64(field.GetValue(this));
                            down = down2;
                            label = new Label {
                                Text = jae.get_Description(),
                                Location = new Point((12 + down.Size.Width) + 12, num3 + 2),
                                Size = new Size(80, 0x11),
                                AutoSize = true,
                                BackColor = Color.Transparent
                            };
                            if (jae.get_SettingsType() == "Percentage")
                            {
                                down.Maximum = 100M;
                                down.Minimum = 0M;
                                down.Size = new Size(0x42, 0x16);
                                label.Location = new Point((12 + down.Size.Width) + 12, num3 + 2);
                            }
                            dictionary.Remove(jae.get_Category());
                            dictionary.Add(jae.get_Category(), (num3 + 12) + down.Size.Height);
                            list[num].Controls.Add(down);
                            list[num].Controls.Add(label);
                            goto Label_0E2F;
                        }
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                        {
                            if (jae.get_SettingsType().Contains("Percentage") || !(jae.get_SettingsType() != "Percentage"))
                            {
                                goto Label_0A9E;
                            }
                            NumericUpDown down3 = new NumericUpDown {
                                Location = new Point(12, num3)
                            };
                            int[] numArray2 = new int[4];
                            numArray2[0] = -1592738368;
                            numArray2[1] = 7;
                            down3.Maximum = new decimal(numArray2);
                            down3.Name = jae.get_FieldName();
                            down3.Size = new Size(120, 20);
                            down3.Value = Convert.ToUInt64(field.GetValue(this));
                            down = down3;
                            label = new Label {
                                Text = jae.get_Description(),
                                Location = new Point((12 + down.Size.Width) + 12, num3 + 2),
                                Size = new Size(80, 0x11),
                                AutoSize = true,
                                BackColor = Color.Transparent
                            };
                            if (jae.get_SettingsType() == "Percentage")
                            {
                                down.Maximum = 100M;
                                down.Minimum = 0M;
                                down.Size = new Size(0x42, 0x16);
                                label.Location = new Point((12 + down.Size.Width) + 12, num3 + 2);
                            }
                            dictionary.Remove(jae.get_Category());
                            dictionary.Add(jae.get_Category(), (num3 + 12) + down.Size.Height);
                            list[num].Controls.Add(down);
                            list[num].Controls.Add(label);
                            goto Label_0E2F;
                        }
                        case TypeCode.Single:
                        case TypeCode.Double:
                        {
                            NumericUpDown down4 = new NumericUpDown {
                                Location = new Point(12, num3),
                                DecimalPlaces = 3
                            };
                            int[] numArray3 = new int[4];
                            numArray3[0] = 1;
                            numArray3[3] = 0x10000;
                            down4.Increment = new decimal(numArray3);
                            int[] numArray4 = new int[4];
                            numArray4[0] = -1592738368;
                            numArray4[1] = 7;
                            down4.Maximum = new decimal(numArray4);
                            down4.Minimum = new decimal(new int[] { -1592738368, 7, 0, -2147483648 });
                            down4.Name = jae.get_FieldName();
                            down4.Size = new Size(120, 20);
                            down4.Value = Convert.ToDecimal(field.GetValue(this));
                            down = down4;
                            label = new Label {
                                Text = jae.get_Description(),
                                Location = new Point((12 + down.Size.Width) + 12, num3 + 2),
                                Size = new Size(80, 0x11),
                                AutoSize = true,
                                BackColor = Color.Transparent
                            };
                            dictionary.Remove(jae.get_Category());
                            dictionary.Add(jae.get_Category(), (num3 + 12) + down.Size.Height);
                            list[num].Controls.Add(down);
                            list[num].Controls.Add(label);
                            goto Label_0E2F;
                        }
                        case TypeCode.String:
                        {
                            TextBox box = new TextBox {
                                Location = new Point(12, num3),
                                Name = jae.get_FieldName(),
                                Size = new Size(100, 20),
                                TabIndex = 3,
                                Text = Convert.ToString(field.GetValue(this))
                            };
                            label = new Label {
                                Text = jae.get_Description(),
                                Location = new Point((12 + box.Size.Width) + 12, num3 + 2),
                                TextAlign = ContentAlignment.MiddleLeft,
                                Size = new Size(80, 0x11),
                                AutoSize = true,
                                BackColor = Color.Transparent
                            };
                            if (jae.get_SettingsType() == "List")
                            {
                                label.Size = new Size(0x214, 20);
                                label.Location = new Point(12, num3 + 2);
                                label.TextAlign = ContentAlignment.BottomLeft;
                                box.Size = new Size(0x214, 20);
                                box.Location = new Point(12, (num3 + label.Height) + 5);
                            }
                            dictionary.Remove(jae.get_Category());
                            dictionary.Add(jae.get_Category(), (box.Location.Y + box.Size.Height) + 12);
                            list[num].Controls.Add(label);
                            list[num].Controls.Add(box);
                            goto Label_0E2F;
                        }
                        default:
                            goto Label_0E2F;
                    }
                    label = new Label {
                        Text = jae.get_Description(),
                        Location = new Point(12, num3),
                        Size = new Size(80, 0x11),
                        AutoSize = true,
                        BackColor = Color.Transparent
                    };
                    dictionary.Remove(jae.get_Category());
                    dictionary.Add(jae.get_Category(), num3 + label.Size.Height);
                    list[num].Controls.Add(label);
                    goto Label_0E2F;
                Label_0A9E:
                    label6 = new Label();
                    label6.Text = jae.get_Description();
                    label6.Location = new Point(12, num3);
                    label6.Size = new Size(80, 0x11);
                    label6.AutoSize = true;
                    label6.BackColor = Color.Transparent;
                    label = label6;
                    dictionary.Remove(jae.get_Category());
                    dictionary.Add(jae.get_Category(), num3 + label.Size.Height);
                    list[num].Controls.Add(label);
                Label_0E2F:
                    if (!jae.get_SettingsType().Contains("Percentage") || !(jae.get_SettingsType() != "Percentage"))
                    {
                        continue;
                    }
                    int num4 = 0;
                    if (label != null)
                    {
                        num4 = label.Size.Width + label.Location.X;
                    }
                    string str = "";
                    FieldInfo info2 = base.GetType().GetField(jae.get_FieldName());
                    if (info2 == null)
                    {
                        continue;
                    }
                    string str2 = "this";
                    if (jae.get_RessourceType() != "")
                    {
                        str2 = jae.get_RessourceType();
                    }
                    string str3 = jae.get_SettingsType();
                    if (str3 != null)
                    {
                        if (!(str3 == "AtPercentage"))
                        {
                            if (str3 == "BelowPercentage")
                            {
                                goto Label_0F14;
                            }
                            if (str3 == "AbovePercentage")
                            {
                                goto Label_0F29;
                            }
                        }
                        else
                        {
                            str = "at " + str2 + " percentage:";
                        }
                    }
                    goto Label_0F3C;
                Label_0F14:
                    str = "below " + str2 + " percentage:";
                    goto Label_0F3C;
                Label_0F29:
                    str = "above " + str2 + " percentage:";
                Label_0F3C:
                    label11 = new Label();
                    label11.Text = str;
                    label11.Location = new Point(num4 + 0x18, num3);
                    label11.Size = new Size(90, 0x11);
                    label11.AutoSize = true;
                    label11.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
                    label11.ForeColor = Color.FromArgb(0x34, 0x34, 0x34);
                    Label label10 = label11;
                    NumericUpDown down5 = new NumericUpDown {
                        Location = new Point((((num4 + 0x18) + label10.Location.X) + label10.Width) + 0x18, num3 - 4),
                        Maximum = 100M,
                        Minimum = 0M,
                        Name = jae.get_FieldName(),
                        Size = new Size(0x26, 0x16),
                        Value = Convert.ToUInt64(info2.GetValue(this))
                    };
                    list[num].Controls.Add(label10);
                    list[num].Controls.Add(down5);
                }
                bool flag = true;
                foreach (TnbExpendablePanel panel3 in list)
                {
                    if (!flag)
                    {
                        panel3.Fold = true;
                    }
                    flag = false;
                    this._iseviaqesigen.Controls.Add(panel3);
                }
                this._iseviaqesigen.Click += new EventHandler(this.Soijeaw);
                this._ruruadeufoewSamo.Shown += new EventHandler(this.Soijeaw);
                this._ruruadeufoewSamo.ShowDialog();
                this.VaholihuicoroiEwe(this._ruruadeufoewSamo);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Settings > ToForm(): " + exception, true);
            }
        }

        private void VaholihuicoroiEwe(Form gueboeqiekuope)
        {
            try
            {
                foreach (TukiuJae jae in this._listFormSetting)
                {
                    if (jae.get_FieldName() == string.Empty)
                    {
                        continue;
                    }
                    Control[] controlArray = gueboeqiekuope.Controls.Find(jae.get_FieldName(), true);
                    FieldInfo field = base.GetType().GetField(jae.get_FieldName());
                    if ((field != null) && (controlArray.Length > 0))
                    {
                        switch (System.Type.GetTypeCode(field.FieldType))
                        {
                            case TypeCode.Boolean:
                                field.SetValue(this, ((TnbSwitchButton) controlArray[0]).Value);
                                break;

                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                                field.SetValue(this, Convert.ChangeType(((NumericUpDown) controlArray[0]).Value, field.FieldType));
                                break;

                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                                field.SetValue(this, Convert.ChangeType(((NumericUpDown) controlArray[0]).Value, field.FieldType));
                                break;

                            case TypeCode.Single:
                            case TypeCode.Double:
                                field.SetValue(this, Convert.ChangeType(((NumericUpDown) controlArray[0]).Value, field.FieldType));
                                break;

                            case TypeCode.String:
                                field.SetValue(this, controlArray[0].Text);
                                break;
                        }
                        if (jae.get_SettingsType().Contains("Percentage") && (jae.get_SettingsType() != "Percentage"))
                        {
                            Control[] controlArray2 = gueboeqiekuope.Controls.Find(jae.get_FieldName() + jae.get_SettingsType(), true);
                            FieldInfo info2 = base.GetType().GetField(jae.get_FieldName() + jae.get_SettingsType());
                            if ((info2 != null) && (controlArray2.Length > 0))
                            {
                                info2.SetValue(this, Convert.ChangeType(((NumericUpDown) controlArray2[0]).Value, info2.FieldType));
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Settings > ReadForm(Form form): " + exception, true);
            }
        }

        private class TukiuJae
        {
            public TukiuJae(string description, string fieldName, string category = "Main", string settingsType = "", string ressourceType = "")
            {
                this.set_Description(description);
                this.set_FieldName(fieldName);
                this.set_Category(category);
                this.set_SettingsType(settingsType);
                this.set_RessourceType(ressourceType);
            }

            public string _ibaxukaeweoPirida { get; private set; }

            public string _pocuwujicOv { get; private set; }

            public string _uduqajeobebobeWakupo { get; private set; }

            public string _vobiuq { get; private set; }

            public string _weatairuo { get; private set; }
        }
    }
}

