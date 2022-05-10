using WPM_API.Common.Files.Models;
using Microsoft.Extensions.Options;
using System.IO;

namespace WPM_API.Common.Files.Impl
{
    public class FileFactoryService: IFileFactoryService
    {
        public IFileService Attachments { get; }

        public FileFactoryService(IOptions<FileFactoryOptions> options)
        {
            Attachments = new FileService(Path.Combine("App_Data", options.Value.AttachmentsFolder));
        }
    }
}
