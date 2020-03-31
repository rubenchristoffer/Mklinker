using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using Mklinker.Commands;

namespace Mklinker {

	class ArgumentHandler : IArgumentHandler {

		public string[] ParseStringToArguments(string input) {
			return input.Split('"')
				.Select((element, index) => index % 2 == 0 ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { element })
				.SelectMany(element => element).ToArray();
		}

	}

}
