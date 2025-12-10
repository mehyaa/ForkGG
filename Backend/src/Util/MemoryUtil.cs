using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Fork.Util;

public static class MemoryUtil
{
    public static int GetTotalInstalledMemory()
    {
        long bytes = 0;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            bytes = GetWindowsRam();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            bytes = GetLinuxRam();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            bytes = GetMacRam();
        }
        else
        {
            throw new PlatformNotSupportedException("Operating system not supported.");
        }

        // Convert Bytes to MB
        return (int)(bytes / 1024 / 1024);
    }

    // ---------------- WINDOWS (P/Invoke GlobalMemoryStatusEx) ----------------
    // This is the most accurate Win32 API for physical hardware memory.

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx([In] [Out] MEMORYSTATUSEX lpBuffer);

    private static long GetWindowsRam()
    {
        MEMORYSTATUSEX memStatus = new();
        if (GlobalMemoryStatusEx(memStatus))
        {
            return (long)memStatus.ullTotalPhys;
        }

        return 0; // API call failed
    }

    // ---------------- LINUX (Parse /proc/meminfo) ----------------
    // Linux stores hardware details in the /proc virtual file system.

    private static long GetLinuxRam()
    {
        try
        {
            // /proc/meminfo contains "MemTotal:  16300000 kB"
            foreach (string line in File.ReadLines("/proc/meminfo"))
                if (line.StartsWith("MemTotal:"))
                {
                    // Split by whitespace and get the number
                    string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2 && long.TryParse(parts[1], out long kb))
                    {
                        return kb * 1024; // Convert kB to Bytes
                    }
                }
        }
        catch
        {
            /* Permission or IO errors can be handled here */
        }

        return 0;
    }

    // ---------------- MACOS (sysctl hw.memsize) ----------------
    // macOS exposes hardware stats via the sysctl command.

    private static long GetMacRam()
    {
        try
        {
            ProcessStartInfo info = new()
            {
                FileName = "sysctl",
                Arguments = "-n hw.memsize", // -n prints value only
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process? process = Process.Start(info);
            process?.WaitForExit();
            string? output = process?.StandardOutput.ReadToEnd();

            if (long.TryParse(output?.Trim(), out long bytes))
            {
                return bytes;
            }
        }
        catch
        {
            /* Handle errors */
        }

        return 0;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class MEMORYSTATUSEX
    {
        // 1. The size of the structure (Must be first)
        public uint dwLength;

        // 2. Memory load percentage
        public uint dwMemoryLoad;

        // 3. Total Physical Memory (What you want)
        public ulong ullTotalPhys;

        // 4. Available Physical Memory
        public ulong ullAvailPhys;

        // 5. Total Page File (Commit Limit)
        public ulong ullTotalPageFile;

        // 6. Available Page File
        public ulong ullAvailPageFile;

        // 7. Total Virtual Memory (128 TB on x64)
        public ulong ullTotalVirtual;

        // 8. Available Virtual Memory
        public ulong ullAvailVirtual;

        // 9. Available Extended Virtual Memory (Must be last)
        public ulong ullAvailExtendedVirtual;

        public MEMORYSTATUSEX()
        {
            this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
        }
    }
}