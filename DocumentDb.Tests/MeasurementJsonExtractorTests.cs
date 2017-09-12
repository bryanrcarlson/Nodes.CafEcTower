using System;
using Xunit;
using Nsar.Nodes.CafEcTower.DocumentDb.Extract;
using Nsar.Nodes.Models.DocumentDb.Measurement;
using System.Collections.Generic;

namespace DocumentDb.Tests
{
    public class MeasurementJsonExtractorTests
    {
        [Fact]
        public void ToMeasurements_ValidData_ReturnsCorrectMeasurements()
        {
            // Arrange
            MeasurementJsonExtractor sut = new MeasurementJsonExtractor();
            List<Measurement> expected = getValidMeasurements();

            // Act
            List<Measurement> actual = sut.ToMeasurements(getJsonValidMeasurements());

            // Assert
            // TODO: Add Equals functions to Measurement class
            for(int i = 0; i < 2; i++)
            {
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].PhysicalQuantities[0].Value, actual[i].PhysicalQuantities[0].Value);
                Assert.Equal(expected[i].PhysicalQuantities[0].Unit, actual[i].PhysicalQuantities[0].Unit);
            }
        }

        private string getJsonValidMeasurements()
        {
            return "[ { \"name\": \"WindSpeedTsResultant\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 3.014338, \"unit\": \"m/s\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"TemperatureAirTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 27.80702, \"unit\": \"C\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"PrecipitationTsAccum\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 0, \"unit\": \"m\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"WindDirection\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 125.9, \"unit\": \"deg\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"RelativeHumidityTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 22.4503, \"unit\": \"%\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"BatteryVoltageTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 13.01541, \"unit\": \"V\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"PressureAirTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 93334.82, \"unit\": \"Pa\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"ParDensityTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 0.0002833229, \"unit\": \"mol/(m^2 s)\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"TemperaturePanelTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 29.87764, \"unit\": \"C\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] } ]";
        }

        private List<Measurement> getValidMeasurements()
        {
            List<Measurement> results = new List<Measurement>()
            {
                new Measurement(
                    "", "", "", "WindSpeedTsResultant", "", "", "", "", "", "", null, "", null, DateTime.Parse("2017-09-06T00:00:00Z"), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(3.014338m, "m/s", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    }),
                new Measurement(
                    "", "", "", "TemperatureAirTsAvg", "", "", "", "", "", "", null, "", null, DateTime.Parse("2017-09-06T00:00:00Z"), new List<PhysicalQuantity>()
                    {
                        new PhysicalQuantity(27.80702m, "C", 0, 0, 0, DateTime.Parse("2017-09-06T00:04:15.9797575Z"), "DocumentDbMeasurementTransformer")
                    })
            };

            return results;
        }
    }
}
