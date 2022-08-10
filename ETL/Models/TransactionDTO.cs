using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Models
{
    public class TransactionDTO
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address { get; set; }
        public decimal payment { get; set; }
        public DateTime date { get; set; }
        public long account_number { get; set; }
        public string service { get; set; }
    }
}
