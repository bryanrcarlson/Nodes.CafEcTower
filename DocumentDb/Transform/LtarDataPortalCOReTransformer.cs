using Nsar.Nodes.Models.DocumentDb.Measurement;
using Nsar.Nodes.Models.LtarDataPortal.CORe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nsar.Nodes.CafEcTower.DocumentDb.Transform
{
    public class LtarDataPortalCOReTransformer
    {
        public List<Observation> ToCOReObservations(
            string ltarSiteAcronym,
            string stationId,
            char recordType,
            int utcOffset, 
            List<Measurement> measurements)
        {
            List<Observation> observations = new List<Observation>();

            IEnumerable<IGrouping<DateTime, Measurement>> groups = measurements.GroupBy(d => d.MeasurementDateTime);
            foreach (IGrouping<DateTime, Measurement> group in groups)
            {
                // TODO: Don't hardcode!  When you have time...
                // TODO: Unit conversions!
                //var at = group.SingleOrDefault(m => m.Name == "TemperatureAirTsAvg");
                //var airTemp = at?.PhysicalQuantities[0].Value;

                var airTemp = group.SingleOrDefault(m => m.Name == "TemperatureAirTsAvg")?.PhysicalQuantities[0].Value;

                var windSp = group.SingleOrDefault(m => m.Name == "WindSpeedTsResultant")?.PhysicalQuantities[0].Value;

                var windDir = group.SingleOrDefault(m => m.Name == "WindDirection")?.PhysicalQuantities[0].Value;

                var rH = group.SingleOrDefault(m => m.Name == "RelativeHumidityTsAvg")?.PhysicalQuantities[0].Value;

                var precip = group.SingleOrDefault(m => m.Name == "PrecipitationTsAccum")?.PhysicalQuantities[0].Value;

                var airPress = group.SingleOrDefault(m => m.Name == "PressureAirTsAvg")?.PhysicalQuantities[0].Value;

                var par = group.SingleOrDefault(m => m.Name == "ParDensityTsAvg")?.PhysicalQuantities[0].Value;

                var battVolt = group.SingleOrDefault(m => m.Name == "BatteryVoltageTsAvg")?.PhysicalQuantities[0].Value;

                var loggerTemp = group.SingleOrDefault(m => m.Name == "TemperaturePanelTsAvg")?.PhysicalQuantities[0].Value;
                
                //var airTemp = group.SingleOrDefault(m => m.Name == "TemperatureAirTsAvg").PhysicalQuantities[0].Value;
                //var windSp = group.SingleOrDefault(m => m.Name == "WindSpeedTsResultant").PhysicalQuantities[0].Value;
                //var windDir = group.SingleOrDefault(m => m.Name == "WindDirection").PhysicalQuantities[0].Value;
                //var rH = group.SingleOrDefault(m => m.Name == "RelativeHumidityTsAvg").PhysicalQuantities[0].Value;
                //var precip = group.SingleOrDefault(m => m.Name == "PrecipitationTsAccum").PhysicalQuantities[0].Value;
                //var airPress = group.SingleOrDefault(m => m.Name == "PressureAirTsAvg").PhysicalQuantities[0].Value;
                //var par = group.SingleOrDefault(m => m.Name == "ParDensityTsAvg").PhysicalQuantities[0].Value;
                //var battVolt = group.SingleOrDefault(m => m.Name == "BatteryVoltageTsAvg").PhysicalQuantities[0].Value;
                //var loggerTemp = group.SingleOrDefault(m => m.Name == "TemperaturePanelTsAvg").PhysicalQuantities[0].Value;
                //var dto = new DateTimeOffset(group.Key, new TimeSpan(utcHourOffset, 0, 0));

                DateTimeOffset dtoUtc = new DateTimeOffset(group.Key, TimeSpan.Zero);
                DateTimeOffset dtoSpecified = dtoUtc.ToOffset(TimeSpan.FromHours(utcOffset));

                // Convert units if needed
                // m to mm
                if(precip != null) precip = precip * 1000;
                // Pa to kPa
                if (airPress != null) airPress = airPress / 1000;
                // mol/(m^2 s) to umol/(m^2 s)
                if (par != null) par = par * 1000000;

                Observation o = new Observation(
                    ltarSiteAcronym,
                    stationId,
                    dtoSpecified,
                    recordType,
                    airTemp,
                    windSp,
                    windDir,
                    rH,
                    precip,
                    airPress,
                    par,
                    null,
                    null,
                    battVolt,
                    loggerTemp);

                observations.Add(o);
            }

            return observations;
        }

        public CORe ToCORe(
            string ltarSiteAcronym,
            string stationId,
            char recordType,
            int utcOffset,
            List<Measurement> measurements)
        {
            throw new NotImplementedException();
        }
    }
}
