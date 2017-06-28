using Newtonsoft.Json.Linq;
using Nsar.Common.Measure.Models;
using Nsar.Nodes.CafEcTower.LoggerNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Transform
{
    public class MeteorologyRecordToMeasurementTransformer
    {
        private readonly string stationsMap;
        private readonly string measurementMap;

        public MeteorologyRecordToMeasurementTransformer()
        {
            //TODO: Error checking
            this.stationsMap = File.ReadAllText(@"Assets/map-station-name-to-geocoordinates.json");
            //TODO: Add measurementMap
            
        }
        public List<Measurement> ToMeasurements(MeteorologyRecord record)
        {
            throw new NotImplementedException();

            List<Measurement> measurements = new List<Measurement>();

            foreach(MeteorologyObservation obs in record.Observations)
            {
                Measurement measurement = new Measurement(
                    nameof(obs.RH_Avg), obs.TIMESTAMP,
                    getLatFromStation(record.Metadata.StationName),
                    getLonFromStation(record.Metadata.StationName),
                    new PhysicalQuantity(
                        obs.RH_Avg,
                        record.Metadata.Variables.Find(v => v.FieldName == nameof(obs.RH_Avg)).Units));
            }

            return measurements;
        }

        private double getLatFromStation(string stationName)
        {
            var stations = JObject.Parse(stationsMap);
            //var lat = 
            //    from s in stations["stations"]
            //    where (string)s["name"] == stationName select(string)s["lat"]

            //var lat = stations["stations"]
            //    .OfType<JProperty>()
            //    .Where(n => n.name == stationName)
            //    .First()
            //    .Value<double>();

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
    }
}
