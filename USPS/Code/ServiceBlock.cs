using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS.Code
{
    public class ServiceBlock
    {
        public string Name { get; set; }
        public string GlobalGUID { get; set; }
        public string InstanceGUID { get; set; }
        public Types block_type { get; set; }

        public enum Types { Condition, Service, Terminating }
        public Dictionary<string, string> nextBlock { get; set; }
    }
}