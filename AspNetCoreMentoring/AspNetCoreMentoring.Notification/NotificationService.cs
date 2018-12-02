using System.Net;
using System.Threading.Tasks;
using AspNetCoreMentoring.Notification.EmailTemplates;
using AspNetCoreMentoring.Notification.Models;
using SendGrid;

namespace AspNetCoreMentoring.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly ISendGridClient _sendGridClient;

        private readonly IEmailBuilderRetriever _emailBuilderRetriever;

        public NotificationService(
            ISendGridClient sendGridClient,
            IEmailBuilderRetriever emailBuilderRetriever)
        {
            Guard.ArgumentNotNull(nameof(sendGridClient), sendGridClient);
            _sendGridClient = sendGridClient;

            Guard.ArgumentNotNull(nameof(emailBuilderRetriever), emailBuilderRetriever);
            _emailBuilderRetriever = emailBuilderRetriever;
        }

        public async Task NotifyAsync(BaseEmailTemplateModel baseEmailTemplateModel)
        {
            Guard.ArgumentNotNull(nameof(baseEmailTemplateModel), baseEmailTemplateModel);

            var email = await _emailBuilderRetriever.GetMessage(baseEmailTemplateModel);

            var response = await _sendGridClient.SendEmailAsync(email);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                await _sendGridClient.SendEmailAsync(email);
            }
        }
    }
}
