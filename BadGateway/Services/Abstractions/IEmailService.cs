using BadGateway.DataAccess.Models;

namespace BadGateway.Services.Abstractions
{
    public interface IEmailService
    {
        void SendWelcomeEmail(string email, string userName);

        void SendNewPostNotification(Subscriber subscriber, Post post);
    }
}
