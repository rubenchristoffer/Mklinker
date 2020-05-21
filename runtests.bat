@echo off
dotnet restore
dotnet build
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
dotnet tool install --global dotnet-reportgenerator-globaltool --version 4.5.3
reportgenerator "-reports:Mklinker.Tests/coverage.opencover.xml" "-targetdir:gh-pages/CoverageReport" -reporttypes:Html
pause