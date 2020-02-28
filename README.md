# Mklinker
<a href="../../releases/latest">
	<img src="https://travis-ci.org/rubenchristoffer/Mklinker.svg?branch=master" />
</a>

Mklinker is a console utility that let's you create multiple symbolic / junction / hard links at once based on a single config file. Just copy the Mklinker.exe binary and run "mklinker" in a console window in order to create the config file 'linker.config' and use the commands below to setup links.

## Why use Mklinker?
- With Mklinker you can keep the config file around and if you for example re-install your operating system you can just use Mklinker and restore all the various links you had before.
- With Mklinker you can send someone the config file and they can easily create all the links. 

## Features
- Mklinker supports symbolic, junction and hard links on Windows platforms.
- Supports relative paths in config file, but will turn them into absolute paths when running mklink commands.

## Commands
**LinkAll**:    Runs mklink commands for each link element in the config file.  
**AddLink**:    Adds a new link to the config file.  
**RemoveLink**: Removes a link in the config file.  
**List**:       Lists all the link elements in the config file.  
**Validate**:	Validates all link elements to see if it has an incorrect configuration or if source path does not exist. By default it only shows the link elements that are incorrect, but you can add the "all" argument to display all config elements regardless of validation outcome.  
