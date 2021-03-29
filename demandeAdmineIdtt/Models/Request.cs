using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace demandeAdmineIdtt.Models
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string RequestDate { get; set; }

        public string Flag { get; set; }
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey("User")]
        public string User_Id { get; set; }
        //relations

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public void SendMail()
        {
            List<string> requestedDocs = new List<string>();
            if (Documents.Count() > 0)
            {
                foreach (var e in Documents)
                {
                    requestedDocs.Add(e.Type);
                }
            }

            MailMessage mc = new MailMessage(User.Email, "lloumir@mediaocean.com")
            {
                Subject = "DPA: " + Title,
                Body = $"Hello ,<br/>I will need these documents :<br/> ",
                IsBodyHtml = true
            };
            foreach (var item in requestedDocs)
            {
                mc.Body += " -" + item + "<br/>";

            }

            mc.Body += $"within a deadline not exceeding {RequestDate}<br/> Thanks <br/>{User.UserName}";

            if (Flag.Equals("Urgent"))
            {
                mc.Priority = MailPriority.High;
            }
            else
                mc.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.Timeout = 1000000;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            NetworkCredential nc = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["Email"].ToString(), System.Configuration.ConfigurationManager.AppSettings["Password"].ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = nc;
            smtp.Send(mc);
        }
    }
}