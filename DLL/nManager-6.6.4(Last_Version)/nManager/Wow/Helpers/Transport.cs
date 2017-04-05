namespace nManager.Wow.Helpers
{
    using nManager.Wow.Class;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    public class Transport
    {
        [XmlAttribute(AttributeName="AContinentId"), DefaultValue(-1)]
        public int AContinentId = -1;
        public Point AInsidePoint = new Point();
        [DefaultValue(0)]
        public uint ALift;
        public Point AOutsidePoint = new Point();
        public Point APoint = new Point();
        [XmlIgnore]
        public bool ArrivalIsA;
        [XmlAttribute(AttributeName="BContinentId"), DefaultValue(-1)]
        public int BContinentId = -1;
        public Point BInsidePoint = new Point();
        [DefaultValue(0)]
        public uint BLift;
        public Point BOutsidePoint = new Point();
        public Point BPoint = new Point();
        [XmlAttribute(AttributeName="Faction")]
        public Npc.FactionType Faction;
        [XmlAttribute(AttributeName="Id")]
        public uint Id;
        [XmlAttribute(AttributeName="Name")]
        public string Name;
        [XmlIgnore]
        public bool UseALift;
        [XmlIgnore]
        public bool UseBLift;

        public bool ShouldSerializeAInsidePoint()
        {
            return this.AInsidePoint.IsValid;
        }

        public bool ShouldSerializeAOutsidePoint()
        {
            return this.AOutsidePoint.IsValid;
        }

        public bool ShouldSerializeAPoint()
        {
            return this.APoint.IsValid;
        }

        public bool ShouldSerializeBInsidePoint()
        {
            return this.AOutsidePoint.IsValid;
        }

        public bool ShouldSerializeBOutsidePoint()
        {
            return this.BOutsidePoint.IsValid;
        }

        public bool ShouldSerializeBPoint()
        {
            return this.BPoint.IsValid;
        }
    }
}

