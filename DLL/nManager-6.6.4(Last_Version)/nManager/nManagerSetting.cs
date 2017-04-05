namespace nManager
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.MemoryClass;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    [Serializable]
    public class nManagerSetting : Settings
    {
        private static readonly Dictionary<UInt128, int> _blackListGuidByTime = new Dictionary<UInt128, int>();
        private static readonly Dictionary<Point, float> _blackListZone = new Dictionary<Point, float>();
        private static string _esuileqeje = "";
        private static nManagerSetting _kuobuboanoeb;
        public bool ActivateAlwaysOnTopFeature;
        public bool ActivateAutoFacingDamageDealer;
        public bool ActivateAutoFacingHealerBot;
        public bool ActivateAutoMaillingFeature;
        public bool ActivateAutoMilling;
        public bool ActivateAutoProspecting;
        public bool ActivateAutoRepairFeature = true;
        public bool ActivateAutoSellingFeature = true;
        public bool ActivateAutoSmelting;
        public bool ActivateBeastSkinning = true;
        public bool ActivateBroadcastingMimesis;
        public bool ActivateChestLooting;
        public List<string> ActivatedPluginsList = new List<string>();
        public bool ActivateHerbsHarvesting = true;
        public bool ActivateLootStatistics = true;
        public bool ActivateMonsterLooting = true;
        public bool ActivateMovementsDamageDealer;
        public bool ActivateMovementsHealerBot;
        public bool ActivatePathFindingFeature = true;
        public bool ActivatePluginsSystem = true;
        public static bool ActivateProductTipOff = true;
        public bool ActivateReloggerFeature;
        public bool ActivateSafeResurrectionSystem = true;
        public bool ActivateSkillsAutoTraining = true;
        public bool ActivateVeinsHarvesting = true;
        public bool ActiveStopTNBAfterXLevelup;
        public bool ActiveStopTNBAfterXMinutes;
        public bool ActiveStopTNBAfterXStucks;
        public bool ActiveStopTNBIfReceivedAtMostXWhispers;
        public bool AllowTNBToSetYourMaxFps = true;
        public string AquaticMountName = "";
        public bool AutoAssignTalents;
        public bool AutoCloseChatFrame = true;
        public bool AutoConfirmOnBoPItems = true;
        public static string AutoStartBattleNet = "";
        public static string AutoStartCharacter = "";
        public static string AutoStartEmail = "";
        public static bool AutoStartLoggingInfoProvided = false;
        public static string AutoStartPassword = "";
        public static bool AutoStartProduct = false;
        public static string AutoStartProductName = "";
        public static string AutoStartProfileName = "";
        public static string AutoStartRealmName = "";
        public string BattleNetSubAccount = "";
        public bool BeastNinjaSkinning;
        public bool BecomeApprenticeIfNeededByProduct;
        public bool BecomeApprenticeOfSecondarySkillsWhileQuesting;
        public string BeverageName = "";
        public int BroadcastingPort = 0x198f;
        public bool CanPullUnitsAlreadyInFight = true;
        public string CombatClass = "OfficialTnbClassSelector";
        public bool DeactivateFlyingMount;
        public int DontHarvestIfMoreThanXUnitInAggroRange = 4;
        public float DontHarvestIfPlayerNearRadius;
        public List<string> DontHarvestTheFollowingObjects = new List<string>();
        public List<string> DontMailTheseItems = new List<string>();
        public bool DontPullMonsters;
        public bool DontSellReagents = true;
        public List<string> DontSellTheseItems = new List<string>();
        public bool DoRegenManaIfLow;
        public int DrinkBeverageWhenManaIsUnderXPercent = 0x23;
        public int EatFoodWhenHealthIsUnderXPercent = 0x23;
        public string EmailOfTheBattleNetAccount = "";
        public string FlyingMountName = "";
        public string FoodName = "";
        public List<string> ForceToMailTheseItems = new List<string>();
        public List<string> ForceToSellTheseItems = new List<string>();
        public float GatheringSearchRadius = 70f;
        public string GroundMountName = "";
        public bool HarvestDuringLongDistanceMovements;
        public string HealerClass = "Tnb_HealerClass.dll";
        public List<string> HerbsToBeMilled = new List<string>();
        public bool HideCharacterNameFromTitle;
        public bool HideSdkFiles = true;
        public bool IgnoreFightIfMounted = true;
        public string LastProductLoaded;
        public bool LaunchExpiredPlugins;
        public bool MailBlue = true;
        public bool MailGray;
        public bool MailGreen = true;
        public string MaillingFeatureRecipient = "";
        public string MaillingFeatureSubject = "Hey";
        public bool MailPurple = true;
        public bool MailWhite = true;
        public bool MakeStackOfElementalsItems = true;
        public float MaxDistanceToGoToMailboxesOrNPCs = 4000f;
        public List<string> MineralsToProspect = new List<string>();
        public uint MinimumDistanceToUseMount = 15;
        public int NumberOfBeverageWeGot;
        public int NumberOfFoodsWeGot;
        public bool OnlyTrainCurrentlyUsedSkills = true;
        public bool OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum = true;
        public bool OnlyUseMillingInTown;
        public bool OnlyUseProspectingInTown;
        public string PasswordOfTheBattleNetAccount = "";
        public bool PauseTNBIfNearByPlayer;
        public bool PlayASongIfNewWhispReceived;
        public bool RecordWhispsInLogFiles = true;
        public int RepairWhenDurabilityIsUnderPercent = 0x23;
        public bool SellBlue;
        public bool SellGray = true;
        public bool SellGreen;
        public int SellItemsWhenLessThanXSlotLeft = 2;
        public bool SellPurple;
        public bool SellWhite;
        public int SendMailWhenLessThanXSlotLeft = 2;
        public int StopTNBAfterXLevelup = 110;
        public int StopTNBAfterXMinutes = 0x5a0;
        public int StopTNBAfterXStucks = 80;
        public bool StopTNBIfBagAreFull;
        public bool StopTNBIfHonorPointsLimitReached;
        public bool StopTNBIfPlayerHaveBeenTeleported;
        public int StopTNBIfReceivedAtMostXWhispers = 10;
        public int TimeBetweenEachMillingAttempt = 15;
        public int TimeBetweenEachProspectingAttempt = 15;
        public bool TrainMountingCapacity = true;
        public bool UseFrameLock;
        public bool UseGroundMount = true;
        public bool UseHearthstone;
        public bool UseLootARange = true;
        public bool UseMollE;
        public bool UseRobot;
        public bool UseSpiritHealer;

        public static void AddBlackList(UInt128 guid, int timeInMilisec = -1)
        {
            try
            {
                if (Information.DevMode)
                {
                    Logging.WriteDebug(string.Concat(new object[] { "Blacklist (", guid, ") for ", timeInMilisec, "ms from ", Hook.CurrentCallStack }));
                }
                if (_blackListGuidByTime.ContainsKey(guid))
                {
                    _blackListGuidByTime.Remove(guid);
                }
                if (timeInMilisec >= 0)
                {
                    timeInMilisec += Others.Times;
                }
                _blackListGuidByTime.Add(guid, timeInMilisec);
            }
            catch (Exception exception)
            {
                Logging.WriteError("AddBlackList(UInt128 guid, int timeInMilisec = -1): " + exception, true);
            }
        }

        public static void AddBlackListZone(Point position, float radius)
        {
            try
            {
                if (!_blackListZone.ContainsKey(position))
                {
                    _blackListZone.Add(position, radius);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("AddBlackListZone(Point position, float radius): " + exception, true);
            }
        }

        public static void AddRangeBlackListZone(Dictionary<Point, float> listBlackZone)
        {
            try
            {
                foreach (KeyValuePair<Point, float> pair in listBlackZone)
                {
                    if (!_blackListZone.ContainsKey(pair.Key))
                    {
                        _blackListZone.Add(pair.Key, pair.Value);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("AddRangeBlackListZone(Dictionary<Point, float> listBlackZone): " + exception, true);
            }
        }

        public static List<UInt128> GetListGuidBlackListed()
        {
            try
            {
                List<UInt128> list = new List<UInt128>();
                foreach (KeyValuePair<UInt128, int> pair in _blackListGuidByTime)
                {
                    if ((pair.Value == -1) || (pair.Value <= Others.Times))
                    {
                        list.Add(pair.Key);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetListGuidBlackListed(): " + exception, true);
                return new List<UInt128>();
            }
        }

        public static Dictionary<Point, float> GetListZoneBlackListed()
        {
            try
            {
                return _blackListZone;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetListZoneBlackListed(): " + exception, true);
                return new Dictionary<Point, float>();
            }
        }

        public static bool IsBlackListed(UInt128 guid)
        {
            try
            {
                return (_blackListGuidByTime.ContainsKey(guid) && ((_blackListGuidByTime[guid] >= Others.Times) || (_blackListGuidByTime[guid] == -1)));
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsBlackListed(UInt128 guid): " + exception, true);
                return false;
            }
        }

        public static bool IsBlackListedZone(Point position)
        {
            try
            {
                foreach (KeyValuePair<Point, float> pair in _blackListZone)
                {
                    float introduced4 = pair.Key.DistanceTo(position);
                    if (introduced4 <= pair.Value)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsBlackListedZone(Point position): " + exception, true);
                return false;
            }
        }

        public static bool Load()
        {
            try
            {
                if (File.Exists(Settings.AdviserFilePathAndName("General")))
                {
                    CurrentSetting = Settings.Load<nManagerSetting>(Settings.AdviserFilePathAndName("General"));
                    return true;
                }
                CurrentSetting = new nManagerSetting();
            }
            catch (Exception exception)
            {
                Logging.WriteError("nManagerSetting > Load(): " + exception, true);
            }
            return false;
        }

        public bool Save()
        {
            try
            {
                return base.Save(Settings.AdviserFilePathAndName("General"));
            }
            catch (Exception exception)
            {
                Logging.WriteError("nManagerSetting > Save(): " + exception, true);
                return false;
            }
        }

        public static nManagerSetting CurrentSetting
        {
            get
            {
                if ((_kuobuboanoeb == null) || ((nManager.Wow.ObjectManager.ObjectManager.Me.Name != _esuileqeje) && nManager.Wow.Helpers.Usefuls.InGame))
                {
                    Load();
                    _esuileqeje = nManager.Wow.ObjectManager.ObjectManager.Me.Name;
                }
                return _kuobuboanoeb;
            }
            set
            {
                _kuobuboanoeb = value;
            }
        }
    }
}

