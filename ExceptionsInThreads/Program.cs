using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExceptionsInThreads
{
    class Program
    {
        class A : MarshalByRefObject 
        {
            public void M()
            {
                var d = new Thread(RaiseException);
                d.Start();
            }

            public static void RaiseException(object state)
            {
                Console.WriteLine("RaiseException");
                throw new Exception();
            }
        }

        static void Main(string[] args)
        {
            var ads = new AppDomainSetup();
            ads.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            ads.DisallowBindingRedirects = false;
            ads.DisallowCodeDownload = false;
            ads.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            var ad = AppDomain.CreateDomain("AD", null, ads);

            A a = (A)ad.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, "A");
            a.M();

            
            Thread.Sleep(5000);
            Console.WriteLine("waiting...");
            Console.ReadKey();
        }
    }
}
