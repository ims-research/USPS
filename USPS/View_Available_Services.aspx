<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View_Available_Services.aspx.cs" Inherits="USPS.ViewAvailableServices" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:GridView ID="serviceGrid" runat="server" AutoGenerateColumns="false">
    <Columns>
     <asp:TemplateField HeaderText="Service Name">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Value.ServiceInformation[\"Name\"]") %>'></asp:Label>
                </ItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Value.ServiceInformation[\"Description\"]") %>'></asp:Label>
                </ItemTemplate>
    </asp:TemplateField>
         <asp:TemplateField HeaderText="Provider">
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Value.ServiceInformation[\"Provider\"]") %>'></asp:Label>
                </ItemTemplate>
    </asp:TemplateField>
    <%--<asp:TemplateField HeaderText="rer">
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text="<%# ((Dictionary<string, USPS.Code.Service>)Container.DataItem).value %>"></asp:Label>
                </ItemTemplate>
           </asp:TemplateField>--%>
    </Columns></asp:GridView>
    <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" 
        ontextchanged="TextBox1TextChanged"></asp:TextBox>
    </asp:Content>
