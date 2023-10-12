namespace Huawei.Btk.Application.Services.EmailServices
{
    public interface IEmailSender
    {
        void SendEmail(MessageForEmail message);
    }
}
