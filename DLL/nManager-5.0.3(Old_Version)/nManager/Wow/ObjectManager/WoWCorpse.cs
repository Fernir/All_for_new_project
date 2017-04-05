namespace nManager.Wow.ObjectManager
{
    using nManager.Helpful;
    using nManager.Wow.Patchables;
    using System;

    public class WoWCorpse : WoWObject
    {
        public WoWCorpse(uint address) : base(address)
        {
        }

        public T GetDescriptor<T>(Descriptors.CorpseFields field) where T: struct
        {
            try
            {
                return base.GetDescriptor<T>((uint) field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.CorpseFields field): " + exception, true);
                return default(T);
            }
        }
    }
}

