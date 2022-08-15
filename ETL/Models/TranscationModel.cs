using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Models
{
    public class TranscationModel
    {
        public string city { get; set; }
        public List<Service> services { get; set; } = new List<Service>();
        public decimal total { get; set; }
    }
}
