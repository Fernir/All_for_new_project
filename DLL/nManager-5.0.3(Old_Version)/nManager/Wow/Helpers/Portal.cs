namespace nManager.Wow.Helpers
{
    using nManager.Wow.Class;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    public class Portal : Transport
    {
        [XmlAttribute(AttributeName="AContinentId")]
        public int AContinentId;
        public Point APoint = new Point();
        [XmlAttribute(AttributeName="BContinentId")]
        public int BContinentId;
        public Point BPoint = new Point();
        [XmlAttribute(AttributeName="Faction")]
        public Npc.FactionType Faction;
        [XmlAttribute(AttributeName="Id")]
        public uint Id;
        [XmlAttribute(AttributeName="Name")]
        public string Name;
        [DefaultValue(0), XmlAttribute(AttributeName="RequireAchievementId")]
        public int RequireAchivementId;
        [DefaultValue(0), XmlAttribute(AttributeName="RequireQuestId")]
        public int RequireQuestId;
    }
}

