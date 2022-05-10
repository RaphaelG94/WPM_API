namespace  WPM_API.TransferModels
{
    public  class ClientPropertyViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }
    }

    public class ClientPropertyAddViewModel
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}