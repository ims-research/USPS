using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPS.Code
{
    public class Condition
    {
        public string con_type { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description{ get; set; }
        public List<string> possible_values { get; set; }
        
        public Condition()
        {
            Initialise_Variables();
        }

        private void Initialise_Variables()
        {
            possible_values = new List<string>();
        }
    }
}