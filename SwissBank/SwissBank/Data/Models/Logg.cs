using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwissBank.Data.Models
{
    public class Logg
    {
        public int Id { get; set; }
        public string Log { get; set; }
        public DateTime DateTime { get; set; }
        public User user { get; set; }
    }
}
