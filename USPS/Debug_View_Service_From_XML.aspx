<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeBehind="Debug_View_Service_From_XML.aspx.cs" Inherits="USPS.Debug_View_Service_From_XML" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Table ID="Files_Table" runat="server">
    <asp:TableRow><asp:TableCell><asp:ListBox ID="lstFiles" runat="server" 
            onselectedindexchanged="lstFiles_SelectedIndexChanged" AutoPostBack="True"
            Width="339px" Height="481px" style="TOP: 16px; LEFT: 313px;"></asp:ListBox></asp:TableCell>
            <asp:TableCell><asp:Label ID="lblSelectedFile" runat="server" Text="File Information"
        Width="339px" Height="481px" style="TOP: 16px; LEFT: 313px;"></asp:Label></asp:TableCell>
        </asp:TableRow>
    </asp:Table>

</asp:Content>