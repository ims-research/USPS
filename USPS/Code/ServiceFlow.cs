using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS.Code
{
    public class ServiceFlow
    {
        public Dictionary<string, ServiceBlock> blocks;
        public ServiceFlow()
        {
            blocks = new Dictionary<string, ServiceBlock>();
        }
    }
}