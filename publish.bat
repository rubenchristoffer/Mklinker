@echo off
set version=2.1.3

dotnet clean
dotnet restore
cd Mklinker
dotnet pack

dotnet publish -r win-x64 -c Release -o bin/Mklinker-%version%-Win64 /p:PublishSingleFile=true
dotnet publish -r win-x86 -c Release -o bin/Mklinker-%version%-Win32 /p:PublishSingleFile=true

dotnet publish -r linux-x64 -c Release -o bin/Mklinker-%version%-Linux64 /p:PublishSingleFile=true
dotnet publish -r linux-arm -c Release -o bin/Mklinker-%version%-LinuxARM /p:PublishSingleFile=true

dotnet publish -r osx-x64 -c Release -o bin/Mklinker-%version%-OSX64 /p:PublishSingleFile=true
pause