namespace AspNetCoreMentoring.Notification.Models
{
    public class ResetPasswordEmailModel : BaseEmailTemplateModel
    {
        public object ResetPasswordLink { get; set; }
    }
}
