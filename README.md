# Mklinker
<p align="left">
	<a href="https://travis-ci.com/rubenchristoffer/Mklinker">
		<img src="https://travis-ci.com/rubenchristoffer/Mklinker.svg?branch=master" alt="Build" />
	</a>
	<a href="https://ci.appveyor.com/project/rubenchristoffer/Mklinker">
		<img src="https://ci.appveyor.com/api/projects/status/dc9ohkt96solg9cj?svg=true" alt="AppVeyor Build" />
	</a>
	<a href="https://ci.appveyor.com/project/rubenchristoffer/Mklinker/build/tests">
		<img src="https://img.shields.io/appveyor/tests/rubenchristoffer/Mklinker.svg" alt="AppVeyor Tests">
	</a>
	<a href="https://rubenchristoffer.github.io/Mklinker/CoverageReport/">
		<img src="https://img.shields.io/badge/coverage_report-generated-green.svg?style=flat" alt="Coverage Report" />
	</a>
	<a href="../../releases/latest">
		<img src="https://img.shields.io/github/v/release/rubenchristoffer/Mklinker.svg?style=flat" alt="Release" />
	</a>
	<a href="https://www.nuget.org/packages/Mklinker/">
		<img src="https://img.shields.io/nuget/v/Mklinker.svg?style=flat" alt="NuGet" />
	</a>
	<a href="../../blob/master/LICENSE">
		<img src="https://img.shields.io/github/license/rubenchristoffer/Mklinker.svg?style=flat" alt="License" />
	</a>
</p>

Mklinker is a cross-platform console utility that let's you create multiple symbolic / junction / hard links at once based on a single config file. 

## Why use Mklinker? Who cares about symbolic links?
- With Mklinker you can keep the config file around and if you for example re-install your operating system you can just use Mklinker and restore all the various links you had before. This is especially useful if you are using a syncing program like Dropbox as it allows you to easily add links to outside folders like `AppData`, `Program Files` or `My Documents`
- With Mklinker you can easily setup outside references in your projects (you're welcome to use it in your github repos!)
- With Mklinker you can send someone the config file and they can easily create all the links 
- Mklinker is cross-platform (windows, linux, mac)

## Installation
### Using NuGet (preferred)
Installing using NuGet is the easiest and preferred way as it allows you to call `mklinker` commands globally using command-line. `Note that you need .NET Core 3.1 SDK installed` (download <a href="https://dotnet.microsoft.com/download/dotnet-core/3.1">here</a>).

#### Instructions
Go to NuGet package <a href="https://www.nuget.org/packages/Mklinker/">here</a> and copy command listed under .NET CLI to install Mklinker as a global tool. You can then run `mklinker` from the command-line globally.

### Using pre-compiled binaries
Using pre-compiled binaries is a viable alternative if you do not have the .NET SDK installed and just want to run mklinker locally rather than globally. Note that it *is* possible to make it global, but in order to do so you would have to add the folder where you copy mklinker to the <a href="https://en.wikipedia.org/wiki/PATH_(variable)">PATH environmental variable</a> or copy mklinker to an existing folder that is specified in the PATH environmental variable.

#### Instructions
1.  Download the ZIP file from from <a href="../../releases/">releases</a> containing Mklinker binaries
1.  Unzip the ZIP file (I personally recommend using <a href="https://7-zip.org/">7-Zip</a> for this)
1.  Copy the file for your operating system and architecture and put it into a folder of your choosing
1.  Run "mklinker" in a console window (or whatever you rename your file to) and a help menu will tell you the commands you can use.

### From source code
If you are a purist or simply paranoid it is possible to compile the binaries yourself from the source code!

#### Instructions
1.  Clone the repository using `git clone https://github.com/rubenchristoffer/Mklinker.git` (you need git installed!)
1.  Run `publish.bat` (you need <a href="https://dotnet.microsoft.com/download/dotnet-core/3.1">.NET Core 3.1 SDK</a> installed)
 
The binaries will then be compiled and put into the `Mklinker/bin/` folder in the format `Mklinker-x.y.z-OS`. You can then copy the binaries to wherever you want to install and run Mklinker. See the instructions above `Using pre-compiled binaries` for more info about what to do with the binaries you just compiled.

## Features
- Supports symbolic, junction and hard links on Windows platform  
- Supports symbolic and hard links on Linux and Mac platforms  
- Supports relative paths in config file, but will turn them into absolute paths when running linking commands.  
- Supports variables for paths (e.g. "C:\Users\\?User?\Desktop") which means that you can create more dynamic configs. Nested variables (variables inside variables) are also supported.  
- Supports custom config file name (if you don't like 'linker.config')
- Supports config validation where you can check for errors and warnings
- Supports interactive mode where you can run multiple commands without 'Mklinker' in front
- Supports a scan command which will help with finding circular paths that can arise due to symbolic directories / junctions pointing to a parent folder
- Supports adding multiple links at once using optional regex filters that also works for sub-directories (allows for recursion)

## How do I use Mklinker?
Run `mklinker help` to see all verbs / commands and their description. You can also run `mklinker help [VERB]` or `mklinker [VERB] --help` in order to get more information about that given verb / command.
