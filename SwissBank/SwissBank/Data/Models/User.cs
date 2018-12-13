using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SwissBank.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Transactionses = new HashSet<Transactions>(); 
            Loggs = new HashSet<Logg>(); 
        }

        public double Monney { get; set; }

        public ICollection<Transactions> Transactionses { get; set; }
        public ICollection<Logg> Loggs { get; set; }
    }
}
