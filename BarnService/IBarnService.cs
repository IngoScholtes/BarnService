using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarnServer
{
    /// <summary>
    /// A simple structure that contain a single observation record, as read from the data file
    /// </summary>
    public struct ObservationRecord
    {
        //TODO: Possibly update the format of an observation record depending on the data format
        DateTime Timestamp;
        string Mouse;
        string Box;
    }

    [System.ServiceModel.ServiceContract(CallbackContract=typeof(IBarnCallback))]
    public interface IBarnService
    {

        /// <summary>
        /// This method can be used by the client to request all records within a certain date/time range
        /// </summary>
        /// <param name="from">The start time of the period to request</param>
        /// <param name="to">The end time of the period to request</param>
        /// <returns>An array of observation records</returns>
        [System.ServiceModel.OperationContract()]
        ObservationRecord[] GetRecords(DateTime from, DateTime to);

        /// <summary>
        /// Starts the monitoring of a file. Every subsequent added data will be pushed to the client via the callback channel
        /// </summary>
        [System.ServiceModel.OperationContract(IsOneWay=true)]
        void StartMonitoring();

        /// <summary>
        /// Stops the monitoring of a file, i.e. stops pushing file updates to the client
        /// </summary>
        [System.ServiceModel.OperationContract(IsOneWay = true)]
        void StopMonitoring();
    }
    
    
}
