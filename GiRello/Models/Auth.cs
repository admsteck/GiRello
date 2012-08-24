using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GiRello.Models
{
    public class AuthContext : DbContext
    {
        public DbSet<Auth> Auths { get; set; }
    }

    public class Auth
    {
        [Key, Required]
        public string Token { get; set; }
        public string GithubUser { get; set; }
        public string BitbucketUser { get; set; }
    }
}