cd %1
cd..
if NOT EXIST bin md bin
if NOT EXIST bin\DotNet35 md bin\DotNet35
cd bin\DotNet35
xcopy /D /Y %2*.dll
xcopy /D /Y %2*.pdb
xcopy /D /Y %2*.config
xcopy /D /Y %2*.exe
xcopy /D /Y %2*.xml
xcopy /D /Y %2*.db
