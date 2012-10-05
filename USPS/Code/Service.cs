using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace USPS.Code
{
    public class Service
    {
        public Dictionary<string, string> Service_Information { get; set; }
        public Dictionary<string, string> Service_Config { get; set; }
        public Dictionary<string, string> SIP_Headers { get; set; }
        public Dictionary<string, string> SIP_Responses { get; set; }
        public Dictionary<string, string> Capabilities { get; set; }

        public Service()
        {
            Initialise_Variables();
        }

        public Service(string filename)
        {
            Initialise_Variables();
            parseXMLFile(filename);
        }

        public void parseXMLFile(string filename)
        {

            XDocument doc = XDocument.Load(filename);
            foreach (XElement block in doc.Elements("Service"))
            {
                foreach (XElement element in block.Elements("Service_Information").Elements())
                {
                    Service_Information[element.Name.ToString()] = element.Value;
                }
                foreach (XElement element in block.Elements("Service_Config").Elements())
                {
                    Service_Config[element.Name.ToString()] = element.Value;
                }
                foreach (XElement element in block.Elements("SIP_Headers").Elements())
                {
                    SIP_Headers[element.Name.ToString()] = element.Value;
                }
                foreach (XElement element in block.Elements("SIP_Responses").Elements())
                {
                    SIP_Responses[element.Name.ToString()] = element.Value;
                }
                foreach (XElement element in block.Elements("Capabalities").Elements())
                {
                    Capabilities[element.Name.ToString()] = element.Value;
                }
            }

        }

        private void Initialise_Variables()
        {
            Service_Information = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Service_Config = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            SIP_Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            SIP_Responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Capabilities = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }


    }
}