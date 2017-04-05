namespace nManager.Helpful.Forms
{
    using Microsoft.CSharp;
    using nManager;
    using nManager.Helpful;
    using nManager.Helpful.Forms.UserControls;
    using nManager.Helpful.Interface;
    using nManager.Properties;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using nManager.Wow.Patchables;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    public class DeveloperToolsMainFrame : Form
    {
        private Label AllObjectsButton;
        private IContainer components;
        private Label CsharpExecButton;
        private Label GpsButton;
        private TextBox InformationArea;
        private Label LuaExecButton;
        private TnbControlMenu MainHeader;
        private Label NpcFactionButton;
        private Label NpcTypeButton;
        private string searchInputBox = "Type in the name of the WoWObject you are looking for:";
        private Label SearchObjectButton;
        private Label TargetInfo2Button;
        private Label TargetInfoButton;
        private Label TranslationManagerButton;

        public DeveloperToolsMainFrame()
        {
            try
            {
                this.InitializeComponent();
                this.Translate();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                {
                    base.TopMost = true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > DeveloperToolsMainFrame(): " + exception, true);
            }
        }

        private void AllObjectsButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.AllObjectsButton.Enabled = false;
                object obj3 = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> <title>Get all objects information - " + DateTime.Now.ToString("dd/mm/yy HHh mmMin") + "</title>  <table width=\"100%\" bgcolor=\"#E8E8E8\" border=\"1\">   <tr>   <b>     <td bgcolor=\"#CCCCCC\">Name</td>     <td>Type</td>     <td bgcolor=\"#CCCCCC\">Entry ID</td>     <td>Position X</td>     <td bgcolor=\"#CCCCCC\">Position Y</td>     <td>Position Z</td>     <td bgcolor=\"#CCCCCC\">Distance</td>     <td>Faction</td>     <td bgcolor=\"#CCCCCC\">GUID</td>     <td>Summoned/Created By</td> <td>Unit Created By</td>    </b>   </tr>  ";
                string str = string.Concat(new object[] { 
                    obj3, "<tr>     <td bgcolor=\"#CCCCCC\">", nManager.Wow.ObjectManager.ObjectManager.Me.Name, "</td>     <td>WoWPlayer (", nManager.Wow.ObjectManager.ObjectManager.Me.Guid.GetWoWType, ")</td>     <td bgcolor=\"#CCCCCC\">", nManager.Wow.ObjectManager.ObjectManager.Me.Entry, "</td>     <td>", nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, "</td>     <td bgcolor=\"#CCCCCC\">", nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, "</td>     <td>", nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, "</td>     <td bgcolor=\"#CCCCCC\">", nManager.Wow.ObjectManager.ObjectManager.Me.GetDistance, "</td>     <td>", 
                    nManager.Wow.ObjectManager.ObjectManager.Me.Faction, "</td>     <td bgcolor=\"#CCCCCC\">", nManager.Wow.ObjectManager.ObjectManager.Me.Guid, "</td>     <td>", nManager.Wow.ObjectManager.ObjectManager.Me.SummonedBy, "</td>   </tr>"
                 });
                foreach (WoWPlayer player in nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWPlayer())
                {
                    Application.DoEvents();
                    object obj4 = str;
                    str = string.Concat(new object[] { 
                        obj4, "<tr>     <td bgcolor=\"#CCCCCC\">", player.Name, "</td>     <td>WoWPlayer (", player.Guid.GetWoWType, ")</td>     <td bgcolor=\"#CCCCCC\">", player.Entry, "</td>     <td>", player.Position.X, "</td>     <td bgcolor=\"#CCCCCC\">", player.Position.Y, "</td>     <td>", player.Position.Z, "</td>     <td bgcolor=\"#CCCCCC\">", player.GetDistance, "</td>     <td>", 
                        player.Faction, "</td>     <td bgcolor=\"#CCCCCC\">", player.Guid, "</td>     <td>", player.SummonedBy, "</td>   </tr>"
                     });
                }
                foreach (WoWUnit unit in nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWUnit())
                {
                    Application.DoEvents();
                    object obj5 = str;
                    str = string.Concat(new object[] { 
                        obj5, "<tr>     <td bgcolor=\"#CCCCCC\">", unit.Name, " - (<i><a href=\"http://wowhead.com/npc=", unit.Entry, "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWUnit (", unit.Guid.GetWoWType, ")</td>     <td bgcolor=\"#CCCCCC\">", unit.Entry, "</td>     <td>", unit.Position.X, "</td>     <td bgcolor=\"#CCCCCC\">", unit.Position.Y, "</td>     <td>", unit.Position.Z, "</td>     <td bgcolor=\"#CCCCCC\">", 
                        unit.GetDistance, "</td>     <td>", unit.Faction, "</td>     <td bgcolor=\"#CCCCCC\">", unit.Guid, "</td>     <td>", unit.SummonedBy, "</td>        <td>", unit.CreatedBy, "</td>   </tr>"
                     });
                }
                foreach (WoWGameObject obj2 in nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWGameObject())
                {
                    Application.DoEvents();
                    object obj6 = str;
                    str = string.Concat(new object[] { 
                        obj6, "<tr>     <td bgcolor=\"#CCCCCC\">", obj2.Name, " - (<i><a href=\"http://wowhead.com/object=", obj2.Entry, "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWGameObject (", obj2.Guid.GetWoWType, ")</td>     <td bgcolor=\"#CCCCCC\">", obj2.Entry, "</td>     <td>", obj2.Position.X, "</td>     <td bgcolor=\"#CCCCCC\">", obj2.Position.Y, "</td>     <td>", obj2.Position.Z, "</td>     <td bgcolor=\"#CCCCCC\">", 
                        obj2.GetDistance, "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">", obj2.Guid, "</td>     <td>", obj2.CreatedBy, "</td>   </tr>"
                     });
                }
                foreach (WoWItem item in nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWItem())
                {
                    Application.DoEvents();
                    object obj7 = str;
                    str = string.Concat(new object[] { 
                        obj7, "<tr>     <td bgcolor=\"#CCCCCC\">", item.Name, " - (<i><a href=\"http://wowhead.com/item=", item.Entry, "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWItem (", item.Guid.GetWoWType, ";", item.Guid.GetWoWSubType, ")</td>     <td bgcolor=\"#CCCCCC\">", item.Entry, "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">", item.Guid, "</td>     <td>", item.Owner, "</td><td>IsEquipped by me ? ", 
                        EquippedItems.IsEquippedItemByGuid(item.Guid), "</td>   </tr>"
                     });
                }
                foreach (WoWCorpse corpse in nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWCorpse())
                {
                    Application.DoEvents();
                    object obj8 = str;
                    str = string.Concat(new object[] { 
                        obj8, "<tr>     <td bgcolor=\"#CCCCCC\">", corpse.Name, "</td>     <td>WoWCorpse (", corpse.Guid.GetWoWType, "</td>     <td bgcolor=\"#CCCCCC\">", corpse.Entry, "</td>     <td>", corpse.Position.X, "</td>     <td bgcolor=\"#CCCCCC\">", corpse.Position.Y, "</td>     <td>", corpse.Position.Z, "</td>     <td bgcolor=\"#CCCCCC\">", corpse.GetDistance, "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">", 
                        corpse.Guid, "</td>     <td>-</td>   </tr>"
                     });
                }
                foreach (WoWContainer container in nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWContainer())
                {
                    Application.DoEvents();
                    object obj9 = str;
                    str = string.Concat(new object[] { obj9, "<tr>     <td bgcolor=\"#CCCCCC\">", container.Name, " - (<i><a href=\"http://wowhead.com/item=", container.Entry, "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWContainer (", container.Guid.GetWoWType, ")</td>     <td bgcolor=\"#CCCCCC\">", container.Entry, "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">", container.Guid, "</td>     <td>-</td>   </tr>" });
                }
                str = str + " </table> </body> </html>";
                Others.WriteFile("AllObjectsButton.html", str);
                new Process { StartInfo = { FileName = "AllObjectsButton.html", WorkingDirectory = Application.StartupPath } }.Start();
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > AllObjectsButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this.AllObjectsButton.Enabled = true;
        }

        private void AllObjectsButton_MouseEnter(object sender, EventArgs e)
        {
            this.AllObjectsButton.Image = Resources.greenB_242;
        }

        private void AllObjectsButton_MouseLeave(object sender, EventArgs e)
        {
            this.AllObjectsButton.Image = Resources.blackB_242;
        }

        private void CsharpExecButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.CsharpExecButton.Enabled = false;
                this.ExecuteScript();
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > CsharpExecButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this.CsharpExecButton.Enabled = true;
        }

        private void CsharpExecButton_MouseEnter(object sender, EventArgs e)
        {
            this.CsharpExecButton.Image = Resources.greenB_150;
        }

        private void CsharpExecButton_MouseLeave(object sender, EventArgs e)
        {
            this.CsharpExecButton.Image = Resources.blueB_150;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        internal void ExecuteScript()
        {
            try
            {
                CodeDomProvider provider = new CSharpCodeProvider();
                CompilerParameters options = new CompilerParameters();
                IEnumerable<string> source = from a in AppDomain.CurrentDomain.GetAssemblies()
                    where !a.IsDynamic && !a.CodeBase.Contains(Process.GetCurrentProcess().ProcessName + ".exe")
                    select a.Location;
                options.ReferencedAssemblies.AddRange(source.ToArray<string>());
                CompilerResults results = provider.CompileAssemblyFromSource(options, new string[] { " public class Main : nManager.Helpful.Interface.IScriptOnlineManager { public void Initialize() { " + this.InformationArea.Text + " } } " });
                if (results.Errors.HasErrors)
                {
                    MessageBox.Show(results.Errors.Cast<CompilerError>().Aggregate<CompilerError, string>("Compilator Error :\n", (current, err) => current + err + "\n"));
                }
                else
                {
                    IScriptOnlineManager manager = results.CompiledAssembly.CreateInstance("Main", true) as IScriptOnlineManager;
                    if (manager != null)
                    {
                        manager.Initialize();
                    }
                    else
                    {
                        Logging.WriteError("DeveloperToolsMainFrame > ExecuteScript()#2", true);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > ExecuteScript()#1: " + exception, true);
            }
        }

        private void GpsButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.GpsButton.Enabled = false;
                this.InformationArea.Text = "";
                if (nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)
                {
                    this.InformationArea.Text = string.Concat(new object[] { 
                        "Internal Map name: ", Usefuls.ContinentNameMpqByContinentId(Usefuls.RealContinentId), " (", Usefuls.RealContinentId, ")", Environment.NewLine, nManager.Wow.ObjectManager.ObjectManager.Me.Position, Environment.NewLine, Environment.NewLine, Environment.NewLine, "<Position>", Environment.NewLine, " <X>", nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, "</X>", Environment.NewLine, 
                        " <Y>", nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, "</Y>", Environment.NewLine, " <Z>", nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, "</Z>", Environment.NewLine, " <Type>", nManager.Wow.ObjectManager.ObjectManager.Me.Position.Type, "</Type>", Environment.NewLine, "</Position>"
                     });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > GpsButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this.GpsButton.Enabled = true;
        }

        private void GpsButton_MouseEnter(object sender, EventArgs e)
        {
            this.GpsButton.Image = Resources.greenB;
        }

        private void GpsButton_MouseLeave(object sender, EventArgs e)
        {
            this.GpsButton.Image = Resources.blackB;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DeveloperToolsMainFrame));
            this.SearchObjectButton = new Label();
            this.TargetInfoButton = new Label();
            this.InformationArea = new TextBox();
            this.GpsButton = new Label();
            this.CsharpExecButton = new Label();
            this.TargetInfo2Button = new Label();
            this.NpcFactionButton = new Label();
            this.NpcTypeButton = new Label();
            this.LuaExecButton = new Label();
            this.TranslationManagerButton = new Label();
            this.AllObjectsButton = new Label();
            this.MainHeader = new TnbControlMenu();
            base.SuspendLayout();
            this.SearchObjectButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.SearchObjectButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.SearchObjectButton.ForeColor = Color.White;
            this.SearchObjectButton.Image = Resources.blackB;
            this.SearchObjectButton.Location = new System.Drawing.Point(0x19d, 0x22e);
            this.SearchObjectButton.Margin = new Padding(0);
            this.SearchObjectButton.MaximumSize = new Size(0x6a, 0x1d);
            this.SearchObjectButton.MinimumSize = new Size(0x6a, 0x1d);
            this.SearchObjectButton.Name = "SearchObjectButton";
            this.SearchObjectButton.Size = new Size(0x6a, 0x1d);
            this.SearchObjectButton.TabIndex = 14;
            this.SearchObjectButton.Text = "Search Object";
            this.SearchObjectButton.TextAlign = ContentAlignment.MiddleCenter;
            this.SearchObjectButton.Click += new EventHandler(this.SearchObjectButton_Click);
            this.SearchObjectButton.MouseEnter += new EventHandler(this.SearchObjectButton_MouseEnter);
            this.SearchObjectButton.MouseLeave += new EventHandler(this.SearchObjectButton_MouseLeave);
            this.TargetInfoButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.TargetInfoButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.TargetInfoButton.ForeColor = Color.White;
            this.TargetInfoButton.Image = Resources.blackB;
            this.TargetInfoButton.Location = new System.Drawing.Point(0x12e, 0x205);
            this.TargetInfoButton.Margin = new Padding(0);
            this.TargetInfoButton.MaximumSize = new Size(0x6a, 0x1d);
            this.TargetInfoButton.MinimumSize = new Size(0x6a, 0x1d);
            this.TargetInfoButton.Name = "TargetInfoButton";
            this.TargetInfoButton.Size = new Size(0x6a, 0x1d);
            this.TargetInfoButton.TabIndex = 15;
            this.TargetInfoButton.Text = "Target's information";
            this.TargetInfoButton.TextAlign = ContentAlignment.MiddleCenter;
            this.TargetInfoButton.Click += new EventHandler(this.TargetInfoButton_Click);
            this.TargetInfoButton.MouseEnter += new EventHandler(this.TargetInfoButton_MouseEnter);
            this.TargetInfoButton.MouseLeave += new EventHandler(this.TargetInfoButton_MouseLeave);
            this.InformationArea.BackColor = Color.FromArgb(250, 250, 250);
            this.InformationArea.BorderStyle = BorderStyle.None;
            this.InformationArea.Location = new System.Drawing.Point(0x23, 0x45);
            this.InformationArea.Margin = new Padding(0);
            this.InformationArea.MaximumSize = new Size(730, 0x1b3);
            this.InformationArea.MinimumSize = new Size(730, 0x1b3);
            this.InformationArea.Multiline = true;
            this.InformationArea.Name = "InformationArea";
            this.InformationArea.Size = new Size(730, 0x1b3);
            this.InformationArea.TabIndex = 0x15;
            this.GpsButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.GpsButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.GpsButton.ForeColor = Color.White;
            this.GpsButton.Image = Resources.blackB;
            this.GpsButton.Location = new System.Drawing.Point(190, 0x205);
            this.GpsButton.Margin = new Padding(0);
            this.GpsButton.MaximumSize = new Size(0x6a, 0x1d);
            this.GpsButton.MinimumSize = new Size(0x6a, 0x1d);
            this.GpsButton.Name = "GpsButton";
            this.GpsButton.Size = new Size(0x6a, 0x1d);
            this.GpsButton.TabIndex = 0x17;
            this.GpsButton.Text = "GPS infos";
            this.GpsButton.TextAlign = ContentAlignment.MiddleCenter;
            this.GpsButton.Click += new EventHandler(this.GpsButton_Click);
            this.GpsButton.MouseEnter += new EventHandler(this.GpsButton_MouseEnter);
            this.GpsButton.MouseLeave += new EventHandler(this.GpsButton_MouseLeave);
            this.CsharpExecButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.CsharpExecButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.CsharpExecButton.ForeColor = Color.White;
            this.CsharpExecButton.Image = Resources.blueB_150;
            this.CsharpExecButton.Location = new System.Drawing.Point(0x22, 0x22e);
            this.CsharpExecButton.Margin = new Padding(0);
            this.CsharpExecButton.MaximumSize = new Size(150, 0x1d);
            this.CsharpExecButton.MinimumSize = new Size(150, 0x1d);
            this.CsharpExecButton.Name = "CsharpExecButton";
            this.CsharpExecButton.Size = new Size(150, 0x1d);
            this.CsharpExecButton.TabIndex = 0x18;
            this.CsharpExecButton.Text = "Execute C# code";
            this.CsharpExecButton.TextAlign = ContentAlignment.MiddleCenter;
            this.CsharpExecButton.Click += new EventHandler(this.CsharpExecButton_Click);
            this.CsharpExecButton.MouseEnter += new EventHandler(this.CsharpExecButton_MouseEnter);
            this.CsharpExecButton.MouseLeave += new EventHandler(this.CsharpExecButton_MouseLeave);
            this.TargetInfo2Button.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.TargetInfo2Button.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.TargetInfo2Button.ForeColor = Color.White;
            this.TargetInfo2Button.Image = Resources.blackB;
            this.TargetInfo2Button.Location = new System.Drawing.Point(0x19d, 0x205);
            this.TargetInfo2Button.Margin = new Padding(0);
            this.TargetInfo2Button.MaximumSize = new Size(0x6a, 0x1d);
            this.TargetInfo2Button.MinimumSize = new Size(0x6a, 0x1d);
            this.TargetInfo2Button.Name = "TargetInfo2Button";
            this.TargetInfo2Button.Size = new Size(0x6a, 0x1d);
            this.TargetInfo2Button.TabIndex = 0x19;
            this.TargetInfo2Button.Text = "Target's information 2";
            this.TargetInfo2Button.TextAlign = ContentAlignment.MiddleCenter;
            this.TargetInfo2Button.Click += new EventHandler(this.TargetInfo2Button_Click);
            this.TargetInfo2Button.MouseEnter += new EventHandler(this.TargetInfo2Button_MouseEnter);
            this.TargetInfo2Button.MouseLeave += new EventHandler(this.TargetInfo2Button_MouseLeave);
            this.NpcFactionButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.NpcFactionButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.NpcFactionButton.ForeColor = Color.White;
            this.NpcFactionButton.Image = Resources.blackB;
            this.NpcFactionButton.Location = new System.Drawing.Point(0x12e, 0x22e);
            this.NpcFactionButton.Margin = new Padding(0);
            this.NpcFactionButton.MaximumSize = new Size(0x6a, 0x1d);
            this.NpcFactionButton.MinimumSize = new Size(0x6a, 0x1d);
            this.NpcFactionButton.Name = "NpcFactionButton";
            this.NpcFactionButton.Size = new Size(0x6a, 0x1d);
            this.NpcFactionButton.TabIndex = 0x1a;
            this.NpcFactionButton.Text = "NPC Faction List";
            this.NpcFactionButton.TextAlign = ContentAlignment.MiddleCenter;
            this.NpcFactionButton.Click += new EventHandler(this.NpcFactionButton_Click);
            this.NpcFactionButton.MouseEnter += new EventHandler(this.NpcFactionButton_MouseEnter);
            this.NpcFactionButton.MouseLeave += new EventHandler(this.NpcFactionButton_MouseLeave);
            this.NpcTypeButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.NpcTypeButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.NpcTypeButton.ForeColor = Color.White;
            this.NpcTypeButton.Image = Resources.blackB;
            this.NpcTypeButton.Location = new System.Drawing.Point(190, 0x22e);
            this.NpcTypeButton.Margin = new Padding(0);
            this.NpcTypeButton.MaximumSize = new Size(0x6a, 0x1d);
            this.NpcTypeButton.MinimumSize = new Size(0x6a, 0x1d);
            this.NpcTypeButton.Name = "NpcTypeButton";
            this.NpcTypeButton.Size = new Size(0x6a, 0x1d);
            this.NpcTypeButton.TabIndex = 0x1b;
            this.NpcTypeButton.Text = "NPC Type List";
            this.NpcTypeButton.TextAlign = ContentAlignment.MiddleCenter;
            this.NpcTypeButton.Click += new EventHandler(this.NpcTypeButton_Click);
            this.NpcTypeButton.MouseEnter += new EventHandler(this.NpcTypeButton_MouseEnter);
            this.NpcTypeButton.MouseLeave += new EventHandler(this.NpcTypeButton_MouseLeave);
            this.LuaExecButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.LuaExecButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.LuaExecButton.ForeColor = Color.White;
            this.LuaExecButton.Image = Resources.blueB_150;
            this.LuaExecButton.Location = new System.Drawing.Point(0x22, 0x205);
            this.LuaExecButton.Margin = new Padding(0);
            this.LuaExecButton.MaximumSize = new Size(150, 0x1d);
            this.LuaExecButton.MinimumSize = new Size(150, 0x1d);
            this.LuaExecButton.Name = "LuaExecButton";
            this.LuaExecButton.Size = new Size(150, 0x1d);
            this.LuaExecButton.TabIndex = 0x1d;
            this.LuaExecButton.Text = "Execute LUA code";
            this.LuaExecButton.TextAlign = ContentAlignment.MiddleCenter;
            this.LuaExecButton.Click += new EventHandler(this.LuaExecButton_Click);
            this.LuaExecButton.MouseEnter += new EventHandler(this.LuaExecButton_MouseEnter);
            this.LuaExecButton.MouseLeave += new EventHandler(this.LuaExecButton_MouseLeave);
            this.TranslationManagerButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.TranslationManagerButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.TranslationManagerButton.ForeColor = Color.White;
            this.TranslationManagerButton.Image = Resources.blueB_242;
            this.TranslationManagerButton.Location = new System.Drawing.Point(0x20d, 0x205);
            this.TranslationManagerButton.Margin = new Padding(0);
            this.TranslationManagerButton.MaximumSize = new Size(0xf2, 0x1d);
            this.TranslationManagerButton.MinimumSize = new Size(0xf2, 0x1d);
            this.TranslationManagerButton.Name = "TranslationManagerButton";
            this.TranslationManagerButton.Size = new Size(0xf2, 0x1d);
            this.TranslationManagerButton.TabIndex = 0x20;
            this.TranslationManagerButton.Text = "Open Translation Manager";
            this.TranslationManagerButton.TextAlign = ContentAlignment.MiddleCenter;
            this.TranslationManagerButton.Click += new EventHandler(this.TranslationManagerButton_Click);
            this.TranslationManagerButton.MouseEnter += new EventHandler(this.TranslationManagerButton_MouseEnter);
            this.TranslationManagerButton.MouseLeave += new EventHandler(this.TranslationManagerButton_MouseLeave);
            this.AllObjectsButton.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this.AllObjectsButton.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.AllObjectsButton.ForeColor = Color.White;
            this.AllObjectsButton.Image = Resources.blackB_242;
            this.AllObjectsButton.Location = new System.Drawing.Point(0x20d, 0x22e);
            this.AllObjectsButton.Margin = new Padding(0);
            this.AllObjectsButton.MaximumSize = new Size(0xf2, 0x1d);
            this.AllObjectsButton.MinimumSize = new Size(0xf2, 0x1d);
            this.AllObjectsButton.Name = "AllObjectsButton";
            this.AllObjectsButton.Size = new Size(0xf2, 0x1d);
            this.AllObjectsButton.TabIndex = 0x21;
            this.AllObjectsButton.Text = "Get all ingame objects information";
            this.AllObjectsButton.TextAlign = ContentAlignment.MiddleCenter;
            this.AllObjectsButton.Click += new EventHandler(this.AllObjectsButton_Click);
            this.AllObjectsButton.MouseEnter += new EventHandler(this.AllObjectsButton_MouseEnter);
            this.AllObjectsButton.MouseLeave += new EventHandler(this.AllObjectsButton_MouseLeave);
            this.MainHeader.BackgroundImage = Resources._800x43_controlbar;
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = (Image) manager.GetObject("MainHeader.LogoImage");
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new Size(800, 0x2b);
            this.MainHeader.TabIndex = 0x22;
            this.MainHeader.TitleFont = new Font("Microsoft Sans Serif", 12f);
            this.MainHeader.TitleForeColor = Color.FromArgb(0xde, 0xde, 0xde);
            this.MainHeader.TitleText = "Developer Tools - TheNoobBot - DevVersionRestrict";
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = SystemColors.ActiveCaptionText;
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.ClientSize = new Size(800, 600);
            base.Controls.Add(this.MainHeader);
            base.Controls.Add(this.AllObjectsButton);
            base.Controls.Add(this.TranslationManagerButton);
            base.Controls.Add(this.LuaExecButton);
            base.Controls.Add(this.NpcTypeButton);
            base.Controls.Add(this.NpcFactionButton);
            base.Controls.Add(this.TargetInfo2Button);
            base.Controls.Add(this.CsharpExecButton);
            base.Controls.Add(this.GpsButton);
            base.Controls.Add(this.InformationArea);
            base.Controls.Add(this.TargetInfoButton);
            base.Controls.Add(this.SearchObjectButton);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            this.MaximumSize = new Size(800, 600);
            this.MinimumSize = new Size(800, 600);
            base.Name = "DeveloperToolsMainFrame";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Translate Tools";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox box = new TextBox();
            Button button = new Button();
            Button button2 = new Button();
            form.Text = title;
            label.Text = promptText;
            box.Text = value;
            button.Text = "OK";
            button2.Text = "Cancel";
            button.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
            label.SetBounds(9, 20, 0x174, 13);
            box.SetBounds(12, 0x24, 0x174, 20);
            button.SetBounds(0xe4, 0x48, 0x4b, 0x17);
            button2.SetBounds(0x135, 0x48, 0x4b, 0x17);
            label.AutoSize = true;
            box.Anchor |= AnchorStyles.Right;
            button.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            button2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            form.ClientSize = new Size(0x18c, 0x6b);
            form.Controls.AddRange(new Control[] { label, box, button, button2 });
            form.ClientSize = new Size(System.Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = button;
            form.CancelButton = button2;
            DialogResult result = form.ShowDialog();
            value = box.Text;
            return result;
        }

        private void LuaExecButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.LuaExecButton.Enabled = false;
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"1\")", false, true);
                Lua.LuaDoString(this.InformationArea.Text, false, true);
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"0\")", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > LuaExecButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this.LuaExecButton.Enabled = true;
        }

        private void LuaExecButton_MouseEnter(object sender, EventArgs e)
        {
            this.LuaExecButton.Image = Resources.greenB_150;
        }

        private void LuaExecButton_MouseLeave(object sender, EventArgs e)
        {
            this.LuaExecButton.Image = Resources.blueB_150;
        }

        private void NpcFactionButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.NpcFactionButton.Enabled = false;
                this.InformationArea.Text = "";
                foreach (Npc.FactionType type in Enum.GetValues(typeof(Npc.FactionType)).Cast<Npc.FactionType>().ToList<Npc.FactionType>())
                {
                    this.InformationArea.Text = this.InformationArea.Text + type + Environment.NewLine;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > NpcFactionButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this.NpcFactionButton.Enabled = true;
        }

        private void NpcFactionButton_MouseEnter(object sender, EventArgs e)
        {
            this.NpcFactionButton.Image = Resources.greenB;
        }

        private void NpcFactionButton_MouseLeave(object sender, EventArgs e)
        {
            this.NpcFactionButton.Image = Resources.blackB;
        }

        private void NpcTypeButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.NpcTypeButton.Enabled = false;
                this.InformationArea.Text = "";
                foreach (Npc.NpcType type in Enum.GetValues(typeof(Npc.NpcType)).Cast<Npc.NpcType>().ToList<Npc.NpcType>())
                {
                    this.InformationArea.Text = this.InformationArea.Text + type + Environment.NewLine;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > NpcTypeButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this.NpcTypeButton.Enabled = true;
        }

        private void NpcTypeButton_MouseEnter(object sender, EventArgs e)
        {
            this.NpcTypeButton.Image = Resources.greenB;
        }

        private void NpcTypeButton_MouseLeave(object sender, EventArgs e)
        {
            this.NpcTypeButton.Image = Resources.blackB;
        }

        private void SearchObjectButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.SearchObjectButton.Enabled = false;
                string str = "";
                if (InputBox(this.SearchObjectButton.Text, this.searchInputBox, ref str) != DialogResult.OK)
                {
                    this.SearchObjectButton.Enabled = true;
                    return;
                }
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)
                {
                    this.SearchObjectButton.Enabled = true;
                    return;
                }
                if (string.IsNullOrEmpty(str))
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Name_Empty));
                    this.SearchObjectButton.Enabled = true;
                    return;
                }
                Npc npc = new Npc();
                List<WoWGameObject> woWGameObjectByName = nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByName(str);
                if (woWGameObjectByName.Count > 0)
                {
                    WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(woWGameObjectByName, false);
                    if (nearestWoWGameObject.IsValid)
                    {
                        npc.Entry = nearestWoWGameObject.Entry;
                        npc.Position = nearestWoWGameObject.Position;
                        npc.Name = nearestWoWGameObject.Name;
                        npc.Faction = UnitRelation.GetObjectRacialFaction(nearestWoWGameObject.Faction);
                    }
                }
                if (npc.Entry <= 0)
                {
                    List<WoWUnit> woWUnitByName = nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByName(str);
                    if (woWUnitByName.Count > 0)
                    {
                        WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(woWUnitByName, false, false, false);
                        if (unit.IsValid)
                        {
                            npc.Entry = unit.Entry;
                            npc.Position = unit.Position;
                            npc.Name = unit.Name;
                            npc.Faction = UnitRelation.GetObjectRacialFaction(unit.Faction);
                        }
                    }
                }
                if (npc.Entry <= 0)
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NPCNotFound));
                    this.SearchObjectButton.Enabled = true;
                    return;
                }
                npc.ContinentIdInt = Usefuls.ContinentId;
                npc.Faction = (Npc.FactionType) Enum.Parse(typeof(Npc.FactionType), nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction, true);
                if (Usefuls.IsOutdoors)
                {
                    npc.Position.Type = "Flying";
                }
                this.InformationArea.Text = string.Concat(new object[] { 
                    "  <Npc>", Environment.NewLine, "    <Entry>", npc.Entry, "</Entry>", Environment.NewLine, "    <Name>", npc.Name, "</Name>", Environment.NewLine, "    <Position>", Environment.NewLine, "      <X>", npc.Position.X, "</X>", Environment.NewLine, 
                    "      <Y>", npc.Position.Y, "</Y>", Environment.NewLine, "      <Z>", npc.Position.Z, "</Z>", Environment.NewLine, "      <Type>", npc.Position.Type, "</Type>", Environment.NewLine, "    </Position>", Environment.NewLine, "    <Faction>", (Npc.FactionType) Enum.Parse(typeof(Npc.FactionType), nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction, true), 
                    "</Faction>", Environment.NewLine, "    <Type>None</Type>", Environment.NewLine, "    <ContinentId>", npc.ContinentId, "</ContinentId>", Environment.NewLine, "  </Npc>"
                 });
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > SearchObjectButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this.SearchObjectButton.Enabled = true;
        }

        private void SearchObjectButton_MouseEnter(object sender, EventArgs e)
        {
            this.SearchObjectButton.Image = Resources.greenB;
        }

        private void SearchObjectButton_MouseLeave(object sender, EventArgs e)
        {
            this.SearchObjectButton.Image = Resources.blackB;
        }

        private void TargetInfo2Button_Click(object sender, EventArgs e)
        {
            try
            {
                this.TargetInfo2Button.Enabled = false;
                this.InformationArea.Text = "";
                if (nManager.Wow.ObjectManager.ObjectManager.Target.IsValid)
                {
                    string str = "";
                    if (nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.QuestGiver))
                    {
                        UnitQuestGiverStatus status = (UnitQuestGiverStatus) Memory.WowMemory.Memory.ReadInt(nManager.Wow.ObjectManager.ObjectManager.Target.GetBaseAddress + 0xf4);
                        if (status > UnitQuestGiverStatus.None)
                        {
                            str = "QuestGiverStatus: " + status + Environment.NewLine;
                        }
                    }
                    this.InformationArea.Text = string.Concat(new object[] { 
                        "Name: ", nManager.Wow.ObjectManager.ObjectManager.Target.Name, Environment.NewLine, "BaseAddress: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetBaseAddress, Environment.NewLine, "Entry: ", nManager.Wow.ObjectManager.ObjectManager.Target.Entry, Environment.NewLine, "Position: ", nManager.Wow.ObjectManager.ObjectManager.Target.Position, Environment.NewLine, "Faction: ", (Npc.FactionType) Enum.Parse(typeof(Npc.FactionType), nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction, true), Environment.NewLine, "ContinentId: ", 
                        Usefuls.ContinentNameByContinentId(Usefuls.ContinentId), " (", Usefuls.ContinentId, ")", Environment.NewLine, "IsDead : ", nManager.Wow.ObjectManager.ObjectManager.Target.IsDead, Environment.NewLine, "IsTrivial : ", nManager.Wow.ObjectManager.ObjectManager.Target.IsTrivial, Environment.NewLine, "UnitFlag: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags), Environment.NewLine, "UnitFlag2: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitFlags2>(Descriptors.UnitFields.Flags2), 
                        Environment.NewLine, "NPCFlag: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags), Environment.NewLine, str, "DynamicFlag: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags), Environment.NewLine
                     });
                    if (nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Taxi))
                    {
                        this.InformationArea.Text = this.InformationArea.Text + "If you have the TaxiWindow opened while requesting those informations, TaxiNodes will be dumped to DebugLog" + Environment.NewLine;
                        Gossip.ExportTaxiInfo();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > TargetInfo2Button_Click(object sender, EventArgs e): " + exception, true);
            }
            this.TargetInfo2Button.Enabled = true;
        }

        private void TargetInfo2Button_MouseEnter(object sender, EventArgs e)
        {
            this.TargetInfo2Button.Image = Resources.greenB;
        }

        private void TargetInfo2Button_MouseLeave(object sender, EventArgs e)
        {
            this.TargetInfo2Button.Image = Resources.blackB;
        }

        private void TargetInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.TargetInfoButton.Enabled = false;
                this.InformationArea.Text = "";
                if (!nManager.Wow.ObjectManager.ObjectManager.Target.IsValid)
                {
                    this.TargetInfoButton.Enabled = true;
                    return;
                }
                nManager.Wow.Class.Point position = nManager.Wow.ObjectManager.ObjectManager.Target.Position;
                if (Usefuls.IsOutdoors)
                {
                    position.Type = "Flying";
                }
                this.InformationArea.Text = string.Concat(new object[] { 
                    "  <Npc>", Environment.NewLine, "    <Entry>", nManager.Wow.ObjectManager.ObjectManager.Target.Entry, "</Entry>", Environment.NewLine, "    <Name>", nManager.Wow.ObjectManager.ObjectManager.Target.Name, "</Name>", Environment.NewLine, "    <Position>", Environment.NewLine, "      <X>", position.X, "</X>", Environment.NewLine, 
                    "      <Y>", position.Y, "</Y>", Environment.NewLine, "      <Z>", position.Z, "</Z>", Environment.NewLine, "      <Type>", position.Type, "</Type>", Environment.NewLine, "    </Position>", Environment.NewLine, "    <Faction>", (Npc.FactionType) Enum.Parse(typeof(Npc.FactionType), nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction, true), 
                    "</Faction>", Environment.NewLine, "    <Type>None</Type>", Environment.NewLine, "    <ContinentId>", Usefuls.ContinentNameByContinentId(Usefuls.ContinentId), "</ContinentId>", Environment.NewLine, "  </Npc>", Environment.NewLine, Environment.NewLine, "Distance: ", position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position)
                 });
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > TargetInfoButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this.TargetInfoButton.Enabled = true;
        }

        private void TargetInfoButton_MouseEnter(object sender, EventArgs e)
        {
            this.TargetInfoButton.Image = Resources.greenB;
        }

        private void TargetInfoButton_MouseLeave(object sender, EventArgs e)
        {
            this.TargetInfoButton.Image = Resources.blackB;
        }

        private void Translate()
        {
            this.MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Developer_Tools) + " - " + Information.MainTitle;
            this.Text = this.MainHeader.TitleText;
            this.LuaExecButton.Text = nManager.Translate.Get(nManager.Translate.Id.LuaExecButton);
            this.GpsButton.Text = nManager.Translate.Get(nManager.Translate.Id.GpsButton);
            this.TargetInfoButton.Text = nManager.Translate.Get(nManager.Translate.Id.TargetInfoButton);
            this.TargetInfo2Button.Text = nManager.Translate.Get(nManager.Translate.Id.TargetInfo2Button);
            this.TranslationManagerButton.Text = nManager.Translate.Get(nManager.Translate.Id.TranslationManagerButton);
            this.CsharpExecButton.Text = nManager.Translate.Get(nManager.Translate.Id.CsharpExecButton);
            this.NpcTypeButton.Text = nManager.Translate.Get(nManager.Translate.Id.NpcTypeButton);
            this.NpcFactionButton.Text = nManager.Translate.Get(nManager.Translate.Id.NpcFactionButton);
            this.SearchObjectButton.Text = nManager.Translate.Get(nManager.Translate.Id.SearchObjectButton);
            this.AllObjectsButton.Text = nManager.Translate.Get(nManager.Translate.Id.AllObjectsButton);
            this.searchInputBox = nManager.Translate.Get(nManager.Translate.Id.SearchObjectBox);
        }

        private void TranslationManagerButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.TranslationManagerButton.Enabled = false;
                new TranslationManagementMainFrame().ShowDialog();
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > SearchObjectButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this.TranslationManagerButton.Enabled = true;
        }

        private void TranslationManagerButton_MouseEnter(object sender, EventArgs e)
        {
            this.TranslationManagerButton.Image = Resources.greenB_242;
        }

        private void TranslationManagerButton_MouseLeave(object sender, EventArgs e)
        {
            this.TranslationManagerButton.Image = Resources.blueB_242;
        }
    }
}

