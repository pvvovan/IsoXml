using System;
using System.Collections.Generic;
using System.Text;

namespace IsoXml.Model
{
    public class OuterBoundary
    {
        public string Name { get; set; }
        public List<GeoPoint> Points { get; set; }
        public List<InnerBoundary> InnerBoudnaries { get; set; }
    }
}
