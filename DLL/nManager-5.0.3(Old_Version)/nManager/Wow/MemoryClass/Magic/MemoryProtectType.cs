namespace nManager.Wow.MemoryClass.Magic
{
    using System;

    public static class MemoryProtectType
    {
        public const uint PAGE_EXECUTE = 0x10;
        public const uint PAGE_EXECUTE_READ = 0x20;
        public const uint PAGE_EXECUTE_READWRITE = 0x40;
        public const uint PAGE_EXECUTE_WRITECOPY = 0x80;
        public const uint PAGE_GUARD = 0x100;
        public const uint PAGE_NOACCESS = 1;
        public const uint PAGE_NOCACHE = 0x200;
        public const uint PAGE_READONLY = 2;
        public const uint PAGE_READWRITE = 4;
        public const uint PAGE_WRITECOMBINE = 0x400;
        public const uint PAGE_WRITECOPY = 8;
    }
}

