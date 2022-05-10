namespace WPM_API.Models
{
    public class SmbStorageViewModel
    {
        public string Id { get; set; }
        public string ShareName { get; set; }
        public string CustomerId { get; set; }
        public string DataDriveLetter { get; set; }
        public string Path { get; set; }
        public CustomerViewModel Customer { get; set; }
        public string ClientId { get; set; }
        public bool ExistedAlready { get; set; }
        public ClientViewModel Client { get; set; }
    }

    public class SmbStorageIdView
    {
        public string Id { get; set; }
    }
}