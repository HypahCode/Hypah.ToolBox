using System.Diagnostics;

namespace Hypah.Utility.Performance
{
    public static class Benchmark
    {
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        [System.Runtime.Versioning.SupportedOSPlatform("linux")]
        public static BenchmarkResult Run(Action action, int iterations, int numberOfBenchmarks = 1)
        {
            var process = Process.GetCurrentProcess();
            var oldAffinity = IntPtr.Zero;

            if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
            {
                oldAffinity = process.ProcessorAffinity;
                process.ProcessorAffinity = (IntPtr)1;
            }

            double totalMilliseconds = 0;
            var result = new BenchmarkResult();
            var totalRunTime = Stopwatch.StartNew();
            for (int benchmarkNumber = 0; benchmarkNumber < numberOfBenchmarks; benchmarkNumber++)
            {
                var stopwatch = new Stopwatch();

                stopwatch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    action();
                }
                stopwatch.Stop();

                result.Iterations.Add(stopwatch.Elapsed.TotalMilliseconds);
            }

            totalRunTime.Stop();

            if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
            {
                process.ProcessorAffinity = oldAffinity;
            }

            result.TotalRunTime = TimeSpan.FromTicks(totalRunTime.ElapsedTicks);
            result.AvarageRunTime = TimeSpan.FromMilliseconds(totalMilliseconds / numberOfBenchmarks);

            return result;
        }
    }
}
