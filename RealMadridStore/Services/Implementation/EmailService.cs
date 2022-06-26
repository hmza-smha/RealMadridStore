using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridStore.Services.Implementation
{
    public class EmailService
    {
        public async Task<bool> SendEmail(string message, string toEmail, string subject)
        {

            SendGridClient client = new SendGridClient("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");
            SendGridMessage msg = new SendGridMessage();

            msg.SetFrom("22029646@student.ltuc.com", "Mutaz");
            msg.AddTo(toEmail);
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, message);

            await client.SendEmailAsync(msg);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            return response.IsSuccessStatusCode;

        }

    }
}
