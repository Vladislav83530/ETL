using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Models
{
    public class OutputTransaction
    {
        public List<TranscationModel> transcations { get; set; } = new();
    }
}
