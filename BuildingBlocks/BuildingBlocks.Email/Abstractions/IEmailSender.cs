using BuildingBlocks.Email.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Email.Abstractions
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default);
    }
}
