@echo off
echo Obfuscating Habanero.UI.Pro.dll....
"C:\Program Files\Eziriz\.NET Reactor\dotNET_Reactor.exe" -project "licensing\HabaneroPro.nrproj" -q
if NOT EXIST obfucsated md obfuscated
copy bin\*.* obfuscated\
copy licensing\bin\Habanero.UI.Pro.dll Obfuscated\
copy licensing\bin\Habanero.UI.Pro.pdb Obfuscated\
pause
