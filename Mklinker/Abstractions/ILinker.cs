using System;
using System.Collections.Generic;
using System.Text;

namespace Mklinker.Abstractions {

	interface ILinker {

		bool CreateLink(string resolvedTargetPath, string resolvedSourcePath, ConfigLink.LinkType linkType);

	}

}
