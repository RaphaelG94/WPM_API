namespace  WPM_API.TransferModels
{
    public class HDDPartitionViewModels
    {
        public string Id { get; set; }
        public string PartitionNumber { get; set; }
        public string isGpt { get; set; }
        public string DriveLetter { get; set; }
        public string SizeInBytes { get; set; }
        public string Type { get; set; }
        public string Overprovisioning { get; set; }
    }
}