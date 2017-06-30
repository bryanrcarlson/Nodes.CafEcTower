using Nsar.Common.Measure.Models;
using Nsar.Nodes.Models.LoggerNet.Meteorology;
using Nsar.Nodes.CafEcTower.LoggerNet.Transform;
using System.Collections.Generic;
using Xunit;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Tests
{
    public class DocumentDbMeasurementTransformerTests
    {
        [Fact]
        public void ToMeasurement_ValidData_ReturnCorrectMeasurements()
        {
            //# Arrange
            Measurement expectedMeasurement = new Measurement(
                "RelativeHumidityTsAvg",
                new System.DateTime(2017, 6, 20, 11, 30, 0),
                46.7815, -117.0820,
                new PhysicalQuantity(56.22676, "%"));
            Measurement expectedMeasurement_amb_tmpr_avg = new Measurement(
                "TemperatureAirTsAvg",
                new System.DateTime(2017, 6, 20, 11, 30, 0),
                46.7815, -117.0820,
                new PhysicalQuantity(4.940109, "C"));
            List<Measurement> actualMeasurements = new List<Measurement>();

            //# Act
            DocumentDbMeasurementTransformer sut = new DocumentDbMeasurementTransformer();
            actualMeasurements = sut.ToMeasurements(getMockMeteorology());

            //# Assert
            Assert.Equal(
                actualMeasurements
                    .Find(m => m.Name == expectedMeasurement.Name)
                    .PhysicalQuantity, 
                expectedMeasurement.PhysicalQuantity);
            Assert.Equal(
                actualMeasurements
                    .Find(m => m.Name == expectedMeasurement_amb_tmpr_avg.Name)
                    .PhysicalQuantity,
                expectedMeasurement_amb_tmpr_avg.PhysicalQuantity);
        }

        private Meteorology getMockMeteorology()
        {
            Meteorology met = new Meteorology();

            met.Observations = new List<Observation>();
            met.Observations.Add(new  Observation()
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

            met.Metadata = new Metadata()
            {
                FileFormat = "TOA5",
                StationName = "LTAR_CookEast",
                DataLoggerType = "CR3000",
                SerialNumber = 6503,
                OperatingSystemVersion = "CR3000.Std.31",
                DataloggerProgramName = "CPU:DEFAULT.CR3",
                DataloggerProgramSignature = 13636,
                TableName = "LTAR_Met",

                Variables = new List<Variable>()
                {
                    new Variable()
                    {
                        FieldName = "TIMESTAMP",
                        Units = "TS",
                        Processing = ""
                    },
                    new Variable()
                    {
                        FieldName = "RECORD",
                        Units = "",
                        Processing = ""
                    },
                    new Variable()
                    {
                        FieldName = "amb_tmpr_Avg",
                        Units = "C",
                        Processing = "Avg"
                    },
                    new Variable()
                    {
                        FieldName = "rslt_wnd_spd",
                        Units = "m/s",
                        Processing = "Smp"
                    },
                    new Variable()
                    {
                        FieldName = "wnd_dir_compass",
                        Units = "degrees",
                        Processing = "Smp"
                    },
                    new Variable()
                    {
                        FieldName = "RH_Avg",
                        Units = "%",
                        Processing = "Avg"
                    },
                    new Variable()
                    {
                        FieldName = "Precipitation_Tot",
                        Units = "mm",
                        Processing = "Tot"
                    },
                    new Variable()
                    {
                        FieldName = "amb_press_Avg",
                        Units = "kPa",
                        Processing = "Avg"
                    },
                    new Variable()
                    {
                        FieldName = "PAR_density_Avg",
                        Units = "umol/(s m^2)",
                        Processing = "Avg"
                    },
                    new Variable()
                    {
                        FieldName = "batt_volt_Avg",
                        Units = "V",
                        Processing = "Avg"
                    },
                    new Variable()
                    {
                        FieldName = "panel_tmpr_Avg",
                        Units = "C",
                        Processing = "Avg"
                    },
                    new Variable()
                    {
                        FieldName = "std_wnd_dir",
                        Units = "degrees",
                        Processing = "Smp"
                    },
                    new Variable()
                    {
                        FieldName = "VPD_air",
                        Units = "kpa",
                        Processing = "Smp"
                    },
                    new Variable()
                    {
                        FieldName = "Rn_meas_Avg",
                        Units = "W/m^2",
                        Processing = "Avg"
                    }
                }
            };

            return met;
        }
    }
}
