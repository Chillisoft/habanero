set PATH=%PATH%;C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\;C:\Program Files\Microsoft Visual Studio 8\Application\PreEmptive Solutions\Dotfuscator Community Edition\
sn -Vr Build\Debug\Habanero.UI.Pro.dll
Dotfuscator.exe -v Dotfuscator.xml
sn -Vu Dotfuscated\Habanero.UI.Pro.dll
sn -R Dotfuscated\Habanero.UI.Pro.dll Habanero.UI.Pro\key.snk
copy Build\Debug\*.* "Dotfuscated Build\"
copy Dotfuscated\Habanero.UI.Pro.dll "Dotfuscated Build\"
pause
