namespace nManager.Wow.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [Serializable]
    public class Field
    {
        public Field Clone()
        {
            return (Field) base.MemberwiseClone();
        }

        [DefaultValue(1), XmlAttribute]
        public int ArraySize { get; set; }

        [XmlAttribute, DefaultValue("")]
        public string Format { get; set; }

        [XmlIgnore]
        public int Index { get; set; }

        [XmlAttribute, DefaultValue(false)]
        public bool IsIndex { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        [DefaultValue(true), XmlAttribute]
        public bool Visible { get; set; }

        [DefaultValue(100), XmlAttribute]
        public int Width { get; set; }
    }
}

