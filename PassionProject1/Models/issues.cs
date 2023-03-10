using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject1.Models
{
    public class issues
    {
        [Key]
        public int issueId { get; set; }

        public string issueName { get; set; }

        public string issueDescription { get; set; }

        //one issue can have more than one exercise to cure 
        public ICollection<exercise> exercise { get; set; }
    }
}