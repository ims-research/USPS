using System;
using System.Collections.Generic;
using System.Xml;

namespace USPS.Code
{
    public class ServiceFlow
    {

        public Dictionary<string, ServiceBlock> Blocks;

        public string FirstBlockGUID { get; set; }

        public ServiceFlow()
        {
            Blocks = new Dictionary<string, ServiceBlock>();
        }

        private void CreateBlocks(Dictionary<string, ServiceBlock> blocks, Node node)
        {
            ServiceBlock block = new ServiceBlock(node);
            if (node.Children.Count > 0)
            {
                foreach (Node child in node.Children)
                {
                    switch (node.GetType().Name)
                    {
                    case "ServiceNode":
                    case "ConditionNode":
                        block.AddChild(child.Name,new ServiceBlock(child));
                        break;
                    case "ConditionValueNode":
                    case "SIPResponseNode":
                        block.AddChild(child.InstanceGUID, new ServiceBlock(child));
                        break;
                    default:
                        Console.WriteLine("Unkown node type" + child.GetType().Name);
                        break;
                    }
                }
                if (node.Name != "Start")
                {
                    blocks.Add(block.InstanceGUID, block);
                }
                // Double loop to maintain tree order
                foreach (Node child in node.Children)
                {
                    CreateBlocks(blocks, child);
                }
            }
        }

        public ServiceFlow(Node rootNode)
        {
            FirstBlockGUID = rootNode.Children[0].InstanceGUID;
            Dictionary<string, ServiceBlock> blocks = new Dictionary<string, ServiceBlock>();
            CreateBlocks(blocks, rootNode);
            Blocks = blocks;
        }

        //public override string ToString()
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    XmlNode rootNode = xmlDoc.CreateElement("Service_Chain");
        //    xmlDoc.AppendChild(rootNode);

        //    foreach (KeyValuePair<string, ServiceBlock> kvp in Blocks)
        //    {
        //        XmlNode userNode = xmlDoc.CreateElement("user");
        //        XmlAttribute attribute = xmlDoc.CreateAttribute("age");
        //        attribute.Value = "42";
        //        userNode.Attributes.Append(attribute);
        //        userNode.InnerText = "John Doe";
        //        rootNode.AppendChild(userNode);

        //        userNode = xmlDoc.CreateElement("user");
        //        attribute = xmlDoc.CreateAttribute("age");
        //        attribute.Value = "39";
        //        userNode.Attributes.Append(attribute);
        //        userNode.InnerText = "Jane Doe";
        //        rootNode.AppendChild(userNode);

        //        xmlDoc.Save("test-doc.xml");
        //    }
            
        //    return base.ToString();
        //}

    }
}