namespace nManager
{
    using nManager.Helpful;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;

    public static class Statistics
    {
        private static uint _deaths;
        private static uint _farms;
        private static uint _kills;
        private static uint _loots;
        private static int _offSetStats;
        private static int _startHonor;
        private static int _startTime;
        private static int _startXp;
        private static uint _stucks;

        public static int DeathsByHr()
        {
            try
            {
                if (_deaths <= 0)
                {
                    return 0;
                }
                if (Others.TimesSec <= _startTime)
                {
                    return 0;
                }
                return (int) ((_deaths * 0xe10) / (Others.TimesSec - _startTime));
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
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Experience <= _startXp)
                {
                    return 0;
                }
                if (Others.TimesSec <= _startTime)
                {
                    return 0;
                }
                ulong num = (ulong) ((nManager.Wow.ObjectManager.ObjectManager.Me.Experience - _startXp) * 0xe10L);
                return (int) (num / ((long) (Others.TimesSec - _startTime)));
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
                if (_farms <= 0)
                {
                    return 0;
                }
                if (Others.TimesSec <= _startTime)
                {
                    return 0;
                }
                return (int) ((_farms * 0xe10) / (Others.TimesSec - _startTime));
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
                if ((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (Usefuls.GetHonorPoint < 0))
                {
                    return 0;
                }
                if (Usefuls.GetHonorPoint >= 0)
                {
                    if (_startHonor >= 0)
                    {
                        if (Usefuls.GetHonorPoint < _startHonor)
                        {
                            _startHonor = Usefuls.GetHonorPoint;
                            return 0;
                        }
                        if (Others.TimesSec <= _startTime)
                        {
                            return 0;
                        }
                        return (((Usefuls.GetHonorPoint - _startHonor) * 0xe10) / (Others.TimesSec - _startTime));
                    }
                    if (_startHonor < 0)
                    {
                        _startHonor = Usefuls.GetHonorPoint;
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
                if (_kills <= 0)
                {
                    return 0;
                }
                if (Others.TimesSec <= _startTime)
                {
                    return 0;
                }
                return (int) ((_kills * 0xe10) / (Others.TimesSec - _startTime));
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
                if (_loots <= 0)
                {
                    return 0;
                }
                if (Others.TimesSec <= _startTime)
                {
                    return 0;
                }
                return (int) ((_loots * 0xe10) / (Others.TimesSec - _startTime));
            }
            catch (Exception exception)
            {
                Logging.WriteError("LootsByHr(): " + exception, true);
                return 0;
            }
        }

        public static void Reset()
        {
            _loots = 0;
            _kills = 0;
            _deaths = 0;
            _farms = 0;
            _stucks = 0;
            _offSetStats = 0x57;
            _startTime = Others.TimesSec;
            _startXp = nManager.Wow.ObjectManager.ObjectManager.Me.Experience;
            if ((Usefuls.InGame && Usefuls.IsLoadingOrConnecting) && (Usefuls.GetHonorPoint >= 0))
            {
                _startHonor = Usefuls.GetHonorPoint;
            }
            else
            {
                _startHonor = -1;
            }
        }

        public static int RunningTimeInSec()
        {
            try
            {
                if (Others.TimesSec <= _startTime)
                {
                    return 0;
                }
                return (Others.TimesSec - _startTime);
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
                return _deaths;
            }
            set
            {
                _deaths = value;
            }
        }

        public static uint Farms
        {
            get
            {
                return _farms;
            }
            set
            {
                _farms = value;
            }
        }

        public static uint Kills
        {
            get
            {
                return _kills;
            }
            set
            {
                _kills = value;
            }
        }

        public static uint Loots
        {
            get
            {
                return _loots;
            }
            set
            {
                _loots = value;
            }
        }

        public static int OffsetStats
        {
            get
            {
                return _offSetStats;
            }
            set
            {
                _offSetStats = value;
            }
        }

        public static uint Stucks
        {
            get
            {
                return _stucks;
            }
            set
            {
                _stucks = value;
            }
        }
    }
}

