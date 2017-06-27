using CsvHelper;
using Nsar.Nodes.CafEcTower.LoggerNet.Models;
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

        public List<MeteorologyObservation> GetRecords()
        {
            if (this.fileContent.Length <= 0) throw new Exception("No content");

            List<MeteorologyObservation> records = new List<MeteorologyObservation>();

            using (TextReader sr = new StringReader(trimMetaData(this.fileContent)))
            {
                CsvReader csv = new CsvReader(sr);
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.IgnoreQuotes = false;

                records = csv.GetRecords<MeteorologyObservation>().ToList();
            }

            return records;
        }

        public MeteorologyMetadata GetMetadata()
        {
            if (this.fileContent.Length <= 0) throw new Exception("No content");

            MeteorologyMetadata md = new MeteorologyMetadata();
            md.Variables = new List<MeteorologyVariables>();

            using (StringReader sr = new StringReader(this.fileContent))
            {
                string[] fileMeta = sr.ReadLine().Replace("\"", "").Split(',');

                md.FileFormat                   =   fileMeta[0];
                md.StationName                  =    fileMeta[1];
                md.DataLoggerType               =    fileMeta[2];
                md.SerialNumber                 =    Convert.ToInt32(fileMeta[3]);
                md.OperatingSystemVersion       =    fileMeta[4];
                md.DataloggerProgramName        =    fileMeta[5];
                md.DataloggerProgramSignature   =    Convert.ToInt32(fileMeta[6]);
                md.TableName                    =   fileMeta[7];

                string[] fieldNames = sr.ReadLine().Replace("\"", "").Split(',');
                string[] units = sr.ReadLine().Replace("\"", "").Split(',');
                string[] processing = sr.ReadLine().Replace("\"", "").Split(',');

                if ((fieldNames.Length != units.Length) || (units.Length != processing.Length))
                    throw new Exception("Error parsing field metadata");

                for(int col = 0; col < fieldNames.Length; col++)
                {
                    md.Variables.Add(
                        new MeteorologyVariables()
                        {
                            FieldName = fieldNames[col],
                            Units = units[col],
                            Processing = processing[col]
                        });
                }
            }

            return md;
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