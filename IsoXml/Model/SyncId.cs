using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace IsoXml.Model
{
    [Serializable]
    public class SyncId
    {
        private SyncId() { }

        public static readonly SyncId Instance = new SyncId();

        Dictionary<string, string> m_Clients = new Dictionary<string,string>();
        public Dictionary<string, string> Clients
        {
            get { return m_Clients; }
        }

        Dictionary<string, string> m_Farms = new Dictionary<string, string>();
        public Dictionary<string, string> Farms
        {
            get { return m_Farms; }
        }

        Dictionary<string, string> m_Fields = new Dictionary<string, string>();
        public Dictionary<string, string> Fields
        {
            get { return m_Fields; }
        }

        //Dictionary<string, string> m_Lines = new Dictionary<string, string>();
        //public Dictionary<string, string> Lines
        //{
        //    get { return m_Lines; }
        //}
        private const string fileName = "syncIDs.xml";
        public void Load()
        {
            
            if (File.Exists(fileName))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                FileStream fs = File.OpenRead(fileName);
                SyncId ob = (SyncId)serializer.Deserialize(fs);
                this.m_Clients = ob.Clients;
                this.m_Farms = ob.Farms;
                this.m_Fields = ob.Fields;
                //this.m_Lines = ob.Lines;
                fs.Close();
                fs.Dispose();                
            }
        }

        public void Save()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            StreamWriter tD = new StreamWriter(fileName, false, System.Text.Encoding.UTF8);
            serializer.Serialize(tD.BaseStream, this);
            tD.Flush();
            tD.Close();
            tD.Dispose();
        }
    }
}
