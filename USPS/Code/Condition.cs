using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace USPS.Code
{
    public class Condition
    {
        public string ConType { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description{ get; set; }
        public List<string> PossibleValues { get; set; }
        public string GUID { get; set; }

        public Condition()
        {
            InitialiseVariables();
        }

        public Condition(XElement block)
        {
            Name = block.Element("Name").Value;
            Type = block.Element("Type").Value;
            Description = block.Element("Description").Value;
            GUID = block.Element("GUID").Value;
            List<string> possibleValues = new List<string>(block.Element("Possible_Values").Value.Split(',').Select(p => p.Trim()).ToList());
            PossibleValues = possibleValues;
        }

        private void InitialiseVariables()
        {
            PossibleValues = new List<string>();
        }

    }
}