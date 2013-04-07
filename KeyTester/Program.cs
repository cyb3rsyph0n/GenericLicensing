using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(CyberSyphonLicensing.LicenseValidator.ValidateKey("EF8E-8531-29AE-37EE-50CC-A629"));
            Console.ReadLine();
        }
    }
}