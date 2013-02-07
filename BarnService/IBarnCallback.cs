using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarnServer
{
    public interface IBarnCallback
    {
        /// <summary>
        /// A callback method that needs to be implemented by the client. This will be called whenever an update occurs in the file monitored.
        /// </summary>
        /// <param name="record"></param>
        [System.ServiceModel.OperationContract(IsOneWay = true)]
        void PushObservation(ObservationRecord record);
    }
}
