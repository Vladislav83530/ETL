using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Models
{
    public class Service
    {
        public string name { get; set; }
        public List<Payer> payers { get; set; } = new List<Payer>();
        public decimal total { get; set; }
    }
}
