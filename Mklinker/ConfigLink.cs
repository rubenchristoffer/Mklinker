﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Mklinker {

	public struct ConfigLink {

		[XmlElement("SourcePath")]
		public string sourcePath;

		[XmlElement("TargetPath")]
		public string targetPath;

		[XmlAttribute ("Type")]
		public LinkType linkType;

		public enum LinkType {
			None,
			Junction,
			Symbolic,
			Hard
		}

		public ConfigLink(string sourcePath, string targetPath, LinkType linkType) {
			this.sourcePath = sourcePath;
			this.targetPath = targetPath;
			this.linkType = linkType;
		}

	}

}
