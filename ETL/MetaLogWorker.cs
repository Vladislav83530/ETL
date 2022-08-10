using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL
{
    internal class MetaLogWorker : IDisposable
    {
        string path = "";

        public ulong parsedFiles=0;
        public ulong parsedLines=0;
        public ulong foundErrors = 0;
        //public List<string> invalidFiles = new List<string>();
        //public string invalFile = "";
        public void Write()
        {   
            string message = "Parsed Files: " + parsedFiles + "\n" + "Parsed Lines: " + parsedLines + "\n" + "Found Errors: " + foundErrors + "\n";
            using (var sw = new StreamWriter(path, false))
            {
                sw.WriteLine(message);
            }
        }

        public void Dispose()
        {
            parsedFiles = 0;
            parsedLines = 0;
            foundErrors = 0;
        }

        public void SetPath(string path)
        {
            this.path = path;
        }
    }
}
