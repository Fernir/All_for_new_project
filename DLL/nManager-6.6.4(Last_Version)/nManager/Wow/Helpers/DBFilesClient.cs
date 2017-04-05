namespace nManager.Wow.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [Serializable]
    public class DBFilesClient
    {
        public static DBFilesClient Load(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DBFilesClient));
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                DBFilesClient client = (DBFilesClient) serializer.Deserialize(stream);
                client.File = path;
                return client;
            }
        }

        public static void Save(DBFilesClient db)
        {
            string directoryName = Path.GetDirectoryName(db.File);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(DBFilesClient));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using (FileStream stream = new FileStream(db.File, FileMode.Create))
            {
                serializer.Serialize(stream, db, namespaces);
            }
        }

        [XmlIgnore]
        public string File { get; set; }

        [XmlElement("Table")]
        public List<Table> Tables { get; set; }
    }
}

