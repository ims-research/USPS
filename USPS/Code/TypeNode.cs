using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace USPS.Code
{
    public class TypeNode : TreeNode
    {
        public string Name { get; set; }
        public string GlobalGUID { get; set; }
        public string InstanceGUID { get; set; }
        public ServiceBlock.Types block_type { get; set; }

        public TypeNode(ServiceBlock.Types type)
        {
            block_type = type;
        }

    }
}