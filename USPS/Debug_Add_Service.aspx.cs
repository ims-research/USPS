#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

#endregion

namespace USPS
{
    public partial class TestAddService : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> sinfoKeys = new Dictionary<string, string>
                {
                    {"Name", "Voice Mail"},
                    {"Type", "Session Establishment"},
                    {"Version", ""},
                    {"Provider", ""},
                    {"Description", ""}
                };
            Dictionary<string, string> sconfigKeys = new Dictionary<string, string>
                {
                    {"Server_IP", ""},
                    {"Server_Port", ""},
                    {"GUID", ""}
                };
            Dictionary<string, string> headerKeys = new Dictionary<string, string> {{"READ", ""}, {"WRITE", ""}};
            Dictionary<string, string> capKeys = new Dictionary<string, string>
                {
                    {"audio", ""},
                    {"video", ""},
                    {"duplex", ""},
                    {"methods", ""}
                };

            Dictionary<string, Dictionary<string, string>> nodes = new Dictionary<string, Dictionary<string, string>>
                {
                    {"Service_Information", sinfoKeys},
                    {"Service_Config", sconfigKeys},
                    {"SIP_Headers", headerKeys},
                    {"Capabalities", capKeys}
                };

            foreach (KeyValuePair<string, Dictionary<string, string>> kv in nodes)
            {
                Table table = new Table {Caption = kv.Key, ID = kv.Key + "_table"};

                foreach (KeyValuePair<string, string> values in kv.Value)
                {
                    TableRow tempTr = new TableRow();
                    Label tempLabel = new Label();
                    TextBox tempBox = new TextBox();
                    TableCell tck = new TableCell();
                    TableCell tcv = new TableCell();

                    tempLabel.Text = values.Key;
                    if (values.Key == "GUID")
                    {
                        tempBox.Text = Guid.NewGuid().ToString();
                        tempBox.Enabled = false;
                    }
                    else
                    {
                        tempBox.Text = values.Value;
                    }

                    tck.Controls.Add(tempLabel);
                    tcv.Controls.Add(tempBox);

                    tempTr.Cells.Add(tck);
                    tempTr.Cells.Add(tcv);
                    table.Rows.Add(tempTr);
                }
                XML_Table_Panel.Controls.Add(table);
            }
        }

        protected void SaveXMLClick(object sender, EventArgs e)
        {
            string guid = Guid.NewGuid().ToString();

            string file = Server.MapPath("Resources\\Services\\") + guid + ".xml";

            FileStream fs = new FileStream(file, FileMode.Create);
            XmlTextWriter w = new XmlTextWriter(fs, null) {Formatting = Formatting.Indented, Indentation = 4};

            w.WriteStartDocument();
            w.WriteStartElement("Service");
            foreach (Control cntrl in XML_Table_Panel.Controls)
            {
                if (cntrl is Table)
                {
                    Table tempTable = (Table) (cntrl);
                    w.WriteStartElement(tempTable.Caption);
                    foreach (TableRow tr in tempTable.Rows)
                    {
                        Label tempLabel = (Label) tr.Cells[0].Controls[0];
                        string name = tempLabel.Text;
                        TextBox tempBox = (TextBox) tr.Cells[1].Controls[0];
                        string value = tempBox.Text;
                        if (name == "GUID")
                        {
                            value = guid;
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