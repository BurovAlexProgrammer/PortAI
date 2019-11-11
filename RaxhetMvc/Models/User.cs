using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaxhetMvc.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string about { get; set; }
        public string img { get; set; }
    }
}
