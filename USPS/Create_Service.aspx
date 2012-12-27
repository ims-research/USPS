<%@ Page Title="Create New Service Chain" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Create_Service.aspx.cs" Inherits="USPS.CreateService" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="addNodeDialog" class="basic-dialog" title="Adding a condition or service" runat="server">
    <p>Please add either a service to be activated (voicemail, email notification etc) or a condition (presence of end user, time of day etc)</p>
    </div>

    <div id="selectServiceDialog" class="basic-dialog" title="Adding a Service" runat="server">
    <p>Choose Service you wish to be activated</p>
   <asp:DropDownList ID="ddlstService" runat="server"><asp:ListItem>Choose a Service</asp:ListItem></asp:DropDownList>
    </div>

    <div id="chooseServiceResponses" class="basic-dialog" title="Please select desired responses" runat="server">
    <p>Select which responses should be used for further processing</p>
        <ol id="selectableSIPResponse"></ol>
    </div>
        
   
    <div id="addConditionDialog" class="basic-dialog" title="Adding a Condition" runat="server">
    <p>Choose Condition you wish to specify</p>
    </div>

    <div class="centre">
        <juice:Dialog ID="juiceDialogAddNode" TargetControlID="addNodeDialog" AutoOpen="false" runat="server" Modal="true"
            Buttons="{'Add Service': function() { addServiceClick(); }, 'Add Condition': function() { addConditionClick(); } }">
        </juice:Dialog>
       <juice:Dialog ID="juiceSelectServiceDialog" TargetControlID="selectServiceDialog" AutoOpen="false" runat="server" Modal="true"
            Buttons="{'Select Service': function() { selectServiceClick(); } }">
        </juice:Dialog>
        <juice:Dialog ID="juiceChooseServiceResponse" TargetControlID="chooseServiceResponses" AutoOpen="false" runat="server" Modal="true" Width="500"
            Buttons="{'Confirm Service': function() { confirmServiceClick(); } }">
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
        $(function () {
            $("#selectableSIPResponse").selectable();
        });
    </script>
<script type="text/javascript">
    $("#MainContent_addNodeDialogBtn").click(addServiceOrCondition);
    
    function addServiceOrCondition() {
        $("#MainContent_addNodeDialog").dialog("open");
        $("#MainContent_addNodeDialogBtn").remove();
        return false;

    }

    function addServiceClick() {
        $("#MainContent_addNodeDialog").dialog("close");
        $("#MainContent_selectServiceDialog").dialog("open");
    }

    function confirmServiceClick() {
        $("#MainContent_chooseServiceResponses").dialog("close");
        $(".ui-selected").each(function () {
            var text = this.textContent;
            var value = this.getAttribute("value");
            addNodeByName(text, value, "ServiceValue", value);
        });
        $("#selectableSIPResponse").children().remove();
    }

    function selectServiceClick() {
        $("#MainContent_selectServiceDialog").dialog("close");
        var service_guid = $("#MainContent_ddlstService").val();
        var name = $("#MainContent_ddlstService option:selected").text();
        addNodeByName(name, service_guid, "Service", service_guid);
        var current_responses = []
        addNewSipResponse(service_guid,current_responses);
    }


    function addConditionClick() {
        $("#MainContent_addNodeDialog").dialog("close");
        $("#MainContent_addConditionDialog").dialog("open");
        //addNodeType("Condition");
       
    }

    function addSelectItem(container,key,value) {
        var container = $(container);
        //var inputs = container.find('input');
        //var id = inputs.length + 1;
        var html = '<li class="ui-widget-content" value="' + value + '">' + key + '</li>';
        //var html = '<input type="checkbox" id="cb' + id + '" value="' + name + '" /> <label for="cb' + id + '">' + name + '</label>';
        container.append($(html));
    }

    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }

    function guid() {
        return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
    }
        
    function addNodeByName(name, value,type,global_guid) {
        var newnode = {
            "name": name,
            "value": value,
            "type": type,
            "global_guid": global_guid,
            "instance_guid": guid(),
            "children": [],
        }
        addNode(newnode, root, currentID)
        update(root);
    }
</script>
<script type="text/javascript">
    var root = {
        "name": "Start", "type": "Root", "children": []

    };
    
    var currentNode = root;

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
        var nodeType = d.type;
        switch (nodeType) {
            case "Service":
                alert("Service Node Clicked");
                currentID = d.id;
                var current_responses = [];
                for (var i = 0; i < d.children.length; i++) {
                    current_responses.push(d.children[i].name);
                }
                addNewSipResponse(d.global_guid,current_responses);
                break;
            case "Condition":
                alert("Condition Node Clicked")
                currentID = d.id;
                //Add new condition value 
                break;
            case "ServiceValue":
                alert("Service Value Clicked")
                currentID = d.id;
                addServiceOrCondition();
                break;
            case "ConditionValue":
                alert("ConditionValue Clicked")
                currentID = d.id;
                addServiceOrCondition();
                break;
            default: alert("Unknown Node Type");
        }
    }

    function addNewSipResponse(service_guid, current_responses) {
        $.ajax({
            type: "POST",
            url: "Services.asmx/ListServiceResponses",
            data: "{'ServiceGUID':" + JSON.stringify(service_guid) + "}",
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
                for (var key in data) {
                    var found = $.inArray(key, current_responses) > -1;
                    if (!(found)){
                        addSelectItem("#selectableSIPResponse", key, data[key]);
                    }
                }

            },
            error: function (msg) {
                alert(msg);
            }
        });
        $("#MainContent_chooseServiceResponses").dialog("open");
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
            update(root)
            var nodeType = newNode.type;
            switch (nodeType) {
                case "Service":
                case "Condition":
                    currentID = newNode.id;
                    break;
                default:
                    break;
            }
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
</asp:Content>