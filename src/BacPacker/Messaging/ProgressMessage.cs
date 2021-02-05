namespace BacPacker.Messaging
{
    public class ProgressMessage
    {
        // TODO: What makes sense to communicate into the backoffice here?
        public int? Progress { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
