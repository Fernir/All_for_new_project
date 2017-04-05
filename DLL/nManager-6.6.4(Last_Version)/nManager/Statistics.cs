namespace nManager
{
    using nManager.Helpful;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;

    public static class Statistics
    {
        private static int _buwaubiwe;
        private static int _cavagAre;
        private static int _cuepui;
        private static uint _etoufaureowoIvibetiem;
        private static uint _guetiafavarHiwuosiur;
        private static uint _liniujonuoEq;
        private static uint _niepoili;
        private static int _okahicoefu;
        private static uint _wotodoula;

        public static int DeathsByHr()
        {
            try
            {
                if (_niepoili <= 0)
                {
                    return 0;
                }
                if (Others.TimesSec <= _cuepui)
                {
                    return 0;
                }
                return (int) ((_niepoili * 0xe10) / (Others.TimesSec - _cuepui));
            }
            catch (Exception exception)
            {
                Logging.WriteError("DeathsByHr(): " + exception, true);
                return 0;
            }
        }

        public static int ExperienceByHr()
        {
            try
            {
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Experience <= _cavagAre)
                {
                    return 0;
                }
                if (Others.TimesSec <= _cuepui)
                {
                    return 0;
                }
                ulong num = (ulong) ((nManager.Wow.ObjectManager.ObjectManager.Me.Experience - _cavagAre) * 0xe10L);
                return (int) (num / ((long) (Others.TimesSec - _cuepui)));
            }
            catch (Exception exception)
            {
                Logging.WriteError("ExperienceByHr(): " + exception, true);
                return 0;
            }
        }

        public static int FarmsByHr()
        {
            try
            {
                if (_liniujonuoEq <= 0)
                {
                    return 0;
                }
                if (Others.TimesSec <= _cuepui)
                {
                    return 0;
                }
                return (int) ((_liniujonuoEq * 0xe10) / (Others.TimesSec - _cuepui));
            }
            catch (Exception exception)
            {
                Logging.WriteError("FarmsByHr(): " + exception, true);
                return 0;
            }
        }

        public static int HonorByHr()
        {
            try
            {
                if ((!Usefuls.InGame || Usefuls.IsLoading) || (Usefuls.GetHonorPoint < 0))
                {
                    return 0;
                }
                if (Usefuls.GetHonorPoint >= 0)
                {
                    if (_buwaubiwe >= 0)
                    {
                        if (Usefuls.GetHonorPoint < _buwaubiwe)
                        {
                            _buwaubiwe = Usefuls.GetHonorPoint;
                            return 0;
                        }
                        if (Others.TimesSec <= _cuepui)
                        {
                            return 0;
                        }
                        return (((Usefuls.GetHonorPoint - _buwaubiwe) * 0xe10) / (Others.TimesSec - _cuepui));
                    }
                    if (_buwaubiwe < 0)
                    {
                        _buwaubiwe = Usefuls.GetHonorPoint;
                        return 0;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("HonorByHr(): " + exception, true);
                return 0;
            }
            return 0;
        }

        public static int KillsByHr()
        {
            try
            {
                if (_guetiafavarHiwuosiur <= 0)
                {
                    return 0;
                }
                if (Others.TimesSec <= _cuepui)
                {
                    return 0;
                }
                return (int) ((_guetiafavarHiwuosiur * 0xe10) / (Others.TimesSec - _cuepui));
            }
            catch (Exception exception)
            {
                Logging.WriteError("KillsByHr(): " + exception, true);
                return 0;
            }
        }

        public static int LootsByHr()
        {
            try
            {
                if (_wotodoula <= 0)
                {
                    return 0;
                }
                if (Others.TimesSec <= _cuepui)
                {
                    return 0;
                }
                return (int) ((_wotodoula * 0xe10) / (Others.TimesSec - _cuepui));
            }
            catch (Exception exception)
            {
                Logging.WriteError("LootsByHr(): " + exception, true);
                return 0;
            }
        }

        public static void Reset()
        {
            _wotodoula = 0;
            _guetiafavarHiwuosiur = 0;
            _niepoili = 0;
            _liniujonuoEq = 0;
            _etoufaureowoIvibetiem = 0;
            _okahicoefu = 0x57;
            _cuepui = Others.TimesSec;
            _cavagAre = nManager.Wow.ObjectManager.ObjectManager.Me.Experience;
            if ((Usefuls.InGame && Usefuls.IsLoading) && (Usefuls.GetHonorPoint >= 0))
            {
                _buwaubiwe = Usefuls.GetHonorPoint;
            }
            else
            {
                _buwaubiwe = -1;
            }
        }

        public static int RunningTimeInSec()
        {
            try
            {
                if (Others.TimesSec <= _cuepui)
                {
                    return 0;
                }
                return (Others.TimesSec - _cuepui);
            }
            catch (Exception exception)
            {
                Logging.WriteError("RunningTime(): " + exception, true);
                return 0;
            }
        }

        public static uint Deaths
        {
            get
            {
                return _niepoili;
            }
            set
            {
                _niepoili = value;
            }
        }

        public static uint Farms
        {
            get
            {
                return _liniujonuoEq;
            }
            set
            {
                _liniujonuoEq = value;
            }
        }

        public static uint Kills
        {
            get
            {
                return _guetiafavarHiwuosiur;
            }
            set
            {
                _guetiafavarHiwuosiur = value;
            }
        }

        public static uint Loots
        {
            get
            {
                return _wotodoula;
            }
            set
            {
                _wotodoula = value;
            }
        }

        public static int OffsetStats
        {
            get
            {
                return _okahicoefu;
            }
            set
            {
                _okahicoefu = value;
            }
        }

        public static uint Stucks
        {
            get
            {
                return _etoufaureowoIvibetiem;
            }
            set
            {
                _etoufaureowoIvibetiem = value;
            }
        }
    }
}

