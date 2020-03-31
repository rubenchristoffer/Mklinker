using System;
using System.Linq;
using Mklinker.Abstractions;

namespace Mklinker {

	class ArgumentParser : IArgumentParser {

		public string[] ParseStringToArguments(string input) {
			return input.Split('"')
				.Select((element, index) => index % 2 == 0 ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { element })
				.SelectMany(element => element).ToArray();
		}

	}

}
