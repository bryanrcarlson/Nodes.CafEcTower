using Xunit;
using Nsar.Nodes.CafEcTower.LoggerNet.Extract;
using System.Collections.Generic;
using Nsar.Nodes.CafEcTower.LoggerNet.Models;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Tests
{
    public class MeteorologyCsvTableExtractorTests
    {
        private readonly string pathToFileWithValidContent;

        public MeteorologyCsvTableExtractorTests()
        {
            pathToFileWithValidContent = @"Assets/CookEastEcTower_Met_Raw_2017_06_20_1115.dat";
        }

        [Fact]
        public void FilePathConstructor_ValidContent_SetsData()
        {
            //# Arrange 
            string expectedFileName = "CookEastEcTower_Met_Raw_2017_06_20_1115.dat";
            int expectedContentLength = 710;

            //# Act
            MeteorologyCsvTableExtractor sut = new MeteorologyCsvTableExtractor(pathToFileWithValidContent);

            //# Assert
            Assert.Equal(expectedFileName, sut.FileName);
            Assert.Equal(expectedContentLength, sut.FileContent.Length);
        }

        [Fact]
        public void GetRecords_ValidContent_ReturnsCorrectDataRecords()
        {
            //# Arrange
            List<MeteorologyObservation> actualRecords = new List<MeteorologyObservation>();

            MeteorologyObservation expectedRecord = new MeteorologyObservation()
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
            };

            //# Act
            MeteorologyCsvTableExtractor sut = new MeteorologyCsvTableExtractor(pathToFileWithValidContent);
            actualRecords = sut.GetObservations();

            //# Assert
            // TODO: Override obj.Equals for better test
            Assert.Equal(expectedRecord.amb_press_Avg, actualRecords[1].amb_press_Avg);
            Assert.Equal(expectedRecord.RECORD, actualRecords[1].RECORD);
            Assert.Equal(expectedRecord.Rn_meas_Avg, actualRecords[1].Rn_meas_Avg);
        }
        
        [Fact]
        public void GetMetadata_ValidContent_ReturnsCorrectMetadata()
        {
            //# Arrange
            MeteorologyMetadata actualMetadata = new MeteorologyMetadata();
            MeteorologyMetadata expectedMetadata = new MeteorologyMetadata()
            {
                FileFormat = "TOA5",
                StationName = "LTAR_CookEast",
                DataLoggerType = "CR3000",
                SerialNumber = 6503,
                OperatingSystemVersion = "CR3000.Std.31",
                DataloggerProgramName = "CPU:DEFAULT.CR3",
                DataloggerProgramSignature = 13636,
                TableName = "LTAR_Met",

                Variables = new List<MeteorologyVariable>()
                {
                    new MeteorologyVariable()
                    {
                        FieldName = "TIMESTAMP",
                        Units = "TS",
                        Processing = ""
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "RECORD",
                        Units = "",
                        Processing = ""
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "amb_tmpr_Avg",
                        Units = "C",
                        Processing = "Avg"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "rslt_wnd_spd",
                        Units = "m/s",
                        Processing = "Smp"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "wnd_dir_compass",
                        Units = "degrees",
                        Processing = "Smp"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "RH_Avg",
                        Units = "%",
                        Processing = "Avg"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "Precipitation_Tot",
                        Units = "mm",
                        Processing = "Tot"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "amb_press_Avg",
                        Units = "kPa",
                        Processing = "Avg"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "PAR_density_Avg",
                        Units = "umol/(s m^2)",
                        Processing = "Avg"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "batt_volt_Avg",
                        Units = "V",
                        Processing = "Avg"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "panel_tmpr_Avg",
                        Units = "C",
                        Processing = "Avg"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "std_wnd_dir",
                        Units = "degrees",
                        Processing = "Smp"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "VPD_air",
                        Units = "kpa",
                        Processing = "Smp"
                    },
                    new MeteorologyVariable()
                    {
                        FieldName = "Rn_meas_Avg",
                        Units = "W/m^2",
                        Processing = "Avg"
                    }
                }
            };

            //# Act
            MeteorologyCsvTableExtractor sut = new MeteorologyCsvTableExtractor(pathToFileWithValidContent);
            actualMetadata = sut.GetMetadata();

            //# Assert
            // TODO: Override obj.Equals for better testing
            Assert.Equal(expectedMetadata.FileFormat, actualMetadata.FileFormat);
            Assert.Equal(expectedMetadata.TableName, actualMetadata.TableName);
            Assert.Equal(expectedMetadata.SerialNumber, actualMetadata.SerialNumber);

            Assert.Equal(
                expectedMetadata.Variables.Find(mv => mv.FieldName == "TIMESTAMP").Processing,
                actualMetadata.Variables.Find(mv => mv.FieldName == "TIMESTAMP").Processing);
            Assert.Equal(
                expectedMetadata.Variables.Find(mv => mv.FieldName == "amb_tmpr_Avg").Units,
                actualMetadata.Variables.Find(mv => mv.FieldName == "amb_tmpr_Avg").Units);
            Assert.Equal(
                expectedMetadata.Variables.Find(mv => mv.FieldName == "Rn_meas_Avg").Units,
                actualMetadata.Variables.Find(mv => mv.FieldName == "Rn_meas_Avg").Units);
        }
    }
}