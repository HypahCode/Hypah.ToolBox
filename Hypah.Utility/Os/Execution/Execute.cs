using Hypah.Utility.Files;
using System.Diagnostics;

namespace Hypah.Utility.Os.Execution
{
    public sealed class Execute
    {
        public ProcessStartInfo ProcessStartInfo { get; } = new();

        public static Execute Process(FilePath exeFile) => new(exeFile);

        public Execute(FilePath exeFile)
        {
            ProcessStartInfo.FileName = exeFile.ToString();
            ProcessStartInfo.UseShellExecute = false;
            ProcessStartInfo.RedirectStandardOutput = true;
            ProcessStartInfo.RedirectStandardError = true;
        }

        public Execute WithArguments(params string[] args)
        {
            var argLine = string.Join(" ", args);
            if (string.IsNullOrEmpty(ProcessStartInfo.Arguments))
                ProcessStartInfo.Arguments = argLine;
            else
                ProcessStartInfo.Arguments += " " + argLine;
            return this;
        }

        public Execute WithWorkingDirectory(FilePath workingDirectory)
        {
            ProcessStartInfo.WorkingDirectory = workingDirectory.ToString();
            return this;
        }

        public Execute WithEnvVariable(string key, string value)
        {
            ProcessStartInfo.EnvironmentVariables[key] = value;
            return this;
        }

        public Execute WithShellExecute(bool useShellExecute)
        {
            ProcessStartInfo.UseShellExecute = useShellExecute;
            return this;
        }

        public Execute SuppressWindow()
        {
            ProcessStartInfo.CreateNoWindow = true;
            ProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            return this;
        }

        public Process? Start()
        {
            return System.Diagnostics.Process.Start(ProcessStartInfo);
        }
    }
}
