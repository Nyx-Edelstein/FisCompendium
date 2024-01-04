using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using FisCompendium.Data.Utility;
using FisCompendium.Repository;
using FisCompendium.Web.Utilities.UserData.Interfaces;

namespace FisCompendium.Web.Utilities.UserData
{
    public class EmailProvider : IEmailProvider
    {
        private IRepository<ExceptionLog> _ExceptionLogRepository { get; }

        public EmailProvider(IRepository<ExceptionLog> _exceptionLogRepository)
        {
            _ExceptionLogRepository = _exceptionLogRepository;
        }

        public void SendRecoveryEmail(string userName, string recoveryEmail, string recoveryTicket)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("admin@chaossnek.com", @"8xk#F!^UGJ&e15cH$ZQ1aAV#48HV^il3I78bEzI$1&HDBYEDcGS!OQ61v5ni4Yxptpk1r^U&lfv^vnRl0DRxH%!kJ9@qa&iGlWR"),
                EnableSsl = true
            })
            {
                using (var mailMessage = new MailMessage("admin@chaossnek.com", recoveryEmail)
                {
                    Subject = $"LotG - Password Recovery For {userName}",
                    SubjectEncoding = Encoding.UTF8,
                    Body = RecoveryTemplate(userName, recoveryTicket),
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                })
                {
                    try
                    {
                        client.Send(mailMessage);
                    }
                    catch (SmtpException ex)
                    {
                        var logItem = new ExceptionLog
                        {
                            Message = $"Failure to send recovery email for user: {userName}; email: {recoveryEmail}",
                            ExceptionType = ex.GetType().FullName,
                        };
                        _ExceptionLogRepository.Upsert(logItem);
                        throw;
                    }
                }
            }
        }

        public static string RecoveryTemplate(string userName, string recoveryTicket)=> $@"<!DOCTYPE html>
<html>
<body>
<h3>Hello, {userName}!</h3>
You appear to be rather forgetful. It's okay, we can fix that! Just copy and paste the following recovery ticket to the password recovery form:<br><br>
<strong>{recoveryTicket}</strong><br><br>
<font size=""2"">(If you did not request this recovery email, please ignore and delete this message, and notify the site administrator)</font>
</body>
</html>";

    }
}
