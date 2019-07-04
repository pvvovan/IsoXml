using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsoXml.Model
{
    public class Prescription
    {
        public List<PrescriptionCell> Cells { get; set; }
    }

    public class PrescriptionCell
    {
        public List<GeoPoint> CellPoints { get; set; }

        public PrescriptionAttribute Attribute { get; set; }
    }

    public class PrescriptionAttribute : Dictionary<string, double>
    {
        public new double this[string column] 
        { 
            get 
            {
                return base[column]; 
            } 
            set 
            {
                base[column] = value; 
            }
        }
    }
}
