using System;
using System.Collections.Generic;
using System.Text;

namespace IsoXml.Model
{
    public class Client
    {
        public string Name { get; set; }

        public string ID { get; set; }

        public List<Farm> Farms { get; set; }
    }
}
