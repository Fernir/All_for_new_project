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
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DeveloperToolsMainFrame : Form
    {
        private Label _aniehoBaqul;
        private Label _ataotVia;
        private Label _axuhilatu;
        private RichTextBox _babio;
        private Label _igaharo;
        private Label _ixeijoume;
        private IContainer _koiduferaluOwiakio;
        private Label _leuloli;
        private Label _noucuonoifu;
        private string _ucoperaovev = "Type in the name of the WoWObject you are looking for:";
        private Label _ukeabutoAfa;
        private Label _uvociJous;
        private Label _xauxe;
        private TnbControlMenu _xuwoagPo;

        public DeveloperToolsMainFrame()
        {
            try
            {
                this.Utaeriopasa();
                this.GeuxecAtajioxoa();
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

        [CompilerGenerated]
        private static bool <ExecuteScript>b__1(Assembly a)
        {
            return (!a.IsDynamic && !a.CodeBase.Contains(Process.GetCurrentProcess().ProcessName + ".exe"));
        }

        [CompilerGenerated]
        private static string <ExecuteScript>b__2(Assembly a)
        {
            return a.Location;
        }

        [CompilerGenerated]
        private static string <ExecuteScript>b__3(string current, CompilerError err)
        {
            return (current + err + "\n");
        }

        private void Ajagajina(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                this._uvociJous.Enabled = false;
                string str = "";
                if (InputBox(this._uvociJous.Text, this._ucoperaovev, ref str) != DialogResult.OK)
                {
                    this._uvociJous.Enabled = true;
                    return;
                }
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)
                {
                    this._uvociJous.Enabled = true;
                    return;
                }
                if (string.IsNullOrEmpty(str))
                {
                    MessageBox.Show(Translate.Get(Translate.Id.Name_Empty));
                    this._uvociJous.Enabled = true;
                    return;
                }
                Npc npc = new Npc();
                List<WoWGameObject> woWGameObjectByName = nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByName(str);
                if (woWGameObjectByName.Count > 0)
                {
                    WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(woWGameObjectByName, true);
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
                        WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(woWUnitByName, true, true, true);
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
                    MessageBox.Show(Translate.Get(Translate.Id.NPCNotFound));
                    this._uvociJous.Enabled = true;
                    return;
                }
                npc.ContinentIdInt = Usefuls.ContinentId;
                npc.Faction = (Npc.FactionType) Enum.Parse(typeof(Npc.FactionType), nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction, true);
                if (Usefuls.IsOutdoors)
                {
                    npc.Position.Type = "Flying";
                }
                this._babio.Text = string.Concat(new object[] { 
                    "  <Npc>", Environment.NewLine, "    <Entry>", npc.Entry, "</Entry>", Environment.NewLine, "    <Name>", npc.Name, "</Name>", Environment.NewLine, "    <Position>", Environment.NewLine, "      <X>", npc.Position.X, "</X>", Environment.NewLine, 
                    "      <Y>", npc.Position.Y, "</Y>", Environment.NewLine, "      <Z>", npc.Position.Z, "</Z>", Environment.NewLine, "      <Type>", npc.Position.Type, "</Type>", Environment.NewLine, "    </Position>", Environment.NewLine, "    <Faction>", (Npc.FactionType) Enum.Parse(typeof(Npc.FactionType), nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction, true), 
                    "</Faction>", Environment.NewLine, "    <Type>None</Type>", Environment.NewLine, "    <ContinentId>", npc.ContinentId, "</ContinentId>", Environment.NewLine, "  </Npc>"
                 });
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > SearchObjectButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this._uvociJous.Enabled = true;
        }

        private void Beawunegoubi(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._leuloli.Image = Resources.blueB_150;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this._koiduferaluOwiakio != null))
            {
                this._koiduferaluOwiakio.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EgefeubiorehRavoeme(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._noucuonoifu.Image = Resources.blackB;
        }

        private void Esounap(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._axuhilatu.Image = Resources.blackB;
        }

        private void Fojovu(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                this._axuhilatu.Enabled = false;
                this._babio.Text = "";
                if (!nManager.Wow.ObjectManager.ObjectManager.Target.IsValid)
                {
                    this._axuhilatu.Enabled = true;
                    return;
                }
                nManager.Wow.Class.Point position = nManager.Wow.ObjectManager.ObjectManager.Target.Position;
                if (Usefuls.IsOutdoors)
                {
                    position.Type = "Flying";
                }
                this._babio.Text = string.Concat(new object[] { 
                    "  <Npc>", Environment.NewLine, "    <Entry>", nManager.Wow.ObjectManager.ObjectManager.Target.Entry, "</Entry>", Environment.NewLine, "    <Name>", nManager.Wow.ObjectManager.ObjectManager.Target.Name, "</Name>", Environment.NewLine, "    <Position>", Environment.NewLine, "      <X>", position.X, "</X>", Environment.NewLine, 
                    "      <Y>", position.Y, "</Y>", Environment.NewLine, "      <Z>", position.Z, "</Z>", Environment.NewLine, "      <Type>", position.Type, "</Type>", Environment.NewLine, "    </Position>", Environment.NewLine, "    <Faction>", (Npc.FactionType) Enum.Parse(typeof(Npc.FactionType), nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction, true), 
                    "</Faction>", Environment.NewLine, "    <Type>None</Type>", Environment.NewLine, "    <ContinentId>", Usefuls.ContinentNameByContinentId(Usefuls.ContinentId), "</ContinentId>", Environment.NewLine, "  </Npc>", Environment.NewLine, Environment.NewLine, "Distance: ", position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position), Environment.NewLine, Environment.NewLine, "Distance2D: ", 
                    position.DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position)
                 });
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > TargetInfoButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this._axuhilatu.Enabled = true;
        }

        private void GeuxecAtajioxoa()
        {
            this._xuwoagPo.TitleText = Translate.Get(Translate.Id.Developer_Tools) + " - " + Information.MainTitle;
            this.Text = this._xuwoagPo.TitleText;
            this._leuloli.Text = Translate.Get(Translate.Id.LuaExecButton);
            this._aniehoBaqul.Text = Translate.Get(Translate.Id.GpsButton);
            this._axuhilatu.Text = Translate.Get(Translate.Id.TargetInfoButton);
            this._noucuonoifu.Text = Translate.Get(Translate.Id.TargetInfo2Button);
            this._xauxe.Text = Translate.Get(Translate.Id.TranslationManagerButton);
            this._ixeijoume.Text = Translate.Get(Translate.Id.CsharpExecButton);
            this._igaharo.Text = Translate.Get(Translate.Id.NpcTypeButton);
            this._ukeabutoAfa.Text = Translate.Get(Translate.Id.NpcFactionButton);
            this._uvociJous.Text = Translate.Get(Translate.Id.SearchObjectButton);
            this._ataotVia.Text = Translate.Get(Translate.Id.AllObjectsButton);
            this._ucoperaovev = Translate.Get(Translate.Id.SearchObjectBox);
        }

        private void Giegeub(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._ixeijoume.Image = Resources.greenB_150;
        }

        private void Ijiufe(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                this._aniehoBaqul.Enabled = false;
                this._babio.Text = "";
                if (nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)
                {
                    string str = string.Concat(new object[] { "Fake Map name: ", Usefuls.ContinentNameMpqByContinentId(Usefuls.ContinentId), " (", Usefuls.ContinentId, ")" });
                    this._babio.Text = string.Concat(new object[] { 
                        "Internal Map name: ", Usefuls.ContinentNameMpqByContinentId(Usefuls.RealContinentId), " (", Usefuls.RealContinentId, ")", Environment.NewLine, (Usefuls.ContinentId != Usefuls.RealContinentId) ? (str + Environment.NewLine) : "", nManager.Wow.ObjectManager.ObjectManager.Me.Position, Environment.NewLine, Environment.NewLine, Environment.NewLine, "<Position>", Environment.NewLine, " <X>", nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, "</X>", 
                        Environment.NewLine, " <Y>", nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, "</Y>", Environment.NewLine, " <Z>", nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, "</Z>", Environment.NewLine, " <Type>", nManager.Wow.ObjectManager.ObjectManager.Me.Position.Type, "</Type>", Environment.NewLine, "</Position>"
                     });
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > GpsButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this._aniehoBaqul.Enabled = true;
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

        private void MaewaAjEbauh(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._ataotVia.Image = Resources.blackB_242;
        }

        private void NanauGoi(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                this._noucuonoifu.Enabled = false;
                this._babio.Text = "";
                if (nManager.Wow.ObjectManager.ObjectManager.Target.IsValid)
                {
                    string str = "";
                    if (nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Taxi) && (nManager.Wow.ObjectManager.ObjectManager.Target.UnitFlightMasteStatus != UnitFlightMasterStatus.None))
                    {
                        str = "Flight Master Status: " + nManager.Wow.ObjectManager.ObjectManager.Target.UnitFlightMasteStatus + Environment.NewLine;
                    }
                    string str2 = "";
                    if (nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.QuestGiver))
                    {
                        QuestGiverStatus status = (QuestGiverStatus) Memory.WowMemory.Memory.ReadInt(nManager.Wow.ObjectManager.ObjectManager.Target.GetBaseAddress + 0xec);
                        if (status > QuestGiverStatus.None)
                        {
                            str2 = "Quest Giver Status: " + status + Environment.NewLine;
                        }
                    }
                    this._babio.Text = string.Concat(new object[] { 
                        "Name: ", nManager.Wow.ObjectManager.ObjectManager.Target.Name, Environment.NewLine, "BaseAddress: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetBaseAddress, Environment.NewLine, "Entry: ", nManager.Wow.ObjectManager.ObjectManager.Target.Entry, Environment.NewLine, "Position: ", nManager.Wow.ObjectManager.ObjectManager.Target.Position, Environment.NewLine, "Faction: ", (Npc.FactionType) Enum.Parse(typeof(Npc.FactionType), nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction, true), Environment.NewLine, "ContinentId: ", 
                        Usefuls.ContinentNameByContinentId(Usefuls.ContinentId), " (", Usefuls.ContinentId, ")", Environment.NewLine, "IsDead : ", nManager.Wow.ObjectManager.ObjectManager.Target.IsDead, Environment.NewLine, "IsTrivial : ", nManager.Wow.ObjectManager.ObjectManager.Target.IsTrivial, Environment.NewLine, "UnitClassification : ", nManager.Wow.ObjectManager.ObjectManager.Target.UnitClassification, Environment.NewLine, "UnitFlag: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags), 
                        Environment.NewLine, "UnitFlag2: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitFlags2>(Descriptors.UnitFields.Flags2), Environment.NewLine, "UnitFlag3: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitFlags3>(Descriptors.UnitFields.Flags3), Environment.NewLine, "StateAnimID: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<StateAnimID>(Descriptors.UnitFields.StateAnimID), Environment.NewLine, "NPCFlag: ", nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags), Environment.NewLine, str2, str, "MovementStatus: ", 
                        nManager.Wow.ObjectManager.ObjectManager.Target.MovementStatus, Environment.NewLine, "DynamicFlag: ", nManager.Wow.ObjectManager.ObjectManager.Target.Tuecom<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags), Environment.NewLine, "IsBlacklisted: ", nManagerSetting.IsBlackListed(nManager.Wow.ObjectManager.ObjectManager.Target.Guid), Environment.NewLine, "IsInBlacklistedArea: ", nManagerSetting.IsBlackListedZone(nManager.Wow.ObjectManager.ObjectManager.Target.Position), Environment.NewLine
                     });
                    if (nManager.Wow.ObjectManager.ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Taxi))
                    {
                        this._babio.Text = this._babio.Text + "If you have the TaxiWindow opened while requesting those informations, TaxiNodes will be dumped to DebugLog" + Environment.NewLine;
                        Gossip.ExportTaxiInfo();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > TargetInfo2Button_Click(object sender, EventArgs e): " + exception, true);
            }
            this._noucuonoifu.Enabled = true;
        }

        private void Narotaonico(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._ixeijoume.Image = Resources.blueB_150;
        }

        private void Neojijofomuik(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._aniehoBaqul.Image = Resources.blackB;
        }

        private void NitiurUsiqetu(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._noucuonoifu.Image = Resources.greenB;
        }

        private void NunaumiujuEmeufosom(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._igaharo.Image = Resources.blackB;
        }

        private void Nutuamea(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._igaharo.Image = Resources.greenB;
        }

        private void Omenoure(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._xauxe.Image = Resources.greenB_242;
        }

        private void Otuigof(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._aniehoBaqul.Image = Resources.greenB;
        }

        private void OvofoahoIwi(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._uvociJous.Image = Resources.blackB;
        }

        private void OvuwiuxevifOb(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                this._igaharo.Enabled = false;
                this._babio.Text = "";
                foreach (Npc.NpcType type in Enum.GetValues(typeof(Npc.NpcType)).Cast<Npc.NpcType>().ToList<Npc.NpcType>())
                {
                    this._babio.Text = this._babio.Text + type + Environment.NewLine;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > NpcTypeButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this._igaharo.Enabled = true;
        }

        private void Oxiagu(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._ataotVia.Image = Resources.greenB_242;
        }

        private void RouvejiotibIpea(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._axuhilatu.Image = Resources.greenB;
        }

        private void SobaevOqiexaec(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._xauxe.Image = Resources.blueB_242;
        }

        private void Tadir(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                this._xauxe.Enabled = false;
                new TranslationManagementMainFrame().ShowDialog();
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > SearchObjectButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this._xauxe.Enabled = true;
        }

        private void Ucaeboegoex(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._ukeabutoAfa.Image = Resources.greenB;
        }

        private void UnowesoinalLoa(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                this._leuloli.Enabled = false;
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"1\")", false, true);
                Lua.LuaDoString(this._babio.Text, !Usefuls.InGame, true);
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"0\")", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > LuaExecButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this._leuloli.Enabled = true;
        }

        private void Upauv(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                this._ukeabutoAfa.Enabled = false;
                this._babio.Text = "";
                foreach (Npc.FactionType type in Enum.GetValues(typeof(Npc.FactionType)).Cast<Npc.FactionType>().ToList<Npc.FactionType>())
                {
                    this._babio.Text = this._babio.Text + type + Environment.NewLine;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > NpcFactionButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this._ukeabutoAfa.Enabled = true;
        }

        internal void Urituhi()
        {
            try
            {
                CodeDomProvider provider = new CSharpCodeProvider();
                CompilerParameters options = new CompilerParameters();
                if (CS$<>9__CachedAnonymousMethodDelegate4 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate4 = new Func<Assembly, bool>(DeveloperToolsMainFrame.<ExecuteScript>b__1);
                }
                if (CS$<>9__CachedAnonymousMethodDelegate5 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate5 = new Func<Assembly, string>(DeveloperToolsMainFrame.<ExecuteScript>b__2);
                }
                IEnumerable<string> source = AppDomain.CurrentDomain.GetAssemblies().Where<Assembly>(CS$<>9__CachedAnonymousMethodDelegate4).Select<Assembly, string>(CS$<>9__CachedAnonymousMethodDelegate5);
                options.ReferencedAssemblies.AddRange(source.ToArray<string>());
                options.ReferencedAssemblies.Add("nManager.dll");
                CompilerResults results = provider.CompileAssemblyFromSource(options, new string[] { "using System.Linq;" + Environment.NewLine + "using System;" + Environment.NewLine + "using System.Reflection;" + Environment.NewLine + "using System.Threading;" + Environment.NewLine + "using System.Windows.Forms;" + Environment.NewLine + "using System.Diagnostics; " + Environment.NewLine + "using nManager.Helpful;" + Environment.NewLine + "using nManager.Wow;" + Environment.NewLine + "using nManager.Wow.Bot.States;" + Environment.NewLine + "using nManager.Wow.Bot.Tasks;" + Environment.NewLine + "using nManager.Wow.Class;" + Environment.NewLine + "using nManager.Wow.Enums;" + Environment.NewLine + "using nManager.Wow.Helpers;" + Environment.NewLine + "using nManager.Wow.ObjectManager;" + Environment.NewLine + "using Timer = nManager.Helpful.Timer;" + Environment.NewLine + "public class Main : nManager.Helpful.Interface.IScriptOnlineManager { public void Initialize() { " + this._babio.Text + " } } " });
                if (results.Errors.HasErrors)
                {
                    if (CS$<>9__CachedAnonymousMethodDelegate6 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate6 = new Func<string, CompilerError, string>(DeveloperToolsMainFrame.<ExecuteScript>b__3);
                    }
                    MessageBox.Show(results.Errors.Cast<CompilerError>().Aggregate<CompilerError, string>("Compilator Error :\n", CS$<>9__CachedAnonymousMethodDelegate6));
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

        private void Utaeriopasa()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DeveloperToolsMainFrame));
            this._uvociJous = new Label();
            this._axuhilatu = new Label();
            this._babio = new RichTextBox();
            this._aniehoBaqul = new Label();
            this._ixeijoume = new Label();
            this._noucuonoifu = new Label();
            this._ukeabutoAfa = new Label();
            this._igaharo = new Label();
            this._leuloli = new Label();
            this._xauxe = new Label();
            this._ataotVia = new Label();
            this._xuwoagPo = new TnbControlMenu();
            base.SuspendLayout();
            this._uvociJous.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._uvociJous.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._uvociJous.ForeColor = Color.White;
            this._uvociJous.Image = Resources.blackB;
            this._uvociJous.Location = new System.Drawing.Point(0x19d, 0x22e);
            this._uvociJous.Margin = new Padding(0);
            this._uvociJous.MaximumSize = new Size(0x6a, 0x1d);
            this._uvociJous.MinimumSize = new Size(0x6a, 0x1d);
            this._uvociJous.Name = "SearchObjectButton";
            this._uvociJous.Size = new Size(0x6a, 0x1d);
            this._uvociJous.TabIndex = 14;
            this._uvociJous.Text = "Search Object";
            this._uvociJous.TextAlign = ContentAlignment.MiddleCenter;
            this._uvociJous.Click += new EventHandler(this.Ajagajina);
            this._uvociJous.MouseEnter += new EventHandler(this.Uwenoasau);
            this._uvociJous.MouseLeave += new EventHandler(this.OvofoahoIwi);
            this._axuhilatu.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._axuhilatu.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._axuhilatu.ForeColor = Color.White;
            this._axuhilatu.Image = Resources.blackB;
            this._axuhilatu.Location = new System.Drawing.Point(0x12e, 0x205);
            this._axuhilatu.Margin = new Padding(0);
            this._axuhilatu.MaximumSize = new Size(0x6a, 0x1d);
            this._axuhilatu.MinimumSize = new Size(0x6a, 0x1d);
            this._axuhilatu.Name = "TargetInfoButton";
            this._axuhilatu.Size = new Size(0x6a, 0x1d);
            this._axuhilatu.TabIndex = 15;
            this._axuhilatu.Text = "Target's information";
            this._axuhilatu.TextAlign = ContentAlignment.MiddleCenter;
            this._axuhilatu.Click += new EventHandler(this.Fojovu);
            this._axuhilatu.MouseEnter += new EventHandler(this.RouvejiotibIpea);
            this._axuhilatu.MouseLeave += new EventHandler(this.Esounap);
            this._babio.BackColor = Color.FromArgb(250, 250, 250);
            this._babio.BorderStyle = BorderStyle.None;
            this._babio.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this._babio.Location = new System.Drawing.Point(0x23, 0x45);
            this._babio.Margin = new Padding(0);
            this._babio.MaximumSize = new Size(730, 0x1b3);
            this._babio.MinimumSize = new Size(730, 0x1b3);
            this._babio.Multiline = true;
            this._babio.Name = "InformationArea";
            this._babio.Size = new Size(730, 0x1b3);
            this._babio.TabIndex = 0x15;
            this._aniehoBaqul.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._aniehoBaqul.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._aniehoBaqul.ForeColor = Color.White;
            this._aniehoBaqul.Image = Resources.blackB;
            this._aniehoBaqul.Location = new System.Drawing.Point(190, 0x205);
            this._aniehoBaqul.Margin = new Padding(0);
            this._aniehoBaqul.MaximumSize = new Size(0x6a, 0x1d);
            this._aniehoBaqul.MinimumSize = new Size(0x6a, 0x1d);
            this._aniehoBaqul.Name = "GpsButton";
            this._aniehoBaqul.Size = new Size(0x6a, 0x1d);
            this._aniehoBaqul.TabIndex = 0x17;
            this._aniehoBaqul.Text = "GPS infos";
            this._aniehoBaqul.TextAlign = ContentAlignment.MiddleCenter;
            this._aniehoBaqul.Click += new EventHandler(this.Ijiufe);
            this._aniehoBaqul.MouseEnter += new EventHandler(this.Otuigof);
            this._aniehoBaqul.MouseLeave += new EventHandler(this.Neojijofomuik);
            this._ixeijoume.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._ixeijoume.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._ixeijoume.ForeColor = Color.White;
            this._ixeijoume.Image = Resources.blueB_150;
            this._ixeijoume.Location = new System.Drawing.Point(0x22, 0x22e);
            this._ixeijoume.Margin = new Padding(0);
            this._ixeijoume.MaximumSize = new Size(150, 0x1d);
            this._ixeijoume.MinimumSize = new Size(150, 0x1d);
            this._ixeijoume.Name = "CsharpExecButton";
            this._ixeijoume.Size = new Size(150, 0x1d);
            this._ixeijoume.TabIndex = 0x18;
            this._ixeijoume.Text = "Execute C# code";
            this._ixeijoume.TextAlign = ContentAlignment.MiddleCenter;
            this._ixeijoume.Click += new EventHandler(this.Wiacaopeutala);
            this._ixeijoume.MouseEnter += new EventHandler(this.Giegeub);
            this._ixeijoume.MouseLeave += new EventHandler(this.Narotaonico);
            this._noucuonoifu.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._noucuonoifu.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._noucuonoifu.ForeColor = Color.White;
            this._noucuonoifu.Image = Resources.blackB;
            this._noucuonoifu.Location = new System.Drawing.Point(0x19d, 0x205);
            this._noucuonoifu.Margin = new Padding(0);
            this._noucuonoifu.MaximumSize = new Size(0x6a, 0x1d);
            this._noucuonoifu.MinimumSize = new Size(0x6a, 0x1d);
            this._noucuonoifu.Name = "TargetInfo2Button";
            this._noucuonoifu.Size = new Size(0x6a, 0x1d);
            this._noucuonoifu.TabIndex = 0x19;
            this._noucuonoifu.Text = "Target's information 2";
            this._noucuonoifu.TextAlign = ContentAlignment.MiddleCenter;
            this._noucuonoifu.Click += new EventHandler(this.NanauGoi);
            this._noucuonoifu.MouseEnter += new EventHandler(this.NitiurUsiqetu);
            this._noucuonoifu.MouseLeave += new EventHandler(this.EgefeubiorehRavoeme);
            this._ukeabutoAfa.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._ukeabutoAfa.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._ukeabutoAfa.ForeColor = Color.White;
            this._ukeabutoAfa.Image = Resources.blackB;
            this._ukeabutoAfa.Location = new System.Drawing.Point(0x12e, 0x22e);
            this._ukeabutoAfa.Margin = new Padding(0);
            this._ukeabutoAfa.MaximumSize = new Size(0x6a, 0x1d);
            this._ukeabutoAfa.MinimumSize = new Size(0x6a, 0x1d);
            this._ukeabutoAfa.Name = "NpcFactionButton";
            this._ukeabutoAfa.Size = new Size(0x6a, 0x1d);
            this._ukeabutoAfa.TabIndex = 0x1a;
            this._ukeabutoAfa.Text = "NPC Faction List";
            this._ukeabutoAfa.TextAlign = ContentAlignment.MiddleCenter;
            this._ukeabutoAfa.Click += new EventHandler(this.Upauv);
            this._ukeabutoAfa.MouseEnter += new EventHandler(this.Ucaeboegoex);
            this._ukeabutoAfa.MouseLeave += new EventHandler(this.Woceufeob);
            this._igaharo.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._igaharo.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._igaharo.ForeColor = Color.White;
            this._igaharo.Image = Resources.blackB;
            this._igaharo.Location = new System.Drawing.Point(190, 0x22e);
            this._igaharo.Margin = new Padding(0);
            this._igaharo.MaximumSize = new Size(0x6a, 0x1d);
            this._igaharo.MinimumSize = new Size(0x6a, 0x1d);
            this._igaharo.Name = "NpcTypeButton";
            this._igaharo.Size = new Size(0x6a, 0x1d);
            this._igaharo.TabIndex = 0x1b;
            this._igaharo.Text = "NPC Type List";
            this._igaharo.TextAlign = ContentAlignment.MiddleCenter;
            this._igaharo.Click += new EventHandler(this.OvuwiuxevifOb);
            this._igaharo.MouseEnter += new EventHandler(this.Nutuamea);
            this._igaharo.MouseLeave += new EventHandler(this.NunaumiujuEmeufosom);
            this._leuloli.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._leuloli.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._leuloli.ForeColor = Color.White;
            this._leuloli.Image = Resources.blueB_150;
            this._leuloli.Location = new System.Drawing.Point(0x22, 0x205);
            this._leuloli.Margin = new Padding(0);
            this._leuloli.MaximumSize = new Size(150, 0x1d);
            this._leuloli.MinimumSize = new Size(150, 0x1d);
            this._leuloli.Name = "LuaExecButton";
            this._leuloli.Size = new Size(150, 0x1d);
            this._leuloli.TabIndex = 0x1d;
            this._leuloli.Text = "Execute LUA code";
            this._leuloli.TextAlign = ContentAlignment.MiddleCenter;
            this._leuloli.Click += new EventHandler(this.UnowesoinalLoa);
            this._leuloli.MouseEnter += new EventHandler(this.Uwaemauhubia);
            this._leuloli.MouseLeave += new EventHandler(this.Beawunegoubi);
            this._xauxe.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._xauxe.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._xauxe.ForeColor = Color.White;
            this._xauxe.Image = Resources.blueB_242;
            this._xauxe.Location = new System.Drawing.Point(0x20d, 0x205);
            this._xauxe.Margin = new Padding(0);
            this._xauxe.MaximumSize = new Size(0xf2, 0x1d);
            this._xauxe.MinimumSize = new Size(0xf2, 0x1d);
            this._xauxe.Name = "TranslationManagerButton";
            this._xauxe.Size = new Size(0xf2, 0x1d);
            this._xauxe.TabIndex = 0x20;
            this._xauxe.Text = "Open Translation Manager";
            this._xauxe.TextAlign = ContentAlignment.MiddleCenter;
            this._xauxe.Click += new EventHandler(this.Tadir);
            this._xauxe.MouseEnter += new EventHandler(this.Omenoure);
            this._xauxe.MouseLeave += new EventHandler(this.SobaevOqiexaec);
            this._ataotVia.BackColor = Color.FromArgb(0xe8, 0xe8, 0xe8);
            this._ataotVia.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this._ataotVia.ForeColor = Color.White;
            this._ataotVia.Image = Resources.blackB_242;
            this._ataotVia.Location = new System.Drawing.Point(0x20d, 0x22e);
            this._ataotVia.Margin = new Padding(0);
            this._ataotVia.MaximumSize = new Size(0xf2, 0x1d);
            this._ataotVia.MinimumSize = new Size(0xf2, 0x1d);
            this._ataotVia.Name = "AllObjectsButton";
            this._ataotVia.Size = new Size(0xf2, 0x1d);
            this._ataotVia.TabIndex = 0x21;
            this._ataotVia.Text = "Get all ingame objects information";
            this._ataotVia.TextAlign = ContentAlignment.MiddleCenter;
            this._ataotVia.Click += new EventHandler(this.XufiqocolilipiVuge);
            this._ataotVia.MouseEnter += new EventHandler(this.Oxiagu);
            this._ataotVia.MouseLeave += new EventHandler(this.MaewaAjEbauh);
            this._xuwoagPo.BackgroundImage = Resources._800x43_controlbar;
            this._xuwoagPo.Location = new System.Drawing.Point(0, 0);
            this._xuwoagPo.LogoImage = (Image) manager.GetObject("MainHeader.LogoImage");
            this._xuwoagPo.Name = "MainHeader";
            this._xuwoagPo.Size = new Size(800, 0x2b);
            this._xuwoagPo.TabIndex = 0x22;
            this._xuwoagPo.TitleFont = new Font("Microsoft Sans Serif", 12f);
            this._xuwoagPo.TitleForeColor = Color.FromArgb(0xde, 0xde, 0xde);
            this._xuwoagPo.TitleText = "Developer Tools - TheNoobBot - DevVersionRestrict";
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = SystemColors.ActiveCaptionText;
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.ClientSize = new Size(800, 600);
            base.Controls.Add(this._xuwoagPo);
            base.Controls.Add(this._ataotVia);
            base.Controls.Add(this._xauxe);
            base.Controls.Add(this._leuloli);
            base.Controls.Add(this._igaharo);
            base.Controls.Add(this._ukeabutoAfa);
            base.Controls.Add(this._noucuonoifu);
            base.Controls.Add(this._ixeijoume);
            base.Controls.Add(this._aniehoBaqul);
            base.Controls.Add(this._babio);
            base.Controls.Add(this._axuhilatu);
            base.Controls.Add(this._uvociJous);
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

        private void Uwaemauhubia(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._leuloli.Image = Resources.greenB_150;
        }

        private void Uwenoasau(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._uvociJous.Image = Resources.greenB;
        }

        private void Wiacaopeutala(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                this._ixeijoume.Enabled = false;
                this.Urituhi();
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeveloperToolsMainFrame > CsharpExecButton_Click(object sender, EventArgs e): " + exception, true);
            }
            this._ixeijoume.Enabled = true;
        }

        private void Woceufeob(object moleileucucisUgofe, EventArgs deisiko)
        {
            this._ukeabutoAfa.Image = Resources.blackB;
        }

        private void XufiqocolilipiVuge(object moleileucucisUgofe, EventArgs deisiko)
        {
            try
            {
                this._ataotVia.Enabled = false;
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
                    string unitId = player.GetUnitId();
                    if (unitId != "none")
                    {
                        Logging.WriteDebug("UnitID Found for player: " + player.Name + " is " + unitId);
                    }
                }
                foreach (WoWUnit unit in nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWUnit())
                {
                    Application.DoEvents();
                    object obj5 = str;
                    str = string.Concat(new object[] { 
                        obj5, "<tr>     <td bgcolor=\"#CCCCCC\">", unit.Name, " - (<i><a href=\"http://wowhead.com/npc=", unit.Entry, "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWUnit (", unit.Guid.GetWoWType, ")</td>     <td bgcolor=\"#CCCCCC\">", unit.Entry, "</td>     <td>", unit.Position.X, "</td>     <td bgcolor=\"#CCCCCC\">", unit.Position.Y, "</td>     <td>", unit.Position.Z, "</td>     <td bgcolor=\"#CCCCCC\">", 
                        unit.GetDistance, "</td>     <td>", unit.Faction, "</td>     <td bgcolor=\"#CCCCCC\">", unit.Guid, "</td>     <td>", unit.SummonedBy, "</td>        <td>", unit.CreatedBy, "</td>   </tr>"
                     });
                    string str3 = unit.GetUnitId();
                    if (str3 != "none")
                    {
                        Logging.WriteDebug("UnitID Found for unit: " + unit.Name + " is " + str3);
                    }
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
            this._ataotVia.Enabled = true;
        }
    }
}

