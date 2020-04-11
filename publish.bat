@echo off
dotnet clean
dotnet restore
cd Mklinker
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true
dotnet publish -r win-x86 -c Release /p:PublishSingleFile=true

dotnet publish -r linux-x64 -c Release /p:PublishSingleFile=true
dotnet publish -r linux-arm -c Release /p:PublishSingleFile=true

dotnet publish -r osx-x64 -c Release /p:PublishSingleFile=true
pause