using System;

namespace BacPacker
{

    [Serializable]
    public class NoSupportedExporterException : Exception
    {
        public NoSupportedExporterException() { }
        public NoSupportedExporterException(string message) : base(message) { }
        public NoSupportedExporterException(string message, Exception inner) : base(message, inner) { }
        protected NoSupportedExporterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
