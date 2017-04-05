namespace nManager.Wow.Class
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class SpellList
    {
        public SpellList()
        {
        }

        public SpellList(uint id, string name = "")
        {
            this.Id = id;
            this.Name = name;
        }

        public uint Id { get; set; }

        public string Name { get; set; }
    }
}

