using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using USPS.Code;

namespace USPS
{
    public partial class Debug_View_Service_From_XML : System.Web.UI.Page
    {
        private string service_directory;

        protected void Page_Load(object sender, EventArgs e)
        {
            service_directory = Path.Combine(Request.PhysicalApplicationPath, "Resources\\Services\\"); 
            if (!this.IsPostBack) 
            { 
                CreateFileList(); 
            } 
        } 
 
    private void CreateFileList() 
    {
        string[] fileList = Directory.GetFiles(service_directory);
        lstFiles.DataSource = fileList; 
        lstFiles.DataBind();
        lblSelectedFile.Text = ""; 
    }
    protected void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        string fileName = lstFiles.SelectedItem.Text;
        System.Text.StringBuilder displayText = new System.Text.StringBuilder();
        Service temp_Service = new Service(fileName);
    }

    //protected void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string fileName = lstFiles.SelectedItem.Text; 
    //    System.Text.StringBuilder displayText = new System.Text.StringBuilder(); 
    //    displayText.Append("<b>"); 
    //    displayText.Append(fileName); 
    //    displayText.Append("</b><br /><br />"); 
    //    displayText.Append("Created: "); 
    //    displayText.Append(File.GetCreationTime(fileName).ToString()); 
    //    displayText.Append("<br />Last Accessed: "); 
    //    displayText.Append(File.GetLastAccessTime(fileName).ToString()); 
    //    displayText.Append("<br />"); 
 
    //    // Show attribute information. GetAttributes() can return a combination 
    //    // of enumerated values, so you need to evaluate it with the 
    //    // bitwise and (&) operator. 
    //    FileAttributes attributes = File.GetAttributes(fileName); 
    //    if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden) 
    //    { 
    //        displayText.Append("This is a hidden file.<br />"); 
    //    } 
    //    if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) 
    //    { 
    //        displayText.Append("This is a read-only file.<br />"); 
    //    }
    //    lblSelectedFile.Text = displayText.ToString();
    //}

    }
}