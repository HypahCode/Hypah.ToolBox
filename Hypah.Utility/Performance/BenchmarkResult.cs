
namespace Hypah.Utility.Performance
{
    public class BenchmarkResult
    {
        public TimeSpan TotalRunTime;
        public TimeSpan AvarageRunTime;

        public List<double> Iterations { get; } = new List<double>();
    }
}
