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
        readonly string pathOutputBase = @"..\..\..\Files\Folder_B\";
        string pathOutputToday = "";
        int outputCounter = 0;

        FolderReader folderReader = new();
        MetaLogWorker metalogworker = new();
        readonly FileConverter fileConverter = new();      
        readonly System.Timers.Timer timer = new(1000);

        public FileManager()
        {
            timer.Elapsed += ProcessFiles;
        }

        public void Start()
        {
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Enabled = false;
        }

        private async Task WriteToJsonFromOutputAsync(Output output)
        {
            pathOutputToday = pathOutputBase + DateTime.Now.ToString("yyyy-dd-MM") + @"\";
            if (!Directory.Exists(pathOutputToday))
            {
                Directory.CreateDirectory(pathOutputToday);
                outputCounter = 0;
            }
            
            using (StreamWriter sw = new(pathOutputToday + "output" + outputCounter++ + ".json"))
            {
                //outputCounter++;
                await sw.WriteAsync(JsonSerializer.Serialize(output));
            }
        }
        private async void ProcessFiles(object? sender, ElapsedEventArgs e)
        {
            await ProcessFileAsync();
        }

        private async Task ProcessFileAsync()
        {

            await Task.Run(() =>
            {
                if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59 && DateTime.Now.Second >= 59)
                {
                    metalogworker.SetPath(pathOutputBase + DateTime.Now.ToString("yyyy-dd-MM") + @$"\meta.log");
                    metalogworker.Write();
                    metalogworker.Dispose();
                }

                List<string> FilesPaths = folderReader.GetFilesPath().ToList();
                if (FilesPaths.Count == 0) return;
                List<string> csvPaths = FilesPaths.Where(x => x.Contains(".csv")).ToList();
                List<string> txtPaths = FilesPaths.Where(x => x.Contains(".txt")).ToList();

                csvPaths?.ForEach(async x =>
                {
                    Output output = await fileConverter.GetOutputFromCSVFileAsync(x, metalogworker);
                    await WriteToJsonFromOutputAsync(output);
                    File.Delete(x);
                });

                txtPaths?.ForEach(async x =>
                {
                    Output output = await fileConverter.GetOutputFromTXTFileAsync(x, metalogworker);
                    await WriteToJsonFromOutputAsync(output);
                    File.Delete(x);
                });
            });
        }
    }
}
