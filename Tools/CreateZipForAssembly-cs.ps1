# source assembly file path
if (-not ($Args[0])) { return 100 }
# target dir path
if (-not ($Args[1])) { return 101 }

$references = @("System.IO.Compression.FileSystem")
$source = @"
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;

public static class ZipHelper
{
    public static int CreateZipForAssembly(string sourceAssemblyFilePath, string targetDirPath)
    {
        var assemblyName = Path.GetFileNameWithoutExtension(sourceAssemblyFilePath);
        var assembly = Assembly.LoadFrom(sourceAssemblyFilePath);
        var assemblyFileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
        if (assemblyFileVersion == null) return 200;

        var sourceDirPath = Path.GetDirectoryName(sourceAssemblyFilePath);
        var targetZipFileName = string.Format("{0}-{1}.zip", assemblyName, assemblyFileVersion.Version);
        var targetZipFilePath = Path.Combine(targetDirPath, targetZipFileName);

        Directory.CreateDirectory(targetDirPath);
        File.Delete(targetZipFilePath);
        ZipFile.CreateFromDirectory(sourceDirPath, targetZipFilePath);

        return 0;
    }
}
"@

Add-Type -TypeDefinition $source -Language CSharp -ReferencedAssemblies $references
[ZipHelper]::CreateZipForAssembly($Args[0], $Args[1])
