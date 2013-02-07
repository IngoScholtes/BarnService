using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace BarnServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Note: The directory and filename to monitor are set in the App.config file!
            var host = new ServiceHost(new BarnService(                
                    BarnServer.Properties.Settings.Default.Directory,
                    BarnServer.Properties.Settings.Default.Filename)
                    );
            try
            {
                host.Open();
                Console.WriteLine("BarnService is running...");
                Console.ReadKey();
                Console.WriteLine("BarnService stopped.");
            }
            catch (Exception)
            {
                Console.WriteLine("Faild to start server. Make sure to run server with administrative privileges.");
                Console.ReadKey();
            }
        }
    }
}
