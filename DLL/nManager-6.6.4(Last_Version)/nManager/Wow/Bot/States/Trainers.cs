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
        private static ulong _biatealotei;
        private static uint _eloilibibaone;
        private static uint _kiaganijioqirKowoepupi;
        private static uint _puevimahe;
        private Npc _qofuq = new Npc();
        private List<Npc> _riweluib = new List<Npc>();
        private bool _vukapuaquo = true;
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

        private static uint BuriogaliatuaUko()
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

        private static void Hijei(int xeoxuIpecDapanik, SkillRank uwaodeujDa, SkillLine asibodukoavuniIbo, Npc ajeabaowaoxi)
        {
            if (!TeacherFoundNoSpam.Contains(ajeabaowaoxi))
            {
                SkillRank rank;
                TeacherFoundNoSpam.Add(ajeabaowaoxi);
                if (uwaodeujDa == SkillRank.ZenMaster)
                {
                    rank = uwaodeujDa + 100;
                }
                else
                {
                    rank = uwaodeujDa + 0x4b;
                }
                string str = "You don't know this skill yet";
                if (uwaodeujDa > SkillRank.None)
                {
                    str = string.Concat(new object[] { "You are currently ", uwaodeujDa, " of ", asibodukoavuniIbo.ToString(), ". Level ", xeoxuIpecDapanik, "/", (int) uwaodeujDa });
                }
                Logging.Write(string.Concat(new object[] { "Teacher of ", asibodukoavuniIbo.ToString(), " found. ", str, ". You will become ", rank, " of ", asibodukoavuniIbo.ToString(), "." }));
                Logging.Write(string.Concat(new object[] { "Informations about the teacher of ", asibodukoavuniIbo.ToString(), ". Id: ", ajeabaowaoxi.Entry, ", Name: ", ajeabaowaoxi.Name, ", Distance: ", ajeabaowaoxi.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position), ", Coords: ", ajeabaowaoxi.Position }));
            }
        }

        public override void Run()
        {
            if (this._riweluib.Count > 0)
            {
                Npc target = new Npc();
                for (int i = 0; i < this._riweluib.Count; i++)
                {
                    Npc npc2 = this._riweluib[i];
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
                if ((target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= 800f) || !Quest.TravelToQuestZone(target.Position, ref this._vukapuaquo, target.ContinentIdInt, false, target.Type.ToString()))
                {
                    uint baseAddress = MovementManager.FindTarget(ref target, 0f, true, false, 0f, false);
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
                            Quest.CompleteQuest();
                            Gossip.TrainAllAvailableSpells();
                            TeacherFoundNoSpam.Remove(target);
                            SpellManager.UpdateSpellBook();
                            this._vukapuaquo = true;
                        }
                    }
                }
            }
        }

        private static bool TuefokeUwuniUmeolio(int xeoxuIpecDapanik, SkillRank rocoafoqi, SkillLine asibodukoavuniIbo, bool xeceahuinin = false)
        {
            uint num = 0;
            uint num2 = 0;
            int num3 = 15;
            if (xeceahuinin)
            {
                num3 = 5;
            }
            bool flag = false;
            switch (rocoafoqi)
            {
                case SkillRank.Journeyman:
                {
                    SkillLine line3 = asibodukoavuniIbo;
                    if (line3 > SkillLine.Engineering)
                    {
                        switch (line3)
                        {
                            case SkillLine.Enchanting:
                            case SkillLine.Inscription:
                            case SkillLine.Jewelcrafting:
                                goto Label_05B4;

                            case SkillLine.Fishing:
                                goto Label_05D0;

                            case SkillLine.Skinning:
                                goto Label_05A6;

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
                                goto Label_05A6;

                            case SkillLine.Cooking:
                            case SkillLine.FirstAid:
                                goto Label_05D0;

                            case SkillLine.Tailoring:
                            case SkillLine.Engineering:
                            case SkillLine.Blacksmithing:
                            case SkillLine.Leatherworking:
                            case SkillLine.Alchemy:
                                goto Label_05B4;
                        }
                    }
                    goto Label_0B3E;
                }
                case SkillRank.Expert:
                {
                    SkillLine line4 = asibodukoavuniIbo;
                    if (line4 > SkillLine.Engineering)
                    {
                        switch (line4)
                        {
                            case SkillLine.Enchanting:
                            case SkillLine.Inscription:
                            case SkillLine.Jewelcrafting:
                                goto Label_06D8;

                            case SkillLine.Fishing:
                                goto Label_06F4;

                            case SkillLine.Skinning:
                                goto Label_06CA;

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
                                goto Label_06CA;

                            case SkillLine.Cooking:
                                goto Label_06F4;

                            case SkillLine.Tailoring:
                            case SkillLine.Engineering:
                            case SkillLine.Blacksmithing:
                            case SkillLine.Leatherworking:
                            case SkillLine.Alchemy:
                                goto Label_06D8;

                            case SkillLine.FirstAid:
                                num = 0x5cc6;
                                num2 = 0x23;
                                break;
                        }
                    }
                    goto Label_0B3E;
                }
                case SkillRank.Artisan:
                {
                    SkillLine line5 = asibodukoavuniIbo;
                    if (line5 > SkillLine.Engineering)
                    {
                        switch (line5)
                        {
                            case SkillLine.Enchanting:
                            case SkillLine.Inscription:
                            case SkillLine.Jewelcrafting:
                                goto Label_07FE;

                            case SkillLine.Fishing:
                                goto Label_081A;

                            case SkillLine.Skinning:
                                goto Label_07F0;

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
                                goto Label_07F0;

                            case SkillLine.Cooking:
                                goto Label_081A;

                            case SkillLine.Tailoring:
                            case SkillLine.Engineering:
                            case SkillLine.Blacksmithing:
                            case SkillLine.Leatherworking:
                            case SkillLine.Alchemy:
                                goto Label_07FE;

                            case SkillLine.FirstAid:
                                num = 0x17318;
                                num2 = 50;
                                break;
                        }
                    }
                    goto Label_0B3E;
                }
                case SkillRank.None:
                    switch (asibodukoavuniIbo)
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
                            if ((nManager.Products.Products.ProductName != "ProfessionsManager") || (BuriogaliatuaUko() == 0))
                            {
                                return false;
                            }
                            if ("Profession" != asibodukoavuniIbo.ToString())
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
                    goto Label_0B3E;

                case SkillRank.Apprentice:
                    switch (asibodukoavuniIbo)
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
                    goto Label_0B3E;

                case SkillRank.Master:
                    switch (asibodukoavuniIbo)
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
                    goto Label_0B3E;

                case SkillRank.GrandMaster:
                    switch (asibodukoavuniIbo)
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
                    goto Label_0B3E;

                case SkillRank.IllustriousGrandMaster:
                    switch (asibodukoavuniIbo)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Tailoring:
                        case SkillLine.Alchemy:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Leatherworking:
                        case SkillLine.Engineering:
                        case SkillLine.Enchanting:
                        case SkillLine.Skinning:
                        case SkillLine.Jewelcrafting:
                        case SkillLine.Inscription:
                            num = 0x8b290;
                            num2 = 80;
                            break;

                        case SkillLine.Cooking:
                            num = 0x8b290;
                            num2 = 1;
                            break;

                        case SkillLine.FirstAid:
                            num = 0x45948;
                            num2 = 80;
                            break;

                        case SkillLine.Archaeology:
                            num = 0x927c0;
                            num2 = 80;
                            break;
                    }
                    goto Label_0B3E;

                case SkillRank.ZenMaster:
                case SkillRank.DraenorMaster:
                case SkillRank.Legion:
                    return false;

                default:
                    goto Label_0B3E;
            }
            if (!nManagerSetting.CurrentSetting.BecomeApprenticeIfNeededByProduct || (BuriogaliatuaUko() == 0))
            {
                return false;
            }
            if (((((asibodukoavuniIbo == SkillLine.Mining) && !nManagerSetting.CurrentSetting.ActivateVeinsHarvesting) && (((nManager.Products.Products.ProductName != "Quester") || (nManager.Products.Products.ProductName != "Gatherer")) || (nManager.Products.Products.ProductName == "Grinder"))) || (((asibodukoavuniIbo == SkillLine.Herbalism) && !nManagerSetting.CurrentSetting.ActivateHerbsHarvesting) && (((nManager.Products.Products.ProductName == "Quester") || (nManager.Products.Products.ProductName == "Gatherer")) || (nManager.Products.Products.ProductName == "Grinder")))) || (((asibodukoavuniIbo == SkillLine.Skinning) && !nManagerSetting.CurrentSetting.ActivateBeastSkinning) && ((nManager.Products.Products.ProductName == "Quester") || (nManager.Products.Products.ProductName == "Grinder"))))
            {
                return false;
            }
            flag = true;
            num = 10;
            num2 = 1;
            goto Label_0B3E;
        Label_05A6:
            num = 0x128e;
            num2 = 10;
            goto Label_0B3E;
        Label_05B4:
            num = 0x128e;
            num2 = 20;
            goto Label_0B3E;
        Label_05D0:
            num = 0x251c;
            num2 = 1;
            goto Label_0B3E;
        Label_06CA:
            num = 0xb98c;
            num2 = 0x19;
            goto Label_0B3E;
        Label_06D8:
            num = 0xb98c;
            num2 = 0x23;
            goto Label_0B3E;
        Label_06F4:
            num = 0x5cc6;
            num2 = 1;
            goto Label_0B3E;
        Label_07F0:
            num = 0x17318;
            num2 = 40;
            goto Label_0B3E;
        Label_07FE:
            num = 0x17318;
            num2 = 50;
            goto Label_0B3E;
        Label_081A:
            num = 0x17318;
            num2 = 1;
        Label_0B3E:
            if (nManagerSetting.CurrentSetting.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum)
            {
                num *= 2;
            }
            if (((asibodukoavuniIbo == SkillLine.Riding) && (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= num2)) && ((Usefuls.GetMoneyCopper - _biatealotei) >= num))
            {
                _biatealotei += num;
                _kiaganijioqirKowoepupi = num;
                return true;
            }
            if ((((rocoafoqi - num3) > xeoxuIpecDapanik) || (nManager.Wow.ObjectManager.ObjectManager.Me.Level < num2)) || ((Usefuls.GetMoneyCopper - _biatealotei) < num))
            {
                return false;
            }
            if (flag && ((BuriogaliatuaUko() - _eloilibibaone) >= 1))
            {
                _eloilibibaone++;
                _puevimahe = 1;
            }
            else if (flag)
            {
                return false;
            }
            _biatealotei += num;
            _kiaganijioqirKowoepupi = num;
            return true;
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
                if (((!Usefuls.InGame || Usefuls.IsLoading) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                _biatealotei = 0L;
                _eloilibibaone = 0;
                this._riweluib = new List<Npc>();
                foreach (KeyValuePair<string, int> pair in SkillList)
                {
                    _kiaganijioqirKowoepupi = 0;
                    _puevimahe = 0;
                    this._qofuq = new Npc();
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
                        if ((!flag2 && nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills) && TuefokeUwuniUmeolio(num, maxValue, none, true))
                        {
                            this._qofuq = NpcDB.GetNpcNearby(archaeologyTrainer, true);
                        }
                        else if (TuefokeUwuniUmeolio(num, maxValue, none, false))
                        {
                            this._qofuq = NpcDB.GetNpcNearby(archaeologyTrainer, false);
                        }
                        if (this._qofuq.Entry > 0)
                        {
                            this._qofuq.InternalData = ((int) none) + "," + ((int) maxValue);
                            this._riweluib.Add(this._qofuq);
                            Hijei(num, maxValue, none, this._qofuq);
                        }
                        else
                        {
                            if (flag)
                            {
                                _eloilibibaone -= _puevimahe;
                            }
                            _biatealotei -= _kiaganijioqirKowoepupi;
                        }
                    }
                }
                return (this._riweluib.Count > 0);
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

