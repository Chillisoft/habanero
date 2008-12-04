cd %1
cd..
if NOT EXIST bin md bin
cd bin
xcopy /D /Y %2*.dll
xcopy /D /Y %2*.pdb
xcopy /D /Y %2*.config
xcopy /D /Y %2*.exe
xcopy /D /Y %2*.xml
