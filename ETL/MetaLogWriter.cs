using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL
{
    internal class MetaLogWriter
    {
        string path = "";
        public MetaLogWriter(string path)
        {
            this.path = path;
        }

        public void Write(string message)
        {
            File.AppendAllText(path, message);
        }
    }
}
