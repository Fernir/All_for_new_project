namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Trainers : nManager.FiniteStateMachine.State
    {
        private static uint _lastPriceAddedToWhishList;
        private static uint _lastPrimarySkillsWhishList;
        private List<Npc> _listOfTeachers = new List<Npc>();
        private static uint _primarySkillsSlotOnWhishList;
        private Npc _teacher = new Npc();
        private static uint _whishListSum;
        private static readonly Spell Alchemy = new Spell("Alchemy");
        private static readonly Spell Blacksmithing = new Spell("Blacksmithing");
        private static readonly Spell Enchanting = new Spell("Enchanting");
        private static readonly Spell Engineering = new Spell("Engineering");
        private static readonly Spell Herbalism = new Spell("Herb Gathering");
        private static readonly Spell Inscription = new Spell("Inscription");
        private static readonly Spell Jewelcrafting = new Spell("Jewelcrafting");
        private static readonly Spell Leatherworking = new Spell("Leatherworking");
        private static readonly Spell Mining = new Spell("Smelting");
        public static List<KeyValuePair<string, int>> SkillList = new List<KeyValuePair<string, int>> { new KeyValuePair<string, int>("Archaeology", Skill.GetValue(0x31a)), new KeyValuePair<string, int>("Fishing", Skill.GetValue(0x164)), new KeyValuePair<string, int>("Mining", Skill.GetValue(0xba)), new KeyValuePair<string, int>("Herbalism", Skill.GetValue(0xb6)), new KeyValuePair<string, int>("Skinning", Skill.GetValue(0x189)), new KeyValuePair<string, int>("Riding", Skill.GetValue(0x2fa)) };
        private static readonly Spell Skinning = new Spell("Skinning");
        private static readonly Spell Tailoring = new Spell("Tailoring");
        private static readonly List<Npc> TeacherFoundNoSpam = new List<Npc>();

        private static bool IsNewSkillAvailable(int value, SkillRank maxValue, SkillLine skillLine, bool hardCheck = false)
        {
            uint num = 0;
            uint num2 = 0;
            int num3 = 15;
            if (hardCheck)
            {
                num3 = 5;
            }
            bool flag = false;
            switch (maxValue)
            {
                case SkillRank.Journeyman:
                {
                    SkillLine line3 = skillLine;
                    if (line3 > SkillLine.Engineering)
                    {
                        switch (line3)
                        {
                            case SkillLine.Enchanting:
                            case SkillLine.Inscription:
                            case SkillLine.Jewelcrafting:
                                goto Label_05A8;

                            case SkillLine.Fishing:
                                goto Label_05C4;

                            case SkillLine.Skinning:
                                goto Label_059A;

                            case SkillLine.Archaeology:
                                num = 0x3e8;
                                num2 = 20;
                                break;

                            case SkillLine.Riding:
                                num = 0x243d58;
                                num2 = 60;
                                break;
                        }
                    }
                    else
                    {
                        switch (line3)
                        {
                            case SkillLine.Herbalism:
                            case SkillLine.Mining:
                                goto Label_059A;

                            case SkillLine.Cooking:
                            case SkillLine.FirstAid:
                                goto Label_05C4;

                            case SkillLine.Tailoring:
                            case SkillLine.Engineering:
                            case SkillLine.Blacksmithing:
                            case SkillLine.Leatherworking:
                            case SkillLine.Alchemy:
                                goto Label_05A8;
                        }
                    }
                    goto Label_0B32;
                }
                case SkillRank.Expert:
                {
                    SkillLine line4 = skillLine;
                    if (line4 > SkillLine.Engineering)
                    {
                        switch (line4)
                        {
                            case SkillLine.Enchanting:
                            case SkillLine.Inscription:
                            case SkillLine.Jewelcrafting:
                                goto Label_06CC;

                            case SkillLine.Fishing:
                                goto Label_06E8;

                            case SkillLine.Skinning:
                                goto Label_06BE;

                            case SkillLine.Archaeology:
                                num = 0x61a8;
                                num2 = 0x23;
                                break;

                            case SkillLine.Riding:
                                return false;
                        }
                    }
                    else
                    {
                        switch (line4)
                        {
                            case SkillLine.Herbalism:
                            case SkillLine.Mining:
                                goto Label_06BE;

                            case SkillLine.Cooking:
                                goto Label_06E8;

                            case SkillLine.Tailoring:
                            case SkillLine.Engineering:
                            case SkillLine.Blacksmithing:
                            case SkillLine.Leatherworking:
                            case SkillLine.Alchemy:
                                goto Label_06CC;

                            case SkillLine.FirstAid:
                                num = 0x5cc6;
                                num2 = 0x23;
                                break;
                        }
                    }
                    goto Label_0B32;
                }
                case SkillRank.Artisan:
                {
                    SkillLine line5 = skillLine;
                    if (line5 > SkillLine.Engineering)
                    {
                        switch (line5)
                        {
                            case SkillLine.Enchanting:
                            case SkillLine.Inscription:
                            case SkillLine.Jewelcrafting:
                                goto Label_07F2;

                            case SkillLine.Fishing:
                                goto Label_080E;

                            case SkillLine.Skinning:
                                goto Label_07E4;

                            case SkillLine.Archaeology:
                                num = 0x186a0;
                                num2 = 50;
                                break;

                            case SkillLine.Riding:
                                return false;
                        }
                    }
                    else
                    {
                        switch (line5)
                        {
                            case SkillLine.Herbalism:
                            case SkillLine.Mining:
                                goto Label_07E4;

                            case SkillLine.Cooking:
                                goto Label_080E;

                            case SkillLine.Tailoring:
                            case SkillLine.Engineering:
                            case SkillLine.Blacksmithing:
                            case SkillLine.Leatherworking:
                            case SkillLine.Alchemy:
                                goto Label_07F2;

                            case SkillLine.FirstAid:
                                num = 0x17318;
                                num2 = 50;
                                break;
                        }
                    }
                    goto Label_0B32;
                }
                case SkillRank.None:
                    switch (skillLine)
                    {
                        case SkillLine.Cooking:
                        case SkillLine.FirstAid:
                            if (!nManagerSetting.CurrentSetting.BecomeApprenticeOfSecondarySkillsWhileQuesting || (nManager.Products.Products.ProductName != "Quester"))
                            {
                                return false;
                            }
                            num = 0x5f;
                            num2 = 1;
                            break;

                        case SkillLine.Tailoring:
                        case SkillLine.Engineering:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Enchanting:
                        case SkillLine.Inscription:
                        case SkillLine.Jewelcrafting:
                            if ((nManager.Products.Products.ProductName != "ProfessionsManager") || (PrimarySkillSlotAvailable() == 0))
                            {
                                return false;
                            }
                            if ("Profession" != skillLine.ToString())
                            {
                                return false;
                            }
                            flag = true;
                            num = 10;
                            num2 = 5;
                            break;

                        case SkillLine.Fishing:
                            if ((!nManagerSetting.CurrentSetting.BecomeApprenticeOfSecondarySkillsWhileQuesting || (nManager.Products.Products.ProductName != "Quester")) && (!nManagerSetting.CurrentSetting.BecomeApprenticeIfNeededByProduct || (nManager.Products.Products.ProductName != "Fisherbot")))
                            {
                                return false;
                            }
                            num = 0x5f;
                            num2 = 5;
                            break;

                        case SkillLine.Archaeology:
                            if ((nManagerSetting.CurrentSetting.BecomeApprenticeOfSecondarySkillsWhileQuesting && !(nManager.Products.Products.ProductName != "Quester")) || (nManagerSetting.CurrentSetting.BecomeApprenticeIfNeededByProduct && !(nManager.Products.Products.ProductName != "Archaeologist")))
                            {
                                num = 0x3e8;
                                num2 = 20;
                                break;
                            }
                            return false;

                        case SkillLine.Riding:
                            num = 0x9470;
                            num2 = 20;
                            break;
                    }
                    goto Label_0B32;

                case SkillRank.Apprentice:
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                            num = 0x1db;
                            num2 = 1;
                            break;

                        case SkillLine.Cooking:
                        case SkillLine.FirstAid:
                        case SkillLine.Fishing:
                            num = 0x1db;
                            num2 = 1;
                            break;

                        case SkillLine.Tailoring:
                        case SkillLine.Engineering:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Enchanting:
                        case SkillLine.Inscription:
                        case SkillLine.Jewelcrafting:
                            num = 0x1db;
                            num2 = 10;
                            break;

                        case SkillLine.Archaeology:
                            num = 0x3e8;
                            num2 = 20;
                            break;

                        case SkillLine.Riding:
                            num = 0x73f78;
                            num2 = 40;
                            break;
                    }
                    goto Label_0B32;

                case SkillRank.Master:
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                            num = 0x512d4;
                            num2 = 0x37;
                            break;

                        case SkillLine.Cooking:
                        case SkillLine.Fishing:
                            num = 0x512d4;
                            num2 = 1;
                            break;

                        case SkillLine.Tailoring:
                        case SkillLine.Engineering:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Enchanting:
                        case SkillLine.Inscription:
                        case SkillLine.Jewelcrafting:
                            num = 0x512d4;
                            num2 = 0x41;
                            break;

                        case SkillLine.FirstAid:
                            num = 0x22ca4;
                            num2 = 0x41;
                            break;

                        case SkillLine.Archaeology:
                            num = 0x249f0;
                            num2 = 0x41;
                            break;

                        case SkillLine.Riding:
                            return false;
                    }
                    goto Label_0B32;

                case SkillRank.GrandMaster:
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Tailoring:
                        case SkillLine.Engineering:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Enchanting:
                        case SkillLine.Skinning:
                        case SkillLine.Jewelcrafting:
                        case SkillLine.Inscription:
                            num = 0x73f78;
                            num2 = 0x4b;
                            break;

                        case SkillLine.Cooking:
                            num = 0x73f78;
                            num2 = 1;
                            break;

                        case SkillLine.FirstAid:
                            num = 0x39fbc;
                            num2 = 0x4b;
                            break;

                        case SkillLine.Fishing:
                            return false;

                        case SkillLine.Archaeology:
                            num = 0x3d090;
                            num2 = 0x4b;
                            break;
                    }
                    goto Label_0B32;

                case SkillRank.IllustriousGrandMaster:
                {
                    SkillLine line8 = skillLine;
                    if (line8 > SkillLine.Tailoring)
                    {
                        switch (line8)
                        {
                            case SkillLine.Engineering:
                            case SkillLine.Enchanting:
                            case SkillLine.Skinning:
                            case SkillLine.Jewelcrafting:
                            case SkillLine.Inscription:
                                goto Label_0B05;

                            case SkillLine.Archaeology:
                                num = 0x927c0;
                                num2 = 80;
                                break;
                        }
                    }
                    else
                    {
                        switch (line8)
                        {
                            case SkillLine.Herbalism:
                            case SkillLine.Mining:
                            case SkillLine.Tailoring:
                            case SkillLine.Alchemy:
                            case SkillLine.Blacksmithing:
                            case SkillLine.Leatherworking:
                                goto Label_0B05;

                            case SkillLine.Cooking:
                                num = 0x8b290;
                                num2 = 1;
                                break;

                            case SkillLine.FirstAid:
                                num = 0x45948;
                                num2 = 80;
                                break;
                        }
                    }
                    goto Label_0B32;
                }
                case SkillRank.ZenMaster:
                case SkillRank.DraenorMaster:
                    return false;

                default:
                    goto Label_0B32;
            }
            if (!nManagerSetting.CurrentSetting.BecomeApprenticeIfNeededByProduct || (PrimarySkillSlotAvailable() == 0))
            {
                return false;
            }
            if (((((skillLine == SkillLine.Mining) && !nManagerSetting.CurrentSetting.ActivateVeinsHarvesting) && (((nManager.Products.Products.ProductName != "Quester") || (nManager.Products.Products.ProductName != "Gatherer")) || (nManager.Products.Products.ProductName == "Grinder"))) || (((skillLine == SkillLine.Herbalism) && !nManagerSetting.CurrentSetting.ActivateHerbsHarvesting) && (((nManager.Products.Products.ProductName == "Quester") || (nManager.Products.Products.ProductName == "Gatherer")) || (nManager.Products.Products.ProductName == "Grinder")))) || (((skillLine == SkillLine.Skinning) && !nManagerSetting.CurrentSetting.ActivateBeastSkinning) && ((nManager.Products.Products.ProductName == "Quester") || (nManager.Products.Products.ProductName == "Grinder"))))
            {
                return false;
            }
            flag = true;
            num = 10;
            num2 = 1;
            goto Label_0B32;
        Label_059A:
            num = 0x128e;
            num2 = 10;
            goto Label_0B32;
        Label_05A8:
            num = 0x128e;
            num2 = 20;
            goto Label_0B32;
        Label_05C4:
            num = 0x251c;
            num2 = 1;
            goto Label_0B32;
        Label_06BE:
            num = 0xb98c;
            num2 = 0x19;
            goto Label_0B32;
        Label_06CC:
            num = 0xb98c;
            num2 = 0x23;
            goto Label_0B32;
        Label_06E8:
            num = 0x5cc6;
            num2 = 1;
            goto Label_0B32;
        Label_07E4:
            num = 0x17318;
            num2 = 40;
            goto Label_0B32;
        Label_07F2:
            num = 0x17318;
            num2 = 50;
            goto Label_0B32;
        Label_080E:
            num = 0x17318;
            num2 = 1;
            goto Label_0B32;
        Label_0B05:
            num = 0x8b290;
            num2 = 80;
        Label_0B32:
            if (nManagerSetting.CurrentSetting.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum)
            {
                num *= 2;
            }
            if (((skillLine == SkillLine.Riding) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= num2)) && ((Usefuls.GetMoneyCopper - _whishListSum) >= num))
            {
                _whishListSum += num;
                _lastPriceAddedToWhishList = num;
                return true;
            }
            if ((((maxValue - num3) > value) || (nManager.Wow.ObjectManager.ObjectManager.Me.Level < num2)) || ((Usefuls.GetMoneyCopper - _whishListSum) < num))
            {
                return false;
            }
            if (flag && ((PrimarySkillSlotAvailable() - _primarySkillsSlotOnWhishList) >= 1))
            {
                _primarySkillsSlotOnWhishList++;
                _lastPrimarySkillsWhishList = 1;
            }
            else if (flag)
            {
                return false;
            }
            _whishListSum += num;
            _lastPriceAddedToWhishList = num;
            return true;
        }

        private static uint PrimarySkillSlotAvailable()
        {
            uint num = 2;
            if (Mining.KnownSpell)
            {
                num--;
            }
            if (Alchemy.KnownSpell)
            {
                num--;
            }
            if ((num > 0) && Skinning.KnownSpell)
            {
                num--;
            }
            if ((num > 0) && Herbalism.KnownSpell)
            {
                num--;
            }
            if ((num > 0) && Tailoring.KnownSpell)
            {
                num--;
            }
            if ((num > 0) && Enchanting.KnownSpell)
            {
                num--;
            }
            if ((num > 0) && Engineering.KnownSpell)
            {
                num--;
            }
            if ((num > 0) && Inscription.KnownSpell)
            {
                num--;
            }
            if ((num > 0) && Blacksmithing.KnownSpell)
            {
                num--;
            }
            if ((num > 0) && Jewelcrafting.KnownSpell)
            {
                num--;
            }
            if ((num > 0) && Leatherworking.KnownSpell)
            {
                num--;
            }
            return num;
        }

        public override void Run()
        {
            if (this._listOfTeachers.Count > 0)
            {
                Npc target = new Npc();
                for (int i = 0; i < this._listOfTeachers.Count; i++)
                {
                    Npc npc2 = this._listOfTeachers[i];
                    if (target.Entry <= 0)
                    {
                        goto Label_026F;
                    }
                    string productName = nManager.Products.Products.ProductName;
                    if (productName != null)
                    {
                        if (!(productName == "Gatherer"))
                        {
                            if ((productName == "Quester") || (productName == "Grinder"))
                            {
                                goto Label_0166;
                            }
                        }
                        else
                        {
                            if ((((target.Type == Npc.NpcType.MiningTrainer) && (npc2.Type != Npc.NpcType.RidingTrainer)) || (((target.Type == Npc.NpcType.HerbalismTrainer) && (npc2.Type != Npc.NpcType.RidingTrainer)) && (npc2.Type != Npc.NpcType.MiningTrainer))) || (((target.Type == Npc.NpcType.SkinningTrainer) && (npc2.Type != Npc.NpcType.RidingTrainer)) && ((npc2.Type != Npc.NpcType.MiningTrainer) && (npc2.Type != Npc.NpcType.HerbalismTrainer))))
                            {
                                continue;
                            }
                            if (npc2.Type == Npc.NpcType.RidingTrainer)
                            {
                                target = npc2;
                            }
                            else if ((npc2.Type == Npc.NpcType.MiningTrainer) && (target.Type != Npc.NpcType.RidingTrainer))
                            {
                                target = npc2;
                            }
                            else if (((npc2.Type == Npc.NpcType.HerbalismTrainer) && (target.Type != Npc.NpcType.RidingTrainer)) && (target.Type != Npc.NpcType.MiningTrainer))
                            {
                                target = npc2;
                            }
                            else if (((npc2.Type == Npc.NpcType.SkinningTrainer) && (target.Type != Npc.NpcType.RidingTrainer)) && ((target.Type != Npc.NpcType.MiningTrainer) && (target.Type != Npc.NpcType.HerbalismTrainer)))
                            {
                                target = npc2;
                            }
                        }
                    }
                    goto Label_023B;
                Label_0166:
                    if ((((target.Type == Npc.NpcType.SkinningTrainer) && (npc2.Type != Npc.NpcType.RidingTrainer)) || (((target.Type == Npc.NpcType.MiningTrainer) && (npc2.Type != Npc.NpcType.RidingTrainer)) && (npc2.Type != Npc.NpcType.SkinningTrainer))) || (((target.Type == Npc.NpcType.HerbalismTrainer) && (npc2.Type != Npc.NpcType.RidingTrainer)) && ((npc2.Type != Npc.NpcType.SkinningTrainer) && (npc2.Type != Npc.NpcType.MiningTrainer))))
                    {
                        continue;
                    }
                    if (npc2.Type == Npc.NpcType.RidingTrainer)
                    {
                        target = npc2;
                    }
                    else if ((npc2.Type == Npc.NpcType.SkinningTrainer) && (target.Type != Npc.NpcType.RidingTrainer))
                    {
                        target = npc2;
                    }
                    else if (((npc2.Type == Npc.NpcType.MiningTrainer) && (target.Type != Npc.NpcType.RidingTrainer)) && (target.Type != Npc.NpcType.SkinningTrainer))
                    {
                        target = npc2;
                    }
                    else if (((npc2.Type == Npc.NpcType.HerbalismTrainer) && (target.Type != Npc.NpcType.RidingTrainer)) && ((target.Type != Npc.NpcType.SkinningTrainer) && (target.Type != Npc.NpcType.MiningTrainer)))
                    {
                        target = npc2;
                    }
                Label_023B:
                    if ((target != npc2) && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(npc2.Position) < nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(target.Position)))
                    {
                        target = npc2;
                    }
                    continue;
                Label_026F:
                    target = npc2;
                }
                uint baseAddress = MovementManager.FindTarget(ref target, 0f, true, false, 0f);
                if (!MovementManager.InMovement)
                {
                    if ((baseAddress == 0) && (target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 10f))
                    {
                        NpcDB.DelNpc(target);
                    }
                    else if (baseAddress > 0)
                    {
                        string[] strArray = target.InternalData.Split(new char[] { ',' });
                        if (strArray.Length == 2)
                        {
                            SkillRank rank2;
                            SkillLine line = (SkillLine) Others.ToInt32(strArray[0]);
                            SkillRank rank = (SkillRank) Others.ToInt32(strArray[1]);
                            if (rank == SkillRank.ZenMaster)
                            {
                                rank2 = rank + 100;
                            }
                            else
                            {
                                rank2 = rank + 0x4b;
                            }
                            string str = "";
                            if (rank != SkillRank.None)
                            {
                                str = " We were only " + rank.ToString() + " of " + line.ToString() + ".";
                            }
                            Logging.Write("We have just reached the Teacher of " + line.ToString() + ", " + target.Name + ". We are now going to learn " + rank2.ToString() + " of " + line.ToString() + "." + str);
                        }
                        Interact.InteractWith(baseAddress, false);
                        Thread.Sleep((int) (500 + Usefuls.Latency));
                        Gossip.TrainAllAvailableSpells();
                        TeacherFoundNoSpam.Remove(target);
                        SpellManager.UpdateSpellBook();
                    }
                }
            }
        }

        private static void TeacherFound(int value, SkillRank skillRank, SkillLine skillLine, Npc teacher)
        {
            if (!TeacherFoundNoSpam.Contains(teacher))
            {
                SkillRank rank;
                TeacherFoundNoSpam.Add(teacher);
                if (skillRank == SkillRank.ZenMaster)
                {
                    rank = skillRank + 100;
                }
                else
                {
                    rank = skillRank + 0x4b;
                }
                string str = "You don't know this skill yet";
                if (skillRank > SkillRank.None)
                {
                    str = string.Concat(new object[] { "You are currently ", skillRank, " of ", skillLine.ToString(), ". Level ", value, "/", (int) skillRank });
                }
                Logging.Write(string.Concat(new object[] { "Teacher of ", skillLine.ToString(), " found. ", str, ". You will become ", rank, " of ", skillLine.ToString(), "." }));
                Logging.Write(string.Concat(new object[] { "Informations about the teacher of ", skillLine.ToString(), ". Id: ", teacher.Entry, ", Name: ", teacher.Name, ", Distance: ", teacher.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position), ", Coords: ", teacher.Position }));
            }
        }

        public override List<nManager.FiniteStateMachine.State> BeforeStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Learning skills";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if ((((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))) || !nManager.Products.Products.IsStarted)
                {
                    return false;
                }
                _whishListSum = 0;
                _primarySkillsSlotOnWhishList = 0;
                this._listOfTeachers = new List<Npc>();
                foreach (KeyValuePair<string, int> pair in SkillList)
                {
                    _lastPriceAddedToWhishList = 0;
                    _lastPrimarySkillsWhishList = 0;
                    this._teacher = new Npc();
                    SkillLine none = SkillLine.None;
                    Npc.NpcType archaeologyTrainer = Npc.NpcType.None;
                    bool flag = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    switch (pair.Key)
                    {
                        case "Archaeology":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills && (nManager.Products.Products.ProductName != "Archaeologist"))
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Archaeology;
                            archaeologyTrainer = Npc.NpcType.ArchaeologyTrainer;
                            break;

                        case "Fishing":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills && (nManager.Products.Products.ProductName != "Fisherbot"))
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Fishing;
                            archaeologyTrainer = Npc.NpcType.FishingTrainer;
                            break;

                        case "Mining":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills && (!nManagerSetting.CurrentSetting.ActivateVeinsHarvesting || ((nManagerSetting.CurrentSetting.ActivateVeinsHarvesting && (nManager.Products.Products.ProductName != "Gatherer")) && ((nManager.Products.Products.ProductName != "Grinder") && (nManager.Products.Products.ProductName != "Quester")))))
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Mining;
                            archaeologyTrainer = Npc.NpcType.MiningTrainer;
                            flag = true;
                            break;

                        case "Herbalism":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills && (!nManagerSetting.CurrentSetting.ActivateHerbsHarvesting || ((nManagerSetting.CurrentSetting.ActivateHerbsHarvesting && (nManager.Products.Products.ProductName != "Gatherer")) && ((nManager.Products.Products.ProductName != "Grinder") && (nManager.Products.Products.ProductName != "Quester")))))
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Herbalism;
                            archaeologyTrainer = Npc.NpcType.HerbalismTrainer;
                            flag = true;
                            break;

                        case "Skinning":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills && (!nManagerSetting.CurrentSetting.ActivateBeastSkinning || (!nManagerSetting.CurrentSetting.ActivateMonsterLooting && !nManagerSetting.CurrentSetting.BeastNinjaSkinning)))
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Skinning;
                            archaeologyTrainer = Npc.NpcType.SkinningTrainer;
                            flag = true;
                            break;

                        case "Riding":
                            if (!nManagerSetting.CurrentSetting.TrainMountingCapacity)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Riding;
                            archaeologyTrainer = Npc.NpcType.RidingTrainer;
                            flag2 = true;
                            break;

                        case "Alchemy":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Alchemy;
                            archaeologyTrainer = Npc.NpcType.AlchemyTrainer;
                            flag = true;
                            break;

                        case "Cooking":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Cooking;
                            archaeologyTrainer = Npc.NpcType.CookingTrainer;
                            break;

                        case "FirstAid":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.FirstAid;
                            archaeologyTrainer = Npc.NpcType.FirstAidTrainer;
                            break;

                        case "Tailoring":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Tailoring;
                            archaeologyTrainer = Npc.NpcType.TailoringTrainer;
                            flag = true;
                            break;

                        case "Enchanting":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Enchanting;
                            archaeologyTrainer = Npc.NpcType.EnchantingTrainer;
                            flag = true;
                            break;

                        case "Engineering":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Engineering;
                            archaeologyTrainer = Npc.NpcType.EngineeringTrainer;
                            flag = true;
                            break;

                        case "Inscription":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Inscription;
                            archaeologyTrainer = Npc.NpcType.InscriptionTrainer;
                            flag = true;
                            break;

                        case "Blacksmithing":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Blacksmithing;
                            archaeologyTrainer = Npc.NpcType.BlacksmithingTrainer;
                            flag = true;
                            break;

                        case "Jewelcrafting":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Jewelcrafting;
                            archaeologyTrainer = Npc.NpcType.JewelcraftingTrainer;
                            flag = true;
                            break;

                        case "Leatherworking":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                            {
                                flag3 = true;
                            }
                            none = SkillLine.Leatherworking;
                            archaeologyTrainer = Npc.NpcType.LeatherworkingTrainer;
                            flag = true;
                            break;

                        default:
                        {
                            continue;
                        }
                    }
                    SkillRank maxValue = (SkillRank) Skill.GetMaxValue(none);
                    int num = Skill.GetValue(none);
                    if (pair.Key.Contains(none.ToString()) && (pair.Value != num))
                    {
                        KeyValuePair<string, int> item = new KeyValuePair<string, int>(pair.Key, num);
                        SkillList.Remove(pair);
                        SkillList.Add(item);
                        num += Skill.GetSkillBonus(none);
                        Logging.Write(string.Concat(new object[] { "Your skill in ", none, " has increased to ", num, "." }));
                        return false;
                    }
                    if (nManagerSetting.CurrentSetting.ActivateSkillsAutoTraining && !flag3)
                    {
                        if ((!flag2 && nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills) && IsNewSkillAvailable(num, maxValue, none, true))
                        {
                            this._teacher = NpcDB.GetNpcNearby(archaeologyTrainer, true);
                        }
                        else if (IsNewSkillAvailable(num, maxValue, none, false))
                        {
                            this._teacher = NpcDB.GetNpcNearby(archaeologyTrainer, false);
                        }
                        if (this._teacher.Entry > 0)
                        {
                            this._teacher.InternalData = ((int) none) + "," + ((int) maxValue);
                            this._listOfTeachers.Add(this._teacher);
                            TeacherFound(num, maxValue, none, this._teacher);
                        }
                        else
                        {
                            if (flag)
                            {
                                _primarySkillsSlotOnWhishList -= _lastPrimarySkillsWhishList;
                            }
                            _whishListSum -= _lastPriceAddedToWhishList;
                        }
                    }
                }
                return (this._listOfTeachers.Count > 0);
            }
        }

        public override List<nManager.FiniteStateMachine.State> NextStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override int Priority { get; set; }
    }
}

