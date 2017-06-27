using CsvHelper;
using LoggerNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Extract
{
    /// <summary>
    /// Represents an extractor for a comma seperated .dat file generated from the "LTAR_Met" table by LoggerNet Admin.
    /// </summary>
    public class MeteorologyCsvTableExtractor
    {
        private readonly string fileName;
        private readonly string fileContent;

        public string FileName { get { return fileName; } }
        public string FileContent { get { return fileContent; } }

        public MeteorologyCsvTableExtractor(string pathToFile)
        {
            if (!File.Exists(pathToFile))
                throw new ArgumentException("File does not exist");

            try
            {
                fileContent = File.ReadAllText(pathToFile);
            }
            catch(Exception e)
            {
                throw e;
            }

            try
            {
                fileName = Path.GetFileName(pathToFile);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public MeteorologyCsvTableExtractor(string fileName, string fileContent)
        {
            this.fileContent = fileContent;
            this.fileName = fileName;
        }

        public List<MeteorologyRecord> GetRecords()
        {
            if (this.fileContent.Length <= 0) throw new Exception("No content");

            List<MeteorologyRecord> records = new List<MeteorologyRecord>();

            using (TextReader sr = new StringReader(trimMetaData(this.fileContent)))
            {
                CsvReader csv = new CsvReader(sr);
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.IgnoreQuotes = false;

                records = csv.GetRecords<MeteorologyRecord>().ToList();
            }

            return records;
        }

        private string trimMetaData(string fileContent)
        {
            StringBuilder trimmed = new StringBuilder();

            using (StringReader sr = new StringReader(fileContent))
            {
                sr.ReadLine();  // skip first line of meta data
                trimmed.AppendLine(sr.ReadLine());  // read header
                sr.ReadLine();  // skip units row
                sr.ReadLine();  // skip statistics row

                trimmed.Append(sr.ReadToEnd()); // read all records
            }

            return trimmed.ToString();
        }
    }
}