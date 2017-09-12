using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nsar.Nodes.Models.DocumentDb.Measurement;
using Newtonsoft.Json;

namespace Nsar.Nodes.CafEcTower.DocumentDb.Extract
{
    public class MeasurementJsonExtractor
    {
        public MeasurementJsonExtractor()
        {

        }

        public List<Measurement> ToMeasurements(string jsonMeasurements)
        {
            List<Measurement> results = new List<Measurement>();
            results = JsonConvert.DeserializeObject<List<Measurement>>(jsonMeasurements);

            return results;
        }
    }
}
