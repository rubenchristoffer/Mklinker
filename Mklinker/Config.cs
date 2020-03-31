using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Mklinker.Abstractions;

namespace Mklinker {

	[XmlRoot("Config")]
	public class Config : IConfig {

		[XmlAttribute("Version")]
		public string Version { get; set; }

		[XmlArray("Variables")]
		public List<Variable> Variables { get; private set; }

		[XmlArray("LinkList")]
		[XmlArrayItem("Link")]
		public List<ConfigLink> LinkList { get; private set; }

		public Config() : this("Undefined") {}

		public Config(string version) {
			Variables = new List<Variable>();
			LinkList = new List<ConfigLink>();
			Version = version;
		}

		string IConfig.Serialize() {
			XmlSerializer serializer = new XmlSerializer(GetType());

			using (StringWriter writer = new StringWriter()) {
				serializer.Serialize(writer, this);
				return writer.ToString();
			}
		}

		IConfig IConfig.Deserialize(string xml) {
			if (xml.Length == 0)
				return new Config();

			XmlSerializer serializer = new XmlSerializer(typeof(Config));

			using (StringReader reader = new StringReader(xml)) {
				return (Config)serializer.Deserialize(reader);
			}
		}

	}

}
