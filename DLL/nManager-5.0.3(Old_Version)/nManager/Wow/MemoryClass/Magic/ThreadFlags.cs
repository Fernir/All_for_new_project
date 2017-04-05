namespace nManager.Wow.MemoryClass.Magic
{
    using System;

    public static class ThreadFlags
    {
        public const uint CREATE_SUSPENDED = 4;
        public const uint STACK_SIZE_PARAM_IS_A_RESERVATION = 0x10000;
        public const uint STILL_ACTIVE = 0x103;
        public const uint THREAD_EXECUTE_IMMEDIATELY = 0;
    }
}

