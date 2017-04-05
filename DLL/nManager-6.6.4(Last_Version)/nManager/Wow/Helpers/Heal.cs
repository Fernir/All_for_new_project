namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using System;
    using System.Runtime.CompilerServices;

    public class Heal
    {
        public static void StartHeal()
        {
            try
            {
                Logging.Write("Start healing process.");
                IsHealing = true;
                if (MovementManager.InMovement)
                {
                    MovementManager.StopMove();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartHealBot(): " + exception, true);
                IsHealing = true;
            }
        }

        public static void StartHealBot()
        {
            try
            {
                Logging.Write("Start healing process.");
                IsHealing = true;
                if (MovementManager.InMovement)
                {
                    MovementManager.StopMove();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartHealBot(): " + exception, true);
                IsHealing = true;
            }
        }

        public static void StopHeal()
        {
            try
            {
                Logging.Write("Stop healing process.");
                IsHealing = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StopHeal(): " + exception, true);
                IsHealing = false;
            }
        }

        public static bool IsHealing
        {
            [CompilerGenerated]
            get
            {
                return <IsHealing>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <IsHealing>k__BackingField = value;
            }
        }
    }
}

