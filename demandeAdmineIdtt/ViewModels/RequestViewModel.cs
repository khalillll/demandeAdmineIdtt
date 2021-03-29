using demandeAdmineIdtt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demandeAdmineIdtt.ViewModels
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string RequestDate { get; set; }

        public string Flag { get; set; }
        public string Status { get; set; }

     
        public List<int> Documents { get; set; }

        public IEnumerable<SelectListItem> documentsList { get; set; }


        public string userId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}