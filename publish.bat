@echo off
dotnet clean
dotnet restore
cd Mklinker
dotnet pack

dotnet publish -r win-x64 -c Release -o bin/Mklinker-2.1.2-Win64 /p:PublishSingleFile=true
dotnet publish -r win-x86 -c Release -o bin/Mklinker-2.1.2-Win32 /p:PublishSingleFile=true

dotnet publish -r linux-x64 -c Release -o bin/Mklinker-2.1.2-Linux64 /p:PublishSingleFile=true
dotnet publish -r linux-arm -c Release -o bin/Mklinker-2.1.2-LinuxARM /p:PublishSingleFile=true

dotnet publish -r osx-x64 -c Release -o bin/Mklinker-2.1.2-OSX64 /p:PublishSingleFile=true
pause