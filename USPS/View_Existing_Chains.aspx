<%@ Page Title="View Existing Service Chains" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="View_Existing_Chains.aspx.cs" Inherits="USPS.View_Existing_Chains"  %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div class="centre">
        <div id="navcontainer" style="width:auto">
        <ul id="Chainlist">
        </ul>
        </div>
        </div>
    <script src="Scripts/jquery-1.8.2.js" type="text/javascript"></script>
<script src="Scripts/jquery-ui-1.9.0.js" type="text/javascript"></script>
   <script type="text/javascript">
       $(window).load(function () {
           getServiceChains();
       });
       
       function addChain(data) {
           var container = $("#Chainlist");
           for (var key in data) {
               debugger;

               var html = '<li value="' + data[key].FirstBlockGUID + '"><a href="#">'+data[key].Name+'</a></li>';
               container.append($(html));
           }
       }
       
       function getServiceChains() {
           $.ajax({
               type: "POST",
               url: "Services.asmx/ListExistingChains",
               data: {},
               contentType: "application/json; charset=utf-8",
               dataFilter: function (data) {
                   var msg = eval('(' + data + ')');
                   if (msg.hasOwnProperty('d'))
                       return msg.d;
                   else
                       return msg;
               },
               success: function (data) {
                   console.log(data);
                   debugger;
                   addChain(data);
               },
               error: function (msg) {
                   alert(msg);
               }
           });
       }
   </script>

</asp:Content>