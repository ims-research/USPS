using System.Collections.Generic;

namespace USPS.Code
{
    public class ServiceBlock
    {
        public string Name { get; set; }
        public string GlobalGUID { get; set; }
        public string InstanceGUID { get; set; }

        public Dictionary<string, ServiceBlock> NextBlocks { get; set; }
        public ServiceBlock ParentBlock { get; set; }

        public ServiceBlock()
        {
            NextBlocks = new Dictionary<string, ServiceBlock>();
        }

        public void AddChild(string key, ServiceBlock block)
        {
            NextBlocks.Add(key,block);
        }

        public ServiceBlock(Node node)
        {
            NextBlocks = new Dictionary<string, ServiceBlock>();
            Name = node.Name;
            GlobalGUID = node.GlobalGUID;
            InstanceGUID = node.InstanceGUID;
        }

    }
}