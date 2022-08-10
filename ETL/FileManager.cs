using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Timers;
using ETL.Models;

namespace ETL
{
    internal class FileManager
    {
        string pathOutputBase = @"..\..\..\Files\Folder_B\";
        string pathOutputToday = "";
        int outputCounter = 0;

        FolderReader folderReader = new();
        MetaLogWorker metalogworker = new();
        readonly FileConverter fileConverter = new();
        readonly System.Timers.Timer timer = new(1000);

        List<string> filePaths_Checked = new List<string>();

        public FileManager()
        {
            timer.Elapsed += CheckFiles;
        }

        private async void CheckFiles(object? sender, ElapsedEventArgs e)
        {
            List<string> paths = folderReader.GetFilesPath(@"..\..\..\Files\Folder_A\").ToList();
            if (paths != null && paths.Count > 0)
            {
                pathOutputToday = pathOutputBase + DateTime.Now.ToString("yyyy-dd-MM") + @"\";
                if (!Directory.Exists(pathOutputToday))
                {
                    Directory.CreateDirectory(pathOutputToday);
                    outputCounter = 0;
                }
                await ProcessFileAsync(paths);

                filePaths_Checked.AddRange(paths);
            }
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59 && DateTime.Now.Second >= 59)
            {
                metalogworker.SetPath(pathOutputBase + DateTime.Now.ToString("yyyy-dd-MM") + @$"\meta.log");
                metalogworker.Write();
                metalogworker.Dispose();
                Console.WriteLine("Meta log created");
                filePaths_Checked.Clear();
            }

        }

        private async Task WriteToJsonFromOutputAsync(OutputTransaction output)
        {
            pathOutputToday = pathOutputBase + DateTime.Now.ToString("yyyy-dd-MM") + @"\";
            if (!Directory.Exists(pathOutputToday))
            {
                Directory.CreateDirectory(pathOutputToday);
                outputCounter = 0;
            }

            using (StreamWriter sw = new(pathOutputToday + "output" + ++outputCounter + ".json"))
            {
                await sw.WriteAsync(JsonSerializer.Serialize(output));
            }

        }
        public void Start()
        {
            timer.Enabled = true;
        }
        public void Stop()
        {
            timer.Enabled = false;
        }

        private async Task ProcessFileAsync(IEnumerable<string> paths)
        {
            await Task.Run(() =>
            {
                List<string> csvPaths = paths.Where(x => x.Contains(".csv")).ToList();
                List<string> txtPaths = paths.Where(x => x.Contains(".txt")).ToList();

                csvPaths?.ForEach(async x =>
                {
                    OutputTransaction output = await fileConverter.GetOutputFromCSVFileAsync(x, metalogworker);
                    await WriteToJsonFromOutputAsync(output);
                });

                txtPaths?.ForEach(async x =>
                {
                    OutputTransaction output = await fileConverter.GetOutputFromTXTFileAsync(x, metalogworker);
                    await WriteToJsonFromOutputAsync(output);
                });
            });
        }
    }
}
