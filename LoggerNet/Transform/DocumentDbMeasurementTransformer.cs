using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Nsar.Common.Measure.Models;
using Nsar.Nodes.Models.LoggerNet.Meteorology;
using Nsar.Nodes.Models.DocumentDb.Measurement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Transform
{
    public class DocumentDbMeasurementTransformer
    {
        private readonly string stationsMap;
        private readonly Dictionary<string, string> mapDataFieldsToMeasurementName;

        public DocumentDbMeasurementTransformer()
        {
            //TODO: Error checking

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
        }
        public List<Measurement> ToMeasurements(Meteorology meteorology)
        {
            List<Measurement> measurements = new List<Measurement>();

            foreach(Observation obs in meteorology.Observations)
            {
                foreach(Variable variable in meteorology.Metadata.Variables)
                {
                    // Skip TIMESTAMP and RECORD
                    if (variable.FieldName == "TIMESTAMP" ||
                        variable.FieldName == "RECORD")
                        continue;

                    // Look up property based on string, get value
                    var value = obs.GetType().GetProperty(variable.FieldName).GetValue(obs, null);

                    Measurement measurement = getMeasurementWithDefaultValues();
                    Common.Measure.Models.PhysicalQuantity pq = new Common.Measure.Models.PhysicalQuantity(
                        Convert.ToDecimal(value),
                        variable.Units);
                    Common.Measure.PhysicalQuantityConverter pqConverter = new Common.Measure.PhysicalQuantityConverter();
                    Common.Measure.Models.PhysicalQuantity pqMetric = pqConverter.Convert(pq);
                    measurement.name = getMeasurementNameFromFieldName(variable.FieldName);
                    measurement.measurementDateTime = obs.TIMESTAMP;
                    measurement.physicalQuantities.Add(
                        new PhysicalQuantity(pqMetric.Value, pqMetric.Unit, pqMetric.Precision)
                        {
                            qcAppliedCode = 0,
                            qcResultCode = 0,
                            qualityCode = 0,
                            sourceId = "DocumentDbMeasurementTransformer",
                            submissionDateTime = DateTime.Now
                        });
                    measurement.location = new Location()
                    {
                        type = "Point",
                        coordinates = new List<double>()
                        {
                            getLatFromStation(meteorology.Metadata.StationName),
                            getLonFromStation(meteorology.Metadata.StationName)
                        }
                    };
                    measurement.fieldId = meteorology.Metadata.StationName;

                    // TODO: Unit conversion?
                    //Measurement measurement = new Measurement(
                    //    getMeasurementNameFromFieldName(variable.FieldName),
                    //    obs.TIMESTAMP,
                    //    getLatFromStation(record.Metadata.StationName),
                    //    getLonFromStation(record.Metadata.StationName),
                    //    new PhysicalQuantity(
                    //        Convert.ToDouble(value),
                    //        variable.Units));

                    measurement.id = measurement.fieldId + "_" + measurement.name + "_" + measurement.measurementDateTime.ToString("s");
                    measurements.Add(measurement);
                }
            }

            return measurements;
        }

        private double getLatFromStation(string stationName)
        {
            var stations = JObject.Parse(stationsMap);

            double lat = stations.Property("stations")
                .Values()
                .Single(s => s.Value<string>("name") == stationName)
                ["lat"].Value<double>();
            
            return lat;
        }

        private double getLonFromStation(string stationName)
        {
            var stations = JObject.Parse(stationsMap);
            double lon = stations.Property("stations")
                .Values()
                .Single(s => s.Value<string>("name") == stationName)
                ["lon"].Value<double>();

            return lon;
        }

        private string getMeasurementNameFromFieldName(string fieldName)
        {
            return mapDataFieldsToMeasurementName[fieldName];
        }

        private Measurement getMeasurementWithDefaultValues()
        {
            Measurement measurement = new Measurement();

            measurement.type = "Measurement";
            measurement.metadataId = "";
            measurement.schemaVersion = "0.1.0";

            measurement.physicalQuantities = new List<PhysicalQuantity>();

            return measurement;
        }
    }
}
