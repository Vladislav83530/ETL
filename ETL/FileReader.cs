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

        public async Task<string[]> CSVReadAsync(string path)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false
            };

            using var streamReader = new StreamReader(path);
            using var csvReader = new CsvReader(streamReader, csvConfig);

            List<string> lines = new List<string>();
            string value = "";
            while (csvReader.Read())
            {
                for (int i = 0; csvReader.TryGetField(i, out value); i++)
                {
                    lines.Add(value);
                }
            }
            streamReader.Close();
            csvReader.Dispose();
            streamReader.Dispose();

            return lines.ToArray();
        }
    }
}
