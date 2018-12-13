using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwissBank.Data.Models
{
    public class Transactions
    {        
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Amount { get; set; }
        public DateTime DateTime { get; set; }
        public User Sender { get; set; }
        [Required]
        public User Reseaver { get; set; }
    }
}
