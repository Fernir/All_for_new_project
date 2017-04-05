namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Tracker
    {
        public static void TrackCreatureFlags(List<string> listTrackCreatureFlags)
        {
            try
            {
                nManager.Wow.Enums.TrackCreatureFlags flags = listTrackCreatureFlags.Aggregate<string, nManager.Wow.Enums.TrackCreatureFlags>(0, (current, s) => current | ((nManager.Wow.Enums.TrackCreatureFlags) Enum.Parse(typeof(nManager.Wow.Enums.TrackCreatureFlags), s, true)));
                nManager.Wow.ObjectManager.ObjectManager.Me.MeCreatureTrack = flags;
            }
            catch (Exception exception)
            {
                Logging.WriteError("TrackCreatureFlags(List<string> listTrackCreatureFlags): " + exception, true);
            }
        }

        public static void TrackObjectFlags(List<string> listTrackObjectFlags)
        {
            try
            {
                nManager.Wow.Enums.TrackObjectFlags flags = listTrackObjectFlags.Aggregate<string, nManager.Wow.Enums.TrackObjectFlags>(nManager.Wow.Enums.TrackObjectFlags.None, (current, s) => current | ((nManager.Wow.Enums.TrackObjectFlags) Enum.Parse(typeof(nManager.Wow.Enums.TrackObjectFlags), s, true)));
                nManager.Wow.ObjectManager.ObjectManager.Me.MeObjectTrack = flags;
            }
            catch (Exception exception)
            {
                Logging.WriteError("TrackObjectFlags(List<string> listTrackObjectFlags): " + exception, true);
            }
        }
    }
}

