namespace nManager.Wow.Helpers.PathFinderClass
{
    using DetourLayer;
    using nManager.Wow.Class;
    using System;
    using System.Runtime.CompilerServices;

    public static class Utility
    {
        public static float[] Origin;

        static Utility()
        {
            float[] numArray = new float[3];
            numArray[0] = -17066.67f;
            numArray[2] = -17066.67f;
            Origin = numArray;
        }

        public static string GetDetourSupportedVersion()
        {
            int num = 740;
            return num.ToString();
        }

        public static bool HasFailed(this DetourStatus status)
        {
            return status.HasFlag((DetourStatus) (-2147483648));
        }

        public static bool HasSucceeded(this DetourStatus status)
        {
            return status.HasFlag(DetourStatus.Success);
        }

        public static bool IsInProgress(this DetourStatus status)
        {
            return status.HasFlag(DetourStatus.InProgress);
        }

        public static bool IsPartialResult(this DetourStatus status)
        {
            return status.HasFlag(DetourStatus.PartialResult);
        }

        public static bool IsWrongVersion(this DetourStatus status)
        {
            return status.HasFlag(DetourStatus.WrongVersion);
        }

        public static float[] ToFloatArray(this Point v)
        {
            return new float[] { v.X, v.Y, v.Z };
        }

        public static float[] ToRecast(this float[] v)
        {
            return new float[] { -v[1], v[2], -v[0] };
        }

        public static Point ToRecast(this Point v)
        {
            return new Point(-v.Y, v.Z, -v.X, "None");
        }

        public static float[] ToWoW(this float[] v)
        {
            return new float[] { -v[2], -v[0], v[1] };
        }

        public static Point ToWoW(this Point v)
        {
            return new Point(-v.Z, -v.X, v.Y, "None");
        }

        public static float TileSize
        {
            get
            {
                return 533.3333f;
            }
        }
    }
}

