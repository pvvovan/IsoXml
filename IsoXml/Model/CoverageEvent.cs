using System;
using System.Collections.Generic;
using System.Text;

namespace IsoXml.Model
{
    public class CoverageEvent        
    {
        public string EventName { get; set; }

        public List<CoveragePolygon> Polygons { get; set; }
    }

    public class CoveragePolygon
    {
        public List<GeoPoint> Points { get; set; }
    }
}
