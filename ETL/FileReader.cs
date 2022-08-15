using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL
{
    internal class FileReader
    {
        public async Task<string> TxtReadAsync(string path)
        {
            using (StreamReader streamReader = new(path))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        public async Task<string[]> CsvReadAsync(string path)
        {
            List<string> lines = new List<string>();
            string value;


            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };

            using var streamReader = new StreamReader(path);
            using var csvReader = new CsvReader(streamReader, csvConfig);

            while (await csvReader.ReadAsync())
            {
                for (int i = 0; csvReader.TryGetField(i, out value); i++)
                {
                    lines.Add(value);
                }
            }
            streamReader.Dispose();
            csvReader.Dispose();

            return lines.ToArray();
        }
    }
}
