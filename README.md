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
	<a href="../../blob/master/LICENSE">
		<img src="https://img.shields.io/github/license/rubenchristoffer/Mklinker.svg?style=flat" alt="License" />
	</a>
</p>

Mklinker is a cross-platform console utility that let's you create multiple symbolic / junction / hard links at once based on a single config file. Just copy the Mklinker binary from <a href="../../releases/">releases</a> and run "mklinker" in a console window in order to create the config file 'linker.config' and use the commands below to setup links.

## Why use Mklinker?
- With Mklinker you can keep the config file around and if you for example re-install your operating system you can just use Mklinker and restore all the various links you had before.
- With Mklinker you can send someone the config file and they can easily create all the links.  
- Mklinker is cross-platform  

## Features
- Supports symbolic, junction and hard links on Windows platform  
- Supports symbolic and hard links on Linux and Mac platforms  
- Supports relative paths in config file, but will turn them into absolute paths when running linking commands.  
- Supports variables for paths (e.g. "C:\Users\\?User?\Desktop") which means that you can create more dynamic configs. Nested variables (variables inside variables) are also supported.  
- Supports custom config file name (if you don't like 'linker.config')
- Supports config validation where you can check for errors and warnings
- Supports interactive mode where you can run multiple commands without 'Mklinker' in front

## How do I use Mklinker?
Run `Mklinker help` to see all verbs / commands. You can also run `Mklinker help [VERB]` in order to get more information about that given verb / command.
