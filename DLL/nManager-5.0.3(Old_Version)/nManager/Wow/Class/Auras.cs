namespace nManager.Wow.Class
{
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Auras
    {
        public class UnitAura
        {
            public override string ToString()
            {
                return string.Format("AuraCasterLevel: {1}{0}AuraCount: {2}{0}AuraCreatorGUID: {3}{0}AuraDuration: {4}{0}AuraFlags: {5}{0}AuraSpellEndTime: {6}{0}AuraSpellId: {7}{0}AuraTimeLeftInMs: {8}{0}AuraMask: {9}{0}AuraUnk1: {10}{0}AuraUnk2: {11}{0}", new object[] { Environment.NewLine, this.AuraCasterLevel, this.AuraCount, this.AuraCreatorGUID, this.AuraDuration, this.AuraFlag, this.AuraSpellEndTime, this.AuraSpellId, this.AuraTimeLeftInMs, this.AuraMask.ToString("X8"), this.AuraUnk1, this.AuraUnk2 });
            }

            public void TryCancel()
            {
                if (this.Cancellable)
                {
                    Lua.LuaDoString(string.Format("for i = 1,40 do local spellId = select(11, UnitAura('player', i)) if spellId == {0} then CancelUnitBuff('player', i) end end", this.AuraSpellId), false, true);
                }
            }

            public byte AuraCasterLevel { get; set; }

            public int AuraCount { get; set; }

            public UInt128 AuraCreatorGUID { get; set; }

            public int AuraDuration { get; set; }

            public byte AuraFlag { get; set; }

            public uint AuraMask { get; set; }

            public int AuraSpellEndTime { get; set; }

            public uint AuraSpellId { get; set; }

            public int AuraTimeLeftInMs
            {
                get
                {
                    if (this.AuraSpellEndTime == 0)
                    {
                        return 0;
                    }
                    return (this.AuraSpellEndTime - Usefuls.GetWoWTime);
                }
            }

            public byte AuraUnk1 { get; set; }

            public uint AuraUnk2 { get; set; }

            public bool Cancellable
            {
                get
                {
                    return this.Flags.HasFlag(UnitAuraFlags.Cancelable);
                }
            }

            public UnitAuraFlags Flags
            {
                get
                {
                    return (UnitAuraFlags) this.AuraFlag;
                }
            }

            public bool IsActive
            {
                get
                {
                    return this.Flags.HasFlag(UnitAuraFlags.Active);
                }
            }

            public bool IsHarmful
            {
                get
                {
                    return this.Flags.HasFlag(UnitAuraFlags.Harmful);
                }
            }

            public bool IsPassive
            {
                get
                {
                    if (!this.Flags.HasFlag(UnitAuraFlags.None | UnitAuraFlags.Passive))
                    {
                        return this.Flags.HasFlag(UnitAuraFlags.None);
                    }
                    return true;
                }
            }

            public bool IsValid
            {
                get
                {
                    return ((this.AuraCount != -1) && (this.AuraCreatorGUID > 0));
                }
            }
        }

        public class UnitAuras
        {
            public UnitAuras(uint baseAddress)
            {
                this.UnitBaseAddress = baseAddress;
                this.Auras = new List<nManager.Wow.Class.Auras.UnitAura>();
            }

            public List<nManager.Wow.Class.Auras.UnitAura> Auras { get; set; }

            private uint UnitBaseAddress { get; set; }
        }
    }
}

