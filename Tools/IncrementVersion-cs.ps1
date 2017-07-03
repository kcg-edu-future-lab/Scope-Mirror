$source = @"
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length < 1) return 100;

        IncrementVersion(args[0]);
        return 0;
    }

    public static void IncrementVersion(string projDirPath)
    {
        var assemblyInfoPath = Directory.EnumerateFiles(projDirPath, "AssemblyInfo.cs", SearchOption.AllDirectories).First();
        var contents = File.ReadLines(assemblyInfoPath, Encoding.UTF8)
            .Select(IncrementLine)
            .ToArray();
        File.WriteAllLines(assemblyInfoPath, contents, Encoding.UTF8);
    }

    static string IncrementLine(string line)
    {
        if (line.StartsWith("//")) return line;

        var match = Regex.Match(line, @"Assembly(File)?Version\(""([0-9\.]+)""\)");
        if (!match.Success) return line;

        var oldVersion = match.Groups[2].Value;
        var newVersion = IncrementBuildNumber(oldVersion);
        return line.Replace(oldVersion, newVersion);
    }

    static string IncrementBuildNumber(string version)
    {
        return Regex.Replace(version, @"^(\d+\.\d+\.)(\d+)((\.\d+)?)$", m => m.Groups[1].Value + IncrementNumber(m.Groups[2].Value) + m.Groups[3].Value);
    }

    static string IncrementNumber(string i)
    {
        return (int.Parse(i) + 1).ToString();
    }
}
"@

Add-Type -TypeDefinition $source -Language CSharp

# $Args[0]: project directory path
[Program]::Main($Args)
