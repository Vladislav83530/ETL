using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL
{
    internal class AppManager
    {
        private FileManager fileManager;
        bool isEnabled=false;
        public AppManager()
        {
            fileManager = new();
        }

        public void Start()
        {
            fileManager.Start();
            isEnabled = true;
        }

        public void Stop()
        {
            fileManager.Stop();
            isEnabled = false;
        }
    }
}
