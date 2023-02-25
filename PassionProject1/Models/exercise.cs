using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject1.Models
{
    public class exercise
    {
        public int exerciseId { get; set; }

        public string exerciseName { get; set; }

        //one exercise can have effect on multiple issues
        public ICollection<issues> issues { get; set; }
    }
}