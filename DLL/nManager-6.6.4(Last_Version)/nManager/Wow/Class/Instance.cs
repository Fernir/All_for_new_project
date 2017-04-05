namespace nManager.Wow.Class
{
    using nManager.Wow.Enums;
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class Instance
    {
        [XmlAttribute("Active")]
        public bool Active;
        public Point DepartureLocation = new Point();
        public int EntranceContinentId = -1;
        public Point EntranceLocation = new Point();
        public int InstanceContinentId = -1;
        [XmlAttribute("Id")]
        public int InstanceId;
        [XmlAttribute("Level")]
        public uint InstanceLevel = 110;
        [XmlAttribute("Name")]
        public string InstanceName = "None";
        public float InstancePriority = 1f;
        public bool KillBosses;
        public uint LastUnitId;
        public bool Reset = true;
        public InstanceType Type;
    }
}

