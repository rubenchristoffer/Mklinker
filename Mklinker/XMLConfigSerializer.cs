using Mklinker.Abstractions;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Mklinker {

	class XMLConfigSerializer : IConfigSerializer {

		public string Serialize(IConfig config) {
			XmlSerializer serializer = new XmlSerializer(config.GetType());

			using (StringWriter writer = new StringWriter()) {
				serializer.Serialize(writer, config);
				return writer.ToString();
			}
		}

		public IConfig Deserialize(string serializedString) {
			if (serializedString.Length == 0)
				throw new SerializationException("Serialized string is empty");

			// TODO: Fix specific type here later
			XmlSerializer serializer = new XmlSerializer(typeof(Config));

			using (StringReader reader = new StringReader(serializedString)) {
				return (Config) serializer.Deserialize(reader);
			}
		}

	}

}
