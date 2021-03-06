﻿using System.Linq;
using CommandLine;
using System.IO.Abstractions;
using Mklinker.Abstractions;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.CompilerServices;
using System;

namespace Mklinker.Commands {

	[Verb ("addlinks", HelpText = "Adds multiple new links at once to config file with optional filtering")]
	class AddLinksCommand : GlobalOptions {

		[Value (0, HelpText = "The path to the source directory", Required = true)]
		public string sourceDirectoryPath { get; private set; }

		[Value (1, HelpText = "The path to the target directory", Required = true)]
		public string targetDirectoryPath { get; private set; }

		[Value (2, Default = ConfigLink.LinkType.Default, HelpText = "The type of link you want to create. Default is Symbolic for files and Junction for directories")]
		public ConfigLink.LinkType linkType { get; private set; }

		[Option('r', "regex", Default = @"[\s\S]*", HelpText = "Regex filter deciding which files / directories to add links for in source folder. It matches based on file / directory name only. Default will match everything", Required = false)]
		public string regexFilter { get; private set; }

		[Option ('a', "absolute-regex", Default = @"[\s\S]*", HelpText = "Additional Regex filter deciding which files / directories to add links for in source folder. It matches based on absolute (full) path. Default will match everything. Both regex filters have to be matched in order for file / directory to be added", Required = false)]
		public string absoluteRegexFilter { get; private set; }

		[Option('s', "subdirs", Default = false, HelpText = "Determines if files (not directories) from subdirectories are included as well (recursive)", Required = false)]
		public bool includeSubdirectories { get; private set; }

		[Option('d', "dirs", Default = false, HelpText = "Determines if directory links should be created instead of file links. ", Required = false)]
		public bool linkDirectories { get; private set; }

		public AddLinksCommand () : base () { }

		public AddLinksCommand (string sourceDirectoryPath, string targetDirectoryPath, ConfigLink.LinkType linkType, string regexFilter, string absoluteRegexFilter, bool includeSubdirectories, bool linkDirectories, string path) : base (path) {
			this.sourceDirectoryPath = sourceDirectoryPath;
			this.targetDirectoryPath = targetDirectoryPath;
			this.linkType = linkType;
			this.regexFilter = regexFilter;
			this.absoluteRegexFilter = absoluteRegexFilter;
			this.includeSubdirectories = includeSubdirectories;
			this.linkDirectories = linkDirectories;
		}

		internal void Execute (IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathResolver pathResolver) {
			if (!configHandler.DoesConfigExist(path)) {
				console.WriteLine($"Config '{ path }' does not exist. Type 'help config' in order to see how you create a config file.", IConsole.ContentType.Negative);
				return;
			}

			IConfig config = configHandler.LoadConfig(path);
			CreateLinks (console, configHandler, fileSystem, pathResolver, config, sourceDirectoryPath, targetDirectoryPath);
		}

		internal void CreateLinks (IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathResolver pathResolver, IConfig config, string sourceDirectory, string targetDirectory) {
			if (!linkDirectories) {
				foreach (string file in fileSystem.Directory.GetFiles (pathResolver.GetAbsoluteResolvedPath (sourceDirectory, config.Variables))) {
					TryCreateLink (console, configHandler, fileSystem, pathResolver, config, file, targetDirectory, true);
				}

				// Recursive - Will create links for all files in sub-dirs as well
				if (includeSubdirectories) {
					foreach (string directory in fileSystem.Directory.GetDirectories (pathResolver.GetAbsoluteResolvedPath (sourceDirectory, config.Variables))) {
						CreateLinks (console, configHandler, fileSystem, pathResolver, config, directory, fileSystem.Path.Combine (targetDirectory, fileSystem.Path.GetFileName (directory)));
					}
				}
			} else {
				foreach (string directory in fileSystem.Directory.GetDirectories (pathResolver.GetAbsoluteResolvedPath (sourceDirectory, config.Variables))) {
					TryCreateLink (console, configHandler, fileSystem, pathResolver, config, directory, targetDirectory, false);
				}
			}
		}

		internal void TryCreateLink (IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathResolver pathResolver, IConfig config, string sourcePath, string targetBasePath, bool isFile) {
			try {
				// Check absolute path regex filter first
				if (Regex.IsMatch (pathResolver.GetAbsoluteResolvedPath(sourcePath, config.Variables), absoluteRegexFilter)) {
					string fileOrDirectoryName = fileSystem.Path.GetFileName (sourcePath);

					// Check file / directory name regex filter second
					if (Regex.IsMatch (fileOrDirectoryName, regexFilter)) {
						AddLinkCommand addLinkCommand = new AddLinkCommand (
						sourcePath,
						fileSystem.Path.Combine(targetBasePath, fileOrDirectoryName),
						linkType,
						path);

						addLinkCommand.Execute (console, configHandler, fileSystem, pathResolver);
					}
				}
			} catch (ArgumentException) {
				console.WriteLine ("Regex provided is invalid!", IConsole.ContentType.Negative);
			}
		}

	}

}
