using Hypah.Utility.Os.Windows;

namespace Hypah.Utility.Os
{
    public static class Sleep
    {
        public static void Prevent()
        {
            Kernel32.SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
        }

        public static void Allow()
        {
            Kernel32.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }
    }
}
