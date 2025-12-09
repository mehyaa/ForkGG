using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using ForkCommon.Model.Entity.Pocos;

namespace Fork.Util.JavaUtils;

public class JavaVersionUtils
{
    private const string BIT_PATTERN = "64-Bit";
    private static readonly Regex VersionRegex = new(".* version \"([0-9._]*)\"");

    public static JavaVersion? GetInstalledJavaVersion(string? javaPath)
    {
        if (string.IsNullOrEmpty(javaPath))
        {
            //TODO get default path from settings
            //javaPath = AppSettingsSerializer.Instance.AppSettings.DefaultJavaPath;

            // Fallback to PATH
            javaPath = FindJavaInPath();
        }

        return CheckForPathJava(javaPath);
    }


    private static string FindJavaInPath()
    {
        // 1. Split the PATH string into separate directories
        string[] paths = GetUpdatedPathValue()?.Split(';') ?? [];

        // 2. Iterate through them in order
        foreach (string path in paths)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                continue;
            }

            try
            {
                // 3. Construct the full path to java.exe
                string fullPath = Path.Combine(path.Trim(), "java.exe");

                // 4. Return the first one that actually exists
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            catch (Exception)
            {
                // Ignore invalid paths/characters in PATH variable
            }
        }

        // Fallback if nothing found
        return "java";
    }

    private static JavaVersion? CheckForPathJava(string javaPath)
    {
        try
        {
            ProcessStartInfo procStartInfo = new(javaPath, "-version ");

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.RedirectStandardError = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            Process proc = new() { StartInfo = procStartInfo };
            proc.Start();
            JavaVersion? version = InterpretJavaVersionOutput(proc.StandardError.ReadToEnd());
            if (version != null)
            {
                version.JavaPath = javaPath;
            }

            return version;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static JavaVersion? InterpretJavaVersionOutput(string output)
    {
        Match versionMatch = VersionRegex.Match(output);
        if (versionMatch.Success)
        {
            JavaVersion result = new() { Version = versionMatch.Groups[1].Value };
            if (TryParseJavaVersion(result.Version.Split(".")[0], out int computedVersion))
            {
                if (computedVersion == 1)
                {
                    TryParseJavaVersion(result.Version.Split(".")[1], out computedVersion);
                }

                result.VersionComputed = computedVersion;
            }

            if (output.Contains(BIT_PATTERN))
            {
                result.Is64Bit = true;
            }

            return result;
        }

        return null;
    }

    private static string? GetUpdatedPathValue()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
        }

        // Handle Linux and MacOS: Use Shell command instead of built in functionality
        string shell = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "/bin/zsh" : "/bin/bash";

        return GetShellEnvironmentVariable(shell, "PATH");
    }

    private static string GetShellEnvironmentVariable(string shellPath, string variableName)
    {
        // The command tells the shell to run an interactive login process, 
        // print the value of the variable, and exit.
        ProcessStartInfo psi = new()
        {
            FileName = shellPath,
            Arguments = $"-lc \"echo ${variableName}\"", // -l for login shell, -c for command
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process? p = Process.Start(psi);
        p?.WaitForExit();
        // Trim to remove any potential newline or whitespace
        return p?.StandardOutput.ReadToEnd().Trim() ?? "";
    }

    private static bool TryParseJavaVersion(string versionString, out int version)
    {
        return int.TryParse(versionString, out version) || int.TryParse(versionString.Split(".")[1], out version);
    }
}