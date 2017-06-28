using Nsar.Common.Measure.Models;
using Nsar.Nodes.CafEcTower.LoggerNet.Models;
using Nsar.Nodes.CafEcTower.LoggerNet.Transform;
using System.Collections.Generic;
using Xunit;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Tests
{
    public class MeteorologyRecordToMeasurementTransformerTests
    {
        [Fact]
        public void ToMeasurement_ValidData_ReturnCorrectMeasurements()
        {
            //# Arrange
            Measurement expectedMeasurement = new Measurement(
                "RelativeHumidityAvg",
                new System.DateTime(2017, 6, 20, 11, 30, 0),
                46.7815, -117.0820,
                new PhysicalQuantity(56.22676, "%"));
            List<Measurement> actualMeasurements = new List<Measurement>();

            //# Act
            MeteorologyRecordToMeasurementTransformer sut = new MeteorologyRecordToMeasurementTransformer();
            actualMeasurements = sut.ToMeasurements(getMockMeteorologyRecord());

            //# Assert
            Assert.Equal(
                actualMeasurements
                    .Find(m => m.Name == expectedMeasurement.Name)
                    .PhysicalQuantity.Value, 
                expectedMeasurement.PhysicalQuantity.Value);
        }

        private MeteorologyRecord getMockMeteorologyRecord()
        {
            MeteorologyRecord record = new MeteorologyRecord();

            record.Observations = new List<MeteorologyObservation>();
            record.Observations.Add(new  MeteorologyObservation()
            {
                TIMESTAMP = new System.DateTime(2017, 6, 20, 11, 30, 00),
                RECORD = 15,
                amb_tmpr_Avg = 4.940109,
                rslt_wnd_spd = 4.940109,
                wnd_dir_compass = 259.7,
                RH_Avg = 56.22676,
                Precipitation_Tot = 0,
                amb_press_Avg = 93.25672,
                PAR_density_Avg = 1956.598,
                batt_volt_Avg = 13.63667,
                panel_tmpr_Avg = 25.22728,
                std_wnd_dir = 14.26,
                VPD_air = 1.244421,
                Rn_meas_Avg = 643.2509
            });

            record.Metadata = new MeteorologyMetadata()
            {
                FileFormat = "TOA5",
                StationName = "LTAR_CookEast",
                DataLoggerType = "CR3000",
                SerialNumber = 6503,
                OperatingSystemVersion = "CR3000.Std.31",
                DataloggerProgramName = "CPU:DEFAULT.CR3",
                DataloggerProgramSignature = 13636,
                TableName = "LTAR_Met",

                Variables = new List<MeteorologyVariables>()
                {
                    new MeteorologyVariables()
                    {
                        FieldName = "TIMESTAMP",
                        Units = "TS",
                        Processing = ""
                    },
                    new MeteorologyVariables()
                    {
                        FieldName = "amb_tmpr_Avg",
                        Units = "C",
                        Processing = "Avg"
                    },
                    new MeteorologyVariables()
                    {
                        FieldName = "Rn_meas_Avg",
                        Units = "W/m^2",
                        Processing = "Avg"
                    }
                }
            };

            return record;
        }
    }
}
