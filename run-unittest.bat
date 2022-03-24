@echo off
title Run UnitTests
echo Run UnitTests
dotnet test %cd%/src/AntSolution.sln --collect:"XPlat Code Coverage" /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

reportgenerator "-reports:**\coverage.opencover.xml" "-targetdir:coverage-report"
echo Finish
pause

