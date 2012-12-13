using System.Collections.Generic;
using System.Xml;

namespace USPS.Code
{
    public class ServiceFlow
    {

        public Dictionary<string, ServiceBlock> Blocks;
        public ServiceFlow()
        {
            Blocks = new Dictionary<string, ServiceBlock>();
        }

        public override string ToString()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("Service_Chain");
            xmlDoc.AppendChild(rootNode);

            foreach (KeyValuePair<string, ServiceBlock> kvp in Blocks)
            {
                XmlNode userNode = xmlDoc.CreateElement("user");
                XmlAttribute attribute = xmlDoc.CreateAttribute("age");
                attribute.Value = "42";
                userNode.Attributes.Append(attribute);
                userNode.InnerText = "John Doe";
                rootNode.AppendChild(userNode);

                userNode = xmlDoc.CreateElement("user");
                attribute = xmlDoc.CreateAttribute("age");
                attribute.Value = "39";
                userNode.Attributes.Append(attribute);
                userNode.InnerText = "Jane Doe";
                rootNode.AppendChild(userNode);

                xmlDoc.Save("test-doc.xml");
            }
            
            return base.ToString();
        }

    }
}