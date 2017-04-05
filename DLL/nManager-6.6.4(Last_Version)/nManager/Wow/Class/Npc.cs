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
        private FactionType _boeda;
        private Point _igaqivigae = new Point();
        private NpcType _loedacItuoj;
        private int _neaxeisievixoeFeuxov;
        private UInt128 _ofiejaefoe;
        private int _ojalapeo;
        private string _witiweMiv = "NoName";
        [DefaultValue(false)]
        public bool ForceTravel;
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
                return this._ojalapeo;
            }
            set
            {
                this._ojalapeo = value;
            }
        }

        public int Entry
        {
            get
            {
                return this._neaxeisievixoeFeuxov;
            }
            set
            {
                this._neaxeisievixoeFeuxov = value;
            }
        }

        [DefaultValue(0)]
        public FactionType Faction
        {
            get
            {
                return this._boeda;
            }
            set
            {
                this._boeda = value;
            }
        }

        [XmlIgnore]
        public UInt128 Guid
        {
            get
            {
                return this._ofiejaefoe;
            }
            set
            {
                this._ofiejaefoe = value;
            }
        }

        [XmlIgnore]
        public string InternalData { get; set; }

        public string Name
        {
            get
            {
                return this._witiweMiv;
            }
            set
            {
                this._witiweMiv = value;
            }
        }

        public Point Position
        {
            get
            {
                return this._igaqivigae;
            }
            set
            {
                this._igaqivigae = value;
            }
        }

        [DefaultValue(0)]
        public NpcType Type
        {
            get
            {
                return this._loedacItuoj;
            }
            set
            {
                this._loedacItuoj = value;
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

