namespace nManager.Wow.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [Serializable]
    public class Table
    {
        public Table Clone()
        {
            Table table = new Table {
                Name = this.Name,
                Build = this.Build,
                Fields = new List<Field>()
            };
            foreach (Field field in this.Fields)
            {
                table.Fields.Add(field.Clone());
            }
            return table;
        }

        [XmlAttribute]
        public int Build { get; set; }

        [XmlElement("Field")]
        public List<Field> Fields { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
    }
}

