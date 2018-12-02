using SendGrid;

namespace AspNetCoreMentoring.Notification.EmailClient
{
    public class EmailClientFactory : IEmailClientFactory
    {
        public ISendGridClient CreateClient(string apiKey)
        {
            Guard.ArgumentNotNull(nameof(apiKey), apiKey);

            return new SendGridClient(apiKey);
        }
    }
}
