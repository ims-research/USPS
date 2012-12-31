using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS.Code
{
    public class SIPResponseNode : Node
    {
       public List<string> Values { get; set; }

       public SIPResponseNode()
        {
            Values = new List<string>();
        }
    }
}