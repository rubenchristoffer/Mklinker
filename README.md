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
	<a href="https://coveralls.io/github/rubenchristoffer/Mklinker?branch=master">
		<img src="https://coveralls.io/repos/github/rubenchristoffer/Mklinker/badge.svg?branch=master" alt="Coverage Status" />
	</a>
	<a href="../../releases/latest">
		<img src="https://img.shields.io/github/v/release/rubenchristoffer/Mklinker.svg?style=flat" alt="Release" />
	</a>
	<a href="../../blob/master/LICENSE">
		<img src="https://img.shields.io/github/license/rubenchristoffer/Mklinker.svg?style=flat" alt="License" />
	</a>
</p>

Mklinker is a console utility that let's you create multiple symbolic / junction / hard links at once based on a single config file. Just copy the Mklinker.exe binary and run "mklinker" in a console window in order to create the config file 'linker.config' and use the commands below to setup links.

## Why use Mklinker?
- With Mklinker you can keep the config file around and if you for example re-install your operating system you can just use Mklinker and restore all the various links you had before.
- With Mklinker you can send someone the config file and they can easily create all the links. 

## Features
- Mklinker supports symbolic, junction and hard links on Windows platform (linux / mac support is planned)
- Supports relative paths in config file, but will turn them into absolute paths when running mklink commands.
