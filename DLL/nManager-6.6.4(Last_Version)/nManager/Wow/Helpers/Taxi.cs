namespace nManager.Wow.Helpers
{
    using nManager.Wow.Class;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    public class Taxi : Transport
    {
        [XmlAttribute(AttributeName="ContinentId")]
        public int ContinentId;
        [DefaultValue(0)]
        public uint EndOfPath;
        public Point Position = new Point();
        [XmlAttribute(AttributeName="RequireAchievementId"), DefaultValue(0)]
        public int RequireAchivementId;
        [DefaultValue(0), XmlAttribute(AttributeName="RequireQuestId")]
        public int RequireQuestId;
        [DefaultValue("")]
        public string Xcoord;
        [DefaultValue("")]
        public string Ycoord;
    }
}

