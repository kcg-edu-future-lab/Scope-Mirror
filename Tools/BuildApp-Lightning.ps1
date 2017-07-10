# $msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
$msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

.\IncrementVersion-cs.ps1 ..\ScopeMirror-Lightning\ScopeMirror.Lightning.Guest
.\IncrementVersion-cs.ps1 ..\ScopeMirror-Lightning\ScopeMirror.Lightning.Host

$slnFilePath = "..\ScopeMirror-Lightning\ScopeMirror-Lightning.sln"
& $msbuild $slnFilePath /p:Configuration=Release /t:Clean
& $msbuild $slnFilePath /p:Configuration=Release /t:Rebuild

# In case of PowerShell, the work directory is that of .ps1 file.
.\CreateZipForAssembly-cs.ps1 ..\ScopeMirror-Lightning\ScopeMirror.Lightning.Guest\bin\Release\ScopeMirror.Lightning.Guest.exe ..\Downloads
.\CreateZipForAssembly-cs.ps1 ..\ScopeMirror-Lightning\ScopeMirror.Lightning.Host\bin\Release\ScopeMirror.Lightning.Host.exe ..\Downloads

explorer ..\Downloads
