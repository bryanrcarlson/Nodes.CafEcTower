using System;
using System.Collections.Generic;
using System.Text;

namespace Nsar.Nodes.CafEcTower.LoggerNet.Models
{
    public class MeteorologyMetadata
    {
        public string   FileFormat                  { get; set; }
        public string   StationName                 { get; set; }
        public string   DataLoggerType              { get; set; }
        public int      SerialNumber                { get; set; }
        public string   OperatingSystemVersion      { get; set; }
        public string   DataloggerProgramName       { get; set; }
        public int      DataloggerProgramSignature  { get; set; }
        public string   TableName                   { get; set; }

        public List<MeteorologyVariable> Variables { get; set; }
    }
}
