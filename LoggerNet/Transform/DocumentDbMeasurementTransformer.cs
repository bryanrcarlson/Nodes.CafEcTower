using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nsar.Nodes.Models.LoggerNet.TOA5;
using Nsar.Nodes.Models.DocumentDb.Measurement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using Nsar.Nodes.CafEcTower.LoggerNet.Core;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Transform
{
    public class DocumentDbMeasurementTransformer
    {
        private readonly string targetSchemaVersion;
        private readonly string documentType;
        private readonly string metadataId;

        private readonly IMapper map;
        
        public string TargetSchemaVersion { get { return targetSchemaVersion; } }
        public string DocumentType { get { return documentType; } }
        public string MetadataId { get { return metadataId; } }

        public DocumentDbMeasurementTransformer(IMapper map,
            string targetSchemaVersion,
            string documentType = "Measurement",
            string metadataId = "CafMeteorologyEcTower")
        {
            this.targetSchemaVersion = targetSchemaVersion;
            this.documentType = documentType;
            this.metadataId = metadataId;
            this.map = map;
        }

        public List<Measurement> ToMeasurements(TOA5 meteorology)
        {
            List<Measurement> measurements = new List<Measurement>();

            foreach(IObservation obs in meteorology.Observations)
            {
                foreach (Variable variable in meteorology.Metadata.Variables)
                {
                    // Skip TIMESTAMP and RECORD
                    if (variable.FieldName == "TIMESTAMP" ||
                        variable.FieldName == "RECORD")
                        continue;

                    Measurement measurement = CreateMeasurementFromVariable(variable, obs, meteorology.Metadata);
                    measurements.Add(measurement);
                }
            }

            return measurements;
        }

        private Measurement CreateMeasurementFromVariable(Variable variable, IObservation observation, Metadata metadata)
        {
            // Look up property based on string, get value
            var value = observation.GetType().GetProperty(variable.FieldName).GetValue(observation, null);

            Common.Measure.Models.PhysicalQuantity pq = new Common.Measure.Models.PhysicalQuantity(
                        Convert.ToDecimal(value),
                        variable.Units);

            // TODO: create interface, use constructor injection
            Common.Measure.PhysicalQuantityConverter pqConverter = new Common.Measure.PhysicalQuantityConverter();

            Common.Measure.Models.PhysicalQuantity pqMetric = pqConverter.Convert(pq);
            string name = map.GetMeasurementName(variable.FieldName);
            DateTime measurementDateTime = new DateTime(observation.TIMESTAMP.Ticks, DateTimeKind.Utc);

            List<PhysicalQuantity> physicalQuantitis = new List<PhysicalQuantity>() {
                new PhysicalQuantity(pqMetric.Value, pqMetric.Unit, 0, 0, 0, DateTime.UtcNow, "DocumentDbMeasurementTransformer")
            };

            Location location = new Location("Point",
                map.GetLatFromStation(metadata),
                map.GetLonFromStation(metadata));

            string fieldId = map.GetFieldID(metadata);

            string partitionKey = "EcTower_" + fieldId + "_" + name;
            string id = fieldId + "_" + name + "_" + measurementDateTime.ToString("o");

            return new Measurement(partitionKey, id, DocumentType, name, TargetSchemaVersion, metadataId,
                "", "", "", "", null, fieldId, location, measurementDateTime, physicalQuantitis);
        }
    }
}
