namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Db2Caller
    {
        public static uint WowClientDB2__GetRowPointer(int index, uint offset, bool box = false)
        {
            try
            {
                Usefuls.UpdateLastHardwareAction();
                string[] asm = new string[] { "push 0", "push 0", "push 0", "push " + Memory.WowProcess.WowModule, "push " + index, "mov ecx, " + (Memory.WowProcess.WowModule + offset), "call " + (Memory.WowProcess.WowModule + 0x21aa3f), "retn" };
                uint num = Memory.WowMemory.InjectAndExecute(asm);
                if (box)
                {
                    MessageBox.Show(num.ToString("X"));
                }
                return num;
            }
            catch (Exception exception)
            {
                Logging.WriteError("WowClientDB2__GetRowPointer(int index, uint offset): " + exception, true);
            }
            return 0;
        }
    }
}

