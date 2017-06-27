using LoggerNet.Models;
using System;
using System.Collections.Generic;
using System.IO;

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
            return null;
        }
    }
}
