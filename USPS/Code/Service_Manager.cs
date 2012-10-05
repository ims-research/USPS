using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Linq;

namespace USPS.Code
{
    public class Service_Manager
    {
        public static Dictionary<string, Service> service_list = null;
        public static Dictionary<string, Condition> condition_list= null;

        public static void Load_Services(string from_directory)
        {
            service_list = new Dictionary<string, Service>();
            string[] fileList = Directory.GetFiles(from_directory, "*.xml");
            foreach (string file in fileList)
            {
                Service temp_service = new Service(file);
                service_list.Add(temp_service.Service_Config["GUID"], temp_service);
            }
        }

        public static void Load_Conditions(string file_name)
        {
            condition_list = new Dictionary<string, Condition>();
            XDocument doc = XDocument.Load(file_name);
            foreach (XElement block in doc.Elements("Conditions").Elements("Condition"))
            {
                Condition new_condition = new Condition();
                new_condition.Name = block.Element("Name").Value;
                new_condition.Type = block.Element("Type").Value;
                new_condition.Description = block.Element("Description").Value;
                List<string> possible_values = new List<string>(block.Element("Possible_Values").Value.Split(',').Select(p => p.Trim()).ToList());
                new_condition.possible_values = possible_values;
                condition_list.Add(new_condition.Name, new_condition);
            }
        }
    }
}