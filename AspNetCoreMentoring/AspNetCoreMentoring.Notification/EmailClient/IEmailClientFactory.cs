using SendGrid;

namespace AspNetCoreMentoring.Notification.EmailClient
{
    public interface IEmailClientFactory
    {
        ISendGridClient CreateClient(string apiKey);
    }
}
