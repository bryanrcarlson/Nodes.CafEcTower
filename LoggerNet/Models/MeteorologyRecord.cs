using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerNet.Models
{
    public class MeteorologyRecord
    {
        public DateTime TIMESTAMP { get; set; }
        public int RECORD { get; set; }
        public double amb_tmpr_Avg { get; set; }
        public double rslt_wnd_spd { get; set; }
        public double wnd_dir_compass { get; set; }
        public double RH_Avg { get; set; }
        public double Precipitation_Tot { get; set; }
        public double amb_press_Avg { get; set; }
        public double PAR_density_Avg { get; set; }
        public double batt_volt_Avg { get; set; }
        public double panel_tmpr_Avg { get; set; }
        public double std_wnd_dir { get; set; }
        public double VPD_air { get; set; }
        public double Rn_meas_Avg { get; set; }
    }
}