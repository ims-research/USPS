<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
         CodeBehind="Default.aspx.cs" Inherits="USPS._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        User Service Policy Server
    </h2>
    <div class="welcomeDisplay">
        <asp:LoginView ID="HeadLoginView" runat="server" EnableViewState="false">
            <AnonymousTemplate>
                <p>Please login to view your available services</p>
            </AnonymousTemplate>
            <LoggedInTemplate>
                Welcome <asp:LoginName ID="HeadLoginName" runat="server" />!
                <p> Please select an option from above to proceed </p>
            </LoggedInTemplate>
        </asp:LoginView>
    </div>
    
</asp:Content>