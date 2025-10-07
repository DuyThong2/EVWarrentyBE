using BuildingBlocks.Email.Abstractions;
using BuildingBlocks.Email.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Email.Smtp
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpClient _client;
        private readonly string _from;

        public SmtpEmailSender(string host, int port, string username, string password, bool enableSsl = true)
        {
            _from = username;
            _client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl
            };
        }

        public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_from),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsHtml
            };

            foreach (var to in message.To)
            {
                mail.To.Add(to);
            }

            await _client.SendMailAsync(mail, cancellationToken);
        }
    }
}
