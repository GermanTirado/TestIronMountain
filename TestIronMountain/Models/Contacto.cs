using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestIronMountain.Models
{
    public class Contact
    {
        public int id { get; set; }
        public string regDate { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string curp { get; set; }
        public int active { get; set; }
    }
}
