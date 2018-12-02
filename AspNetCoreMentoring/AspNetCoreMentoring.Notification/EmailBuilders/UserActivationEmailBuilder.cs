using System.Threading.Tasks;
using AspNetCoreMentoring.Notification.Models;
using SendGrid.Helpers.Mail;

namespace AspNetCoreMentoring.Notification.EmailBuilders
{
    public class UserActivationEmailBuilder : EmailBuilder<UserActivationEmailModel>
    {

        public UserActivationEmailBuilder(IRazorViewToStringRenderer emailRenderer) : base(emailRenderer)
        {

        }

        public override string CreateEmailSubject(BaseEmailTemplateModel emailTemplateModel)
        {
            Guard.ArgumentNotNull(nameof(emailTemplateModel), emailTemplateModel);

            return "Account Activation";
        }

        public override async Task<string> CreateEmailBodyAsync(BaseEmailTemplateModel emailTemplateModel)
        {
            Guard.ArgumentNotNull(nameof(emailTemplateModel), emailTemplateModel);

            return await EmailRenderer.RenderViewToStringAsync("Views/UserActivation.cshtml", emailTemplateModel);
        }
    }
}
