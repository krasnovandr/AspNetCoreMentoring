using AspNetCoreMentoring.Notification.Models;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.Notification.EmailTemplates
{
    public interface IEmailBuilderRetriever
    {
        Task<SendGridMessage> GetMessage(BaseEmailTemplateModel baseEmailTemplateModel);
    }
}
