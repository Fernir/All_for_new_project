namespace nManager.Helpful
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public static class XmlSerializer
    {
        public static T Deserialize<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            T local = (T) serializer.Deserialize(stream);
            stream.Close();
            return local;
        }

        public static bool Serialize(string path, object @object)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
            FileStream stream = null;
            try
            {
                using (stream = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        if (@object.GetType().ToString() == "Quester.Profile.QuesterProfile")
                        {
                            XmlAttributes attributes2 = new XmlAttributes();
                            XmlTypeAttribute attribute = new XmlTypeAttribute {
                                Namespace = ""
                            };
                            attributes2.XmlType = attribute;
                            attributes2.Xmlns = false;
                            XmlAttributes attributes = attributes2;
                            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
                            overrides.Add(@object.GetType(), attributes);
                            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(@object.GetType(), overrides);
                            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                            namespaces.Add("", "");
                            serializer.Serialize(writer, @object, namespaces);
                        }
                        else
                        {
                            new System.Xml.Serialization.XmlSerializer(@object.GetType()).Serialize((TextWriter) writer, @object);
                        }
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                try
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                catch
                {
                }
                Logging.WriteError("Serialize(String path, object @object)#2: " + exception, true);
                MessageBox.Show("XML Serialize: " + exception);
            }
            return false;
        }
    }
}

