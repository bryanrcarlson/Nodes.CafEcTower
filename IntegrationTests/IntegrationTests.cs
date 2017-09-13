using Nsar.Nodes.CafEcTower.DocumentDb.Extract;
using Nsar.Nodes.CafEcTower.DocumentDb.Transform;
using Nsar.Nodes.CafEcTower.LtarDataPortal.Load;
using Nsar.Nodes.Models.DocumentDb.Measurement;
using Nsar.Nodes.Models.LtarDataPortal.CORe;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace IntegrationTests
{
    public class IntegrationTests
    {
        [Fact]
        public void ConvertJsonDocumentDBMeasurementsToCOReCsvString_ValidData_ReturnValid()
        {
            // Arrange
            string json = getJsonValidMeasurements();
            string expected = getValidCOReString();

            MeasurementJsonExtractor extractor = new MeasurementJsonExtractor();
            LtarDataPortalCOReTransformer transformer = new LtarDataPortalCOReTransformer();
            COReCsvStringWriter loader = new COReCsvStringWriter();

            // Act
            List<Measurement> measurements = extractor.ToMeasurements(json);
            List<Observation> observations = transformer.ToCOReObservations("CAF", "001", 'L', -8, measurements);
            string actual = loader.GetContentString(observations);

            // Assert
            Assert.Equal(expected, actual);
        }

        private string getJsonValidMeasurements()
        {
            return "[ { \"name\": \"WindSpeedTsResultant\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 3.014338, \"unit\": \"m/s\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"TemperatureAirTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 27.80702, \"unit\": \"C\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"PrecipitationTsAccum\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 0, \"unit\": \"m\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"WindDirection\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 125.9, \"unit\": \"deg\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"RelativeHumidityTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 22.4503, \"unit\": \"%\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"BatteryVoltageTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 13.01541, \"unit\": \"V\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"PressureAirTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 93334.82, \"unit\": \"Pa\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"ParDensityTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 0.0002833229, \"unit\": \"mol/(m^2 s)\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] }, { \"name\": \"TemperaturePanelTsAvg\", \"measurementDateTime\": \"2017-09-06T00:00:00Z\", \"physicalQuantities\": [ { \"value\": 29.87764, \"unit\": \"C\", \"qualityCode\": 0, \"qcAppliedCode\": 0, \"qcResultCode\": 0, \"submissionDateTime\": \"2017-09-06T00:04:15.9797575Z\", \"sourceId\": \"DocumentDbMeasurementTransformer\" } ] } ]";
        }

        private string getValidCOReString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("LTARSiteAcronym,StationID,DateTime,RecordType,AirTemperature,WindSpeed,WindDirection,RelativeHumidity,Precipitation,AirPressure,PAR,ShortWaveIn,LongWaveIn,BatteryVoltage,LoggerTemperature");
            sb.AppendLine("CAF,001,2017-09-05T16:00-08:00,L,27.81,3.01,125.90,22.45,0.00,93.33,283.32,,,13.02,29.88");
            return sb.ToString();
        }
    }
}
