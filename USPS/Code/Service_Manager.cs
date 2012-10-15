using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace USPS.Code
{
    public class ServiceManager
    {
        public static Dictionary<string, Service> ServiceList = null;
        public static Dictionary<string, Condition> ConditionList= null;

        public static void LoadServices(string fromDirectory)
        {
            ServiceList = new Dictionary<string, Service>();
            string[] fileList = Directory.GetFiles(fromDirectory, "*.xml");
            foreach (string file in fileList)
            {
                Service tempService = new Service(file);
                ServiceList.Add(tempService.ServiceConfig["GUID"], tempService);
            }
        }

        public static void LoadConditions(string file_name)
        {
            ConditionList = new Dictionary<string, Condition>();
            XDocument doc = XDocument.Load(file_name);
            foreach (XElement block in doc.Elements("Conditions").Elements("Condition"))
            {
                Condition newCondition = new Condition();
                newCondition.Name = block.Element("Name").Value;
                newCondition.Type = block.Element("Type").Value;
                newCondition.Description = block.Element("Description").Value;
                List<string> possibleValues = new List<string>(block.Element("Possible_Values").Value.Split(',').Select(p => p.Trim()).ToList());
                newCondition.PossibleValues = possibleValues;
                ConditionList.Add(newCondition.Name, newCondition);
            }
        }
    }
}