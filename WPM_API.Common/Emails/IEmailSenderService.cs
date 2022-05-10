using System.Collections.Generic;
using WPM_API.Common.Emails.Models;

namespace WPM_API.Common.Emails
{
    public interface IEmailSenderService
    {
        void SendEmail(
            IEnumerable<EmailAddressInfo> emailsTo,
            string subject,
            string bodyHtml,
            IEnumerable<EmailAddressInfo> emailsCc = null,
            IEnumerable<EmailAddressInfo> emailsBcc = null,
            Dictionary<string, byte[]> attachments = null);
    }
}
