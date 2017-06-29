using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nsar.Common.Measure.Models;
using Nsar.Nodes.CafEcTower.LoggerNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Transform
{
    public class MeteorologyRecordToMeasurementTransformer
    {
        private readonly string stationsMap;
        private readonly Dictionary<string, string> mapDataFieldsToMeasurementName;

        public MeteorologyRecordToMeasurementTransformer()
        {
            //TODO: Error checking

            // TODO: Create a dictionary<string, geocoordinates>??
            this.stationsMap = File.ReadAllText(@"Assets/map-station-name-to-geocoordinates.json");

            string measurementMap = File.ReadAllText(@"Assets/map-met-record-to-measure.json");
            this.mapDataFieldsToMeasurementName = JsonConvert.DeserializeObject<Dictionary<string, string>>(measurementMap);
        }
        public List<Measurement> ToMeasurements(MeteorologyRecord record)
        {
            List<Measurement> measurements = new List<Measurement>();

            foreach(MeteorologyObservation obs in record.Observations)
            {
                foreach(MeteorologyVariable variable in record.Metadata.Variables)
                {
                    // Skip TIMESTAMP and RECORD
                    if (variable.FieldName == "TIMESTAMP" ||
                        variable.FieldName == "RECORD")
                        continue;

                    // Look up property based on string, get value
                    var value = obs.GetType().GetProperty(variable.FieldName).GetValue(obs, null);

                    // TODO: Unit conversion?
                    Measurement measurement = new Measurement(
                        getMeasurementNameFromFieldName(variable.FieldName),
                        obs.TIMESTAMP,
                        getLatFromStation(record.Metadata.StationName),
                        getLonFromStation(record.Metadata.StationName),
                        new PhysicalQuantity(
                            Convert.ToDouble(value),
                            variable.Units));

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
    }
}
