namespace nManager.Wow.Class
{
    using nManager.Wow.Helpers;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [Serializable]
    public class Npc
    {
        private int _continentId;
        private int _entry;
        private FactionType _faction;
        private UInt128 _guid;
        private string _name = "NoName";
        private Point _position = new Point();
        private NpcType _type;
        [DefaultValue(0)]
        public int SelectGossipOption;

        public string ContinentId
        {
            get
            {
                return Usefuls.ContinentNameByContinentId(this.ContinentIdInt);
            }
            set
            {
                this.ContinentIdInt = Usefuls.ContinentIdByContinentName(value);
            }
        }

        [XmlIgnore]
        public int ContinentIdInt
        {
            get
            {
                return this._continentId;
            }
            set
            {
                this._continentId = value;
            }
        }

        public int Entry
        {
            get
            {
                return this._entry;
            }
            set
            {
                this._entry = value;
            }
        }

        [DefaultValue(0)]
        public FactionType Faction
        {
            get
            {
                return this._faction;
            }
            set
            {
                this._faction = value;
            }
        }

        [XmlIgnore]
        public UInt128 Guid
        {
            get
            {
                return this._guid;
            }
            set
            {
                this._guid = value;
            }
        }

        public string InternalData { get; set; }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public Point Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        [DefaultValue(0)]
        public NpcType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [Serializable]
        public enum FactionType
        {
            Neutral,
            Horde,
            Alliance
        }

        [Serializable]
        public enum NpcType
        {
            None,
            Vendor,
            Repair,
            AuctionHouse,
            Mailbox,
            DruidTrainer,
            RogueTrainer,
            WarriorTrainer,
            PaladinTrainer,
            HunterTrainer,
            PriestTrainer,
            DeathKnightTrainer,
            ShamanTrainer,
            MageTrainer,
            WarlockTrainer,
            AlchemyTrainer,
            BlacksmithingTrainer,
            EnchantingTrainer,
            EngineeringTrainer,
            HerbalismTrainer,
            InscriptionTrainer,
            JewelcraftingTrainer,
            LeatherworkingTrainer,
            TailoringTrainer,
            MiningTrainer,
            SkinningTrainer,
            CookingTrainer,
            FirstAidTrainer,
            FishingTrainer,
            ArchaeologyTrainer,
            RidingTrainer,
            BattlePetTrainer,
            SmeltingForge,
            RuneForge,
            MonkTrainer,
            FlightMaster,
            SpiritHealer,
            SpiritGuide,
            Innkeeper,
            Banker,
            Battlemaster,
            Auctioneer,
            StableMaster,
            GuildBanker,
            QuestGiver
        }
    }
}

