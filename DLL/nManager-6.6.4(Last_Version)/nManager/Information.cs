namespace nManager
{
    using System;

    public static class Information
    {
        public static bool DevMode = !"6.6.4".Contains(".");
        public static string MainTitle = "The Noob Bot 6.6.4";
        public const int MaxWowBuild = 0x6590;
        public const int MinWowBuild = 0x42e9;
        public static string SchedulerTitle = ("The Noob Scheduler for " + MainTitle);
        public const int TargetWowBuild = 0x5d31;
        public const string TargetWowVersion = "7.2.0";
        public const string Version = "6.6.4";
    }
}

