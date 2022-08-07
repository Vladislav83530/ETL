using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using ETL.Models;

namespace ETL
{
    internal class CSVConverter
    {
        StreamReader sreader;
        CsvReader csvreader;
        LineConverter lineConverter = new();
        FileReader freader = new();
        public CSVConverter(string path)
        {
            sreader = new(path);
            csvreader = new(sreader, culture: CultureInfo.InvariantCulture);
        }
        public IEnumerable<T> GetAllRecords<T>()
        {
            return csvreader.GetRecords<T>();
        }

        public async Task<Output> GetOutputFromFileAsync(string path)
        {
            return await lineConverter.GetOutputFromCSVFileAsync(path);
        }
    }
}
