using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        [DefaultValue("On Hold")]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey("User")]
        public string User_Id { get; set; }
        //relations

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Document> Documents { get; set; }


    }
}