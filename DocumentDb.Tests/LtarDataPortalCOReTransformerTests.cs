using System;
using Xunit;
using Nsar.Nodes.CafEcTower.DocumentDb.Extract;
using Nsar.Nodes.Models.DocumentDb.Measurement;
using System.Collections.Generic;
using Nsar.Nodes.CafEcTower.DocumentDb.Transform;
using Nsar.Nodes.Models.LtarDataPortal.CORe;
using System.Globalization;

namespace DocumentDb.Tests
{
    public class LtarDataPortalCOReTransformerTests
    {
        [Fact]
        public void ToCOReContent_ValidData_ReturnsCorrectCORe()
        {
            // Arrange
            LtarDataPortalCOReTransformer sut = new LtarDataPortalCOReTransformer();
            List<Observation> expected = getValidObservations();

            // Act
            List<Observation> actual = sut.ToCOReObservations("CAF", "001", 'L', -8, getValidMeasurements());

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

        private List<Measurement> getValidMeasurements()
        {
            List<Measurement> results = new List<Measurement>()
            {
                new Measurement(
                    "", "", "", "WindSpeedTsResultant", "", "", "", "", "", "", null, "", null, DateTime.ParseExact("2017-09-06T00:00:00Z", "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(3.014338m, "m/s", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    }),
                new Measurement(
                    "", "", "", "TemperatureAirTsAvg", "", "", "", "", "", "", null, "", null, DateTime.ParseExact("2017-09-06T00:00:00Z", "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(27.80702m, "C", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    }),

                new Measurement(
                    "", "", "", "PrecipitationTsAccum", "", "", "", "", "", "", null, "", null, DateTime.ParseExact("2017-09-06T00:00:00Z", "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(0m, "m", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    }),
                new Measurement(
                    "", "", "", "WindDirection", "", "", "", "", "", "", null, "", null, DateTime.ParseExact("2017-09-06T00:00:00Z", "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(125.9m, "deg", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    }),
                new Measurement(
                    "", "", "", "RelativeHumidityTsAvg", "", "", "", "", "", "", null, "", null, DateTime.ParseExact("2017-09-06T00:00:00Z", "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(22.4503m, "%", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    }),
                new Measurement(
                    "", "", "", "BatteryVoltageTsAvg", "", "", "", "", "", "", null, "", null, DateTime.ParseExact("2017-09-06T00:00:00Z", "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(13.01541m, "V", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    }),
                new Measurement(
                    "", "", "", "PressureAirTsAvg", "", "", "", "", "", "", null, "", null, DateTime.ParseExact("2017-09-06T00:00:00Z", "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(93334.82m, "Pa", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    }),
                new Measurement(
                    "", "", "", "ParDensityTsAvg", "", "", "", "", "", "", null, "", null, DateTime.ParseExact("2017-09-06T00:00:00Z", "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(0.0002833229m, "mol/(m^2 s)", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    }),
                new Measurement(
                    "", "", "", "TemperaturePanelTsAvg", "", "", "", "", "", "", null, "", null, DateTime.ParseExact("2017-09-06T00:00:00Z", "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(29.87764m, "C", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    })
            };

            return results;
        }

        private List<Observation> getValidObservations()
        {
            List<Observation> result = new List<Observation>()
           {
               new Observation("CAF", "001", new DateTimeOffset(2017, 09, 05, 16, 0, 0, new TimeSpan(-8, 0, 0)), -8, 'L', 27.80702m, 3.014338m, 125.9m, 22.4503m, 0, 93.33482m, 283.3229m, null, null, 13.01541m, 29.87764m)
           };

            return result;
        }
    }
}
