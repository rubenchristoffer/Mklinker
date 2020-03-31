using System;
using System.Xml.Serialization;

namespace Mklinker {

	public class ConfigLink {

		[XmlElement("SourcePath")]
		public string sourcePath;

		[XmlElement("TargetPath")]
		public string targetPath;

		[XmlAttribute ("Type")]
		public LinkType linkType;

		public enum LinkType {
			Default,
			Junction,
			Symbolic,
			Hard
		}

		public ConfigLink () {}

		public ConfigLink(string sourcePath, string targetPath, LinkType linkType) {
			this.sourcePath = sourcePath;
			this.targetPath = targetPath;
			this.linkType = linkType;
		}

		public override string ToString() {
			return String.Format("{0} link:\n\t- Target: {1}\n\t- Source: {2}", linkType.ToString(), targetPath, sourcePath);
		}
		
	}

}
