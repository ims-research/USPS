using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Xml;
namespace USPS
{
    public partial class Test_Add_Service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> sinfo_keys = new Dictionary<string, string> {{"Name","Voice Mail"}, {"Type","Session Establishment"}, { "Version", ""},{"Provider",""},{ "Description", ""}};
            Dictionary<string, string> sconfig_keys = new Dictionary<string, string> {{ "Server_IP", ""},{"Server_Port",""},{"GUID",""}};
            Dictionary<string, string> header_keys = new Dictionary<string, string> {{ "READ",""},{"WRITE",""}};
            Dictionary<string, string> cap_keys = new Dictionary<string, string> {{ "audio",""},{"video",""},{"duplex",""},{"methods","" }};

            Dictionary<string, Dictionary<string, string>> nodes = new Dictionary<string, Dictionary<string, string>>();
            nodes.Add("Service_Information", sinfo_keys);
            nodes.Add("Service_Config", sconfig_keys);
            nodes.Add("SIP_Headers", header_keys);
            nodes.Add("Capabalities", cap_keys);

            foreach (KeyValuePair<string, Dictionary<string, string>> kv in nodes)
            {
                Table table = new Table();
                table.Caption = kv.Key;
                table.ID = kv.Key + "_table";

                foreach (KeyValuePair<string, string> values in kv.Value)
                {
                    TableRow temp_tr = new TableRow();
                    Label temp_label = new Label();
                    TextBox temp_box = new TextBox();
                    TableCell tck = new TableCell();
                    TableCell tcv = new TableCell();
                    
                    temp_label.Text = values.Key;
                    if (values.Key == "GUID")
                    {
                        temp_box.Text = Guid.NewGuid().ToString();
                        temp_box.Enabled = false;
                    }
                    else
                    {
                        temp_box.Text = values.Value;
                    }

                    tck.Controls.Add(temp_label);
                    tcv.Controls.Add(temp_box);

                    temp_tr.Cells.Add(tck);
                    temp_tr.Cells.Add(tcv);
                    table.Rows.Add(temp_tr);
                }
                XML_Table_Panel.Controls.Add(table);
            }
        }

        protected void SaveXML_Click(object sender, EventArgs e)
        {
            string GUID = "Error";
            GUID = Guid.NewGuid().ToString();

            string file = Server.MapPath("Resources\\Services\\") + GUID + ".xml";

            FileStream fs = new FileStream(file, FileMode.Create);
            XmlTextWriter w = new XmlTextWriter(fs, null);

            w.Formatting = Formatting.Indented;
            w.Indentation = 4;

            w.WriteStartDocument();
            w.WriteStartElement("Service");
            foreach (Control cntrl in XML_Table_Panel.Controls)
            {
                if (cntrl is Table)
                {
                    Table temp_table = (Table)(cntrl);
                    w.WriteStartElement(temp_table.Caption);
                    foreach (TableRow tr in temp_table.Rows)
                    {
                        Label temp_label = (Label)tr.Cells[0].Controls[0];
                        string name = temp_label.Text;
                        TextBox temp_box = (TextBox)tr.Cells[1].Controls[0];
                        string value = temp_box.Text;
                        if (name == "GUID")
                        {
                            value = GUID;
                        }
                        w.WriteStartElement(name);
                        w.WriteString(value);
                        w.WriteEndElement();
                    }
                    w.WriteEndElement();
                }
            }
            w.WriteEndElement();
            w.WriteEndDocument();
            w.Close();

        }
    }
}