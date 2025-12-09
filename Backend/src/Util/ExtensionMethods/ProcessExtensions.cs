using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Fork.Util.ExtensionMethods;

public static class ProcessExtensions
{
    public static async Task<double> CalculateCpuLoad(this Process process, TimeSpan measurementWindow)
    {
        process.Refresh();
        if (process.HasExited) return 0;
        TimeSpan startCpuTime = process.TotalProcessorTime;
        Stopwatch timer = Stopwatch.StartNew();

        await Task.Delay(measurementWindow);

        process.Refresh();
        if (process.HasExited) return 0;
        TimeSpan endCpuTime = process.TotalProcessorTime;
        timer.Stop();
        return (endCpuTime - startCpuTime).TotalMilliseconds /
            (Environment.ProcessorCount * timer.ElapsedMilliseconds) * 100;
    }

    public static Task<double> CalculateMemLoad(this Process process, int maxRam)
    {
        process.Refresh();
        return process.HasExited ? Task.FromResult<double>(0) : Task.FromResult(process.WorkingSet64 / (1024d * 1024d) / maxRam * 100);
    }
}
