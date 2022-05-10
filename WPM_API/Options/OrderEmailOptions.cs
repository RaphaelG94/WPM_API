namespace WPM_API.Options
{
    /// <summary>
    /// Options for E-Mails sending 
    /// when an order in the shop is placed.
    /// </summary>
    public class OrderEmailOptions
    {

        public string ReceiverEmail { get; set; }
        public string Host { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string EnableSsl { get; set; }
    }
}
