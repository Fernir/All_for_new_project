namespace nManager.Wow.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    public class CustomPath : Transport
    {
        [DefaultValue(false)]
        public bool AllowFar;
        public List<Point> Points;
        [DefaultValue(0), XmlAttribute(AttributeName="RequireAchievementId")]
        public int RequireAchivementId;
        [XmlAttribute(AttributeName="RequireQuestId"), DefaultValue(0)]
        public int RequireQuestId;
        [DefaultValue(true)]
        public bool RoundTrip = true;
        [DefaultValue(false)]
        public bool UseMount;
    }
}

