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

            // TODO: Create a dictionary<string, geocoordinates>??
            this.stationsMap = File.ReadAllText(@"Assets/map-station-name-to-geocoordinates.json");

            string measurementMap = File.ReadAllText(@"Assets/map-met-record-to-measure.json");
            this.mapDataFieldsToMeasurementName = JsonConvert.DeserializeObject<Dictionary<string, string>>(measurementMap);
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
                    measurement.Name = getMeasurementNameFromFieldName(variable.FieldName);
                    measurement.MeasurementDateTime = obs.TIMESTAMP;
                    measurement.PhysicalQuantities.Add(
                        new PhysicalQuantity(pqMetric.Value, pqMetric.Unit, pqMetric.Precision)
                        {
                            QcAppliedCode = 0,
                            QcResultCode = 0,
                            QualityCode = 0,
                            SourceId = "DocumentDbMeasurementTransformer",
                            SubmissionDateTime = DateTime.Now
                        });
                    measurement.Location = new Location()
                    {
                        Type = "Point",
                        Coordinates = new List<double>()
                        {
                            getLatFromStation(meteorology.Metadata.StationName),
                            getLonFromStation(meteorology.Metadata.StationName)
                        }
                    };
                    measurement.FieldId = meteorology.Metadata.StationName;

                    // TODO: Unit conversion?
                    //Measurement measurement = new Measurement(
                    //    getMeasurementNameFromFieldName(variable.FieldName),
                    //    obs.TIMESTAMP,
                    //    getLatFromStation(record.Metadata.StationName),
                    //    getLonFromStation(record.Metadata.StationName),
                    //    new PhysicalQuantity(
                    //        Convert.ToDouble(value),
                    //        variable.Units));

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

            measurement.Type = "Measurement";
            measurement.MetadataId = "";
            measurement.SchemaVersion = "0.1.0";

            measurement.PhysicalQuantities = new List<PhysicalQuantity>();

            return measurement;
        }
    }
}
