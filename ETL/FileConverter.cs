using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using ETL.Models;

namespace ETL
{
    internal class FileConverter
    {
        FileReader fileReader = new();

        private IEnumerable<TransactionDTO> GetListTransDTO(string[] lines, MetaLogWorker metalogworker)
        {
            foreach (var line in lines)
            {   TransactionDTO transactionDTO = GetTransactionDTO(line, metalogworker);
                if(transactionDTO!=null)
                    yield return GetTransactionDTO(line, metalogworker);
            }
        }

        private TransactionDTO GetTransactionDTO(string line, MetaLogWorker metalogworker)
        {
            string[] fields = line.Split(",");

            string[] validformats = new[] { "MM/dd/yyyy", "yyyy/MM/dd", "MM/dd/yyyy", "MM/dd/yyyy", "yyyy-dd-MM" };
            TransactionDTO transcationDTO = new();
            try
            {
                transcationDTO = new TransactionDTO()
                {
                    first_name = fields[0].Trim(),
                    last_name = fields[1].Trim(),
                    city = Regex.Replace(Regex.Replace(fields[2].Trim(), @"\“", ""), "\"", ""),
                    payment = decimal.Parse(fields[5].Trim()),
                    date = DateTime.ParseExact(fields[6].Trim(), validformats, null),
                    account_number = long.Parse(fields[7].Trim()),
                    service = fields[8].Trim()
                };
                metalogworker.parsedLines++;
            }
            catch
            {
                metalogworker.foundErrors++;
            }
            return transcationDTO;
        }

        private Output ConvertListDTO_ToOutput(IEnumerable<TransactionDTO> dtoModels)
        {
            Output output = new();

            foreach (var dtoModel in dtoModels)
            {
                var cur_city = output.transcations.FirstOrDefault(x => x.city == dtoModel.city);

                if (cur_city == null)
                {
                    output.transcations.Add(new TranscationModel()
                    {
                        city = dtoModel.city
                    });
                }

                var cur_serv = output.transcations.FirstOrDefault(x => x.city == dtoModel.city).services.FirstOrDefault(x => x.name == dtoModel.service);

                if (cur_serv == null)
                {
                    output.transcations.FirstOrDefault(x => x.city == dtoModel.city).services.Add(new Service()
                    {
                        name = dtoModel.service
                    });
                }

                var cur_payer = output.transcations.FirstOrDefault(x => x.city == dtoModel.city).services
                    .FirstOrDefault(x => x.name == dtoModel.service).payers
                    .FirstOrDefault(x => x.account_number == dtoModel.account_number);

                output.transcations.FirstOrDefault(x => x.city == dtoModel.city).services.FirstOrDefault(x => x.name == dtoModel.service).payers.Add(new Payer()
                {
                    name = dtoModel.first_name + " " + dtoModel.last_name,
                    payment = dtoModel.payment,
                    date = dtoModel.date,
                    account_number = dtoModel.account_number
                });

                output.transcations.ForEach(x =>
                {
                    x.services.ForEach(s =>
                    {
                        s.total = s.payers.Sum(p => p.payment);
                    });
                    x.total = x.services.Sum(s => s.total);
                });
            }
            return output;
        }

        public async Task<Output> GetOutputFromTXTFileAsync(string filePath, MetaLogWorker metalogworker)
        {
            string fileContent = await fileReader.TxtReadAsync(filePath);
            string[] lines = fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<TransactionDTO> dtoS = GetListTransDTO(lines, metalogworker);
            metalogworker.parsedFiles++;
            return ConvertListDTO_ToOutput(dtoS);
        }

        public async Task<Output> GetOutputFromCSVFileAsync(string filePath, MetaLogWorker metalogworker)
        {
            List<string> lines = (await fileReader.CsvReadAsync(filePath)).ToList();

            int indexOfHeaders = lines.IndexOf("first_name,last_name,address,payment,date,account_number,service");

            if (indexOfHeaders != -1) lines.RemoveAt(indexOfHeaders);

            IEnumerable<TransactionDTO> dtoS = GetListTransDTO(lines.ToArray(),metalogworker);
            metalogworker.parsedLines++;
            return ConvertListDTO_ToOutput(dtoS);
        }
    }
}
