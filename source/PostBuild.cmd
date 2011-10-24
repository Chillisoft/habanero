cd %1
cd..
if NOT EXIST bin md bin
cd bin
xcopy /D /Y /S %2*.dll
xcopy /D /Y /S %2*.pdb
xcopy /D /Y /S %2*.config
xcopy /D /Y /S %2*.exe
xcopy /D /Y /S %2*.xml
xcopy /D /Y /S %2*.db
xcopy /D /Y /S %2*.sdf
xcopy /D /Y /S %2*.txt
xcopy /D /Y /S %2*.rtf
xcopy /D /Y /S %2*.htm
xcopy /D /Y /S %2*.manifest
