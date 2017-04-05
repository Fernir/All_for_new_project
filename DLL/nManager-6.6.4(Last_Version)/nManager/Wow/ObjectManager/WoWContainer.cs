namespace nManager.Wow.ObjectManager
{
    using nManager.Helpful;
    using nManager.Wow.Patchables;
    using System;

    public class WoWContainer : WoWItem
    {
        public WoWContainer(uint address) : base(address)
        {
        }

        public T GetDescriptor<T>(Descriptors.ContainerFields field) where T: struct
        {
            try
            {
                return base.Tuecom<T>((uint) field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.ContainerFields field): " + exception, true);
                return default(T);
            }
        }

        public int GetSlot(int slot)
        {
            try
            {
                slot--;
                if ((slot < 0) || (slot > this.NumberSlot))
                {
                    return 0;
                }
                return base.Tuecom<int>((uint) (0xe5 + (slot * 8)));
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSlot(int slot): " + exception, true);
                return 0;
            }
        }

        public int NumberSlot
        {
            get
            {
                try
                {
                    return this.GetDescriptor<int>(Descriptors.ContainerFields.NumSlots);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("NumberSlot: " + exception, true);
                    return 0;
                }
            }
        }
    }
}

