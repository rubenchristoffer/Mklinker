using System;
using System.Collections.Generic;
using System.Text;

namespace Mklinker.Abstractions {

	interface ILinker {

		bool CreateLink(string resolvedSourcePath, string resolvedTargetPath, ConfigLink.LinkType linkType);

	}

}
