using System.Net.Mail;

namespace BaseLibrary.Utilities.Emails
{
    public interface IEmailSender
    {
        bool Send(string? from, string to, string subject, string body);
    }
    public class EmailSender : IEmailSender
    {
        SmtpClient? _client;
        string _from = "support@txsas.com";

        public EmailSender()
        {
            if (ApplicationSettingBase.IsEmailEnabled)
            {
                ConfigureTxsasEmailServer();
            }
            else
            {
                _client = new SmtpClient
                {
                    Timeout = 100000,
                    //_client.UseDefaultCredentials = false;
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = @"C:\Emails\BioMeta\"
                };
                _from = "support@txsas.com";
            }
        }


        private void ConfigureTxsasEmailServer()
        {
            _client = new SmtpClient(ApplicationSettingBase.EmailHost)
            {
                Port = Convert.ToInt32(ApplicationSettingBase.EmailHostPort),
                Timeout = 100000
            };
            if (ApplicationSettingBase.EnableSsl)
                _client.EnableSsl = true;
            _client.DeliveryMethod = SmtpDeliveryMethod.Network;
            _client.Credentials = new System.Net.NetworkCredential(ApplicationSettingBase.FromEmailAddress, ApplicationSettingBase.FromEmailPassword);
            _from = ApplicationSettingBase.FromEmailAddress;
        }


        public bool Send(string? from, string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(from))
                from = _from;// "support@almxpert.com";
                             //body = "Just hello";
            return Send(new MailMessage(from, to, subject, body));
        }

        public bool Send(string? from, string to, string subject, string body, string attachmentPath)
        {
            if (string.IsNullOrEmpty(from))
                from = _from;// "support@almxpert.com";
            var mailMessage = new MailMessage(from, to, subject, body);
            if (File.Exists(attachmentPath))
                mailMessage.Attachments.Add(new Attachment(attachmentPath));
            return Send(mailMessage);
        }

        private bool Send(MailMessage message)
        {
            try
            {
                if (ApplicationSettingBase.IsEmailEnabled == false)
                    return true;
                message.SubjectEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                if (_client is null)
                    throw new ValidationException("SMTP client is not configured.");
                _client.Send(message);
            }
            catch (Exception ex)
            {
                X.Logger?.LogError(ex, CodeHelper.CallingMethodInfo(), null, null);
                return false;
            }
            finally
            {
                message.Dispose();
            }
            return true;
        }

    }
}
