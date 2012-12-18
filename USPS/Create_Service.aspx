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
       $("#MainContent_addNodeDialogBtn").click(function (e) {
        e.preventDefault();
       $("#MainContent_addNodeDialog").dialog("open");
    });

    function AddServiceClick() {
        $("#MainContent_addNodeDialog").dialog("close");
        AddNode("Service");
    }


    function AddConditionClick() {
        $("#MainContent_addNodeDialog").dialog("close");
        AddNode("Condition");
       
    }

    function AddNode(Type) {
        var new_node = { "name": Type, "children": [] }
        treeData.children.push(new_node);
        Update();
    };
</script>
<script type="text/javascript">

    function Update()
    {
        var nodes = tree.nodes(treeData);

        var link = vis.selectAll("pathlink")
        .data(tree.links(nodes))
        .enter().append("svg:path")
        .attr("class", "link")
        .attr("d", diagonal);

        var node = vis.selectAll("g.node")
        .data(nodes)
        .enter().append("svg:g")
        .attr("transform", function (d) { return "translate(" + d.y + "," + d.x + ")"; })

        node.append("svg:circle")
        .attr("r", 4.5);

        node.append("svg:text")
        .attr("dx", function (d) { return d.children ? -8 : 8; })
        .attr("dy", 3)
        .attr("text-anchor", function (d) { return d.children ? "end" : "start"; })
        .text(function (d) { return d.name; });

        nodes.exit().remove();
    }

    var treeData = {
        "name": "Root", "children": []
    };
    var vis = d3.select("div.main").append("svg:svg")
    .attr("width", 920)
    .attr("height", 500)
    .append("svg:g")
    .attr("transform", "translate(40, 0)");

    var tree = d3.layout.tree()
    .size([800, 400]);

    var diagonal = d3.svg.diagonal()
    .projection(function (d) { return [d.y, d.x]; });

    Update()
</script>

<asp:ScriptManager runat="server"></asp:ScriptManager>
</asp:Content>