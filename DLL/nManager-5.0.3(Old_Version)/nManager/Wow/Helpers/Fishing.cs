namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Fishing
    {
        private static List<uint> _listFishingPoles;
        private static List<uint> _listLure;
        public static int[] DraenicBaitList = new int[] { 0x1aed2, 0x1aed3, 0x1aec2, 0x1aed5, 0x1aed6, 0x1aed1, 0x1aed4 };
        private static readonly List<int> DraenorSeasList = new List<int> { 
            0x1c41, 0x1c4e, 0x1c53, 0x1c59, 0x1c41, 0x1c46, 0x1c47, 0x1c57, 0x1c5a, 0x1c5b, 0x1c5e, 0x1c84, 0x1cef, 0x1cf0, 0x1cf2, 0x1cf6, 
            0x1d04, 0x1d0c, 0x1d15, 0x1d16, 0x1d18, 0x1b38, 0x1b8f, 0x1c05
         };
        public static bool FirstLureCheck = true;
        public static nManager.Helpful.Timer ReCheckFishingPoleTimer = new nManager.Helpful.Timer(1.0);

        public static void EquipFishingPoles(string fishingPoleName = "")
        {
            try
            {
                if (ReCheckFishingPoleTimer.IsReady && !IsEquipedFishingPoles(""))
                {
                    Logging.WriteDebug("Parsing inventory to find a Fishing Pole to equip.");
                    if (fishingPoleName != string.Empty)
                    {
                        ItemsManager.EquipItemByName(fishingPoleName);
                        Thread.Sleep(500);
                        Thread.Sleep(Usefuls.Latency);
                        while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        foreach (int num in ListFishingPoles)
                        {
                            if (ItemsManager.GetItemCount(num) > 0)
                            {
                                ItemsManager.EquipItemByName(ItemsManager.GetItemNameById(num));
                                Thread.Sleep(500);
                                Thread.Sleep(Usefuls.Latency);
                                while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                                {
                                    Thread.Sleep(200);
                                }
                                break;
                            }
                        }
                    }
                    ReCheckFishingPoleTimer = new nManager.Helpful.Timer(300000.0);
                    Logging.WriteDebug("Inventory parsed, prevent this function from being parsed for the next five minutes.");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Fishing > EquipFishingPoles(string fishingPoleName = \"\"): " + exception, true);
            }
        }

        public static Point FindTheUltimatePoint(Point fishingHole)
        {
            for (float i = 13f; i <= 20f; i += 3f)
            {
                for (double j = 0.0; j <= 1.5707963267948966; j += 0.31415926535897931)
                {
                    Point point;
                    float num3 = i * ((float) System.Math.Sin(j));
                    float num4 = i * ((float) System.Math.Cos(j));
                    point = new Point(fishingHole.X + num3, fishingHole.Y - num4, fishingHole.Z, "None") {
                        Z = point.Z - 0.8f
                    };
                    Point from = new Point(point) {
                        Z = fishingHole.Z + 20f
                    };
                    if (TraceLine.TraceLineGo(from, point, CGWorldFrameHitFlags.HitTestAllButLiquid))
                    {
                        float zPosition = PathFinder.GetZPosition(point, false);
                        if (zPosition != 0f)
                        {
                            point.Z = zPosition + 1f;
                            return point;
                        }
                    }
                    else
                    {
                        point = new Point(fishingHole.X + num4, fishingHole.Y + num3, fishingHole.Z, "None") {
                            Z = point.Z - 1f
                        };
                        from = new Point(point) {
                            Z = fishingHole.Z + 20f
                        };
                        if (TraceLine.TraceLineGo(from, point, CGWorldFrameHitFlags.HitTestAllButLiquid))
                        {
                            float num6 = PathFinder.GetZPosition(point, false);
                            if (num6 != 0f)
                            {
                                point.Z = num6 + 1f;
                                return point;
                            }
                        }
                        else
                        {
                            point = new Point(fishingHole.X - num3, fishingHole.Y + num4, fishingHole.Z, "None") {
                                Z = point.Z - 1f
                            };
                            from = new Point(point) {
                                Z = fishingHole.Z + 20f
                            };
                            if (TraceLine.TraceLineGo(from, point, CGWorldFrameHitFlags.HitTestAllButLiquid))
                            {
                                float num7 = PathFinder.GetZPosition(point, false);
                                if (num7 != 0f)
                                {
                                    point.Z = num7 + 1f;
                                    return point;
                                }
                            }
                            else
                            {
                                point = new Point(fishingHole.X - num4, fishingHole.Y - num3, fishingHole.Z, "None") {
                                    Z = point.Z - 1f
                                };
                                from = new Point(point) {
                                    Z = fishingHole.Z + 20f
                                };
                                if (TraceLine.TraceLineGo(from, point, CGWorldFrameHitFlags.HitTestAllButLiquid))
                                {
                                    float num8 = PathFinder.GetZPosition(point, false);
                                    if (num8 != 0f)
                                    {
                                        point.Z = num8 + 1f;
                                        return point;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new Point(0f, 0f, 0f, "invalid");
        }

        public static string FishingPolesName()
        {
            try
            {
                foreach (int num in ListFishingPoles)
                {
                    if (ItemsManager.GetItemCount(num) > 0)
                    {
                        return ItemsManager.GetItemNameById(num);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Fishing > FishingPolesName(): " + exception, true);
            }
            return "";
        }

        public static int GetRandomDraenicBait()
        {
            int[] draenicBaitListInInventory = DraenicBaitListInInventory;
            int index = new Random().Next(0, draenicBaitListInInventory.Length);
            if (draenicBaitListInInventory.Length > 0)
            {
                return draenicBaitListInInventory[index];
            }
            return 0;
        }

        public static bool HaveDraenicBaitBuff()
        {
            foreach (int num in DraenicBaitList)
            {
                if (SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(num)))
                {
                    return true;
                }
            }
            return (SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(0x1c813)) || SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(0x1f4e5)));
        }

        public static bool IsEquipedFishingPoles(string fishingPoleName = "")
        {
            try
            {
                WoWItem equippedItem = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_WEAPON, 1);
                if ((fishingPoleName != string.Empty) && (equippedItem.Name == fishingPoleName))
                {
                    return true;
                }
                if (ListFishingPoles.Contains((uint) equippedItem.Entry))
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Fishing >IsEquipedFishingPoles(string fishingPoleName = \"\"): " + exception, true);
            }
            return false;
        }

        public static string LureName()
        {
            try
            {
                foreach (int num in listLure)
                {
                    if (ItemsManager.GetItemCount(num) > 0)
                    {
                        return ItemsManager.GetItemNameById(num);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Fishing > LureName(): " + exception, true);
            }
            return "";
        }

        public static uint SearchBobber()
        {
            try
            {
                foreach (WoWGameObject obj2 in nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByDisplayId((uint) 0x29c))
                {
                    if (obj2.CreatedBy == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)
                    {
                        return obj2.GetBaseAddress;
                    }
                }
                return 0;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Fishing > SearchBobber(): " + exception, true);
            }
            return 0;
        }

        public static void UseDraenicBait()
        {
            if (((Usefuls.ContinentId == 0x45c) || (Usefuls.ContinentId == 0x5b8)) && !HaveDraenicBaitBuff())
            {
                int entry = 0;
                if ((Usefuls.AreaId == 0x1b5c) || (Usefuls.AreaId == 0x1ba6))
                {
                    entry = GetRandomDraenicBait();
                }
                else if (DraenorSeasList.Contains(Usefuls.SubAreaId))
                {
                    entry = DraenicBaitList[6];
                }
                else if (Usefuls.AreaId == 0x1a3f)
                {
                    entry = DraenicBaitList[0];
                }
                else if (Usefuls.AreaId == 0x1a40)
                {
                    entry = DraenicBaitList[1];
                }
                else if (Usefuls.AreaId == 0x1a41)
                {
                    entry = DraenicBaitList[2];
                }
                else if (Usefuls.AreaId == 0x1a42)
                {
                    entry = DraenicBaitList[3];
                }
                else if (Usefuls.AreaId == 0x1a06)
                {
                    entry = DraenicBaitList[4];
                }
                else if (Usefuls.AreaId == 0x1a63)
                {
                    entry = DraenicBaitList[5];
                }
                else if (Usefuls.ContinentId == 0x5b8)
                {
                    entry = 0x1f4e5;
                }
                if ((entry != 0) && (ItemsManager.GetItemCount(entry) > 0))
                {
                    string itemNameById = ItemsManager.GetItemNameById(entry);
                    if (ItemsManager.IsItemUsable(itemNameById))
                    {
                        ItemsManager.UseItem(itemNameById);
                    }
                }
            }
        }

        public static void UseLure(string lureName = "", bool automaticallyUseDraenorBait = false)
        {
            try
            {
                if (automaticallyUseDraenorBait)
                {
                    UseDraenicBait();
                }
                if (IsEquipedFishingPoles("") && !nManager.Wow.ObjectManager.ObjectManager.Me.IsMainHandTemporaryEnchanted)
                {
                    if (lureName != string.Empty)
                    {
                        if (ItemsManager.GetItemCount(ItemsManager.GetItemIdByName(lureName)) > 0)
                        {
                            ItemsManager.UseItem(lureName);
                            Thread.Sleep(0x3e8);
                            Thread.Sleep(Usefuls.Latency);
                            while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(200);
                            }
                            return;
                        }
                        Spell spell = new Spell(lureName);
                        if (spell.KnownSpell && spell.IsSpellUsable)
                        {
                            spell.Launch();
                            return;
                        }
                        if (!spell.KnownSpell && FirstLureCheck)
                        {
                            Logging.Write("Lure from Product Settings is missing, try to use a lure from the list built-in TheNoobBot.");
                            FirstLureCheck = false;
                        }
                    }
                    foreach (int num in listLure)
                    {
                        if (ItemsManager.GetItemCount(num) > 0)
                        {
                            ItemsManager.UseItem(ItemsManager.GetItemNameById(num));
                            Thread.Sleep(0x3e8);
                            while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(200);
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Fishing > UseLure(string lureName = \"\"): " + exception, true);
            }
        }

        public static int[] DraenicBaitListInInventory
        {
            get
            {
                List<int> list = new List<int>();
                foreach (int num in DraenicBaitList)
                {
                    if (ItemsManager.GetItemCount(num) > 0)
                    {
                        list.Add(num);
                    }
                }
                return list.ToArray();
            }
        }

        private static List<uint> ListFishingPoles
        {
            get
            {
                try
                {
                    if (_listFishingPoles == null)
                    {
                        _listFishingPoles = new List<uint>();
                        foreach (string str in Others.ReadFileAllLines(Application.StartupPath + @"\Data\fishingPoles.txt"))
                        {
                            try
                            {
                                _listFishingPoles.Add(Others.ToUInt32(str));
                            }
                            catch
                            {
                            }
                        }
                    }
                    return _listFishingPoles;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Fishing > ListFishingPoles : " + exception, true);
                    return new List<uint>();
                }
            }
        }

        private static IEnumerable<uint> listLure
        {
            get
            {
                try
                {
                    if (_listLure == null)
                    {
                        _listLure = new List<uint>();
                        foreach (string str in Others.ReadFileAllLines(Application.StartupPath + @"\Data\lures.txt"))
                        {
                            try
                            {
                                _listLure.Add(Others.ToUInt32(str));
                            }
                            catch
                            {
                            }
                        }
                    }
                    return _listLure;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Fishing > listLure : " + exception, true);
                    return new List<uint>();
                }
            }
        }
    }
}

