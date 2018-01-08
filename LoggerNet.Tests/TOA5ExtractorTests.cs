using Xunit;
using Nsar.Nodes.CafEcTower.LoggerNet.Extract;
using System.Collections.Generic;
using Nsar.Nodes.Models.LoggerNet.TOA5;
using System;
using Nsar.Nodes.Models.LoggerNet.TOA5.DataTables;
using System.Linq;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Tests
{
    public class TOA5ExtractorTests
    {
        private readonly string pathToFileWithValidContent;
        private readonly string pathToFileToTestTimeZone;

        public TOA5ExtractorTests()
        {
            pathToFileWithValidContent = @"Assets/CookEastEcTower_Flux_Raw_2018_01_08_1330.dat";
        }

        [Fact]
        public void GetObservations_ValidContent_ReturnsCorrectObservations()
        {
            //# Arrange
            List<IObservation> actualObservations = new List<IObservation>();
            TOA5Extractor sut = new TOA5Extractor(pathToFileWithValidContent);

            //# Act
            actualObservations = sut.GetObservations<Flux>().Cast<IObservation>().ToList();

            //# Assert
            Assert.Null(actualObservations[0]
                .GetType()
                .GetProperty("profile_tdr315_wc_Avg3")
                .GetValue(actualObservations[0], null));
            Assert.Equal(3173, actualObservations[0].RECORD);
            Assert.Equal(12.87643, actualObservations[0]
                .GetType()
                .GetProperty("batt_volt_Avg")
                .GetValue(actualObservations[0], null));
        }
        
//        [Fact]
//        public void GetMetadata_ValidContent_ReturnsCorrectMetadata()
//        {
//            //# Arrange
//            Metadata actualMetadata = new Metadata();
//            Metadata expectedMetadata = new Metadata()
//            {
//                FileFormat = "TOA5",
//                StationName = "LTAR_CookEast",
//                DataLoggerType = "CR3000",
//                SerialNumber = 6503,
//                OperatingSystemVersion = "CR3000.Std.31",
//                DataloggerProgramName = "CPU:DEFAULT.CR3",
//                DataloggerProgramSignature = 13636,
//                TableName = "LTAR_Met",
//
//                Variables = new List<Variable>()
//                {
//                    new Variable()
//                    {
//                        FieldName = "TIMESTAMP",
//                        Units = "TS",
//                        Processing = ""
//                    },
//                    new Variable()
//                    {
//                        FieldName = "RECORD",
//                        Units = "",
//                        Processing = ""
//                    },
//                    new Variable()
//                    {
//                        FieldName = "amb_tmpr_Avg",
//                        Units = "C",
//                        Processing = "Avg"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "rslt_wnd_spd",
//                        Units = "m/s",
//                        Processing = "Smp"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "wnd_dir_compass",
//                        Units = "degrees",
//                        Processing = "Smp"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "RH_Avg",
//                        Units = "%",
//                        Processing = "Avg"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "Precipitation_Tot",
//                        Units = "mm",
//                        Processing = "Tot"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "amb_press_Avg",
//                        Units = "kPa",
//                        Processing = "Avg"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "PAR_density_Avg",
//                        Units = "umol/(s m^2)",
//                        Processing = "Avg"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "batt_volt_Avg",
//                        Units = "V",
//                        Processing = "Avg"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "panel_tmpr_Avg",
//                        Units = "C",
//                        Processing = "Avg"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "std_wnd_dir",
//                        Units = "degrees",
//                        Processing = "Smp"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "VPD_air",
//                        Units = "kpa",
//                        Processing = "Smp"
//                    },
//                    new Variable()
//                    {
//                        FieldName = "Rn_meas_Avg",
//                        Units = "W/m^2",
//                        Processing = "Avg"
//                    }
//                }
//            };
//
//            //# Act
//            MeteorologyCsvTableExtractor sut = new MeteorologyCsvTableExtractor(pathToFileWithValidContent);
//            actualMetadata = sut.GetMetadata();
//
//            //# Assert
//            // TODO: Override obj.Equals for better testing
//            Assert.Equal(expectedMetadata.FileFormat, actualMetadata.FileFormat);
//            Assert.Equal(expectedMetadata.TableName, actualMetadata.TableName);
//            Assert.Equal(expectedMetadata.SerialNumber, actualMetadata.SerialNumber);
//
//            Assert.Equal(
//                expectedMetadata.Variables.Find(mv => mv.FieldName == "TIMESTAMP").Processing,
//                actualMetadata.Variables.Find(mv => mv.FieldName == "TIMESTAMP").Processing);
//            Assert.Equal(
//                expectedMetadata.Variables.Find(mv => mv.FieldName == "amb_tmpr_Avg").Units,
//                actualMetadata.Variables.Find(mv => mv.FieldName == "amb_tmpr_Avg").Units);
//            Assert.Equal(
//                expectedMetadata.Variables.Find(mv => mv.FieldName == "Rn_meas_Avg").Units,
//                actualMetadata.Variables.Find(mv => mv.FieldName == "Rn_meas_Avg").Units);
//        }
//
//        [Fact]
//        public void GetObservations_AdjustedTimezone_ReturnsCorrectTimes()
//        {
//            //# Arrange
//            List<IObservation> actualObservations = new List<IObservation>();
//
//            //# Act
//            MeteorologyCsvTableExtractor sut = new MeteorologyCsvTableExtractor(pathToFileToTestTimeZone, -8);
//            actualObservations = sut.GetObservations<Meteorology>().Cast<IObservation>().ToList();
//
//            //# Assert
//            Assert.Equal(actualObservations[0].TIMESTAMP, new DateTime(2017, 06, 20, 8, 30, 0));
//            Assert.Equal(actualObservations[1].TIMESTAMP, new DateTime(2017, 06, 20, 19, 30, 0));
//            Assert.Equal(actualObservations[2].TIMESTAMP, new DateTime(2017, 06, 21, 7, 15, 0));
//        }
//
//        [Fact]
//        public void GetObservations_ContentWithNAN_DoesNotErrorSetsNull()
//        {
//            // Arrange
//            string dataPath = @"Assets/CookEastEcTower_Met_Raw_2017_10_24_0615.dat";
//            MeteorologyCsvTableExtractor sut = new MeteorologyCsvTableExtractor(dataPath);
//
//            // Act
//            List<IObservation> actualObs = sut.GetObservations<Meteorology>()
//                .Cast<IObservation>()
//                .ToList();
//
//            // Assert
//            //            //observation.GetType().GetProperty(variable.FieldName).GetValue(observation, null)
//            Assert.Null(actualObs[0]
//                .GetType()
//                .GetProperty("VPD_air")
//                .GetValue(actualObs[0], null));
//            Assert.Null(actualObs[0]
//                .GetType()
//                .GetProperty("RH_Avg")
//                .GetValue(actualObs[0], null));
//        }
    }
}