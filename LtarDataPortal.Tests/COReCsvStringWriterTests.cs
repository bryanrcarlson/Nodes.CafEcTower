using Nsar.Nodes.CafEcTower.LtarDataPortal.Load;
using Nsar.Nodes.Models.LtarDataPortal.CORe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;

namespace LtarDataPortal.Tests
{
    public class COReCsvStringWriterTests
    {
        [Fact]
        public void GetContentString_ValidData_ReturnsCorrectCOReString()
        {
            // Arrange
            COReCsvStringWriter sut = new COReCsvStringWriter();
            string expected = getValidCOReString();

            // Act
            string actual = sut.GetContentString(getValidObservations());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetFilenameUtcDateTime_ValidData_ReturnsCorrectFilename()
        {
            // Arrange
            COReCsvStringWriter sut = new COReCsvStringWriter();
            string expected = getValidFilenameUtc();
            List<Observation> obs = getValidObservations();

            // Act
            string actual = sut.GetFilenameUtcDateTime(obs[0]);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetFilenamePstDateTime_ValidData_ReturnsCorrectFilename()
        {
            // Arrange
            COReCsvStringWriter sut = new COReCsvStringWriter();
            string expected = getValidFilenamePst();
            List<Observation> obs = getValidObservations();

            // Act
            string actual = sut.GetFilenamePstDateTime(obs[0]);

            // Assert
            Assert.Equal(expected, actual);
        }

        private List<Observation> getValidObservations()
        {
            DateTime dt = DateTime.ParseExact("2017-11-01T00:00:00Z", "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            DateTimeOffset dtoUtc = new DateTimeOffset(dt, TimeSpan.Zero);
            DateTimeOffset dtoSpecified = dtoUtc.ToOffset(TimeSpan.FromHours(-8));
            

            List<Observation> result = new List<Observation>()
           {
               new Observation("CAF", "001", dtoSpecified, -8, 'L', 27.80702m, 3.014338m, 125.9m, 22.4503m, 0, 93.33482m, 283.3229m, null, null, 13.01541m, 29.87764m)
           };

            return result;
        }

        private string getValidCOReString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("LTARSiteAcronym,StationID,DateTime,RecordType,AirTemperature,WindSpeed,WindDirection,RelativeHumidity,Precipitation,AirPressure,PAR,ShortWaveIn,LongWaveIn,BatteryVoltage,LoggerTemperature");
            sb.AppendLine("CAF,001,2017-10-31T16:00-08:00,L,27.80702,3.014338,125.9,22.4503,0,93.33482,283.3229,,,13.01541,29.87764");
            return sb.ToString();
        }

        private string getValidFilenameUtc()
        {
            string month = DateTime.UtcNow.ToString("MM");
            string year = DateTime.UtcNow.ToString("yyyy");
            return "cafMET001L_01_"+year+month+"00_00.csv";
        }

        private string getValidFilenamePst()
        {
            DateTime dt = DateTime.Now;

            if (TimeZoneInfo.Local.IsDaylightSavingTime(dt))
            {
                dt.AddHours(1);
            }
            string month = DateTime.UtcNow.ToString("MM");
            string year = DateTime.UtcNow.ToString("yyyy");
            return "cafMET001L_01_" + year + month + "00_00.csv";
        }
    }
}
