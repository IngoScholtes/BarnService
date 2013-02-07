using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarnClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var callback = new CallBackClient();
            var svc = new BarnService.BarnServiceClient(new System.ServiceModel.InstanceContext(callback));
            Console.WriteLine("Starting stream (press any key to stop) ...");
            svc.StartMonitoring();
            Console.ReadKey();
            Console.WriteLine("Stopping stream.");
            svc.StopMonitoring();
        }
    }

    public class CallBackClient : BarnService.IBarnServiceCallback
    {
        public void PushObservation(BarnService.ObservationRecord record)
        {
            //TODO: Do whatever you like with the records received at the client side
            Console.WriteLine("Received new record!");
        }
    }

}
