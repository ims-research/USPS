using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS.Code
{
    public class Node
    {
        List<Node> children { get; set; }
        public string Name { get; set; }
        public string GlobalGUID { get; set; }
        public string InstanceGUID { get; set; }
        
        public Node()
        {
            children = new List<Node>();
        }
    }
}