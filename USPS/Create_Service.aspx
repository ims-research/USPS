<%@ Page Title="Creante New Service Chain" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Create_Service.aspx.cs" Inherits="USPS.Create_Service" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<asp:ScriptManager runat="server"></asp:ScriptManager>
   
<asp:UpdatePanel runat="server" ID="UpdatePanel">
<ContentTemplate>
 <asp:Panel ID="pnlInstruction" runat="server" CssClass="text-instruction" 
        HorizontalAlign="Center">
        <asp:Label ID="lblInstruction" runat="server" 
            Text="Creating New Service - Please Select Starting Point (Condition or Service)"></asp:Label>
    </asp:Panel>
<asp:Panel runat="server" HorizontalAlign="Center" Width="100%">
    <asp:Panel ID="ConditionPanel" runat="server" HorizontalAlign="Center" Width="100%">
        <asp:RadioButtonList ID="rdblstChoice" runat="server" 
            OnSelectedIndexChanged="rdblstChoice_OnSelectedIndexChanged" 
            AutoPostBack="True" Width="100%" TextAlign="Left" 
            RepeatDirection="Horizontal" RepeatColumns="3" BorderColor="Black" 
            BorderStyle="Groove" BorderWidth="1px">
        <asp:ListItem>Add Service Block</asp:ListItem>
        <asp:ListItem>Add Condition Block</asp:ListItem>
        <asp:ListItem>Add Terminating Block</asp:ListItem>
        <asp:ListItem>Add Another Value to Existing Block</asp:ListItem>
        </asp:RadioButtonList>
    </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="OverallPanel" runat="server" HorizontalAlign="Center">
    <asp:Table runat="server">
    <asp:TableRow>
    <asp:TableCell RowSpan="2">
    <asp:TreeView ID="tvServiceFlow" SelectedNodeStyle-CssClass="selectedTreeNode" runat="server" ShowLines="True" OnSelectedNodeChanged="tvServiceFlow_NodeChange" Width="200" CssClass="tree">
    <NodeStyle NodeSpacing="3" HorizontalPadding="3" VerticalPadding="3" BorderColor="Black" BorderWidth="1"/>
    </asp:TreeView>
    </asp:TableCell>
    <asp:TableCell>
    <asp:Table ID="ServiceTable" runat="server" Visible="false">
    <asp:TableRow>
    <asp:TableCell>
    <asp:Label runat="server" Text="Please Choose Service"></asp:Label>
    </asp:TableCell>
    <asp:TableCell>
    <asp:DropDownList ID="ddlstService" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlstService_SelectedIndexChanged" Width="200"><asp:ListItem>Choose a Service</asp:ListItem></asp:DropDownList>
    </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell>
    <asp:Label ID="Label1" runat="server" Text="Available Responses"></asp:Label>
    </asp:TableCell>
    <asp:TableCell>
    </asp:TableCell>
    <asp:TableCell>
    <asp:Label ID="Label2" runat="server" Text="Selected Responses"></asp:Label>
    </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell RowSpan="2" HorizontalAlign="Center"><asp:ListBox ID="lstbxAvalRes" runat="server" Width="200" SelectionMode="Multiple"><asp:ListItem>Please select service to see available responses</asp:ListItem></asp:ListBox></asp:TableCell>
    <asp:TableCell HorizontalAlign="Center"><asp:Button ID="btnAddResponses" runat="server" Text=">>" OnClick="btnAddResponses_OnClick"  /></asp:TableCell>
    <asp:TableCell RowSpan="2" HorizontalAlign="Center"><asp:ListBox ID="lstbxSelRes" runat="server" Width="200" SelectionMode="Multiple"><asp:ListItem>Please choose a response</asp:ListItem></asp:ListBox></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell HorizontalAlign="Center"><asp:Button ID="btnDelResponses" runat="server" Text="<<" OnClick="btnDelResponses_OnClick"  /></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center"><asp:Button ID="btnAddService" runat="server" Text="Add Service to Flow" OnClick="btnAddService_OnClick"  /></asp:TableCell>
    </asp:TableRow>
    </asp:Table>
    </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell>
    <asp:Table ID="ConditionTable" runat="server"  Visible="false">
    <asp:TableRow>
    <asp:TableCell>
    <asp:Label ID="lblCondition" runat="server" Text="Please Choose Condition"></asp:Label>
    </asp:TableCell>
    <asp:TableCell>
    <asp:DropDownList ID="ddlstCondition" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlstCondition_SelectedIndexChanged" Width="200"><asp:ListItem>Choose a Condition</asp:ListItem></asp:DropDownList>
    </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell>
    <asp:Label ID="lblAvaConditions" runat="server" Text="Available Values"></asp:Label>
    </asp:TableCell>
    <asp:TableCell>
    </asp:TableCell>
    <asp:TableCell>
    <asp:Label ID="lblSelConditions" runat="server" Text="Selected Values"></asp:Label>
    </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell RowSpan="2" HorizontalAlign="Center"><asp:ListBox ID="lstbxAvaValues" runat="server" Width="200" AutoPostBack="False" SelectionMode="Multiple"><asp:ListItem>Please select a condition to see available values</asp:ListItem></asp:ListBox></asp:TableCell>
    <asp:TableCell HorizontalAlign="Center"><asp:Button ID="btnAddValue" runat="server" Text=">>" OnClick="btnAddValue_OnClick"  /></asp:TableCell>
    <asp:TableCell RowSpan="2" HorizontalAlign="Center"><asp:ListBox ID="lstbxSelValues" runat="server" Width="200" AutoPostBack="False" SelectionMode="Multiple"><asp:ListItem>Please choose a value</asp:ListItem></asp:ListBox></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell HorizontalAlign="Center"><asp:Button ID="btnDelValue" runat="server" Text="<<" OnClick="btnDelValue_OnClick"  /></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell ColumnSpan="3" HorizontalAlign="Center"><asp:Button ID="btnAddCondition" runat="server" Text="Add Condition" OnClick="btnAddCondition_OnClick"  /></asp:TableCell>
    </asp:TableRow>
    </asp:Table>
    </asp:TableCell></asp:TableRow>
    <asp:TableRow><asp:TableCell><asp:Label ID="lblChooseAnother" runat="server" Text="Please click on the node you wish to add another block to" Visible="false"></asp:Label></asp:TableCell></asp:TableRow>
    </asp:Table>
        <asp:Button ID="btnSaveFlow" runat="server" Text="Save Service Flow" 
            onclick="btnSaveFlow_Click" />
    </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>