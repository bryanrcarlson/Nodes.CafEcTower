using CsvHelper;
using Nsar.Nodes.Models.LoggerNet.Meteorology;
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
        private readonly int utcOffset;

        public string FileName { get { return fileName; } }
        public string FileContent { get { return fileContent; } }
        public int UtcOffset { get { return utcOffset; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pathToFile">Absolute or relative file path to .dat file containing observations</param>
        /// <param name="utcOffset">UTC timezone offset, accepts negatives.  Use -8 for PST, for example</param>
        public MeteorologyCsvTableExtractor(string pathToFile, int utcOffset = 0)
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

            this.utcOffset = utcOffset;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="fileContent">Contents of file, do not use this for large files</param>
        /// <param name="utcOffset">UTC timezone offset, accepts negatives.  Use -8 for PST, for example</param>
        public MeteorologyCsvTableExtractor(string fileName, string fileContent, int utcOffset = 0)
        {
            this.fileContent = fileContent;
            this.fileName = fileName;
            this.utcOffset = utcOffset;
        }

        public List<Observation> GetObservations()
        {
            if (this.fileContent.Length <= 0) throw new Exception("No content");

            List<Observation> observations = new List<Observation>();

            using (TextReader sr = new StringReader(trimMetaData(this.fileContent)))
            {
                CsvReader csv = new CsvReader(sr);
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.IgnoreQuotes = false;

                observations = csv.GetRecords<Observation>().ToList();
                
            }

            // Datetimes were in unknown timezone (most likely PST, or UTC-08), so convert to UTC
            foreach(Observation obs in observations)
            {
                obs.TIMESTAMP = new DateTime(obs.TIMESTAMP.AddHours((-1 * utcOffset)).Ticks, DateTimeKind.Utc);
            }

            return observations;
        }

        public Metadata GetMetadata()
        {
            if (this.fileContent.Length <= 0) throw new Exception("No content");

            Metadata md = new Metadata();
            md.Variables = new List<Variable>();

            using (StringReader sr = new StringReader(this.fileContent))
            {
                string[] fileMeta = sr.ReadLine().Replace("\"", "").Split(',');

                md.FileFormat                   =    fileMeta[0];
                md.StationName                  =    fileMeta[1];
                md.DataLoggerType               =    fileMeta[2];
                md.SerialNumber                 =    Convert.ToInt32(fileMeta[3]);
                md.OperatingSystemVersion       =    fileMeta[4];
                md.DataloggerProgramName        =    fileMeta[5];
                md.DataloggerProgramSignature   =    Convert.ToInt32(fileMeta[6]);
                md.TableName                    =    fileMeta[7];

                string[] fieldNames = sr.ReadLine().Replace("\"", "").Split(',');
                string[] units = sr.ReadLine().Replace("\"", "").Split(',');
                string[] processing = sr.ReadLine().Replace("\"", "").Split(',');

                if ((fieldNames.Length != units.Length) || (units.Length != processing.Length))
                    throw new Exception("Error parsing field metadata");

                for(int col = 0; col < fieldNames.Length; col++)
                {
                    md.Variables.Add(
                        new Variable()
                        {
                            FieldName = fieldNames[col],
                            Units = units[col],
                            Processing = processing[col]
                        });
                }
            }

            return md;
        }

        public Meteorology GetMeteorology()
        {
            Meteorology met = new Meteorology();

            met.Metadata = GetMetadata();
            met.Observations = GetObservations();

            return met;
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