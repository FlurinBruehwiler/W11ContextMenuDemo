using System.Runtime.InteropServices;
using System.Text;

namespace ContextMenuRegistration
{
    internal class ExecutionMode
    {

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetCurrentPackageFullName(ref int packageFullNameLength, ref StringBuilder packageFullName);

        internal static bool IsRunningWithIdentity()
        {
            if (IsWindows7OrLower())
            {
                return false;
            }

            var sb = new StringBuilder(1024);
            var length = 0;
            var result = GetCurrentPackageFullName(ref length, ref sb);

            return result != 15700;
        }

        private static bool IsWindows7OrLower()
        {
            var versionMajor = Environment.OSVersion.Version.Major;
            var versionMinor = Environment.OSVersion.Version.Minor;
            var version = versionMajor + (double)versionMinor / 10;
            return version <= 6.1;
        }
    }
}
