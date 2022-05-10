using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using WPM_API.Data.Transaction;

namespace  WPM_API.Data.Files
{
    public interface IAttachmentService
    {
        Attachment CreateAttachment(UnitOfWork unitOfWork, string userId, string fileName, byte[] content, ITransactionWrapper tran);
        void DeleteAttachment(UnitOfWork unitOfWork, Attachment attachment, ITransactionWrapper tran);
    }
}
