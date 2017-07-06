# $msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
$msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

.\IncrementVersion-cs.ps1 ..\ScopeMirror-Single\ScopeMirror.Single

$slnFilePath = "..\ScopeMirror-Single\ScopeMirror-Single.sln"
& $msbuild $slnFilePath /p:Configuration=Release /t:Clean
& $msbuild $slnFilePath /p:Configuration=Release /t:Rebuild

# In case of PowerShell, the work directory is that of .ps1 file.
.\CreateZipForAssembly-cs.ps1 ..\ScopeMirror-Single\ScopeMirror.Single\bin\Release\ScopeMirror.Single.exe ..\Downloads

explorer ..\Downloads
