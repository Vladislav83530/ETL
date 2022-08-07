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

        public int parsedFiles=0;
        public int parsedLines=0;
        public int foundErrors = 0;

        public void Write()
        {   
            string message = "parsed files: " + parsedFiles + "\n" + "parsed lines: " + parsedLines + "\n" + "foundErrors: " + foundErrors + "\n";
            using (var sw = new StreamWriter(path))
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
