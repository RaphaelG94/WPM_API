using System.Collections.Generic;
using WPM_API.Code.Scheduler.DataModels;
using WPM_API.Code.Scheduler.SchedulerModels.EmailBuilderModels;
using WPM_API.Models.TemplateModels;

namespace WPM_API.Code.Scheduler.SchedulerActions.EmailBuilders
{
    public class ResetPasswordEmailBuilder : EmailBuilderBase<ResetPasswordNotificationEmailModel>
    {
        public ResetPasswordEmailBuilder(SchedulerActionArgs args) : base(args)
        {
        }

        public override IEnumerable<NotificationEmailData> BuildEmails(ResetPasswordNotificationEmailModel model)
        {
            var forgot = UnitOfWork.Users.GetForgotPasswordRequest(model.UserForgotPasswordId);

            var user = UnitOfWork.Users.Get(forgot.UserId);

            var emailModel = new ResetPasswordModel
            {
                RequestIp = forgot.CreatorIpAddress,
                ResetPasswordUrl = ActionArgs.PathResolver.BuildFullUrl("/ForgotPassword/CompleteResetPassword?id=" + forgot.RequestGuid),
                //ResetPasswordUrl = "TEST",
                UserName = user.UserName
            };

            var res = new NotificationEmailData();
            res.Subject = "Password reset confirmation";
            res.ToEmailAddresses = new[] { user.Email };

            yield return res;
        }
    }
}