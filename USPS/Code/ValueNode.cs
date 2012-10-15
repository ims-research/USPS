using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace USPS.Code
{
    public class ValueNode : TreeNode
    {
        public List<string> values { get; set; }

        public enum Types { ConditionValue, ServiceResponse }

        public Types value_type;

        public ValueNode(Types type)
        {
            value_type = type;
            values = new List<string>();
        }

    }
}