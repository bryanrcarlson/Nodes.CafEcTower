using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Nsar.Nodes.Models.LtarDataPortal.CORe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nsar.Nodes.CafEcTower.LtarDataPortal.Extract
{
    public class COReCsvExtractor
    {
        /// <summary>
        /// Converts a string representation of a CSV file formatted as CORe ingest file to Observations
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="utcOffset">Specify if known UTC offset to ensure integrety, leave null if prefer no check</param>
        /// <returns></returns>
        public List<Observation> GetObservations(string fileContent, int? utcOffsetCheck = null)
        {
            if (fileContent.Length <= 0) throw new ArgumentException("No content to parse");

            List<Observation> obs = new List<Observation>();

            using (TextReader sr = new StringReader(fileContent))
            using (CsvReader csv = new CsvReader(sr))
            {
                while(csv.Read())
                {
                    string ltarSiteAcronym = csv.GetField<string>("LTARSiteAcronym");
                    string stationID = csv.GetField<string>("StationID");
                    DateTimeOffset dateTime = csv.GetField<DateTimeOffset>("DateTime");
                    int utcOffset = utcOffsetCheck ?? dateTime.Offset.Hours;
                    char recordType = csv.GetField<char>("RecordType");
                    decimal? airTemperature = csv.GetField<decimal?>("AirTemperature");
                    decimal? windSpeed = csv.GetField<decimal?>("WindSpeed");
                    decimal? windDirection = csv.GetField<decimal?>("WindDirection");
                    decimal? relativeHumidity = csv.GetField<decimal?>("RelativeHumidity");
                    decimal? precipitation = csv.GetField<decimal?>("Precipitation");
                    decimal? airPressure = csv.GetField<decimal?>("AirPressure");
                    decimal? par = csv.GetField<decimal?>("PAR");
                    decimal? shortWaveIn = csv.GetField<decimal?>("ShortWaveIn");
                    decimal? longWaveIn = csv.GetField<decimal?>("LongWaveIn");
                    decimal? batteryVoltage = csv.GetField<decimal?>("BatteryVoltage");
                    decimal? loggerTemperature = csv.GetField<decimal?>("LoggerTemperature");

                    Observation o = new Observation(
                        ltarSiteAcronym,
                        stationID,
                        dateTime,
                        utcOffset,
                        recordType,
                        airTemperature,
                        windSpeed,
                        windDirection,
                        relativeHumidity,
                        precipitation,
                        airPressure,
                        par,
                        shortWaveIn,
                        longWaveIn,
                        batteryVoltage,
                        loggerTemperature);

                    obs.Add(o);
                }
            }

            return obs;
        }
    }
}
