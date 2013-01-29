#region

using System;
using System.IO;
using System.Web.UI;
using LibServiceInfo;

#endregion

namespace USPS
{
    public partial class DebugViewServiceFromXML : Page
    {
        private string _serviceDirectory;

        protected void Page_Load(object sender, EventArgs e)
        {
            _serviceDirectory = Path.Combine(Request.PhysicalApplicationPath, "Resources\\Services\\");
            if (!IsPostBack)
            {
                CreateFileList();
            }
        }

        private void CreateFileList()
        {
            string[] fileList = Directory.GetFiles(_serviceDirectory);
            lstFiles.DataSource = fileList;
            lstFiles.DataBind();
            lblSelectedFile.Text = "";
        }

        protected void LstFilesSelectedIndexChanged(object sender, EventArgs e)
        {
            string fileName = lstFiles.SelectedItem.Text;
            Service tempService = new Service(fileName);
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