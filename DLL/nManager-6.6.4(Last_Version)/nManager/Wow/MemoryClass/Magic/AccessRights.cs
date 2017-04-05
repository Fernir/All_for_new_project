namespace nManager.Wow.MemoryClass.Magic
{
    using System;

    public static class AccessRights
    {
        public const uint PROCESS_ALL_ACCESS = 0x1f0fff;
        public const uint PROCESS_CREATE_PROCESS = 0x80;
        public const uint PROCESS_CREATE_THREAD = 2;
        public const uint PROCESS_DUP_HANDLE = 0x40;
        public const uint PROCESS_QUERY_INFORMATION = 0x400;
        public const uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
        public const uint PROCESS_SET_INFORMATION = 0x200;
        public const uint PROCESS_SET_QUOTA = 0x100;
        public const uint PROCESS_SUSPEND_RESUME = 0x800;
        public const uint PROCESS_TERMINATE = 1;
        public const uint PROCESS_VM_OPERATION = 8;
        public const uint PROCESS_VM_READ = 0x10;
        public const uint PROCESS_VM_WRITE = 0x20;
        public const uint STANDARD_RIGHTS_REQUIRED = 0xf0000;
        public const uint SYNCHRONIZE = 0x100000;
        public const uint THREAD_ALL_ACCESS = 0x1f03ff;
        public const uint THREAD_DIRECT_IMPERSONATION = 0x200;
        public const uint THREAD_GET_CONTEXT = 8;
        public const uint THREAD_IMPERSONATE = 0x100;
        public const uint THREAD_QUERY_INFORMATION = 0x40;
        public const uint THREAD_SET_CONTEXT = 0x10;
        public const uint THREAD_SET_INFORMATION = 0x20;
        public const uint THREAD_SET_THREAD_TOKEN = 0x80;
        public const uint THREAD_SUSPEND_RESUME = 2;
        public const uint THREAD_TERMINATE = 1;
    }
}

