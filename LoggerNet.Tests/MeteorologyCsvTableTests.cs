using Xunit;
using Nsar.Nodes.CafEcTower.LoggerNet.Extract;
using System.Collections.Generic;
using LoggerNet.Models;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Tests
{
    public class MeteorologyCsvTableTests
    {
        private readonly string pathToFileWithValidContent;

        public MeteorologyCsvTableTests()
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
        public void GetRecords_ValidContent_ReturnsCorrectData()
        {
            //# Arrange
            List<MeteorologyRecord> records = new List<MeteorologyRecord>();

            MeteorologyRecord expectedRecord = new MeteorologyRecord()
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
            records = sut.GetRecords();

            //# Assert
            // TODO: Override obj.Equals
            Assert.Equal(expectedRecord.amb_press_Avg, records[1].amb_press_Avg);
        }
    }
}
