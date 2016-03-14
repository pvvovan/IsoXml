using System;
using System.Collections.Generic;
using System.Text;

namespace IsoXml.Model
{
    public class Field
    {
        public string Name { get; set; }

        public List<AB_Line> AB_Lines { get; set; }

        public List<OuterBoundary> OuterBoundaries { get; set; }
        public string ID { get; set; }

        public List<CoverageEvent> Events { get; set; }

        public List<Prescription> Prescriptions { get; set; }
    }
}
