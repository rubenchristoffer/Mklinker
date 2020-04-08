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

	}

}
