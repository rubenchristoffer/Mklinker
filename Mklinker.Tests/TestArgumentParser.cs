using System;
using System.Collections.Generic;
using System.Text;
using Mklinker.Abstractions;
using System.Linq;

namespace Mklinker.Tests {

	class TestArgumentParser : IArgumentParser {

		public string[] ParseStringToArguments(string input) {
			return input.Split('"')
				.Select((element, index) => index % 2 == 0 ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { element })
				.SelectMany(element => element).ToArray();
		}

	}

}
