<%@ Page Title="Create New Service Chain" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Create_Service.aspx.cs" Inherits="USPS.CreateService" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="addNodeDialog" class="basic-dialog" title="Adding a condition or service" runat="server">
    <p>Please add either a service to be activated (voicemail, email notification etc) or a condition (presence of end user, time of day etc)</p>
    </div>
    <div class="centre">
    <juice:dialog ID="Dialog1" TargetControlID="addNodeDialog" AutoOpen="false" runat="server"
                  Buttons="{'Add Service': function() { AddServiceClick(); }, 'Add Condition': function() { AddConditionClick(); } }"/>
    <p>
    <button id="addNodeDialogBtn" class="addNodeDialog" runat="server">Add a new service or condition</button>
    <juice:button ID="Button1" TargetControlID="addNodeDialogBtn" runat="server" />
    </p>
    </div>

    <script src="Scripts/D3/d3.v2.js" type="text/javascript"></script>
    <script src="Scripts/D3/D3Tree.js" type="text/javascript"></script>

<script type="text/javascript">

    // Respond to the click
        $(".addNodeDialog").click(function (e) {
        e.preventDefault();
        // Open the dialog
        $("#MainContent_addNodeDialog").dialog("open");
    });

    function AddServiceClick() {
        $("#MainContent_addNodeDialog").dialog("close");
        AddNode("service");
        restart();
    }


    function AddConditionClick() {
        $("#MainContent_addNodeDialog").dialog("close");
        AddNode("condition");
        restart();
    }

    function AddNode(Type) {
        var node = { "label": Type}
        var n = nodes.push(node);
        vis.selectAll("node").append("svg:path")
       .attr("d", function (d) {
           switch (d.label) {
               case 'service':
                   return "M15.5,3.029l-10.8,6.235L4.7,21.735L15.5,27.971l10.8-6.235V9.265L15.5,3.029zM15.5,7.029l6.327,3.652L15.5,14.334l-6.326-3.652L15.5,7.029zM24.988,10.599L16,15.789v10.378c0,0.275-0.225,0.5-0.5,0.5s-0.5-0.225-0.5-0.5V15.786l-8.987-5.188c-0.239-0.138-0.321-0.444-0.183-0.683c0.138-0.238,0.444-0.321,0.683-0.183l8.988,5.189l8.988-5.189c0.238-0.138,0.545-0.055,0.684,0.184C25.309,10.155,25.227,10.461,24.988,10.599z";
                   break;
               case 'condition':
                   return "M21.786,12.876l7.556-4.363l-7.556-4.363v2.598H2.813v3.5h18.973V12.876zM10.368,18.124l-7.556,4.362l7.556,4.362V24.25h18.974v-3.501H10.368V18.124z";
                   break;
           }
       });
    };

</script>
<asp:ScriptManager runat="server"></asp:ScriptManager>
</asp:Content>