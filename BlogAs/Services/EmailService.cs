using System.Net;
using System.Net.Mail;

namespace BlogAs.Services;

public class EmailService
{
    public bool SendEmail(string toName, string toEmail, string subject, string body, 
        string fromName = "Equipe vinicim", string fromEmail = "viniciusma44@gmail.com")
    {
        try
        {
        var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);
        
        smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.Username, Configuration.Smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;
        
        var mail = new MailMessage();
        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;
        
        smtpClient.Send(mail);
        return true;       
        }
        catch 
        {
            return false;
        }
        
    }
}