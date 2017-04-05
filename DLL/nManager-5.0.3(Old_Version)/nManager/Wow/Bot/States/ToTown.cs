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
        private readonly Spell _grandExpeditionYak = new Spell(0x1df54);
        private bool _magicMountMammoth;
        private bool _magicMountYak;
        private bool _suspendMailing;
        private bool _suspendSelling;
        private Spell _travelersTundraMammoth = new Spell(0);
        private readonly Spell _travelersTundraMammothAlliance = new Spell(0xeff1);
        private readonly Spell _travelersTundraMammothHorde = new Spell(0xf007);
        private bool _use110G;
        private bool _use74A;
        private bool _useJeeves;
        private bool _useMollE;

        private void DoMillingInTown(Npc npc)
        {
            if (((!this._magicMountMammoth && !this._magicMountYak) || ((npc.Type != Npc.NpcType.Repair) && (npc.Type != Npc.NpcType.Vendor))) && ((nManagerSetting.CurrentSetting.OnlyUseMillingInTown && nManagerSetting.CurrentSetting.ActivateAutoMilling) && ((nManagerSetting.CurrentSetting.HerbsToBeMilled.Count > 0) && Prospecting.NeedRun(nManagerSetting.CurrentSetting.HerbsToBeMilled))))
            {
                new MillingState().Run();
            }
        }

        private void DoProspectingInTown(Npc npc)
        {
            if (((!this._magicMountMammoth && !this._magicMountYak) || ((npc.Type != Npc.NpcType.Repair) && (npc.Type != Npc.NpcType.Vendor))) && ((nManagerSetting.CurrentSetting.OnlyUseProspectingInTown && nManagerSetting.CurrentSetting.ActivateAutoProspecting) && ((nManagerSetting.CurrentSetting.MineralsToProspect.Count > 0) && Prospecting.NeedRun(nManagerSetting.CurrentSetting.MineralsToProspect))))
            {
                new ProspectingState().Run();
            }
        }

        private static Npc DoSpawnRobot(string robot, Npc.NpcType type, bool checkOnly = false)
        {
            int num;
            int num2;
            int num3 = 0;
            string str = robot;
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
            if (!checkOnly)
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
            return new Npc { Entry = unit.Entry, Position = unit.Position, Name = unit.Name, ContinentIdInt = Usefuls.ContinentId, Faction = (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde") ? Npc.FactionType.Horde : Npc.FactionType.Alliance, SelectGossipOption = num3, Type = type };
        }

        private bool NeedDrinkSupplies()
        {
            return (((nManagerSetting.CurrentSetting.BeverageName != "") && (nManagerSetting.CurrentSetting.NumberOfBeverageWeGot > 0)) && (ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.BeverageName) < nManagerSetting.CurrentSetting.NumberOfBeverageWeGot));
        }

        private bool NeedFoodSupplies()
        {
            return (((nManagerSetting.CurrentSetting.FoodName != "") && (nManagerSetting.CurrentSetting.NumberOfFoodsWeGot > 0)) && (ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.FoodName) < nManagerSetting.CurrentSetting.NumberOfFoodsWeGot));
        }

        private int OnComparison(Npc n1, Npc n2)
        {
            return n1.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position).CompareTo(n1.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position));
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
            if ((nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature && !this._suspendMailing) && ((nManagerSetting.CurrentSetting.MaillingFeatureRecipient != string.Empty) && (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft)))
            {
                if (this._useMollE)
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
                if (this._magicMountMammoth && (MountTask.GetMountCapacity() >= MountCapacity.Ground))
                {
                    if (this._travelersTundraMammoth.HaveBuff || this._travelersTundraMammoth.IsSpellUsable)
                    {
                        if (!this._travelersTundraMammoth.HaveBuff)
                        {
                            MountTask.DismountMount(true);
                            this._travelersTundraMammoth.Launch(true, true, true, null);
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
                else if (this._magicMountYak && (MountTask.GetMountCapacity() >= MountCapacity.Ground))
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
                else if (this._use74A)
                {
                    Npc npc9 = DoSpawnRobot("74A", Npc.NpcType.Repair, false);
                    if (npc9 != null)
                    {
                        list.Add(npc9);
                    }
                }
                else if (this._use110G)
                {
                    Npc npc10 = DoSpawnRobot("110G", Npc.NpcType.Repair, false);
                    if (npc10 != null)
                    {
                        list.Add(npc10);
                    }
                }
                else if (this._useJeeves)
                {
                    Npc npc11 = DoSpawnRobot("Jeeves", Npc.NpcType.Repair, false);
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
            if ((this.NeedFoodSupplies() || this.NeedDrinkSupplies()) || (((Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft) && nManagerSetting.CurrentSetting.ActivateAutoSellingFeature) && !this._suspendSelling))
            {
                if (this._magicMountMammoth && (MountTask.GetMountCapacity() >= MountCapacity.Ground))
                {
                    if (this._travelersTundraMammoth.HaveBuff || this._travelersTundraMammoth.IsSpellUsable)
                    {
                        if (!this._travelersTundraMammoth.HaveBuff)
                        {
                            MountTask.DismountMount(true);
                            this._travelersTundraMammoth.Launch(true, true, true, null);
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
                else if (this._magicMountYak && (MountTask.GetMountCapacity() >= MountCapacity.Ground))
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
                else if (this._use74A)
                {
                    Npc npc18 = DoSpawnRobot("74A", Npc.NpcType.Vendor, false);
                    if (npc18 != null)
                    {
                        list.Add(npc18);
                    }
                }
                else if (this._use110G)
                {
                    Npc npc19 = DoSpawnRobot("110G", Npc.NpcType.Vendor, false);
                    if (npc19 != null)
                    {
                        list.Add(npc19);
                    }
                }
                else if (this._useJeeves)
                {
                    Npc npc20 = DoSpawnRobot("Jeeves", Npc.NpcType.Vendor, false);
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
                list.Sort(new Comparison<Npc>(this.OnComparison));
                foreach (Npc npc21 in list)
                {
                    Npc target = npc21;
                    uint baseAddress = MovementManager.FindTarget(ref target, 0f, !nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted, false, 0f);
                    if (MovementManager.InMovement)
                    {
                        break;
                    }
                    if ((baseAddress == 0) && (target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 10f))
                    {
                        NpcDB.DelNpc(target);
                    }
                    else if (baseAddress > 0)
                    {
                        if (!this._travelersTundraMammoth.HaveBuff)
                        {
                            this.DoProspectingInTown(target);
                            this.DoMillingInTown(target);
                        }
                        Interact.InteractWith(baseAddress, false);
                        Thread.Sleep(500);
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
                                this._suspendSelling = true;
                            }
                            if (this.NeedFoodSupplies() || this.NeedDrinkSupplies())
                            {
                                Logging.Write(string.Concat(new object[] { "Buying beverages and food from ", target.Name, " (", target.Entry, ")." }));
                            }
                            for (int i = 0; (i < 10) && this.NeedFoodSupplies(); i++)
                            {
                                Vendor.BuyItem(nManagerSetting.CurrentSetting.FoodName, 1);
                            }
                            for (int j = 0; (j < 10) && this.NeedDrinkSupplies(); j++)
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
                                this._suspendMailing = true;
                            }
                            Lua.LuaDoString("CloseMail()", false, true);
                        }
                    }
                }
            }
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
                if ((((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))) || !nManager.Products.Products.IsStarted)
                {
                    return false;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction != "Horde")
                {
                    if ((this._travelersTundraMammothAlliance != null) && (SpellManager.ExistMountLUA(this._travelersTundraMammothAlliance.NameInGame) || this._travelersTundraMammothAlliance.KnownSpell))
                    {
                        this._magicMountMammoth = true;
                        this._travelersTundraMammoth = this._travelersTundraMammothAlliance;
                    }
                }
                else if ((this._travelersTundraMammothHorde != null) && (SpellManager.ExistMountLUA(this._travelersTundraMammothHorde.NameInGame) || this._travelersTundraMammothHorde.KnownSpell))
                {
                    this._magicMountMammoth = true;
                    this._travelersTundraMammoth = this._travelersTundraMammothHorde;
                }
                this._useMollE = false;
                WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectById(0x2ec75), false);
                if (nearestWoWGameObject.IsValid && (nearestWoWGameObject.CreatedBy == nManager.Wow.ObjectManager.ObjectManager.Me.Guid))
                {
                    this._useMollE = true;
                }
                else if ((nManagerSetting.CurrentSetting.UseMollE && (ItemsManager.GetItemCount(0x9f40) > 0)) && (!ItemsManager.IsItemOnCooldown(0x9f40) && ItemsManager.IsItemUsable(0x9f40)))
                {
                    this._useMollE = true;
                }
                this._use74A = false;
                this._use110G = false;
                this._useJeeves = false;
                if (nManagerSetting.CurrentSetting.UseRobot && ((DoSpawnRobot("74A", Npc.NpcType.Repair, true) != null) || (((ItemsManager.GetItemCount(0x4738) > 0) && !ItemsManager.IsItemOnCooldown(0x4738)) && ItemsManager.IsItemUsable(0x4738))))
                {
                    this._use74A = true;
                }
                else if (nManagerSetting.CurrentSetting.UseRobot && ((DoSpawnRobot("110G", Npc.NpcType.Repair, true) != null) || (((ItemsManager.GetItemCount(0x8541) > 0) && !ItemsManager.IsItemOnCooldown(0x8541)) && ItemsManager.IsItemUsable(0x8541))))
                {
                    this._use110G = true;
                }
                else if (nManagerSetting.CurrentSetting.UseRobot && ((DoSpawnRobot("Jeeves", Npc.NpcType.Repair, true) != null) || (((ItemsManager.GetItemCount(0xbf90) > 0) && !ItemsManager.IsItemOnCooldown(0xbf90)) && ItemsManager.IsItemUsable(0xbf90))))
                {
                    this._useJeeves = true;
                }
                if (Usefuls.GetContainerNumFreeSlots > nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft)
                {
                    this._suspendSelling = false;
                }
                if (Usefuls.GetContainerNumFreeSlots > nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft)
                {
                    this._suspendMailing = false;
                }
                return ((((nManager.Wow.ObjectManager.ObjectManager.Me.GetDurability <= nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent) && (((NpcDB.GetNpcNearby(Npc.NpcType.Repair, false).Entry > 0) || ((MountTask.GetMountCapacity() >= MountCapacity.Ground) && (this._magicMountMammoth || this._magicMountYak))) || ((this._use74A || this._use110G) || this._useJeeves))) && nManagerSetting.CurrentSetting.ActivateAutoRepairFeature) || (((((Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft) && !this._suspendSelling) && (((NpcDB.GetNpcNearby(Npc.NpcType.Vendor, false).Entry > 0) || ((MountTask.GetMountCapacity() >= MountCapacity.Ground) && (this._magicMountMammoth || this._magicMountYak))) || ((this._use74A || this._use110G) || this._useJeeves))) && nManagerSetting.CurrentSetting.ActivateAutoSellingFeature) || ((((Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft) && !this._suspendMailing) && ((NpcDB.GetNpcNearby(Npc.NpcType.Mailbox, false).Entry > 0) || this._useMollE)) && (nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature && (nManagerSetting.CurrentSetting.MaillingFeatureRecipient != string.Empty)))));
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

