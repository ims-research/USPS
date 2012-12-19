<%@ Page Title="Create New Service Chain" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Create_Service.aspx.cs" Inherits="USPS.CreateService" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="addNodeDialog" class="basic-dialog" title="Adding a condition or service" runat="server">
    <p>Please add either a service to be activated (voicemail, email notification etc) or a condition (presence of end user, time of day etc)</p>
    </div>

    <div id="addServiceDialog" class="basic-dialog" title="Adding a Service" runat="server">
    <p>Choose Service you wish to be activated</p>
   <asp:DropDownList ID="ddlstService" runat="server" AutoPostBack="True"><asp:ListItem>Choose a Service</asp:ListItem></asp:DropDownList>
    <%--OnSelectedIndexChanged="ddlstServiceSelectedIndexChanged"--%>
        
    </div>
    <div id="addConditionDialog" class="basic-dialog" title="Adding a Condition" runat="server">
    <p>Choose Condition you wish to specify</p>
    </div>

    <div class="centre">
        <juice:Dialog ID="juiceDialogAddNode" TargetControlID="addNodeDialog" AutoOpen="false" runat="server" Modal="true"
            Buttons="{'Add Service': function() { addServiceClick(); }, 'Add Condition': function() { addConditionClick(); } }">
        </juice:Dialog>
       <juice:Dialog ID="juiceDialogAddService" TargetControlID="addServiceDialog" AutoOpen="false" runat="server" Modal="true"
            Buttons="{'Add Service': function() { addServiceClick(); } }">
        </juice:Dialog>
               <juice:Dialog ID="juiceDialogAddCondition" TargetControlID="addConditionDialog" AutoOpen="false" runat="server" Modal="true"
            Buttons="{'Add Condition': function() { addConditionClick(); } }">
        </juice:Dialog>
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

    function addServiceClick() {
        $("#MainContent_addNodeDialog").dialog("close");
        $("#MainContent_addServiceDialog").dialog("open");
        //addNodeType("Service");
    }


    function addConditionClick() {
        $("#MainContent_addNodeDialog").dialog("close");
        $("#MainContent_addConditionDialog").dialog("open");
        //addNodeType("Condition");
       
    }

    function addNodeType(type) {

        var newnode = {
            "name": type + String(currentID),
            "children": [],
        }
        addNode(newnode, root, currentID)
        update(root);
    }
</script>
<script type="text/javascript">
    var root = {
        "name": "Root", "children": []
    };
    
    var currentID = 1;

    var w = 960,
    h = 1000,
    i = 0,
    duration = 500,
    root;

    var tree = d3.layout.tree()
        .size([h, w - 160]);

    var diagonal = d3.svg.diagonal()
        .projection(function (d) { return [d.y, d.x]; });

    var vis = d3.select("div.main").append("svg:svg")
        .attr("width", w)
        .attr("height", h)
      .append("svg:g")
        .attr("transform", "translate(40,0)");

    d3.select(self.frameElement).style("height", "1000px");

    update(root)

    function update(source) {

        // Compute the new tree layout.
        var nodes = tree.nodes(root).reverse();
        console.log(nodes)
        // Update the nodes…
        var node = vis.selectAll("g.node")
          .data(nodes, function (d) { return d.id || (d.id = ++i); });

        var nodeEnter = node.enter().append("svg:g")
            .attr("class", "node")
            .attr("transform", function (d) { return "translate(" + source.y0 + "," + source.x0 + ")"; });
        //.style("opacity", 1e-6);

        // Enter any new nodes at the parent's previous position.

        nodeEnter.append("svg:circle")
          //.attr("class", "node")
          //.attr("cx", function(d) { return source.x0; })
          //.attr("cy", function(d) { return source.y0; })
          .attr("r", 4.5)
          .style("fill", function (d) { return d._children ? "lightsteelblue" : "#fff"; })
          .on("click", click);

        nodeEnter.append("svg:text")
            .attr("x", function (d) { return d._children ? -8 : 8; })
            .attr("y", 3)
            //.attr("fill","#ccc")
            //.attr("transform", function(d) { return "translate(" + d.y + "," + d.x + ")"; })
            .text(function (d) { return d.name; });

        // Transition nodes to their new position.
        nodeEnter.transition()
            .duration(duration)
            .attr("transform", function (d) { return "translate(" + d.y + "," + d.x + ")"; })
            .style("opacity", 1)
          .select("circle")
            //.attr("cx", function(d) { return d.x; })
            //.attr("cy", function(d) { return d.y; })
            .style("fill", "lightsteelblue");

        node.transition()
          .duration(duration)
          .attr("transform", function (d) { return "translate(" + d.y + "," + d.x + ")"; })
          .style("opacity", 1);


        node.exit().transition()
          .duration(duration)
          .attr("transform", function (d) { return "translate(" + source.y + "," + source.x + ")"; })
          .style("opacity", 1e-6)
          .remove();

        // Update the links…
        var link = vis.selectAll("path.link")
            .data(tree.links(nodes), function (d) { return d.target.id; });

        // Enter any new links at the parent's previous position.
        link.enter().insert("svg:path", "g")
            .attr("class", "link")
            .attr("d", function (d) {
                var o = { x: source.x0, y: source.y0 };
                return diagonal({ source: o, target: o });
            })
          .transition()
            .duration(duration)
            .attr("d", diagonal);

        // Transition links to their new position.
        link.transition()
            .duration(duration)
            .attr("d", diagonal);

        // Transition exiting nodes to the parent's new position.
        link.exit().transition()
            .duration(duration)
            .attr("d", function (d) {
                var o = { x: source.x, y: source.y };
                return diagonal({ source: o, target: o });
            })
            .remove();

        // Stash the old positions for transition.
        nodes.forEach(function (d) {
            d.x0 = d.x;
            d.y0 = d.y;
        });
    }

    // Toggle children on click.
    function click(d) {
        //if (d.children) {
        //    d._children = d.children;
        //    d.children = null;
        //} else {
        //    d.children = d._children;
        //    d._children = null;
        //}
        currentID = d.id;
        $("#MainContent_addNodeDialog").dialog("open");
    }
      
    function addNode(newNode, startNode, parentID) {
        var children;

        if (startNode.children) {
            children = startNode.children;
        }
        else {
            children = startNode._children;
        }

        if (startNode.id == parentID) {
            children.push(newNode);
            // To Generate ID
            update(root)
        }
        else {
            if (children) {
                for (var i = 0; i < children.length; i++) {
                    var node = children[i];
                    addNode(newNode, node, parentID)
                }
            }

        }
    }

   
</script>

<asp:ScriptManager runat="server"></asp:ScriptManager>
</asp:Content>