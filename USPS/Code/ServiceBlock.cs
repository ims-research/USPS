using System.Collections.Generic;

namespace USPS.Code
{
    public class ServiceBlock
    {
        public string Name { get; set; }
        public string GlobalGUID { get; set; }
        public string InstanceGUID { get; set; }
        public Types BlockType { get; set; }

        public enum Types { Condition, Service, Terminating }
        public Dictionary<string, ServiceBlock> NextBlocks { get; set; }
        public ServiceBlock ParentBlock { get; set; }

        public ServiceBlock(Types type)
        {
            NextBlocks = new Dictionary<string, ServiceBlock>();
            BlockType = type;
        }

        public void AddChild(string key, ServiceBlock block)
        {
            NextBlocks.Add(key,block);
        }
    }
}