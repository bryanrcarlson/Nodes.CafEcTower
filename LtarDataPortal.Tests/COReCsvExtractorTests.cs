using Nsar.Nodes.CafEcTower.LtarDataPortal.Extract;
using Nsar.Nodes.CafEcTower.LtarDataPortal.Load;
using Nsar.Nodes.Models.LtarDataPortal.CORe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;

namespace LtarDataPortal.Tests
{
    public class COReCsvExtractorTests
    {
        [Fact]
        public void GetObservationsFromString_ValidData_ReturnsCorrectObservations()
        {
            // Arrange
            COReCsvExtractor sut = new COReCsvExtractor();
            List<Observation> expected = getValidObservations();
            string fileContent = getValidCOReString();

            // Act
            List<Observation> actual = sut.GetObservations(fileContent, -8);

            // Assert
            // TODO: Add Equals functions to Measurement class
            Assert.Equal(expected[0].AirPressure, actual[0].AirPressure);
            Assert.Equal(expected[0].AirTemperature, actual[0].AirTemperature);
            Assert.Equal(expected[0].BatteryVoltage, actual[0].BatteryVoltage);
            Assert.Equal(expected[0].DateTime, actual[0].DateTime);
            Assert.Equal(expected[0].LoggerTemperature, actual[0].LoggerTemperature);
            Assert.Equal(expected[0].LongWaveIn, actual[0].LongWaveIn);
            Assert.Equal(expected[0].LTARSiteAcronym, actual[0].LTARSiteAcronym);
            Assert.Equal(expected[0].PAR, actual[0].PAR);
            Assert.Equal(expected[0].Precipitation, actual[0].Precipitation);
            Assert.Equal(expected[0].RecordType, actual[0].RecordType);
            Assert.Equal(expected[0].RelativeHumidity, actual[0].RelativeHumidity);
            Assert.Equal(expected[0].ShortWaveIn, actual[0].ShortWaveIn);
            Assert.Equal(expected[0].StationID, actual[0].StationID);
            Assert.Equal(expected[0].WindDirection, actual[0].WindDirection);
            Assert.Equal(expected[0].WindSpeed, actual[0].WindSpeed);
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
    }
}
