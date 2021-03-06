﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMentoring.Notification.Models;
using SendGrid.Helpers.Mail;

namespace AspNetCoreMentoring.Notification.EmailBuilders
{
    public abstract class EmailBuilder<TEmailModel> where TEmailModel : BaseEmailTemplateModel
    {
        protected readonly IRazorViewToStringRenderer EmailRenderer;

        protected EmailBuilder(IRazorViewToStringRenderer emailRenderer)
        {
            EmailRenderer = emailRenderer;
        }

        public async Task<SendGridMessage> BuildAsync(EmailServerConnectionSettings connectionSettings, TEmailModel emailTemplateModel)
        {
            var from = new EmailAddress(connectionSettings.SenderEmailAddress, connectionSettings.SenderName);

            var to = new EmailAddress(emailTemplateModel.Email);

            var body = await CreateEmailBodyAsync(emailTemplateModel);

            return MailHelper.CreateSingleEmail(from, to, CreateEmailSubject(emailTemplateModel), string.Empty, body);
        }

        public abstract string CreateEmailSubject(BaseEmailTemplateModel emailTemplateModel);

        public abstract Task<string> CreateEmailBodyAsync(BaseEmailTemplateModel emailTemplateModel);
    }
}
