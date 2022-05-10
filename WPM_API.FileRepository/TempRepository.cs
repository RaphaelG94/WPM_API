namespace WPM_API.FileRepository
{
    public class TempRepository : FileRepository
    {
        public TempRepository(string connectionString, string folder) : base(connectionString, folder)
        {
        }
    }
}
