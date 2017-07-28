using Nsar.Nodes.CafEcTower.LoggerNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nsar.Nodes.Models.LoggerNet.Meteorology;
using Newtonsoft.Json.Linq;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Mappers
{
    public class MapFromMeteorologyToCAFStandards : IMapper
    {
        private readonly string stationsMap;
        private readonly Dictionary<string, string> mapDataFieldsToMeasurementName;
        private readonly Dictionary<string, string> mapStationNameToFieldID;

        public MapFromMeteorologyToCAFStandards()
        {
            // TODO: GODS MANN!  Fix this hardcoded garbage.
            this.stationsMap = "{\"stations\":[{\"name\":\"LTAR_CookEast\",\"lat\":46.78152,\"lon\":-117.08205},{\"name\":\"LTAR_CookWest\",\"lat\":46.78404,\"lon\":-117.09083},{\"name\":\"LTAR_BoydNorth\",\"lat\":46.7551,\"lon\":-117.12605},{\"name\":\"LTAR_BoydSouth\",\"lat\":46.7503,\"lon\":-117.1285}]}";
            this.mapDataFieldsToMeasurementName = new Dictionary<string, string>()
            {
                { "TIMESTAMP", "DateTime" },
                { "RECORD", "RecordNumber" },
                { "amb_tmpr_Avg", "TemperatureAirTsAvg" },
                { "rslt_wnd_spd", "WindSpeedTsResultant" },
                { "wnd_dir_compass", "WindDirection" }, // Not sure if this is a point measurement or was processed
                { "RH_Avg", "RelativeHumidityTsAvg" },
                { "Precipitation_Tot", "PrecipitationTsAccum" },
                { "amb_press_Avg", "PressureAirTsAvg" },
                { "PAR_density_Avg", "ParDensityTsAvg" },
                { "batt_volt_Avg", "BatteryVoltageTsAvg" },
                { "panel_tmpr_Avg", "TemperaturePanelTsAvg" },
                { "std_wnd_dir", "WindDirectionTsStdDev" },
                { "VPD_air", "VaporPressureDeficitAir" },
                { "Rn_meas_Avg", "NetRadiationTsAvg" }
            };
            this.mapStationNameToFieldID = new Dictionary<string, string>()
            {
                {"LTAR_CookEast", "CookEast" },
                {"LTAR_CookWest", "CookWest" },
                {"LTAR_BoydNorth", "BoydNorth" },
                {"LTAR_BoydSouth", "BoydSouth" }
            };
        }

        public string GetFieldID(Metadata metadata)
        {
            return mapStationNameToFieldID[metadata.StationName];
        }

        public double GetLatFromStation(Metadata metadata)
        {
            var stations = JObject.Parse(stationsMap);

            double lat = stations.Property("stations")
                .Values()
                .Single(s => s.Value<string>("name") == metadata.StationName)
                ["lat"].Value<double>();

            return lat;
        }

        public double GetLonFromStation(Metadata metadata)
        {
            var stations = JObject.Parse(stationsMap);
            double lon = stations.Property("stations")
                .Values()
                .Single(s => s.Value<string>("name") == metadata.StationName)
                ["lon"].Value<double>();

            return lon;
        }

        public string GetMeasurementName(string dataField)
        {
            return mapDataFieldsToMeasurementName[dataField];
        }
    }
}
