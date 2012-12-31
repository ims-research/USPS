using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS.Code
{
    public class ConditionValueNode : Node
    {
        public List<string> Values { get; set; }

        public ConditionValueNode()
        {
            Values = new List<string>();
        }
    }
}