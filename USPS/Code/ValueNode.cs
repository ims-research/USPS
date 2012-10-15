using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace USPS.Code
{
    public class ValueNode : TreeNode
    {
        public List<string> Values { get; set; }

        public enum Types { ConditionValue, ServiceResponse }

        public Types ValueType;

        public ValueNode(Types type)
        {
            ValueType = type;
            Values = new List<string>();
        }

    }
}