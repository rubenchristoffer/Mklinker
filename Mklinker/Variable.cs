using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Mklinker {

	[Serializable]
	class Variable {

		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Value")]
		public string value;

		private Variable() {}

		public Variable (string name, string value) {
			this.name = name;
			this.value = value;
		}

	}

}
