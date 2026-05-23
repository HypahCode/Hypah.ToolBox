using System.Diagnostics;

namespace Hypah.Utility.Os.Execution
{
    public static class Terminate
    {
        /// <summary>
        /// Find and kills a process by name
        /// </summary>
        /// <param name="processName"></param>
        /// <returns>Kill count</returns>
        public static int Kill(string processName)
        {
            int killCount = 0;
            foreach (var process in Process.GetProcessesByName(processName))
            {
                process.Kill();
                killCount++;
            }
            return killCount;
        }
    }
}
