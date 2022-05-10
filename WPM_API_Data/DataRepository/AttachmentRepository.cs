using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;

namespace  WPM_API.Data.DataRepository
{
    public class AttachmentRepository : RepositoryEntityBase<Attachment>
    {
        public AttachmentRepository(DataContextProvider context): base(context)
        {
        }
    }
}
