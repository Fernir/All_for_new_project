namespace nManager.Wow.Helpers
{
    using nManager.Wow.Class;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    public class Taxi : Transport
    {
        [XmlIgnore]
        public Point AInsidePoint;
        [XmlIgnore]
        public Point AOutsidePoint;
        [XmlIgnore]
        public Point APoint;
        [XmlIgnore]
        public Point BInsidePoint;
        [XmlIgnore]
        public Point BOutsidePoint;
        [XmlIgnore]
        public Point BPoint;
        [XmlAttribute(AttributeName="ContinentId")]
        public int ContinentId;
        [DefaultValue(0)]
        public uint EndOfPath;
        [XmlAttribute(AttributeName="Faction")]
        public Npc.FactionType Faction;
        [XmlAttribute(AttributeName="Id")]
        public uint Id;
        [XmlAttribute(AttributeName="Name")]
        public string Name;
        public Point Position = new Point();
        [XmlAttribute(AttributeName="RequireAchievementId"), DefaultValue(0)]
        public int RequireAchivementId;
        [XmlAttribute(AttributeName="RequireQuestId"), DefaultValue(0)]
        public int RequireQuestId;
        [DefaultValue("")]
        public string Xcoord;
        [DefaultValue("")]
        public string Ycoord;
    }
}

