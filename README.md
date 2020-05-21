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

## Why use Mklinker?
- With Mklinker you can keep the config file around and if you for example re-install your operating system you can just use Mklinker and restore all the various links you had before.
- With Mklinker you can send someone the config file and they can easily create all the links.  
- Mklinker is cross-platform 

## Installation
### Using pre-compiled binaries
Just copy the Mklinker binary from <a href="../../releases/">releases</a> (you have to unzip the zip file and copy the file for your operating system) and run "mklinker" in a console window (or whatever you rename your file to) and a help menu will tell you the commands you can use.

### Using NuGet
Go to NuGet package <a href="https://www.nuget.org/packages/Mklinker/">here</a> and copy command listed under .NET CLI to install Mklinker as a global tool. You can then run `mklinker` from the command-line globally.

### From source
Clone the repository and run publish.bat and it will compile the binaries and put them into `Mklinker/bin` in the format `Mklinker-x.y.z-OS`. 

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
