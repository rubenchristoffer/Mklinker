# Mklinker
Mklinker is a console utility that let's you create multiple symbolic / junction / hard links at once based on a single config file. Just copy the Mklinker.exe binary and run "mklinker" in order to create the config file 'linker.config' and use the commands below to setup links.

## Why use Mklinker?
- With Mklinker you can keep the config file around and if you for example re-install your operating system you can just use Mklinker and restore all the various links you had before.
- With Mklinker you can send someone the config file and they can easily create all the links. 

## Commands
LinkAll - Runs mklink commands for each link element in the config file.
AddLink - Adds a new link to the config file.
RemoveLink - Removes a link in the config file.
List - Lists all the link elements in the config file.
