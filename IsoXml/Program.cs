using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using DotSpatial.Data;
using System.Linq;

namespace IsoXml
{
    public static class Program
    {
        

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] inputArguments)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
//#warning set try catch
            try
            {
                string current = Directory.GetCurrentDirectory();
                string agGpsDirectory = Path.Combine(current, "AgGPS");
                string isoXmlDirectory = current;
                Model.SyncId.Instance.Load();

                var res = ConsoleNativeMethods.GetDirectories(inputArguments);
                if (res.ShouldConvert)
                {
                    if (res.OutputDirectory != null)
                    {
                        agGpsDirectory = res.InputDirectory;
                        isoXmlDirectory = res.OutputDirectory;
                    }
                }
                else
                    return;
                                
                if (Directory.Exists(agGpsDirectory))
                    ConvertToIsoXml(agGpsDirectory, isoXmlDirectory);
                if (res.ShouldOpenDirectory)
                    Process.Start("explorer.exe", isoXmlDirectory);
            }
            catch (Exception ex)
            {
                StreamWriter wr = new StreamWriter("errorISO.txt");
                wr.WriteLine(ex.Message);
                wr.WriteLine(ex.StackTrace);
                wr.Flush();
                wr.Close();
            }
        }

        public static void ConvertToIsoXml(string agGpsDirectory, string isoXmlDirectory)
        {
            System.Globalization.CultureInfo ci_EN = new System.Globalization.CultureInfo("en-US");

            isoXmlDirectory = Path.Combine(isoXmlDirectory, "G_IsoXml");
            if (!Directory.Exists(isoXmlDirectory))
                Directory.CreateDirectory(isoXmlDirectory);

            XML.ISO11783_TaskData taskData = new XML.ISO11783_TaskData();
            taskData.DataTransferOrigin = "2";
            taskData.ManagementSoftwareManufacturer = "Trimble";
            taskData.ManagementSoftwareVersion = "0.6";
            taskData.VersionMajor = "3";
            taskData.VersionMinor = "0";
            taskData.XFR = new XML.ISO11783_TaskDataXFR[] 
                    {
                        new XML.ISO11783_TaskDataXFR()
                        {
                            A = "CTR00000",
                            B = "1"
                        },
                        new XML.ISO11783_TaskDataXFR()
                        {
                            A = "FRM00000",
                            B = "1"
                        },
                        new XML.ISO11783_TaskDataXFR()
                        {
                            A = "PFD00000",
                            B = "1"
                        }
                    };

            taskData.DVC = new XML.ISO11783_TaskDataDVC[]
                    {
                        new XML.ISO11783_TaskDataDVC()
                        {
                            A = "DVC-1",
                            B = "Trimble Rate",
                            C = "4.0",
                            D = "0000000000000000",
                            F = "00145018171C1D",
                            G = "FFFF00000F6E65"
                        }
                    };

            taskData.DVC[0].DVP = new XML.ISO11783_TaskDataDVP[]
                    {
                        new XML.ISO11783_TaskDataDVP()
                        {
                            A = 211,
                            B = 0,
                            C = 0.001,
                            D = 2,
                            E = "m"
                        },
                        new XML.ISO11783_TaskDataDVP()
                        {
                            A = 212,
                            B = 0,
                            C = 0.001,
                            D = 1,
                            E = "units"
                        },
                        new XML.ISO11783_TaskDataDVP()
                        {
                            A = 213,
                            B = 0,
                            C = 1,
                            D = 1,
                            E = "seeds"
                        },
                        new XML.ISO11783_TaskDataDVP()
                        {
                            A = 214,
                            B = 0,
                            C = 0.001,
                            D = 1,
                            E = "percent"
                        }
                    };

            taskData.DVC[0].DPD = new XML.ISO11783_TaskDataDPD[]
                    {
                        new XML.ISO11783_TaskDataDPD()
                        {
                            A = "115",
                            B = "F100",
                            C = "1",
                            D = "1",
                            E = "Cross Track Error",
                            F = taskData.DVC[0].DVP[0].A
                        },
                        new XML.ISO11783_TaskDataDPD()
                        {
                            A = "116",
                            B = "F101",
                            C = "1",
                            D = "1",
                            E = "Applied Rate",
                            F = taskData.DVC[0].DVP[1].A
                        },
                        new XML.ISO11783_TaskDataDPD()
                        {
                            A = "117",
                            B = "F102",
                            C = "1",
                            D = "1",
                            E = "Population",
                            F = taskData.DVC[0].DVP[2].A
                        },
                        new XML.ISO11783_TaskDataDPD()
                        {
                            A = "118",
                            B = "F103",
                            C = "1",
                            D = "1",
                            E = "Singulation",
                            F = taskData.DVC[0].DVP[3].A
                        },
                        new XML.ISO11783_TaskDataDPD()
                        {
                            A = "119",
                            B = "F104",
                            C = "1",
                            D = "1",
                            E = "Skips_Perc",
                            F = taskData.DVC[0].DVP[3].A
                        },
                        new XML.ISO11783_TaskDataDPD()
                        {
                            A = "120",
                            B = "F105",
                            C = "1",
                            D = "1",
                            E = "Mults_Perc",
                            F = taskData.DVC[0].DVP[3].A
                        }
                    };

            taskData.DVC[0].DET = new XML.ISO11783_TaskDataDET[]
                    {
                        new XML.ISO11783_TaskDataDET()
                        {
                            A="DET-1",
                            B="1",
                            C="2",
                            D="Trimble GNSS XTE",
                            E="2",
                            F="1",
                            DOR = new XML.ISO11783_TaskDataDOR()
                            {
                                A = taskData.DVC[0].DPD[0].A
                            }
                        },
                        new XML.ISO11783_TaskDataDET()
                        {
                            A="DET-2",
                            B="2",
                            C="2",
                            D="Trimble Rate",
                            E="2",
                            F="1",
                            DOR = new XML.ISO11783_TaskDataDOR()
                            {
                                A = taskData.DVC[0].DPD[1].A
                            }
                        },
                        new XML.ISO11783_TaskDataDET()
                        {
                            A="DET-3",
                            B="3",
                            C="2",
                            D="Trimble Population",
                            E="2",
                            F="1",
                            DOR = new XML.ISO11783_TaskDataDOR()
                            {
                                A = taskData.DVC[0].DPD[2].A
                            }
                        },
                        new XML.ISO11783_TaskDataDET()
                        {
                            A="DET-4",
                            B="4",
                            C="2",
                            D="Singulation Percentage",
                            E="2",
                            F="1",
                            DOR = new XML.ISO11783_TaskDataDOR()
                            {
                                A = taskData.DVC[0].DPD[3].A
                            }
                        },
                        new XML.ISO11783_TaskDataDET()
                        {
                            A="DET-5",
                            B="5",
                            C="2",
                            D="Skips Percentage",
                            E="2",
                            F="1",
                            DOR = new XML.ISO11783_TaskDataDOR()
                            {
                                A = taskData.DVC[0].DPD[4].A
                            }
                        },
                        new XML.ISO11783_TaskDataDET()
                        {
                            A="DET-6",
                            B="6",
                            C="2",
                            D="Multiples Percentage",
                            E="2",
                            F="1",
                            DOR = new XML.ISO11783_TaskDataDOR()
                            {
                                A = taskData.DVC[0].DPD[5].A
                            }
                        }
                    };

            taskData.PDT = new XML.ISO11783_TaskDataPDT[]
                    {
                        new XML.ISO11783_TaskDataPDT()
                        {
                            A = "PDT1",
                            B = "AsApplied"
                        }
                    };

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer;
            StreamWriter tD;


            string dataDirectory = Path.Combine(agGpsDirectory, "Data");
            var clDirs = Directory.GetDirectories(dataDirectory);
            List<Model.Client> clients = new List<Model.Client>();
            foreach (var clDir in clDirs)
            {
                var client = new Model.Client() { Name = Path.GetFileName(clDir), Farms = new List<Model.Farm>() };
                clients.Add(client);
                var farmDirs = Directory.GetDirectories(clDir);
                foreach (var farmDir in farmDirs)
                {
                    var farm = new Model.Farm() { Name = Path.GetFileName(farmDir), Fields = new List<Model.Field>() };
                    client.Farms.Add(farm);
                    var fieldDirs = Directory.GetDirectories(farmDir);
                    foreach (var fieldDir in fieldDirs)
                    {
                        Model.Field field = new Model.Field();
                        field.Name = Path.GetFileName(fieldDir);
                        field.AB_Lines = new List<Model.AB_Line>();
                        field.OuterBoundaries = new List<Model.OuterBoundary>();
                        field.Events = new List<Model.CoverageEvent>();
                        farm.Fields.Add(field);

                        // Events
                        var eventDirs = Directory.GetDirectories(fieldDir);
                        foreach (var eventDir in eventDirs)
                        {
                            string eventName = Path.GetFileName(eventDir);
                            string coverageFile = Path.Combine(eventDir, "Coverage.shp");
                            var fEvent = ParseEvent(eventName, coverageFile);
                            if (fEvent != null)
                                field.Events.Add(fEvent);
                            else
                                continue;

                            coverageFile = Path.Combine(eventDir, "Coverage1.shp");
                            fEvent = ParseEvent(eventName + "2", coverageFile);
                            if (fEvent != null)
                                field.Events.Add(fEvent);
                            else
                                continue;

                            coverageFile = Path.Combine(eventDir, "Coverage2.shp");
                            fEvent = ParseEvent(eventName + "3", coverageFile);
                            if (fEvent != null)
                                field.Events.Add(fEvent);
                            else
                                continue;

                            coverageFile = Path.Combine(eventDir, "Coverage3.shp");
                            fEvent = ParseEvent(eventName + "4", coverageFile);
                            if (fEvent != null)
                                field.Events.Add(fEvent);
                            else
                                continue;

                            coverageFile = Path.Combine(eventDir, "Coverage4.shp");
                            fEvent = ParseEvent(eventName + "5", coverageFile);
                            if (fEvent != null)
                                field.Events.Add(fEvent);
                            else
                                continue;

                            coverageFile = Path.Combine(eventDir, "Coverage5.shp");
                            fEvent = ParseEvent(eventName + "6", coverageFile);
                            if (fEvent != null)
                                field.Events.Add(fEvent);
                            else
                                continue;
                        }

                        // AB lines
                        string swathesFile = Path.Combine(fieldDir, "Swaths.shp");
                        if (File.Exists(swathesFile) && new FileInfo(swathesFile).Length > 100)
                        {
                            //try
                            //{
                                using (Shapefile shapeFile = Shapefile.OpenFile(swathesFile))
                                {
//#warning unknown error && isoxml folder to open
                                    try
                                    {
                                        var features = shapeFile.Features;
                                    }
                                    catch { }
                                    //int ic = features.Count;
                                    //for (int i = 0; i < ic; i++)
                                    foreach (var shLine in shapeFile.Features)
                                    {
                                        //var shLine = shapeFile.Features[i];
                                        if (shLine.ShapeType == ShapeType.PolyLine)
                                        {
                                            Model.AB_Line AB_line = new Model.AB_Line();
                                            AB_line.Name = shLine.DataRow["Name"].ToString();
                                            field.AB_Lines.Add(AB_line);
                                            AB_line.Points = new List<Model.GeoPoint>();
                                            foreach (var point in shLine.Coordinates)
                                                AB_line.Points.Add(new Model.GeoPoint() { Longitude = point.X, Latitude = point.Y });
                                        }
                                    }
                                    
                                    shapeFile.Close();
                                }
                            //}
                            //catch { }
                        }

                        // Boundaries
                        string boundaryFile = Path.Combine(fieldDir, "Boundary.shp");
                        if (File.Exists(boundaryFile))
                        {
                            Shapefile shapeFile = Shapefile.OpenFile(boundaryFile);
                            var shPolygon = shapeFile.Features.LastOrDefault();
                            //foreach (var shPolygon in shapeFile.Features)
                            //{
                            if (shPolygon != null && shPolygon.ShapeType == ShapeType.Polygon)
                            {
                                Model.OuterBoundary ouBoundary = new Model.OuterBoundary()
                                {
                                    InnerBoudnaries = new List<Model.InnerBoundary>(),
                                    Points = new List<Model.GeoPoint>()
                                };
                                field.OuterBoundaries.Add(ouBoundary);
                                ouBoundary.Name = shPolygon.DataRow["Name"].ToString();
                                int pa_num = 0;
                                if (pa_num == 0)
                                    foreach (var point in shPolygon.Coordinates)
                                        ouBoundary.Points.Add(new Model.GeoPoint() { Longitude = point.X, Latitude = point.Y });
                                else
                                {
                                    var inner = new Model.InnerBoundary();
                                    inner.Points = new List<Model.GeoPoint>();
                                    ouBoundary.InnerBoudnaries.Add(inner);
                                    foreach (var point in shPolygon.Coordinates)
                                        inner.Points.Add(new Model.GeoPoint() { Longitude = point.X, Latitude = point.Y });
                                }
                                pa_num++;
                                //}
                            }
                            shapeFile.Close();
                            shapeFile.Dispose();
                        }
                    }
                }
            }



            XML.client.XFC clientsXML = new XML.client.XFC();
            clientsXML.Items = new XML.client.CTR[clients.Count];
            for (int i = 0; i < clients.Count; i++)
            {
                clientsXML.Items[i] = new XML.client.CTR();
                //#warning Client ID
                clientsXML.Items[i].A = "CTR" + (i + 1).ToString();
                if (Model.SyncId.Instance.Clients.ContainsKey(clients[i].Name))
                {
                    clientsXML.Items[i].A = Model.SyncId.Instance.Clients[clients[i].Name];
                }

                clients[i].ID = clientsXML.Items[i].A;
                clientsXML.Items[i].B = clients[i].Name;
            }
            serializer = new XmlSerializer(typeof(XML.client.XFC));
            tD = new StreamWriter(Path.Combine(isoXmlDirectory, "CTR00000.xml"), false, System.Text.Encoding.UTF8);
            serializer.Serialize(tD, clientsXML, ns);
            tD.Flush();
            tD.Close();


            XML.farm.XFC farmsXML = new XML.farm.XFC();
            int numOfFarms = 0;
            foreach (var cl in clients)
                foreach (var fa in cl.Farms)
                    numOfFarms++;
            farmsXML.Items = new XML.farm.FRM[numOfFarms];
            int counter = 0;
            foreach (var cl in clients)
                foreach (var fa in cl.Farms)
                {
                    farmsXML.Items[counter] = new XML.farm.FRM();
                    //#warning Farm ID
                    farmsXML.Items[counter].A = "FRM" + (counter + 1).ToString();
                    if (Model.SyncId.Instance.Farms.ContainsKey(fa.Name))
                    {
                        farmsXML.Items[counter].A = Model.SyncId.Instance.Farms[fa.Name];
                    }
                    fa.ID = farmsXML.Items[counter].A;
                    farmsXML.Items[counter].B = fa.Name;
                    farmsXML.Items[counter].I = cl.ID;
                    counter++;
                }
            serializer = new XmlSerializer(typeof(XML.farm.XFC));
            tD = new StreamWriter(Path.Combine(isoXmlDirectory, "FRM00000.xml"), false, System.Text.Encoding.UTF8);
            serializer.Serialize(tD, farmsXML, ns);
            tD.Flush();
            tD.Close();


            XML.field.XFC fieldsXML = new XML.field.XFC();
            int numOfFields = 0, numOfLines = 0;
            foreach (var cl in clients)
                foreach (var fa in cl.Farms)
                    foreach (var fi in fa.Fields)
                    {
                        numOfFields++;
                        //#warning Field ID
                        fi.ID = "PFD" + numOfFields.ToString();
                        if (IsoXml.Model.SyncId.Instance.Fields.ContainsKey(fi.Name))
                        {
                            fi.ID = IsoXml.Model.SyncId.Instance.Fields[fi.Name];
                        }
                        foreach (var l in fi.AB_Lines)
                        {
                            //#warning Line ID
                            l.ID = "LSG" + numOfLines.ToString().PadLeft(5, '0');
                            numOfLines++;
                        }
                    }
            fieldsXML.Items = new XML.field.PFD[numOfFields];
            int fi_num = 0;
            foreach (var cl in clients)
                foreach (var fa in cl.Farms)
                    foreach (var fi in fa.Fields)
                    {
                        var f = new XML.field.PFD();
                        fieldsXML.Items[fi_num] = f;
                        f.A = fi.ID;
                        f.C = fi.Name;
                        f.D = "1000000";
                        f.F = fa.ID;
                        f.PLN = new XML.field.XFCPFDPLN[fi.OuterBoundaries.Count];
                        int ou_num = 0;
                        foreach (var ou in fi.OuterBoundaries)
                        {
                            f.PLN[ou_num] = new XML.field.XFCPFDPLN();
                            f.PLN[ou_num].A = "1";
                            f.PLN[ou_num].B = ou.Name;
                            f.PLN[ou_num].LSG = new XML.field.LSG[ou.InnerBoudnaries.Count + 1];
                            f.PLN[ou_num].LSG[0] = new XML.field.LSG();
                            f.PLN[ou_num].LSG[0].A = "1";
                            f.PLN[ou_num].LSG[0].PNT = new XML.field.LSGPNT[ou.Points.Count];
                            int p_num = 0;
                            foreach (var p in ou.Points)
                            {
                                f.PLN[ou_num].LSG[0].PNT[p_num] = new XML.field.LSGPNT() { A = "2", C = p.Latitude.ToString("F08", ci_EN), D = p.Longitude.ToString("F08", ci_EN) };
                                p_num++;
                            }
                            if (p_num > 0)
                            {
                                f.PLN[ou_num].LSG[0].PNT[0].B = "start";
                                f.PLN[ou_num].LSG[0].PNT[p_num - 1].B = "end";
                            }

                            int io_num = 1;
                            foreach (var io in ou.InnerBoudnaries)
                            {
                                f.PLN[ou_num].LSG[io_num] = new XML.field.LSG();
                                f.PLN[ou_num].LSG[io_num].A = "2";
                                f.PLN[ou_num].LSG[io_num].PNT = new XML.field.LSGPNT[io.Points.Count];
                                int iop_num = 0;
                                foreach (var p in io.Points)
                                {
                                    f.PLN[ou_num].LSG[io_num].PNT[iop_num] = new XML.field.LSGPNT() { A = "2", C = p.Latitude.ToString("F08", ci_EN), D = p.Longitude.ToString("F08", ci_EN) };
                                    iop_num++;
                                }
                                if (iop_num > 0)
                                {
                                    f.PLN[ou_num].LSG[io_num].PNT[0].B = "start";
                                    f.PLN[ou_num].LSG[io_num].PNT[iop_num - 1].B = "end";
                                }
                                io_num++;
                            }

                            ou_num++;
                        }

                        int li_num = 0;
                        f.LSG = new XML.field.LSG[fi.AB_Lines.Count];
                        foreach (var li in fi.AB_Lines)
                        {
                            f.LSG[li_num] = new XML.field.LSG();
                            f.LSG[li_num].A = "5";
                            f.LSG[li_num].B = li.Name;
                            //f.LSG[li_num].P102_ID = li.ID;
                            //f.LSG[li_num].P102_PATHTYPE = "0";
                            f.LSG[li_num].PNT = new XML.field.LSGPNT[li.Points.Count];
                            int p_num = 0;
                            foreach (var p in li.Points)
                            {
                                f.LSG[li_num].PNT[p_num] = new XML.field.LSGPNT() { A = "2", C = p.Latitude.ToString("F08", ci_EN), D = p.Longitude.ToString("F08", ci_EN) };
                                p_num++;
                            }
                            if (p_num > 0)
                            {
                                f.LSG[li_num].PNT[0].B = "start";
                                f.LSG[li_num].PNT[p_num - 1].B = "end";
                            }
                            li_num++;
                        }
                        fi_num++;
                    }
            serializer = new XmlSerializer(typeof(XML.field.XFC));
            tD = new StreamWriter(Path.Combine(isoXmlDirectory, "PFD00000.xml"), false, System.Text.Encoding.UTF8);
            serializer.Serialize(tD, fieldsXML, ns);
            tD.Flush();
            tD.Close();


            // TLGs
            List<XML.ISO11783_TaskDataTSK> tasks = new List<XML.ISO11783_TaskDataTSK>();
            int eventCounter = 0;
            foreach (var cl in clients)
                foreach (var fa in cl.Farms)
                    foreach (var fi in fa.Fields)
                        foreach (var ev in fi.Events)
                        {
                            XML.tlg.TIM tim = new XML.tlg.TIM() { A = "", D = "4" };
                            tim.PTN = new XML.tlg.TIMPTN[] { new XML.tlg.TIMPTN() { A = "", B = "", D = "", F = "", G = "" } };
                            tim.DLV = new XML.tlg.TIMDLV[] 
                                    { 
                                        new XML.tlg.TIMDLV() { A = taskData.DVC[0].DPD[0].B, B = "", C = taskData.DVC[0].DET[0].A },
                                        new XML.tlg.TIMDLV() { A = taskData.DVC[0].DPD[1].B, B = "", C = taskData.DVC[0].DET[1].A },
                                        new XML.tlg.TIMDLV() { A = taskData.DVC[0].DPD[2].B, B = "", C = taskData.DVC[0].DET[2].A },
                                        new XML.tlg.TIMDLV() { A = taskData.DVC[0].DPD[3].B, B = "", C = taskData.DVC[0].DET[3].A },
                                        new XML.tlg.TIMDLV() { A = taskData.DVC[0].DPD[4].B, B = "", C = taskData.DVC[0].DET[4].A },
                                        new XML.tlg.TIMDLV() { A = taskData.DVC[0].DPD[5].B, B = "", C = taskData.DVC[0].DET[5].A }
                                    };
                            serializer = new XmlSerializer(typeof(XML.field.XFC));

                            serializer = new XmlSerializer(typeof(XML.tlg.TIM));
                            tD = new StreamWriter(Path.Combine(isoXmlDirectory, "TLG" + eventCounter.ToString().PadLeft(5, '0') + ".xml"), false, System.Text.Encoding.UTF8);
                            serializer.Serialize(tD, tim, ns);
                            tD.Flush();
                            tD.Close();

                            BinaryWriter binWriter = new BinaryWriter(File.Create(Path.Combine(isoXmlDirectory, "TLG" + eventCounter.ToString().PadLeft(5, '0') + ".bin")));
                            foreach (var poly in ev.Polygons)
                            {
                                if (poly.Points.Count > 0)
                                {
                                    UInt32 timeStart = (UInt32)((poly.Points[0].GpsUtcTime - new DateTime(1980, 1, 1)).TotalMilliseconds);
                                    UInt16 days = (UInt16)((poly.Points[0].GpsUtcTime - new DateTime(1980, 1, 1)).TotalDays);
                                    binWriter.Write(timeStart);
                                    binWriter.Write(days);
                                    double meanLat = 0, meanLon = 0, meanHeight = 0;
                                    foreach (var p in poly.Points)
                                    {
                                        meanLat += p.Latitude;
                                        meanLon += p.Longitude;
                                        meanHeight += p.Elevation;
                                    }
                                    int heightInt = (int)Math.Round(meanHeight * 1000.0 / poly.Points.Count);
                                    meanLat = meanLat / poly.Points.Count;
                                    meanLon = meanLon / poly.Points.Count;
                                    int latInt = (int)Math.Round((meanLat * 10000000.0));
                                    int lonInt = (int)Math.Round((meanLon * 10000000.0));
                                    binWriter.Write(latInt);
                                    binWriter.Write(lonInt);

                                    //binWriter.Write(heightInt);  Elevation not supported !!!

                                    binWriter.Write(poly.Points[0].PositionStatus);

                                    UInt16 hDop = 0;
                                    binWriter.Write(hDop);

                                    binWriter.Write(poly.Points[0].NumberOfSatellites);
                                    //!!!
                                    byte nDlv = 6;
                                    binWriter.Write(nDlv);

                                    byte DLVn = 0;
                                    binWriter.Write(DLVn);
                                    int d = (int)Math.Round(poly.Points[0].CrossTrackError * 1000);
                                    binWriter.Write(d);

                                    DLVn = 1;
                                    binWriter.Write(DLVn);
                                    d = (int)Math.Round(poly.Points[0].AppliedRate * 1000);
                                    binWriter.Write(d);

                                    DLVn = 2;
                                    binWriter.Write(DLVn);
                                    d = (int)Math.Round(poly.Points[0].Population);
                                    binWriter.Write(d);

                                    DLVn = 3;
                                    binWriter.Write(DLVn);
                                    d = (int)Math.Round(poly.Points[0].Singulation * 1000);
                                    binWriter.Write(d);

                                    DLVn = 4;
                                    binWriter.Write(DLVn);
                                    d = (int)Math.Round(poly.Points[0].Skips_Percentage * 1000);
                                    binWriter.Write(d);

                                    DLVn = 5;
                                    binWriter.Write(DLVn);
                                    d = (int)Math.Round(poly.Points[0].Multiples_Percentage * 1000);
                                    binWriter.Write(d);
                                }
                            }
                            binWriter.Flush();
                            binWriter.Close();

                            tasks.Add(new XML.ISO11783_TaskDataTSK()
                            {
                                //#warning Task ID
                                A = "TSK" + eventCounter.ToString(),
                                B = ev.EventName,
                                C = cl.ID,
                                D = fa.ID,
                                E = fi.ID,
                                G = "4",
                                TLG = new XML.ISO11783_TaskDataTLG()
                                {
                                    //#warning Task ID?
                                    A = "TLG" + eventCounter.ToString().PadLeft(5, '0'),
                                    C = "1"
                                },
                                PAN = new XML.ISO11783_TaskDataPAN()
                                {
                                    A = taskData.PDT[0].A
                                }
                            });

                            eventCounter++;
                        }


            // TaskData.xml file
            taskData.TSK = tasks.ToArray();
            ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer = new XmlSerializer(typeof(XML.ISO11783_TaskData));
            tD = new StreamWriter(Path.Combine(isoXmlDirectory, "TaskData.xml"), false, System.Text.Encoding.UTF8);
            serializer.Serialize(tD, taskData, ns);
            tD.Flush();
            tD.Close();            
        }

        private static Model.CoverageEvent ParseEvent(string eventName, string coverageFile)
        {
            if (File.Exists(coverageFile))
            {
                Model.CoverageEvent fEvent = new Model.CoverageEvent();
                fEvent.EventName = eventName;
                fEvent.Polygons = new List<Model.CoveragePolygon>();
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");

                Shapefile sf = Shapefile.OpenFile(coverageFile);
                foreach (var shPolygon in sf.Features)
                {
                    if (shPolygon.ShapeType == ShapeType.Polygon)
                    {
                        Model.CoveragePolygon covPolygon = new Model.CoveragePolygon();
                        covPolygon.Points = new List<Model.GeoPoint>();

                        double xte = 0, height = 0;
                        double appliedRate = -1, population = -1, singulation = -1, skips_perc = -1, mults_perc = -1;
                        if (shPolygon.DataRow.ItemArray.Length > 11)
                        {
                            try
                            {
                                string str_XTE = shPolygon.DataRow["XTE"].ToString();
                                if (string.Compare("/0.000", str_XTE) != 0)
                                    xte = double.Parse(str_XTE, ci);
                            }
                            catch { }
                            try
                            {
                                appliedRate = double.Parse(shPolygon.DataRow["AppldRate"].ToString());
                            }
                            catch { }
                            try
                            {
                                population = double.Parse(shPolygon.DataRow["Population"].ToString());
                            }
                            catch { }
                            try
                            {
                                singulation = double.Parse(shPolygon.DataRow["Singulatn"].ToString());
                            }
                            catch { }
                            try
                            {
                                skips_perc = double.Parse(shPolygon.DataRow["Skips_Perc"].ToString());
                            }
                            catch { }
                            try
                            {
                                mults_perc = double.Parse(shPolygon.DataRow["Mults_Perc"].ToString());
                            }
                            catch { }
                            try
                            {
                                height = double.Parse(shPolygon.DataRow["Height"].ToString());
                            }
                            catch { }
                        }
                        sbyte gpsStatus = sbyte.Parse(shPolygon.DataRow["GPS_Status"].ToString());
                        DateTime time = DateTime.Parse(shPolygon.DataRow["DateClosed"].ToString());
                        string strDayTime = shPolygon.DataRow["TimeClosed"].ToString();
                        time = time.AddHours(byte.Parse(strDayTime.Substring(0, 2)));
                        time = time.AddMinutes(byte.Parse(strDayTime.Substring(3, 2)));
                        time = time.AddSeconds(byte.Parse(strDayTime.Substring(6, 2)));

                        foreach (var point in shPolygon.BasicGeometry.Coordinates)
                            covPolygon.Points.Add(new Model.GeoPoint()
                            {
                                Longitude = point.X,
                                Latitude = point.Y,
                                CrossTrackError = xte,
                                PositionStatus = gpsStatus,
                                GpsUtcTime = time,
                                AppliedRate = appliedRate,
                                Population = population,
                                Singulation = singulation,
                                Skips_Percentage = skips_perc,
                                Multiples_Percentage = mults_perc,
                                Elevation = height
                            });
                        fEvent.Polygons.Add(covPolygon);
                    }
                }
                sf.Close();
                sf.Dispose();
                return fEvent;
            }
            else
                return null;
        }
    }
}
