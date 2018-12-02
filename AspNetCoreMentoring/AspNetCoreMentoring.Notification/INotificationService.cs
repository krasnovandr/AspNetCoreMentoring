using AspNetCoreMentoring.Notification.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.Notification
{
    public interface INotificationService
    {
        Task NotifyAsync(BaseEmailTemplateModel baseEmailTemplateModel);
    }
}
