using System.Collections.Generic;

namespace USPS.Code
{
    public class Condition
    {
        public string ConType { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description{ get; set; }
        public List<string> PossibleValues { get; set; }
        
        public Condition()
        {
            InitialiseVariables();
        }

        private void InitialiseVariables()
        {
            PossibleValues = new List<string>();
        }
    }
}