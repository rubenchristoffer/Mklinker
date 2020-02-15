using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using LinkType = Mklinker.Config.LinkType;

namespace Mklinker {

	public struct ConfigElement {

		public string sourcePath;
		public string targetPath;
		public LinkType linkType;

		public ConfigElement(string sourcePath, string targetPath, LinkType linkType) {
			this.sourcePath = sourcePath;
			this.targetPath = targetPath;
			this.linkType = linkType;
		}

	}

}
