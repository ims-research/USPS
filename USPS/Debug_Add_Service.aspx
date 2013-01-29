<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Debug_Add_Service.aspx.cs" Inherits="USPS.TestAddService" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Panel ID="XML_Table_Panel" runat="server">
        <asp:Button runat="server" Text="Click To Save XML" onclick="SaveXMLClick"/>
    </asp:Panel>
</asp:Content>