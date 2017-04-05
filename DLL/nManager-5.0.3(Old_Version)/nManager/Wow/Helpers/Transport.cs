namespace nManager.Wow.Helpers
{
    using nManager.Wow.Class;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    public class Transport
    {
        [DefaultValue(0), XmlAttribute(AttributeName="AContinentId")]
        public int AContinentId;
        public Point AInsidePoint = new Point();
        public Point AOutsidePoint = new Point();
        public Point APoint = new Point();
        [XmlIgnore]
        public bool ArrivalIsA;
        [XmlAttribute(AttributeName="BContinentId"), DefaultValue(0)]
        public int BContinentId;
        public Point BInsidePoint = new Point();
        public Point BOutsidePoint = new Point();
        public Point BPoint = new Point();
        [XmlAttribute(AttributeName="Faction")]
        public Npc.FactionType Faction;
        [XmlAttribute(AttributeName="Id")]
        public uint Id;
        [XmlAttribute(AttributeName="Name")]
        public string Name;
    }
}

