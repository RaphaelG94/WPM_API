namespace WPM_API.Common.Files
{
    public interface IFileFactoryService
    {
        IFileService Attachments { get; }
    }
}
