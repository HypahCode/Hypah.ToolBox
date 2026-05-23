using Hypah.Utility.Files;

namespace Hypah.Utility.Os.Execution
{
    public static class Explorer
    {
        /// <summary>
        /// Opens explorer with a specified file name highlighted
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void OpenFileLocation(FilePath path)
        {
            if (path.Exists)
            {
                var process = Execute.Process("explorer.exe")
                    .WithArguments("/e,", "/select", path.ToString())
                    .WithShellExecute(false)
                    .Start();
            }
            else
            {
                throw new FileNotFoundException($"File not found: {path}");
            }
        }

        /// <summary>
        /// Opens explorer inside a specified folder
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void OpenFolder(FilePath path)
        {
            if (path.Exists)
            {
                var process = Execute.Process("explorer.exe")
                    .WithArguments($"/open, {path.ToString()}")
                    .WithShellExecute(false)
                    .Start();
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory not found: {path}");
            }
        }
    }
}
