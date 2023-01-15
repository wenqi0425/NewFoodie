﻿using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace NewFoodie.Services.EFServices
{
    public class EmailService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("NewFoodie Administration", "foodieDK@outlook.com"));
            message.To.Add(new MailboxAddress("", $"{email}"));
            message.Subject = $"{subject}";

            Secret secret = new Secret();

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = $"{htmlMessage}"
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Timeout = 5000;
                try
                {
                    client.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                }
                catch (Exception ex)
                {
                    throw;
                }

                // SMTP authentication
                client.Authenticate("foodieDK@outlook.com", secret.Password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
