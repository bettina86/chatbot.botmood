using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace BotMood
{

    public class SendMailService
    {
        private static string apiKey = "sua chave aqui";

        public static async Task Send(string toMail, string toName, string subject, string body)
        {
            var fromMail = "noreply@botenotes.com";
            await Send(fromMail, "BotMood", toMail, toName, subject, body);
        }

        public static async Task Send(string fromMail, string fromName, string toMail, string toName, string subject, string body)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromMail, fromName),
                Subject = subject,
                PlainTextContent = body
                //HtmlContent = "<strong>Hello, Email!</strong>"
            };

            msg.AddTo(new EmailAddress(toMail, toName));
            var response = await client.SendEmailAsync(msg);
        }

    }

}

