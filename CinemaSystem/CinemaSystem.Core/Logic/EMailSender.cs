using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Core.Logic
{
    public class EMailSender
    {
        private readonly SmtpClient _smtpClient;

        public EMailSender()
        {
            _smtpClient = new SmtpClient("smtp.yandex.ru", 587);
            //_smtpClient.Credentials = new NetworkCredential("dareiiiios@yandex.ru", "xycitweqxbtmtngi");
            _smtpClient.Credentials = new NetworkCredential("dareiiiios@yandex.ru", "kxmzsohekoemmqqu");

            _smtpClient.EnableSsl = true;
        }

        public async Task SendMessage(MailMessage email)
        {
            try
            {
                await _smtpClient.SendMailAsync(email);

            }
            catch (Exception)
            { }
        }
    }
}
