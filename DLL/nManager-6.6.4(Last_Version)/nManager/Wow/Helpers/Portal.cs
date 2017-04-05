namespace nManager.Wow.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    public class Portal : Transport
    {
        [DefaultValue(0), XmlAttribute(AttributeName="RequireAchievementId")]
        public int RequireAchivementId;
        [XmlAttribute(AttributeName="RequireQuestId"), DefaultValue(0)]
        public int RequireQuestId;
    }
}

