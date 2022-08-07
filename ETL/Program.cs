using CsvHelper;
using CsvHelper.Configuration;
using ETL;
using System.Text.Json;
using System.Globalization;
using System.Reflection;

AppManager appManager = new AppManager();
appManager.Start();
MetaLogWorker mt = new();
mt.Write();
//FileManager fmanager = new();

Console.ReadKey();
