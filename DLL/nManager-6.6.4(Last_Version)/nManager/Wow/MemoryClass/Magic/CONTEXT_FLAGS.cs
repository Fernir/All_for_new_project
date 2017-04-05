namespace nManager.Wow.MemoryClass.Magic
{
    using System;

    public static class CONTEXT_FLAGS
    {
        private const uint _aviunagaecaSa = 0x10000;
        private const uint _labeudo = 0x10000;
        public const uint CONTEXT_ALL = 0x1003f;
        public const uint CONTEXT_CONTROL = 0x10001;
        public const uint CONTEXT_DEBUG_REGISTERS = 0x10010;
        public const uint CONTEXT_EXTENDED_REGISTERS = 0x10020;
        public const uint CONTEXT_FLOATING_POINT = 0x10008;
        public const uint CONTEXT_FULL = 0x10007;
        public const uint CONTEXT_INTEGER = 0x10002;
        public const uint CONTEXT_SEGMENTS = 0x10004;
    }
}

