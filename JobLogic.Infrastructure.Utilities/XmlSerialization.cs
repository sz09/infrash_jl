using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace JobLogic.Infrastructure.Utilities
{
    public static class XmlSerialization
    {
        /// <summary>
        /// Serializes object to an XML document.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static XmlDocument SerializeToXmlDocument(this object obj)
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(obj.GetType());
            XmlDocument xmlDocument;

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj, emptyNamespaces);
                stream.Position = 0;

                var settings = new XmlReaderSettings { IgnoreWhitespace = true };
                using (var xtr = XmlReader.Create(stream, settings))
                {
                    xmlDocument = new XmlDocument();
                    xmlDocument.Load(xtr);
                }
            }

            return xmlDocument;
        }

        /// <summary>
        /// Serializes object to XML string without any namespaces.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToXmlStringWithoutNamespaces(this object obj)
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(obj.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, obj, emptyNamespaces);
                return stream.ToString();
            }
        }

        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
