using System;
using System.Net;
using System.Net.Mail;
using BadGateway.DataAccess.Models;
using BadGateway.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace BadGateway.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptionsMonitor<EmailNotificationOptions> emailOptions;

        public EmailService(IOptionsMonitor<EmailNotificationOptions> emailOptions)
        {
            this.emailOptions = emailOptions;
        }
        
        public void SendWelcomeEmail(string email, string userName)
        {
            if (!this.emailOptions.CurrentValue.Enabled)
            {
                return;
            }
            
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("exalpe12322@gmail.com", "Example12345");

                MailMessage msg = new MailMessage();
                msg.To.Add(email);
                msg.From = new MailAddress("exalpe12322@gmail.com");
                msg.Subject = "Welcome to Cybersec";
                msg.Body = $"Доброго дня, {userName}! Дякуємо, що підписались на нашу розсилку.";
                client.Send(msg);
            }
            catch (Exception ex)
            {
            }
        }

        public void SendNewPostNotification(Subscriber subscriber, Post post)
        {
            if (!this.emailOptions.CurrentValue.Enabled)
            {
                return;
            }
            
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("exalpe12322@gmail.com", "Example12345");

                MailMessage msg = new MailMessage();
                msg.To.Add(subscriber.Email);
                msg.From = new MailAddress("exalpe12322@gmail.com");
                msg.Subject = post.Title;
                msg.IsBodyHtml = true;
                msg.Body = post.Content;
                client.Send(msg);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
