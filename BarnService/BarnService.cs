using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BarnServer
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class BarnService : IBarnService
    {
        /// <summary>
        /// Whether or not the stream is enabled. This needs to be volatile, as it is accessed in different threads
        /// </summary>
        volatile bool StreamEnabled;

        /// <summary>
        /// The directory to watch
        /// </summary>
        string dir;

        /// <summary>
        /// The name of the file to monitor
        /// </summary>
        string file;

        /// <summary>
        /// Initializes an instance of the service
        /// </summary>
        /// <param name="directory">The directory that contains the file to monitor</param>
        /// <param name="filename">The name of the file to monitor (excluding the directory component of the path)</param>
        public BarnService(string directory, string filename)
        {
            StreamEnabled = false;            
            dir = directory;
            file = filename;
        }

        public ObservationRecord[] GetRecords(DateTime from, DateTime to)
        {
            /// TODO: Read all records within a certain time range from the file and return them to the calling client
            return new ObservationRecord[]{};
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StartMonitoring()
        {
            // Get the callback channel to the client that invoked StartMonitoring
            var callback = OperationContext.Current.GetCallbackChannel<IBarnCallback>();
            StreamEnabled = true;

            Console.WriteLine("Starting stream...");

            // Create a file system watcher that notifies us whenever the file is being changed
            var watcher = new System.IO.FileSystemWatcher();
            watcher.Path = dir;
            watcher.Filter = file;

            // Open the file for reading and move to the end
            var stream = new FileStream(dir + "\\" + file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Write | FileShare.Read);
            stream.Seek(0, SeekOrigin.End);
            var sr = new StreamReader(stream); 
            
            // In a separate thread, wait for changes, read any new data in the file and push them to the client 
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback( v => 
                {                    
                    while(StreamEnabled)
                    {
                        var change = watcher.WaitForChanged(System.IO.WatcherChangeTypes.Changed, 2000);
                        if (!change.TimedOut)
                        {
                            string newData = sr.ReadToEnd();
                            Console.WriteLine("Pushing update: " + newData.Trim());

                            // Decode the string into observation records
                            var records = DecodeData(newData);

                            // Push all observations to the client
                            foreach(var o in records)
                                callback.PushObservation(o);
                        }
                    }
                    Console.WriteLine("Stream ended.");
                }));
        }

        /// <summary>
        /// This method should be implemented in order to create observation records from the added lines in the file that is monitored
        /// </summary>
        /// <param name="newData">The new string added to the file since the last update</param>
        /// <returns>An array of observation records</returns>
        private ObservationRecord[] DecodeData(string newData)
        {
            //TODO: Decode a string and create an array of observation records that will be pushed to the client
            return new ObservationRecord[] { new ObservationRecord() };
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StopMonitoring()
        {            
            StreamEnabled = false;
        }
    }
}
