using Windows.Management.Deployment;
using System.Runtime.InteropServices;
using Windows.Foundation;


namespace ContextMenuRegistration;

public class Registrer
{
    public static void InstallPackage()
    {
        if (!ExecutionMode.IsRunningWithIdentity())
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var externalLocation = Path.Combine(exePath, @"");
            var sparsePkgPath = Path.Combine(exePath, @"mypackage.msix");

            if (RegisterSparsePackage(externalLocation, sparsePkgPath))
            {
                Console.WriteLine("Package Registration succeded!");
            }
            else
            {
                Console.WriteLine("Package Registation failed, running WITHOUT Identity");
            }
        }
        else
        {
            Console.WriteLine("Package Registation didn't happen");
        }

    }

    [DllImport("Shell32.dll")]
    public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

    private const uint ShcneAssocchanged = 0x8000000;
    private const uint ShcnfIdlist = 0x0;

    private static bool RegisterSparsePackage(string externalLocation, string sparsePkgPath)
    {
        var registration = false;
        try
        {
            var externalUri = new Uri(externalLocation);
            var packageUri = new Uri(sparsePkgPath);

            Console.WriteLine("exe Location {0}", externalLocation);
            Console.WriteLine();
            Console.WriteLine("msix Address {0}", sparsePkgPath);
            Console.WriteLine();

            Console.WriteLine("  exe Uri {0}", externalUri);
            Console.WriteLine();
            Console.WriteLine("  msix Uri {0}", packageUri);
            Console.WriteLine();

            var packageManager = new PackageManager();

            var options = new AddPackageOptions
            {
                ExternalLocationUri = externalUri
            };

            var deploymentOperation = packageManager.AddPackageByUriAsync(packageUri, options);

            var opCompletedEvent = new ManualResetEvent(false);
            deploymentOperation.Completed = (_, _) => { opCompletedEvent.Set(); };

            Console.WriteLine("Installing package {0}", sparsePkgPath);
            Console.WriteLine();

            Console.WriteLine("Waiting for package registration to complete...");
            Console.WriteLine();

            opCompletedEvent.WaitOne();

            switch (deploymentOperation.Status)
            {
                case AsyncStatus.Error:
                {
                    var deploymentResult = deploymentOperation.GetResults();
                    Console.WriteLine("Installation Error: {0}", deploymentOperation.ErrorCode);
                    Console.WriteLine();
                    Console.WriteLine("Detailed Error Text: {0}", deploymentResult.ErrorText);
                    Console.WriteLine();
                    break;
                }
                case AsyncStatus.Canceled:
                    Console.WriteLine("Package Registration Canceled");
                    Console.WriteLine();
                    break;
                case AsyncStatus.Completed:
                    registration = true;
                    Console.WriteLine("Package Registration succeeded!");
                    Console.WriteLine();

                    SHChangeNotify(ShcneAssocchanged, ShcnfIdlist, IntPtr.Zero, IntPtr.Zero);
                    break;
                case AsyncStatus.Started:
                default:
                    Console.WriteLine("Installation status unknown");
                    Console.WriteLine();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("AddPackageSample failed, error message: {0}", ex.Message);
            Console.WriteLine();
            Console.WriteLine("Full Stacktrace: {0}", ex);
            Console.WriteLine();

            return registration;
        }

        return registration;
    }
}