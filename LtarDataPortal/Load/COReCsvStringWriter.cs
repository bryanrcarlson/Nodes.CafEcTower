using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Nsar.Nodes.Models.LtarDataPortal.CORe;
using System.IO;

namespace Nsar.Nodes.CafEcTower.LtarDataPortal.Load
{
    public class COReCsvStringWriter
    {
        public string GetContentString(List<Observation> observations)
        {
            string fileContent;

            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer))
            {
                var options = new CsvHelper.TypeConversion.TypeConverterOptions
                {
                    Format = "yyyy-MM-ddTHH:mmzzz"
                };
                CsvHelper.TypeConversion.TypeConverterOptionsFactory.AddOptions<DateTimeOffset>(options);
                csv.WriteRecords(observations);
                writer.Flush();
                stream.Position = 0;
                fileContent = reader.ReadToEnd();
            }

            return fileContent;
        }
    }
}
