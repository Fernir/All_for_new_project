namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class ToTown : nManager.FiniteStateMachine.State
    {
        private bool _dacoaxoupObaoxohao;
        private bool _ecocuhuawaujov;
        private bool _egeruiwEba;
        private bool _feojei;
        private bool _foikaepeimaodOxu;
        private readonly Spell _grandExpeditionYak = new Spell(0x1df54);
        private bool _hihuweaUqeovuho;
        private bool _ihoqi;
        private Point _iqodounotUhaigieca;
        private Spell _ixaojumeoCoba = new Spell(0);
        private bool _qemuoruHab;
        private readonly Spell _travelersTundraMammothAlliance = new Spell(0xeff1);
        private readonly Spell _travelersTundraMammothHorde = new Spell(0xf007);
        private bool _woqegauveAgeotFimuoka;

        private void BeajuaqulQuwo(Npc itataovep)
        {
            if (((!this._hihuweaUqeovuho && !this._qemuoruHab) || ((itataovep.Type != Npc.NpcType.Repair) && (itataovep.Type != Npc.NpcType.Vendor))) && ((nManagerSetting.CurrentSetting.OnlyUseMillingInTown && nManagerSetting.CurrentSetting.ActivateAutoMilling) && ((nManagerSetting.CurrentSetting.HerbsToBeMilled.Count > 0) && Prospecting.NeedRun(nManagerSetting.CurrentSetting.HerbsToBeMilled))))
            {
                new MillingState().Run();
            }
        }

        private bool Egigeolu()
        {
            return (((nManagerSetting.CurrentSetting.FoodName != "") && (nManagerSetting.CurrentSetting.NumberOfFoodsWeGot > 0)) && (ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.FoodName) < nManagerSetting.CurrentSetting.NumberOfFoodsWeGot));
        }

        private int Odaexivacuel(Npc piuraraSuaroahi, Npc olifuisirafoa)
        {
            return piuraraSuaroahi.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position).CompareTo(olifuisirafoa.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position));
        }

        public override void Run()
        {
            List<Npc> list = new List<Npc>();
            Npc item = null;
            if ((nManager.Products.Products.ProductName == "Fisherbot") && FishingTask.IsLaunched)
            {
                FishingTask.StopLoopFish();
                MovementsAction.MoveBackward(true, false);
                Thread.Sleep(50);
                MovementsAction.MoveBackward(false, false);
            }
            if ((nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature && !this._foikaepeimaodOxu) && ((nManagerSetting.CurrentSetting.MaillingFeatureRecipient != string.Empty) && (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft)))
            {
                if (this._feojei)
                {
                    MountTask.DismountMount(true);
                    ItemsManager.UseItem(ItemsManager.GetItemNameById(0x9f40));
                    Thread.Sleep(0x7d0);
                    WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectById(0x2ec75), false);
                    if (nearestWoWGameObject.IsValid && (nearestWoWGameObject.CreatedBy == nManager.Wow.ObjectManager.ObjectManager.Me.Guid))
                    {
                        item = new Npc {
                            Entry = nearestWoWGameObject.Entry,
                            Position = nearestWoWGameObject.Position,
                            Name = nearestWoWGameObject.Name,
                            ContinentIdInt = Usefuls.ContinentId,
                            Faction = (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde") ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
                            SelectGossipOption = 0,
                            Type = Npc.NpcType.Mailbox
                        };
                    }
                }
                if ((item == null) && (NpcDB.GetNpcNearby(Npc.NpcType.Mailbox, false).Entry > 0))
                {
                    item = NpcDB.GetNpcNearby(Npc.NpcType.Mailbox, false);
                }
                list.Add(item);
            }
            if ((nManager.Wow.ObjectManager.ObjectManager.Me.GetDurability <= nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent) && nManagerSetting.CurrentSetting.ActivateAutoRepairFeature)
            {
                if ((this._hihuweaUqeovuho && (MountTask.GetMountCapacity() >= MountCapacity.Ground)) && (Skill.GetValue(SkillLine.Riding) > 0))
                {
                    if (this._ixaojumeoCoba.HaveBuff || this._ixaojumeoCoba.IsSpellUsable)
                    {
                        if (!this._ixaojumeoCoba.HaveBuff)
                        {
                            MountTask.DismountMount(true);
                            this._ixaojumeoCoba.Launch(true, true, true, null);
                            Thread.Sleep(0x7d0);
                        }
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde")
                        {
                            WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(0x7f81, false), false, false, false);
                            if (unit.IsValid && unit.IsAlive)
                            {
                                Npc npc3 = new Npc {
                                    Entry = unit.Entry,
                                    Position = unit.Position,
                                    Name = unit.Name,
                                    ContinentIdInt = Usefuls.ContinentId,
                                    Faction = Npc.FactionType.Horde,
                                    SelectGossipOption = 0,
                                    Type = Npc.NpcType.Repair
                                };
                                list.Add(npc3);
                            }
                        }
                        else
                        {
                            WoWUnit unit2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(0x7f7f, false), false, false, false);
                            if (unit2.IsValid && unit2.IsAlive)
                            {
                                Npc npc5 = new Npc {
                                    Entry = unit2.Entry,
                                    Position = unit2.Position,
                                    Name = unit2.Name,
                                    ContinentIdInt = Usefuls.ContinentId,
                                    Faction = Npc.FactionType.Alliance,
                                    SelectGossipOption = 0,
                                    Type = Npc.NpcType.Repair
                                };
                                list.Add(npc5);
                            }
                        }
                    }
                }
                else if ((this._qemuoruHab && (MountTask.GetMountCapacity() >= MountCapacity.Ground)) && (Skill.GetValue(SkillLine.Riding) > 0))
                {
                    if (this._grandExpeditionYak.HaveBuff || this._grandExpeditionYak.IsSpellUsable)
                    {
                        if (!this._grandExpeditionYak.HaveBuff)
                        {
                            MountTask.DismountMount(true);
                            this._grandExpeditionYak.Launch(true, true, true, null);
                            Thread.Sleep(0x7d0);
                        }
                        WoWUnit unit3 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(0xf566, false), false, false, false);
                        if (unit3.IsValid && unit3.IsAlive)
                        {
                            Npc npc7 = new Npc {
                                Entry = unit3.Entry,
                                Position = unit3.Position,
                                Name = unit3.Name,
                                ContinentIdInt = Usefuls.ContinentId,
                                Faction = (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde") ? Npc.FactionType.Horde : Npc.FactionType.Alliance,
                                SelectGossipOption = 0,
                                Type = Npc.NpcType.Repair
                            };
                            list.Add(npc7);
                        }
                    }
                }
                else if (this._dacoaxoupObaoxohao)
                {
                    Npc npc9 = WailuigiakaenWov("74A", Npc.NpcType.Repair, false);
                    if (npc9 != null)
                    {
                        list.Add(npc9);
                    }
                }
                else if (this._ihoqi)
                {
                    Npc npc10 = WailuigiakaenWov("110G", Npc.NpcType.Repair, false);
                    if (npc10 != null)
                    {
                        list.Add(npc10);
                    }
                }
                else if (this._woqegauveAgeotFimuoka)
                {
                    Npc npc11 = WailuigiakaenWov("Jeeves", Npc.NpcType.Repair, false);
                    if (npc11 != null)
                    {
                        list.Add(npc11);
                    }
                }
                else if (NpcDB.GetNpcNearby(Npc.NpcType.Repair, false).Entry > 0)
                {
                    list.Add(NpcDB.GetNpcNearby(Npc.NpcType.Repair, false));
                }
            }
            if ((this.Egigeolu() || this.Saliuheabanuk()) || (((Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft) && nManagerSetting.CurrentSetting.ActivateAutoSellingFeature) && !this._egeruiwEba))
            {
                if ((this._hihuweaUqeovuho && (MountTask.GetMountCapacity() >= MountCapacity.Ground)) && (Skill.GetValue(SkillLine.Riding) > 0))
                {
                    if (this._ixaojumeoCoba.HaveBuff || this._ixaojumeoCoba.IsSpellUsable)
                    {
                        if (!this._ixaojumeoCoba.HaveBuff)
                        {
                            MountTask.DismountMount(true);
                            this._ixaojumeoCoba.Launch(true, true, true, null);
                            Thread.Sleep(0x7d0);
                        }
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde")
                        {
                            WoWUnit unit4 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(0x7f82, false), false, false, false);
                            if (unit4.IsValid && unit4.IsAlive)
                            {
                                Npc npc12 = new Npc {
                                    Entry = unit4.Entry,
                                    Position = unit4.Position,
                                    Name = unit4.Name,
                                    ContinentIdInt = Usefuls.ContinentId,
                                    Faction = Npc.FactionType.Horde,
                                    SelectGossipOption = 0,
                                    Type = Npc.NpcType.Vendor
                                };
                                list.Add(npc12);
                            }
                        }
                        else
                        {
                            WoWUnit unit5 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(0x7f7e, false), false, false, false);
                            if (unit5.IsValid && unit5.IsAlive)
                            {
                                Npc npc14 = new Npc {
                                    Entry = unit5.Entry,
                                    Position = unit5.Position,
                                    Name = unit5.Name,
                                    ContinentIdInt = Usefuls.ContinentId,
                                    Faction = Npc.FactionType.Alliance,
                                    SelectGossipOption = 0,
                                    Type = Npc.NpcType.Vendor
                                };
                                list.Add(npc14);
                            }
                        }
                    }
                }
                else if ((this._qemuoruHab && (MountTask.GetMountCapacity() >= MountCapacity.Ground)) && (Skill.GetValue(SkillLine.Riding) > 0))
                {
                    if (this._grandExpeditionYak.HaveBuff || this._grandExpeditionYak.IsSpellUsable)
                    {
                        if (!this._grandExpeditionYak.HaveBuff)
                        {
                            MountTask.DismountMount(true);
                            this._grandExpeditionYak.Launch(true, true, true, null);
                            Thread.Sleep(0x7d0);
                        }
                        WoWUnit unit6 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(0xf566, false), false, false, false);
                        if (unit6.IsValid && unit6.IsAlive)
                        {
                            Npc npc16 = new Npc {
                                Entry = unit6.Entry,
                                Position = unit6.Position,
                                Name = unit6.Name,
                                ContinentIdInt = Usefuls.ContinentId,
                                Faction = Npc.FactionType.Neutral,
                                SelectGossipOption = 0,
                                Type = Npc.NpcType.Vendor
                            };
                            list.Add(npc16);
                        }
                    }
                }
                else if (this._dacoaxoupObaoxohao)
                {
                    Npc npc18 = WailuigiakaenWov("74A", Npc.NpcType.Vendor, false);
                    if (npc18 != null)
                    {
                        list.Add(npc18);
                    }
                }
                else if (this._ihoqi)
                {
                    Npc npc19 = WailuigiakaenWov("110G", Npc.NpcType.Vendor, false);
                    if (npc19 != null)
                    {
                        list.Add(npc19);
                    }
                }
                else if (this._woqegauveAgeotFimuoka)
                {
                    Npc npc20 = WailuigiakaenWov("Jeeves", Npc.NpcType.Vendor, false);
                    if (npc20 != null)
                    {
                        list.Add(npc20);
                    }
                }
                else if (NpcDB.GetNpcNearby(Npc.NpcType.Vendor, false).Entry > 0)
                {
                    list.Add(NpcDB.GetNpcNearby(Npc.NpcType.Vendor, false));
                }
            }
            if (list.Count > 0)
            {
                list.Sort(new Comparison<Npc>(this.Odaexivacuel));
                foreach (Npc npc21 in list)
                {
                    Npc target = npc21;
                    bool flag = (target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 400f) || (target.ContinentIdInt != Usefuls.ContinentId);
                    if (!flag && (target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= 400f))
                    {
                        bool flag2;
                        PathFinder.FindPath(nManager.Wow.ObjectManager.ObjectManager.Me.Position, target.Position, Usefuls.ContinentNameMpq, out flag2, true, false, false);
                        if (!flag2)
                        {
                            flag = true;
                        }
                    }
                    if ((flag && ((this._iqodounotUhaigieca == null) || (this._iqodounotUhaigieca.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 0.1f))) && (!this._ecocuhuawaujov && !Usefuls.IsFlying))
                    {
                        MovementManager.StopMove();
                        switch (npc21.Type)
                        {
                            case Npc.NpcType.Vendor:
                                Logging.Write(string.Concat(new object[] { "Calling travel system to NpcVendor ", npc21.Name, " (", npc21.Entry, ")..." }));
                                break;

                            case Npc.NpcType.Repair:
                                Logging.Write(string.Concat(new object[] { "Calling travel system to NpcRepair ", npc21.Name, " (", npc21.Entry, ")..." }));
                                break;

                            case Npc.NpcType.Mailbox:
                                Logging.Write(string.Concat(new object[] { "Calling travel system to Mailbox ", npc21.Name, " (", npc21.Entry, ")..." }));
                                break;

                            default:
                                Logging.Write(string.Concat(new object[] { "Calling travel system for ToTown to ", npc21.Name, " (", npc21.Entry, ")..." }));
                                break;
                        }
                        nManager.Products.Products.TravelToContinentId = target.ContinentIdInt;
                        nManager.Products.Products.TravelTo = target.Position;
                        nManager.Products.Products.TravelFromContinentId = Usefuls.ContinentId;
                        nManager.Products.Products.TravelFrom = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                        nManager.Products.Products.TargetValidationFct = new Func<Point, bool>(Quest.IsNearQuestGiver);
                        this._iqodounotUhaigieca = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                        break;
                    }
                    if ((this._iqodounotUhaigieca != null) && (this._iqodounotUhaigieca.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= 0.1f))
                    {
                        this._ecocuhuawaujov = true;
                    }
                    uint baseAddress = MovementManager.FindTarget(ref target, 0f, !nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted, false, 0f, false);
                    if (MovementManager.InMovement || (target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) >= 5f))
                    {
                        break;
                    }
                    this._ecocuhuawaujov = false;
                    if ((baseAddress == 0) && (target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 5f))
                    {
                        NpcDB.DelNpc(target);
                    }
                    else if (baseAddress > 0)
                    {
                        if (!this._ixaojumeoCoba.HaveBuff)
                        {
                            this.Soisiujaeku(target);
                            this.BeajuaqulQuwo(target);
                        }
                        Interact.InteractWith(baseAddress, false);
                        Thread.Sleep(500);
                        MovementManager.StopMove();
                        if (target.SelectGossipOption != 0)
                        {
                            Lua.LuaDoString("SelectGossipOption(" + target.SelectGossipOption + ")", false, true);
                            Thread.Sleep(500);
                        }
                        else if (((target.Type == Npc.NpcType.Repair) || (target.Type == Npc.NpcType.Vendor)) && !Gossip.SelectGossip(Gossip.GossipOption.Vendor))
                        {
                            Logging.WriteError("Problem with NPC " + npc21.Name + " Removing it for NpcDB", true);
                            NpcDB.DelNpc(npc21);
                            break;
                        }
                        if (target.Type == Npc.NpcType.Repair)
                        {
                            Logging.Write(string.Concat(new object[] { "Repair items from ", target.Name, " (", target.Entry, ")." }));
                            Vendor.RepairAllItems();
                            Thread.Sleep(0x3e8);
                        }
                        if (target.Type == Npc.NpcType.Vendor)
                        {
                            Logging.Write(string.Concat(new object[] { "Selling items to ", target.Name, " (", target.Entry, ")." }));
                            List<WoWItemQuality> itemQuality = new List<WoWItemQuality>();
                            if (nManagerSetting.CurrentSetting.SellGray)
                            {
                                itemQuality.Add(WoWItemQuality.Poor);
                            }
                            if (nManagerSetting.CurrentSetting.SellWhite)
                            {
                                itemQuality.Add(WoWItemQuality.Common);
                            }
                            if (nManagerSetting.CurrentSetting.SellGreen)
                            {
                                itemQuality.Add(WoWItemQuality.Uncommon);
                            }
                            if (nManagerSetting.CurrentSetting.SellBlue)
                            {
                                itemQuality.Add(WoWItemQuality.Rare);
                            }
                            if (nManagerSetting.CurrentSetting.SellPurple)
                            {
                                itemQuality.Add(WoWItemQuality.Epic);
                            }
                            Vendor.SellItems(nManagerSetting.CurrentSetting.ForceToSellTheseItems, nManagerSetting.CurrentSetting.DontSellTheseItems, itemQuality);
                            Thread.Sleep(0xbb8);
                            if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft)
                            {
                                this._egeruiwEba = true;
                            }
                            if (this.Egigeolu() || this.Saliuheabanuk())
                            {
                                Logging.Write(string.Concat(new object[] { "Buying beverages and food from ", target.Name, " (", target.Entry, ")." }));
                            }
                            for (int i = 0; (i < 10) && this.Egigeolu(); i++)
                            {
                                Vendor.BuyItem(nManagerSetting.CurrentSetting.FoodName, 1);
                            }
                            for (int j = 0; (j < 10) && this.Saliuheabanuk(); j++)
                            {
                                Vendor.BuyItem(nManagerSetting.CurrentSetting.BeverageName, 1);
                            }
                        }
                        if ((target.Type == Npc.NpcType.Repair) || (target.Type == Npc.NpcType.Vendor))
                        {
                            Gossip.CloseGossip();
                        }
                        if (target.Type == Npc.NpcType.Mailbox)
                        {
                            List<WoWItemQuality> list3 = new List<WoWItemQuality>();
                            if (nManagerSetting.CurrentSetting.MailGray)
                            {
                                list3.Add(WoWItemQuality.Poor);
                            }
                            if (nManagerSetting.CurrentSetting.MailWhite)
                            {
                                list3.Add(WoWItemQuality.Common);
                            }
                            if (nManagerSetting.CurrentSetting.MailGreen)
                            {
                                list3.Add(WoWItemQuality.Uncommon);
                            }
                            if (nManagerSetting.CurrentSetting.MailBlue)
                            {
                                list3.Add(WoWItemQuality.Rare);
                            }
                            if (nManagerSetting.CurrentSetting.MailPurple)
                            {
                                list3.Add(WoWItemQuality.Epic);
                            }
                            bool mailSendingCompleted = false;
                            for (int k = 7; (k > 0) && !mailSendingCompleted; k--)
                            {
                                Interact.InteractWith(baseAddress, false);
                                Thread.Sleep(0x3e8);
                                Mail.SendMessage(nManagerSetting.CurrentSetting.MaillingFeatureRecipient, nManagerSetting.CurrentSetting.MaillingFeatureSubject, "", nManagerSetting.CurrentSetting.ForceToMailTheseItems, nManagerSetting.CurrentSetting.DontMailTheseItems, list3, out mailSendingCompleted);
                                Thread.Sleep(500);
                            }
                            if (mailSendingCompleted)
                            {
                                Logging.Write(string.Concat(new object[] { "Sending items to the player ", nManagerSetting.CurrentSetting.MaillingFeatureRecipient, " using ", target.Name, " (", target.Entry, ")." }));
                            }
                            if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft)
                            {
                                this._foikaepeimaodOxu = true;
                            }
                            Lua.LuaDoString("CloseMail()", false, true);
                        }
                    }
                }
            }
        }

        private bool Saliuheabanuk()
        {
            return (((nManagerSetting.CurrentSetting.BeverageName != "") && (nManagerSetting.CurrentSetting.NumberOfBeverageWeGot > 0)) && (ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.BeverageName) < nManagerSetting.CurrentSetting.NumberOfBeverageWeGot));
        }

        private void Soisiujaeku(Npc itataovep)
        {
            if (((!this._hihuweaUqeovuho && !this._qemuoruHab) || ((itataovep.Type != Npc.NpcType.Repair) && (itataovep.Type != Npc.NpcType.Vendor))) && ((nManagerSetting.CurrentSetting.OnlyUseProspectingInTown && nManagerSetting.CurrentSetting.ActivateAutoProspecting) && ((nManagerSetting.CurrentSetting.MineralsToProspect.Count > 0) && Prospecting.NeedRun(nManagerSetting.CurrentSetting.MineralsToProspect))))
            {
                new ProspectingState().Run();
            }
        }

        private static Npc WailuigiakaenWov(string cowuixofoamai, Npc.NpcType gidaf, bool ovepoquvoani = false)
        {
            int num;
            int num2;
            int num3 = 0;
            string str = cowuixofoamai;
            if (str != null)
            {
                if (!(str == "74A"))
                {
                    if (str == "110G")
                    {
                        num = 0x8541;
                        num2 = 0x60cc;
                        goto Label_0062;
                    }
                    if (str == "Jeeves")
                    {
                        num = 0xbf90;
                        num2 = 0x8b3a;
                        num3 = 2;
                        goto Label_0062;
                    }
                }
                else
                {
                    num = 0x4738;
                    num2 = 0x3801;
                    goto Label_0062;
                }
            }
            return null;
        Label_0062:
            if (!ovepoquvoani)
            {
                MountTask.DismountMount(true);
                ItemsManager.UseItem(ItemsManager.GetItemNameById(num));
                Thread.Sleep(0x7d0);
            }
            WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(num2, false), false, false, false);
            if (!unit.IsValid || !unit.IsAlive)
            {
                return null;
            }
            return new Npc { Entry = unit.Entry, Position = unit.Position, Name = unit.Name, ContinentIdInt = Usefuls.ContinentId, Faction = (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde") ? Npc.FactionType.Horde : Npc.FactionType.Alliance, SelectGossipOption = num3, Type = gidaf };
        }

        public override List<nManager.FiniteStateMachine.State> BeforeStates
        {
            get
            {
                List<nManager.FiniteStateMachine.State> list = new List<nManager.FiniteStateMachine.State>();
                SmeltingState item = new SmeltingState {
                    Priority = 1
                };
                list.Add(item);
                return list;
            }
        }

        public override string DisplayName
        {
            get
            {
                return "To Town";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if ((((!nManagerSetting.CurrentSetting.ActivateAutoRepairFeature && !nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature) && !nManagerSetting.CurrentSetting.ActivateAutoSellingFeature) && ((nManagerSetting.CurrentSetting.NumberOfBeverageWeGot == 0) || (nManagerSetting.CurrentSetting.BeverageName == ""))) && ((nManagerSetting.CurrentSetting.NumberOfFoodsWeGot == 0) || (nManagerSetting.CurrentSetting.FoodName == "")))
                {
                    return false;
                }
                if (((!Usefuls.InGame || Usefuls.IsLoading) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction != "Horde")
                {
                    if ((this._travelersTundraMammothAlliance != null) && (SpellManager.ExistMountLUA(this._travelersTundraMammothAlliance.NameInGame) || this._travelersTundraMammothAlliance.KnownSpell))
                    {
                        this._hihuweaUqeovuho = true;
                        this._ixaojumeoCoba = this._travelersTundraMammothAlliance;
                    }
                }
                else if ((this._travelersTundraMammothHorde != null) && (SpellManager.ExistMountLUA(this._travelersTundraMammothHorde.NameInGame) || this._travelersTundraMammothHorde.KnownSpell))
                {
                    this._hihuweaUqeovuho = true;
                    this._ixaojumeoCoba = this._travelersTundraMammothHorde;
                }
                this._feojei = false;
                WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectById(0x2ec75), false);
                if (nearestWoWGameObject.IsValid && (nearestWoWGameObject.CreatedBy == nManager.Wow.ObjectManager.ObjectManager.Me.Guid))
                {
                    this._feojei = true;
                }
                else if ((nManagerSetting.CurrentSetting.UseMollE && (ItemsManager.GetItemCount(0x9f40) > 0)) && (!ItemsManager.IsItemOnCooldown(0x9f40) && ItemsManager.IsItemUsable(0x9f40)))
                {
                    this._feojei = true;
                }
                this._dacoaxoupObaoxohao = false;
                this._ihoqi = false;
                this._woqegauveAgeotFimuoka = false;
                if (nManagerSetting.CurrentSetting.UseRobot && ((WailuigiakaenWov("74A", Npc.NpcType.Repair, true) != null) || (((ItemsManager.GetItemCount(0x4738) > 0) && !ItemsManager.IsItemOnCooldown(0x4738)) && ItemsManager.IsItemUsable(0x4738))))
                {
                    this._dacoaxoupObaoxohao = true;
                }
                else if (nManagerSetting.CurrentSetting.UseRobot && ((WailuigiakaenWov("110G", Npc.NpcType.Repair, true) != null) || (((ItemsManager.GetItemCount(0x8541) > 0) && !ItemsManager.IsItemOnCooldown(0x8541)) && ItemsManager.IsItemUsable(0x8541))))
                {
                    this._ihoqi = true;
                }
                else if (nManagerSetting.CurrentSetting.UseRobot && ((WailuigiakaenWov("Jeeves", Npc.NpcType.Repair, true) != null) || (((ItemsManager.GetItemCount(0xbf90) > 0) && !ItemsManager.IsItemOnCooldown(0xbf90)) && ItemsManager.IsItemUsable(0xbf90))))
                {
                    this._woqegauveAgeotFimuoka = true;
                }
                if (Usefuls.GetContainerNumFreeSlots > nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft)
                {
                    this._egeruiwEba = false;
                }
                if (Usefuls.GetContainerNumFreeSlots > nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft)
                {
                    this._foikaepeimaodOxu = false;
                }
                return ((((nManager.Wow.ObjectManager.ObjectManager.Me.GetDurability <= nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent) && (((NpcDB.GetNpcNearby(Npc.NpcType.Repair, false).Entry > 0) || ((MountTask.GetMountCapacity() >= MountCapacity.Ground) && (this._hihuweaUqeovuho || this._qemuoruHab))) || ((this._dacoaxoupObaoxohao || this._ihoqi) || this._woqegauveAgeotFimuoka))) && nManagerSetting.CurrentSetting.ActivateAutoRepairFeature) || (((((Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft) && !this._egeruiwEba) && (((NpcDB.GetNpcNearby(Npc.NpcType.Vendor, false).Entry > 0) || ((MountTask.GetMountCapacity() >= MountCapacity.Ground) && (this._hihuweaUqeovuho || this._qemuoruHab))) || ((this._dacoaxoupObaoxohao || this._ihoqi) || this._woqegauveAgeotFimuoka))) && nManagerSetting.CurrentSetting.ActivateAutoSellingFeature) || ((((Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft) && !this._foikaepeimaodOxu) && ((NpcDB.GetNpcNearby(Npc.NpcType.Mailbox, false).Entry > 0) || this._feojei)) && (nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature && (nManagerSetting.CurrentSetting.MaillingFeatureRecipient != string.Empty)))));
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

