using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using DotSpatial.Data;
using DotSpatial.Topology;
using System.Linq;
using System.Text.RegularExpressions;

namespace AgGPS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //InitializeComponent();
        }

        private static void CreatePosFile(string fieldDir, IsoXml.Model.GeoPoint position)
        {
            string str = position.Longitude.ToString("F06");
            str += "E";
            str += position.Latitude.ToString("F06");
            str += "N";
            FileStream fileStream = File.Create(Path.Combine(fieldDir, str + ".pos"));
            fileStream.Dispose();
        }

        //private void buttonIsoXml_Click(object sender, EventArgs e)
        //{
        //    FolderBrowserDialog fbd = new FolderBrowserDialog();
        //    fbd.ShowNewFolderButton = false;
        //    var res = fbd.ShowDialog();
        //    if (res == System.Windows.Forms.DialogResult.OK)
        //        textBoxIsoXml.Text = fbd.SelectedPath;
        //    if (textBoxIsoXml.Text != "..." && textBoxAgGPS.Text != "...")
        //        buttonConvert.Enabled = true;
        //}

        //private void buttonAgGPS_Click(object sender, EventArgs e)
        //{
        //    FolderBrowserDialog fbd = new FolderBrowserDialog();
        //    fbd.ShowNewFolderButton = true;
        //    var res = fbd.ShowDialog();
        //    if (res == System.Windows.Forms.DialogResult.OK)
        //        textBoxAgGPS.Text = fbd.SelectedPath;
        //    if (textBoxIsoXml.Text != "..." && textBoxAgGPS.Text != "...")
        //        buttonConvert.Enabled = true;
        //}

        //private void buttonConvert_Click(object sender, EventArgs e)
        //{
        //    ConvertToAgGPS(textBoxIsoXml.Text, textBoxAgGPS.Text);

        //    //Polygon[] pg = new Polygon[1];
        //    //Feature f = new Feature();
        //    //FeatureSet fs = new FeatureSet(f.FeatureType);

        //    //Coordinate[] coord = new Coordinate[] { new Coordinate(30.0, 50), new Coordinate(31, 50), new Coordinate(31, 51), new Coordinate(30.0, 50) };
        //    //pg[0] = new Polygon(coord);
        //    //fs.Features.Add(pg[0]);
        //    //fs.SaveAs(@"D:\share\NCH_WinSrv\repos\MonitReport\IsoXml\Temptest.shp", true);

        //    textBoxAgGPS.Text = "...";
        //    textBoxIsoXml.Text = "...";
        //    buttonConvert.Enabled = false;

        //}

        public static void ConvertToAgGPS(string IsoXmlPath, string AgGpsPath)
        {
            CultureInfo ci2 = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci2;

            List<IsoXml.Model.Client> clients = new List<IsoXml.Model.Client>();
            var allIsoXmlFiles = Directory.GetFiles(IsoXmlPath, "*.xml", SearchOption.AllDirectories);

            // parse clients
            XmlSerializer serializer = new XmlSerializer(typeof(IsoXml.XML.client.CTR));
            foreach (var file in allIsoXmlFiles)
            {
                try
                {
                    using (Stream stream = File.OpenRead(file))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stream);
                        XmlNodeList elemList = doc.GetElementsByTagName("CTR");
                        foreach (XmlNode el in elemList)
                        {
                            var strstream = new StringReader(el.OuterXml);
                            object ob = serializer.Deserialize(strstream);
                            IsoXml.XML.client.CTR xcl = ob as IsoXml.XML.client.CTR;
                            clients.Add(new IsoXml.Model.Client()
                            {
                                ID = xcl.A,
                                Name = xcl.B,
                                Farms = new List<IsoXml.Model.Farm>()
                            });
                            IsoXml.Model.SyncId.Instance.Clients[xcl.B] = xcl.A;
                        }                        
                    }
                }
                catch { }
            }

            if (clients.Count == 0)
            {
                clients.Add(new IsoXml.Model.Client()
                {
                    ID = "1",
                    Name = "UnnamedClient",
                    Farms = new List<IsoXml.Model.Farm>()
                });
                IsoXml.Model.SyncId.Instance.Clients[clients[0].Name] = clients[0].ID;
            }

            // parse farms
            serializer = new XmlSerializer(typeof(IsoXml.XML.farm.FRM));
            foreach (var file in allIsoXmlFiles)
            {
                try
                {
                    using (Stream stream = File.OpenRead(file))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stream);
                        XmlNodeList elemList = doc.GetElementsByTagName("FRM");
                        foreach (XmlNode el in elemList)
                        {
                            var strstream = new StringReader(el.OuterXml);
                            object ob = serializer.Deserialize(strstream);
                            IsoXml.XML.farm.FRM xfa = ob as IsoXml.XML.farm.FRM;
                            IsoXml.Model.Farm farm = new IsoXml.Model.Farm()
                            {
                                Fields = new List<IsoXml.Model.Field>(),
                                ID = xfa.A,
                                Name = xfa.B
                            };
                            IsoXml.Model.SyncId.Instance.Farms[farm.Name] = farm.ID;
                            string cl_Id = xfa.I;
                            bool added = false;
                            foreach (var cl in clients)
                                if (cl.ID == cl_Id)
                                {
                                    cl.Farms.Add(farm);
                                    added = true;
                                    break;
                                }
                            if (!added)
                                clients[0].Farms.Add(farm);
                        }
                    }
                }
                catch { }
            }

            // parse fields
            serializer = new XmlSerializer(typeof(IsoXml.XML.field.PFD));
            foreach (var file in allIsoXmlFiles)
            {
                try
                {
                    using (Stream stream = File.OpenRead(file))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stream);
                        XmlNodeList elemList = doc.GetElementsByTagName("PFD");
                        foreach (XmlNode el in elemList)
                        {
                            var strstream = new StringReader(el.OuterXml);
                            object ob = serializer.Deserialize(strstream);
                            IsoXml.XML.field.PFD xfi = ob as IsoXml.XML.field.PFD;

                            IsoXml.Model.Field field = new IsoXml.Model.Field()
                            {
                                ID = xfi.A,
                                Name = xfi.C,
                                AB_Lines = new List<IsoXml.Model.AB_Line>(),
                                OuterBoundaries = new List<IsoXml.Model.OuterBoundary>()
                            };
                            IsoXml.Model.SyncId.Instance.Fields[field.Name] = field.ID;
                            string farm_id = xfi.F;
                            foreach (var cl in clients)
                                foreach (var fa in cl.Farms)
                                    if (fa.ID == farm_id)
                                    {
                                        fa.Fields.Add(field);
                                        goto exit2loop_1;
                                    }
                        exit2loop_1:
                            //CultureInfo ci = new CultureInfo("en-US");

                            if (xfi.PLN != null)
                                foreach (var xou in xfi.PLN)
                                {
                                    var ouBou = new IsoXml.Model.OuterBoundary()
                                    {
                                        InnerBoudnaries = new List<IsoXml.Model.InnerBoundary>(),
                                        Name = xou.B,
                                        Points = new List<IsoXml.Model.GeoPoint>()
                                    };
                                    field.OuterBoundaries.Add(ouBou);
                                    foreach (var xp in xou.LSG[0].PNT)
                                        ouBou.Points.Add(new IsoXml.Model.GeoPoint() { Longitude = double.Parse(xp.D, ci2), Latitude = double.Parse(xp.C, ci2) });
                                    if (xou.LSG.Length > 1)
                                        for (int i = 1; i < xou.LSG.Length; i++)
                                        {
                                            var inBou = new IsoXml.Model.InnerBoundary()
                                            {
                                                Points = new List<IsoXml.Model.GeoPoint>()
                                            };
                                            ouBou.InnerBoudnaries.Add(inBou);
                                            foreach (var xp in xou.LSG[i].PNT)
                                                inBou.Points.Add(new IsoXml.Model.GeoPoint() { Longitude = double.Parse(xp.D, ci2), Latitude = double.Parse(xp.C, ci2) });
                                        }
                                }

                            if (xfi.LSG != null)
                                foreach (var xli in xfi.LSG)
                                {
                                    var line = new IsoXml.Model.AB_Line()
                                    {
                                        //ID = xli.P102_ID,
                                        Name = xli.B,
                                        Points = new List<IsoXml.Model.GeoPoint>()
                                    };
                                    field.AB_Lines.Add(line);
                                    if (xli.PNT != null)
                                        foreach (var xp in xli.PNT)
                                            line.Points.Add(new IsoXml.Model.GeoPoint() { Longitude = double.Parse(xp.D, ci2), Latitude = double.Parse(xp.C, ci2) });
                                }
                        }
                    }
                }
                catch { }
            }

            
            
            // Value scalings
            List<IsoXml.XML.ISO11783_TaskDataVPN> xScalings = new List<IsoXml.XML.ISO11783_TaskDataVPN>();
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "VPN";
            xRoot.IsNullable = true;
            serializer = new XmlSerializer(typeof(IsoXml.XML.ISO11783_TaskDataVPN), xRoot);
            foreach (var file in allIsoXmlFiles)
            {
                try
                {
                    using (Stream stream = File.OpenRead(file))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stream);
                        XmlNodeList elemList = doc.GetElementsByTagName("VPN");
                        foreach (XmlNode el in elemList)
                        {
                            var strstream = new StringReader(el.OuterXml);
                            object ob = serializer.Deserialize(strstream);
                            IsoXml.XML.ISO11783_TaskDataVPN xsc = ob as IsoXml.XML.ISO11783_TaskDataVPN;
                            xScalings.Add(xsc);
                        }
                    }
                }
                catch { }
            }


            // Products
            List<IsoXml.XML.ISO11783_TaskDataPDT> xProducts = new List<IsoXml.XML.ISO11783_TaskDataPDT>();
            xRoot = new XmlRootAttribute();
            xRoot.ElementName = "PDT";
            xRoot.IsNullable = true;
            serializer = new XmlSerializer(typeof(IsoXml.XML.ISO11783_TaskDataPDT), xRoot);
            foreach (var file in allIsoXmlFiles)
            {
                try
                {
                    using (Stream stream = File.OpenRead(file))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stream);
                        XmlNodeList elemList = doc.GetElementsByTagName("PDT");
                        foreach (XmlNode el in elemList)
                        {
                            var strstream = new StringReader(el.OuterXml);
                            object ob = serializer.Deserialize(strstream);
                            IsoXml.XML.ISO11783_TaskDataPDT xpd = ob as IsoXml.XML.ISO11783_TaskDataPDT;
                            xProducts.Add(xpd);
                        }
                    }
                }
                catch { }
            }

            // Tasks with prescription maps        
            xRoot = new XmlRootAttribute();
            xRoot.ElementName = "TSK";
            xRoot.IsNullable = true;
            serializer = new XmlSerializer(typeof(IsoXml.XML.ISO11783_TaskDataTSK), xRoot);
            foreach (var file in allIsoXmlFiles)
            {
                try
                {
                    using (Stream stream = File.OpenRead(file))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stream);
                        XmlNodeList elemList = doc.GetElementsByTagName("TSK");
                        foreach (XmlNode el in elemList)
                        {                            
                            var strstream = new StringReader(el.OuterXml);
                            object ob = serializer.Deserialize(strstream);
                            IsoXml.XML.ISO11783_TaskDataTSK xtsk = ob as IsoXml.XML.ISO11783_TaskDataTSK;

                            if (xtsk.TZN != null && xtsk.GRD != null)
                            {
                                IsoXml.Model.Field field = null;
                                foreach (var cl in clients)
                                    foreach (var fa in cl.Farms)
                                        foreach (var fi in fa.Fields)
                                            if (fi.ID == xtsk.E)
                                                field = fi;
                                if (field.Prescriptions == null)
                                    field.Prescriptions = new List<IsoXml.Model.Prescription>();

                                // parse based on grid type number
                                switch (xtsk.GRD.I)
                                {
                                    case 1: // Grid type 1
                                        Dictionary<int, IsoXml.Model.PrescriptionAttribute> attributes = new Dictionary<int, IsoXml.Model.PrescriptionAttribute>();
                                        foreach (var tzn in xtsk.TZN)
                                        {
                                            int zoneId = tzn.A;
                                            IsoXml.Model.PrescriptionAttribute att = new IsoXml.Model.PrescriptionAttribute();

                                            foreach (var pdv in tzn.PDV)
                                            {
                                                fillAttribute(att, xScalings, xProducts, pdv);
                                                if (pdv.PDV1 != null)
                                                    foreach (var innerPDV in pdv.PDV1)
                                                        fillAttribute(att, xScalings, xProducts, innerPDV);
                                            }
                                            attributes[zoneId] = att;
                                        }

                                        double dLon = xtsk.GRD.D;
                                        double dLat = xtsk.GRD.C;
                                        double startLon = xtsk.GRD.B;
                                        double startLat = xtsk.GRD.A;
                                        string dir = Path.GetDirectoryName(file);
                                        FileStream fStr = File.OpenRead(Path.Combine(dir, xtsk.GRD.G + ".bin"));
                                        BinaryReader binR = new BinaryReader(fStr);

                                        IsoXml.Model.Prescription prescription = new IsoXml.Model.Prescription();                                        
                                        prescription.Cells = new List<IsoXml.Model.PrescriptionCell>();
                                        for (int y = 0; y < xtsk.GRD.F; y++)
                                            for (int x = 0; x < xtsk.GRD.E; x++)                                            
                                            {
                                                IsoXml.Model.PrescriptionCell cell = new IsoXml.Model.PrescriptionCell();
                                                prescription.Cells.Add(cell);
                                                cell.CellPoints = new List<IsoXml.Model.GeoPoint>();
                                                double lon = startLon + x * dLon;
                                                double lat = startLat + y * dLat;
                                                cell.CellPoints.Add(new IsoXml.Model.GeoPoint()
                                                {
                                                    Longitude = lon,
                                                    Latitude = lat
                                                });
                                                cell.CellPoints.Add(new IsoXml.Model.GeoPoint()
                                                {
                                                    Longitude = lon + dLon,
                                                    Latitude = lat
                                                });
                                                cell.CellPoints.Add(new IsoXml.Model.GeoPoint()
                                                {
                                                    Longitude = lon + dLon,
                                                    Latitude = lat + dLat
                                                });
                                                cell.CellPoints.Add(new IsoXml.Model.GeoPoint()
                                                {
                                                    Longitude = lon,
                                                    Latitude = lat + dLat
                                                });
                                                cell.CellPoints.Add(new IsoXml.Model.GeoPoint()
                                                {
                                                    Longitude = lon,
                                                    Latitude = lat
                                                });
                                                
                                                byte tznId = binR.ReadByte();
                                                cell.Attribute = attributes[tznId];                                                
                                            }
                                        binR.Dispose();
                                        fStr.Dispose();
                                        field.Prescriptions.Add(prescription);
                                        break;

                                    case 2: // Grid type 2
                                        dLon = xtsk.GRD.D;
                                        dLat = xtsk.GRD.C;
                                        startLon = xtsk.GRD.B;
                                        startLat = xtsk.GRD.A;
                                        dir = Path.GetDirectoryName(file);
                                        fStr = File.OpenRead(Path.Combine(dir, xtsk.GRD.G + ".bin"));
                                        binR = new BinaryReader(fStr);

                                        prescription = new IsoXml.Model.Prescription();                                        
                                        prescription.Cells = new List<IsoXml.Model.PrescriptionCell>();
                                        for (int y = 0; y < xtsk.GRD.F; y++)
                                            for (int x = 0; x < xtsk.GRD.E; x++)                                            
                                            {
                                                IsoXml.Model.PrescriptionCell cell = new IsoXml.Model.PrescriptionCell();
                                                prescription.Cells.Add(cell);
                                                cell.CellPoints = new List<IsoXml.Model.GeoPoint>();
                                                double lon = startLon + x * dLon;
                                                double lat = startLat + y * dLat;
                                                cell.CellPoints.Add(new IsoXml.Model.GeoPoint()
                                                {
                                                    Longitude = lon,
                                                    Latitude = lat
                                                });
                                                cell.CellPoints.Add(new IsoXml.Model.GeoPoint()
                                                {
                                                    Longitude = lon + dLon,
                                                    Latitude = lat
                                                });
                                                cell.CellPoints.Add(new IsoXml.Model.GeoPoint()
                                                {
                                                    Longitude = lon + dLon,
                                                    Latitude = lat + dLat
                                                });
                                                cell.CellPoints.Add(new IsoXml.Model.GeoPoint()
                                                {
                                                    Longitude = lon,
                                                    Latitude = lat + dLat
                                                });
                                                cell.CellPoints.Add(new IsoXml.Model.GeoPoint()
                                                {
                                                    Longitude = lon,
                                                    Latitude = lat
                                                });

                                                var attribute = new IsoXml.Model.PrescriptionAttribute();
                                                int pdvCount = 0;
                                                foreach (var pdv in xtsk.TZN[0].PDV)
                                                {
                                                    pdvCount++;
                                                    string ProductName = string.Empty;
                                                    var product = xProducts.Where(pr => pr.A == pdv.C).First();
                                                    ProductName += product.B + "_" + pdvCount.ToString();
                                                    double value = binR.ReadInt32();
                                                    var scaling = xScalings.Where(sc => sc.A == pdv.E).First();
                                                    value = value * scaling.C;
                                                    if (!string.IsNullOrEmpty(scaling.E))
                                                        ProductName += ", " + scaling.E;
                                                    attribute[ProductName] = value;
                                                }

                                                cell.Attribute = attribute;
                                            }
                                        binR.Dispose();
                                        fStr.Dispose();
                                        field.Prescriptions.Add(prescription);
                                        break;
                                }
                            }                                            
                        }
                    }
                }
                catch 
                { }
            }

            IsoXml.Model.SyncId.Instance.Save();


            // Generate AgGPS data

            foreach (var cli in clients)
            {
                string cliDir = Path.Combine(AgGpsPath, "AgGPS\\Data\\" + RemoveInvalidPathCharacters(cli.Name, "_"));
                if (!Directory.Exists(cliDir))
                    Directory.CreateDirectory(cliDir);
                foreach (var farm in cli.Farms)
                {
                    string farmDir = Path.Combine(cliDir, RemoveInvalidPathCharacters(farm.Name, "_"));
                    if (!Directory.Exists(farmDir))
                        Directory.CreateDirectory(farmDir);
                    foreach (var field in farm.Fields)
                    {
                        string fieldDir = Path.Combine(farmDir, RemoveInvalidPathCharacters(field.Name, "_"));
                        if (!Directory.Exists(fieldDir))
                            Directory.CreateDirectory(fieldDir);
                        bool posAdded = false;

                        if (field.OuterBoundaries.Count > 0)
                        {
                            string boundaryFile = Path.Combine(fieldDir, "Boundary.shp");
                            Feature f = new Feature();
                            FeatureSet fs = new FeatureSet(f.FeatureType);
                            fs.DataTable.Columns.Add(new DataColumn("Date", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Time", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Version", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Id", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Name", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Area", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Perimeter", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("SwathsIn", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Dist1", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Dist2", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("UniqueID", typeof(string)));

                            if (!posAdded && field.OuterBoundaries.Count > 0 && field.OuterBoundaries[0].Points.Count > 0)
                            {
                                IsoXml.Model.GeoPoint geoPoint = field.OuterBoundaries[0].Points[0];
                                Form1.CreatePosFile(fieldDir, geoPoint);
                                posAdded = true;
                            }

                            foreach (var ou in field.OuterBoundaries)
                                if (ou.Points.Count > 0)
                                {
                                    List<Coordinate> coords = new List<Coordinate>();
                                    foreach (var p in ou.Points)
                                        coords.Add(new Coordinate(p.Longitude, p.Latitude));

                                    List<ILinearRing> Holes = new List<ILinearRing>();
                                    foreach (var io in ou.InnerBoudnaries)
                                    {
                                        List<Coordinate> incoords = new List<Coordinate>();
                                        foreach (var p in io.Points)
                                            incoords.Add(new Coordinate(p.Longitude, p.Latitude));
                                        LinearRing Ring = new LinearRing(incoords);
                                        Holes.Add(Ring);
                                    }
                                    IFeature feature;
                                    if (Holes.Count > 0)
                                        feature = fs.AddFeature(new Polygon(new LinearRing(coords), Holes.ToArray()));
                                    else
                                        feature = fs.AddFeature(new Polygon(coords));

                                    feature.DataRow.BeginEdit();
                                    feature.DataRow["Date"] = DateTime.Now.ToString("yyyyMMdd");
                                    feature.DataRow["Time"] = DateTime.Now.ToString("hh:mm:sstt");
                                    feature.DataRow["Version"] = "5.00.049";
                                    feature.DataRow["Id"] = field.OuterBoundaries.IndexOf(ou);
                                    feature.DataRow["Name"] = ou.Name;
                                    feature.DataRow.EndEdit();
                                }
                            if (fs.Features.Count > 0)
                                fs.SaveAs(boundaryFile, true);
                            fs.Dispose();
                        }


                        if (field.AB_Lines.Count > 0)
                        {
                            string swathFile = Path.Combine(fieldDir, "Swaths.shp");
                            Feature f = new Feature();
                            FeatureSet fs = new FeatureSet(f.FeatureType);
                            fs.DataTable.Columns.Add(new DataColumn("Date", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Time", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Version", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Id", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Name", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Length", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Dist1", typeof(string)));
                            fs.DataTable.Columns.Add(new DataColumn("Dist2", typeof(string)));


                            if (!posAdded && field.AB_Lines.Count > 0 && field.AB_Lines[0].Points.Count > 0)
                            {
                                IsoXml.Model.GeoPoint geoPoint = field.AB_Lines[0].Points[0];
                                Form1.CreatePosFile(fieldDir, geoPoint);
                                posAdded = true;
                            }

                            foreach (var li in field.AB_Lines)
                                if (li.Points.Count > 0)
                                {
                                    if (!posAdded)
                                        if (li.Points.Count > 0)
                                        {
                                            var p = li.Points[0];
                                            string pos = Math.Abs(p.Longitude).ToString("F06");
                                            if (p.Longitude > 0)
                                                pos += "E";
                                            else
                                                pos += "W";

                                            pos += Math.Abs(p.Latitude).ToString("F06");
                                            if (p.Latitude > 0)
                                                pos += "N";
                                            else
                                                pos += "S";

                                            var file = File.Create(Path.Combine(fieldDir, pos + ".pos"));
                                            file.Dispose();
                                            posAdded = true;
                                        }

                                    List<Coordinate> coords = new List<Coordinate>();
                                    foreach (var p in li.Points)
                                        coords.Add(new Coordinate(p.Longitude, p.Latitude));
                                    var ls = new LineString(coords);

                                    IFeature feature = fs.AddFeature(ls);

                                    feature.DataRow.BeginEdit();
                                    feature.DataRow["Date"] = DateTime.Now.ToString("yyyyMMdd");
                                    feature.DataRow["Time"] = DateTime.Now.ToString("hh:mm:sstt");
                                    feature.DataRow["Version"] = "5.00.049";
                                    feature.DataRow["Id"] = li.ID;
                                    feature.DataRow["Name"] = li.Name;
                                    feature.DataRow["Length"] = "0.000";
                                    feature.DataRow["Dist1"] = "0.000";
                                    feature.DataRow["Dist2"] = "0.000";
                                    feature.DataRow.EndEdit();
                                }
                            if (fs.Features.Count > 0)
                                fs.SaveAs(swathFile, true);
                            fs.Dispose();
                        }

                        string prescriptionDirAgGPS = Path.Combine(AgGpsPath, "AgGPS\\Prescriptions\\");
                        GeneratePrescriptions(cli, farm, field, prescriptionDirAgGPS);

                        string prescriptionDirPIQ = Path.Combine(AgGpsPath, "AgData\\Prescriptions\\");
                        GeneratePrescriptions(cli, farm, field, prescriptionDirPIQ);
                    }
                }
            }
        }

        private static void GeneratePrescriptions(IsoXml.Model.Client cli, IsoXml.Model.Farm farm, IsoXml.Model.Field field, string prescriptionDir)
        {
            if (!Directory.Exists(prescriptionDir))
                Directory.CreateDirectory(prescriptionDir);
            int count = 0;
            if (field.Prescriptions != null)
                foreach (var pres in field.Prescriptions)
                {
                    count++;
                    string prescriptionFile = Path.Combine(prescriptionDir,
                        RemoveInvalidPathCharacters(
                        cli.Name + "_" + farm.Name + "_" + field.Name + "_" + count.ToString() + ".shp", "_"));
                    Feature f = new Feature();
                    FeatureSet fs = new FeatureSet(f.FeatureType);
                    foreach (var col in pres.Cells[0].Attribute)
                        fs.DataTable.Columns.Add(new DataColumn(col.Key, typeof(double)));

                    foreach (var cell in pres.Cells)
                    {
                        List<Coordinate> coords = new List<Coordinate>();
                        foreach (var p in cell.CellPoints)
                            coords.Add(new Coordinate(p.Longitude, p.Latitude));
                        IFeature feature = fs.AddFeature(new Polygon(coords));

                        foreach (var att in cell.Attribute)
                            if (!fs.DataTable.Columns.Contains(att.Key))
                                fs.DataTable.Columns.Add(new DataColumn(att.Key, typeof(double)));

                        feature.DataRow.BeginEdit();
                        foreach (var att in cell.Attribute)
                            feature.DataRow[att.Key] = att.Value;
                        feature.DataRow.EndEdit();
                    }
                    fs.SaveAs(prescriptionFile, true);
                    fs.Dispose();
                }
        }

        private static void fillAttribute(IsoXml.Model.PrescriptionAttribute att, List<IsoXml.XML.ISO11783_TaskDataVPN> xScalings, List<IsoXml.XML.ISO11783_TaskDataPDT> xProducts, IsoXml.XML.PDV pdv)
        {
            string ProductName = string.Empty;
            if (pdv.C != null)
            {
                var product = xProducts.Where(pr => pr.A == pdv.C).First();
                ProductName += product.B;
            }
            double value = pdv.B;
            if (pdv.E != null)
            {
                var scaling = xScalings.Where(sc => sc.A == pdv.E).First();
                value = value * scaling.C;
                if (!string.IsNullOrEmpty(scaling.E))
                    ProductName += ", " + scaling.E;
            }
            if (string.IsNullOrEmpty(ProductName))
                ProductName = "UnnamedProduct";
            att[ProductName] = value;
        }

        public static string RemoveInvalidPathCharacters(string filename, string replaceChar)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filename, replaceChar);
        }
    }
}
