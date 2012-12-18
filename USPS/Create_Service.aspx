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
    var w = 920,
       h = 500,
       fill = d3.scale.category20(),
       nodes = [],
       links = [];

    var vis = d3.select("div.main").append("svg:svg")
        .attr("width", w)
        .attr("height", h);

    vis.append("svg:rect")
        .attr("width", w)
        .attr("height", h);

    var force = d3.layout.force()
                .gravity(.05)
                .distance(100)
                .charge(-100)
                .nodes(nodes)
                .links(links)
                .size([w, h]);

    force.on("tick", function () {
        vis.selectAll("line.link")
            .attr("x1", function (d) { return d.source.x; })
            .attr("y1", function (d) { return d.source.y; })
            .attr("x2", function (d) { return d.target.x; })
            .attr("y2", function (d) { return d.target.y; });

        vis.selectAll("circle.node")
            .attr("cx", function (d) { return d.x; })
            .attr("cy", function (d) { return d.y; })
            node.attr("transform", function (d) { return "translate(" + d.x + "," + d.y + ")"; });
    });


    restart();


    function restart() {

        vis.selectAll("line.link")
          .data(links)
        .enter().insert("svg:line", "circle.node")
          .attr("class", "link")
          .attr("x1", function (d) { return d.source.x; })
          .attr("y1", function (d) { return d.source.y; })
          .attr("x2", function (d) { return d.target.x; })
          .attr("y2", function (d) { return d.target.y; });

        force.start();
    }

    // Respond to the click
    $("#MainContent_addNodeDialogBtn").click(function (e) {
        e.preventDefault();
        // Open the dialog
        $("#MainContent_addNodeDialog").dialog("open");
    });

    function AddServiceClick() {
        $("#MainContent_addNodeDialog").dialog("close");
        AddNode("servicaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaae");
    }


    function AddConditionClick() {
        $("#MainContent_addNodeDialog").dialog("close");
        AddNode("conditiaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaon");
       
    }

    function AddNode(Type) {
        var node = { "label": Type }
        nodes.push(node);
        UpdateTree()
        restart();
    };
    function UpdateTree() {
        var container = vis.selectAll("g").data(nodes).enter().append("g").attr("class", "circle.node")
        container.append("svg:circle")
            .attr("class", "circle.node")
            .attr("r", 15)
            .attr("cx", function (d) { return d.x; })
            .attr("cy", function (d) { return d.y; })
            .style("fill", "steelblue")
            .style("stroke", "white")
            .style("stroke-width", "1.5px")
            .call(force.drag);
        
        container.append("text")
            .attr("cx", function (d) { return d.x; })
            .attr("cy", function (d) { return d.y; })
            .text(function (d) { return d.label })
            .call(force.drag);

        container.exit().remove()
    }

</script>
<asp:ScriptManager runat="server"></asp:ScriptManager>
</asp:Content>