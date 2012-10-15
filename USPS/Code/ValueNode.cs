using System.Collections.Generic;

namespace USPS.Code
{
    public class ValueNode
    {
        public List<string> Values { get; set; }

        public enum Types { ConditionValue, ServiceResponse }

        public Types ValueType;
        public TypeNode Parent;
        public List<TypeNode> ChildNodes;

        public ValueNode(Types type)
        {
            ValueType = type;
            Values = new List<string>();
            ChildNodes = new List<TypeNode>();
        }

    }
}