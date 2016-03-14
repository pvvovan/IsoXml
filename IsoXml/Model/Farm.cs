using System;
using System.Collections.Generic;
using System.Text;

namespace IsoXml.Model
{
    public class Farm
    {
        public string Name { get; set; }

        public string ID { get; set; }
        public List<Field> Fields { get; set; }
    }
}
