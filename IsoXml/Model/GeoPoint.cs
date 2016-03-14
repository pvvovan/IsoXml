using System;
using System.Collections.Generic;
using System.Text;

namespace IsoXml.Model
{
    public class GeoPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public sbyte PositionStatus { get; set; }
        //public float PDOP { get; set; }
        public float HDOP { get; set; }
        public byte NumberOfSatellites { get; set; }
        //public TimeSpan GpsUtcTime { get; set; }
        public DateTime GpsUtcTime { get; set; }
        public double Speed { get; set; }
        public double AppliedRate { get; set; }
        public double CrossTrackError { get; set; }
        public double Population { get; set; }
        public double Singulation { get; set; }
        public double Skips_Percentage { get; set; }
        public double Multiples_Percentage { get; set; }
    }
}
