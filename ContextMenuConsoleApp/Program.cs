using System;
using ContextMenuRegistration;

namespace ContextMenuConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Registrer.InstallPackage();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
