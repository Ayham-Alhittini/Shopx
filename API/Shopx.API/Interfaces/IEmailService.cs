using Shopx.API.Helper;

namespace Shopx.API.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage emailMessage);
    }
}
