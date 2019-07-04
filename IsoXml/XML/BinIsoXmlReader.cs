using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IsoXml.XML
{
    public class BinIsoXmlReader
    {
        public List<Model.GeoPoint> Parse(string fileName)
        {
            List<Model.GeoPoint> ps = new List<Model.GeoPoint>();
            BinaryReader binR = new BinaryReader(File.OpenRead(fileName));

            while (binR.BaseStream.Length > binR.BaseStream.Position)
            {

                DateTime dTime = new DateTime(1980, 1, 1).AddMilliseconds(binR.ReadUInt32());
                dTime = dTime.AddDays(binR.ReadUInt16());
                Model.GeoPoint point = new Model.GeoPoint();
                point.GpsUtcTime = dTime;

                point.Latitude = binR.ReadInt32() / 10000000.0;
                point.Longitude = binR.ReadInt32() / 10000000.0;
                //point.Elevation = binR.ReadInt32() / 1000.0;
                point.PositionStatus = binR.ReadSByte();
                //point.PDOP = binR.ReadUInt16() / 10.0f;
                point.HDOP = binR.ReadUInt16() / 10.0f;
                point.NumberOfSatellites = binR.ReadByte();
                //point.GpsUtcTime = new TimeSpan(binR.ReadUInt32() * 10);
                //point.GpsUtcDate = new DateTime(1980, 1, 1).AddDays(binR.ReadUInt16());

                byte nDlv = binR.ReadByte();
                for (int i = 0; i < nDlv; i++)
                {
                    byte DLVn = binR.ReadByte();
                    int d = binR.ReadInt32();
                    if (i == 0)
                        point.Speed = d;
                    else if (i == 1)
                        point.AppliedRate = d;
                }
                ps.Add(point);
            }

            binR.Close();
            return ps;
        }
    }
}
