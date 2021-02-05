using System;

namespace BacPacker.Messaging
{
    public class ProgressReporter : IProgress<ProgressMessage>
    {
        public void Report(ProgressMessage value)
        {
            // TODO: This is where signalr goes :-)
        }
    }
}
