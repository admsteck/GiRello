using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiRello.Models
{
    public class GitPost
    {
        [Required]
        public string payload { get; set; }
    }
}