# $msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
$msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

.\IncrementVersion-cs.ps1 ..\ClipMirror-Single\ClipMirror.Single

$slnFilePath = "..\ClipMirror-Single\ClipMirror-Single.sln"
& $msbuild $slnFilePath /p:Configuration=Release /t:Clean
& $msbuild $slnFilePath /p:Configuration=Release /t:Rebuild

# In case of PowerShell, the work directory is that of .ps1 file.
.\CreateZipForAssembly-cs.ps1 ..\ClipMirror-Single\ClipMirror.Single\bin\Release\ClipMirror.Single.exe ..\Downloads

explorer ..\Downloads
