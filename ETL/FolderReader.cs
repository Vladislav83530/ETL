using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ETL
{
    internal class FolderReader
    {
        string path2 = Path.GetFullPath(@"..\..\..\Files\Folder_A\");
        public IEnumerable<string> GetFilesPath()
        {
            DirectoryInfo th = new DirectoryInfo(path2);
            var files = th.GetFiles().Where(file => file.Extension is ".csv" or ".txt");
            var files_pathes = files.Select(file => file.FullName);
            return files_pathes;
        }
    }
}
