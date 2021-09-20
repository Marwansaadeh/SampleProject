using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MoneyLaundering
{
    public class MailSender : IEmailSender
    {
        public void SendEmail(IEnumerable<IGrouping<string, SuspectPerson>> suspectReportItems)
        {
            foreach (var country in suspectReportItems)
            {
                MailAddress to = new MailAddress(country.Key + "@Test.com");
                MailAddress from = new MailAddress(country.Key + "@mailtrap.io");

                MailMessage message = new MailMessage(from, to);
                message.Subject = "Suspect people";

                StringBuilder body = new StringBuilder();

                body.Append(country.Key + Environment.NewLine + Environment.NewLine);

                foreach (var suspectedPerson in country)
                {
                    body.Append("Customer Name :" + suspectedPerson.CustomerName + Environment.NewLine + "Account Number: " + suspectedPerson.Accountnumber + Environment.NewLine);
                    foreach (var transactions in suspectedPerson.Transactions)
                    {
                        body.Append("Amount: " + transactions.Amount + "Date: " + transactions.Date + Environment.NewLine);

                    }

                }
                body.Append(Environment.NewLine + Environment.NewLine);
                message.Body = body.ToString();

                SmtpClient client = new SmtpClient("smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("b1e6d0d999e5c1", "56da17539ec34b"),
                    EnableSsl = true
                };

                try
                {
                    client.Send(message);
                    Console.WriteLine("Report sent to country");
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
