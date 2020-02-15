using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace Mklinker {

	[XmlRoot("Config")]
	public class Config {

		public const string configFile = "linker.config";

		[XmlArray("Elements")]
		[XmlArrayItem("Element")]
		public List<ConfigElement> elements { get; private set; }

		public enum LinkType {
			Junction,
			Symbolic,
			Hard
		}
		
		public Config () {
			elements = new List<ConfigElement>();
		}

		public ConfigElement[] GetElements () {
			return elements.ToArray();
		}

		public string Serialize () {
			XmlSerializer serializer = new XmlSerializer(GetType());

			using (StringWriter writer = new StringWriter()) {
				serializer.Serialize(writer, this);
				return writer.ToString();
			}
		}

		public static Config Deserialize (string xml) {
			if (xml.Length == 0)
				return new Config();

			XmlSerializer serializer = new XmlSerializer(typeof(Config));

			using (StringReader reader = new StringReader(xml)) {
				return (Config) serializer.Deserialize(reader);
			}
		}

	}

}
