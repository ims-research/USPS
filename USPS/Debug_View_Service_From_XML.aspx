<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeBehind="Debug_View_Service_From_XML.aspx.cs" Inherits="USPS.DebugViewServiceFromXML" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Table ID="Files_Table" runat="server">
        <asp:TableRow><asp:TableCell><asp:ListBox ID="lstFiles" runat="server" 
                                                  onselectedindexchanged="LstFilesSelectedIndexChanged" AutoPostBack="True"
                                                  Width="339px" Height="481px" style="LEFT: 313px; TOP: 16px;"></asp:ListBox></asp:TableCell>
            <asp:TableCell><asp:Label ID="lblSelectedFile" runat="server" Text="File Information"
                                      Width="339px" Height="481px" style="LEFT: 313px; TOP: 16px;"></asp:Label></asp:TableCell>
        </asp:TableRow>
    </asp:Table>

</asp:Content>