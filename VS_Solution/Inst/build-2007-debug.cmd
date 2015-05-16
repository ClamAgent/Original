@echo off
setlocal
set BUILDROOT=\\balu\users\zoli\My Documents\Development\Projects\ClamAgent
rd /S /Q "%BUILDROOT%\Inst\Release.2007"
md "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd\x64\clamd.exe" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd\x64\clamdscan.exe" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd\x64\freshclam.exe" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd\x64\libclamav.dll" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd\x64\libclamav_llvm.dll" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd\x64\libclamunrar.dll" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd\x64\libclamunrar_iface.dll" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd\x64\sigtool.exe" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd-config\clamd.conf" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd-config\clamav.reg" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd-config\freshclam.conf" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\scripts\Install-ClamAgent.ps1" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\scripts\Uninstall-ClamAgent.ps1" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\ClamAgent\bin\Debug\ClamAgent.dll" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\ClamAgent\bin\Debug\ClamAgent.dll.config" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\ClamAgent\bin\Debug\ClamAgent.pdb" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\ClamAgent\bin\Debug\Message Base.dll" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\ClamAgent\bin\Debug\ClamAgent.pdb" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd\x64\clamd.exe" "%BUILDROOT%\Inst\Release.2007"
copy "%BUILDROOT%\Inst\Source\clamd\x64\clamd.exe" "%BUILDROOT%\Inst\Release.2007"
md "%BUILDROOT%\Inst\Release.2007\Microsoft.VC80.CRT"
copy "%BUILDROOT%\Inst\Source\VC80\x64\*"  "%BUILDROOT%\Inst\Release.2007\Microsoft.VC80.CRT"

endlocal