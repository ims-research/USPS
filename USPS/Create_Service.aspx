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
        <asp:DropDownList ID="ddlstService" runat="server">
            <asp:ListItem>Choose a Service</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div id="chooseServiceResponses" class="basic-dialog" title="Please select desired responses" runat="server">
        <p>Select which responses should be used for further processing</p>
        <ol id="selectableSIPResponse"></ol>
    </div>

    <div id="selectConditionDialog" class="basic-dialog" title="Adding a Condition" runat="server">
        <p>Choose Condition you wish to be activated</p>
        <asp:DropDownList ID="ddlstCondition" runat="server">
            <asp:ListItem>Choose a Condition</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div id="chooseConditionOptions" class="basic-dialog" title="Please select desired options" runat="server">
        <p>Select which conditions should be used for further processing</p>
        <ol id="selectableConditionOptions"></ol>
    </div>

    <div id="enterChainName" class="basic-dialog" title="Please enter in a name representing your new chain" runat="server">
        <p>Please enter in a name representing your new chain</p>
        <input id="chainNameText" type="text" value="Type something" />
    </div>


    <div class="centre">
        <juice:Dialog ID="juiceDialogAddNode" TargetControlID="addNodeDialog" AutoOpen="false" runat="server" Modal="true"
            Buttons="{'Add Service': function() { addServiceClick(); }, 'Add Condition': function() { addConditionClick(); } }"></juice:Dialog>
        <juice:Dialog ID="juiceSelectServiceDialog" TargetControlID="selectServiceDialog" AutoOpen="false" runat="server" Modal="true"
            Buttons="{'Select Service': function() { selectServiceClick(); } }"></juice:Dialog>
        <juice:Dialog ID="juiceChooseServiceResponse" TargetControlID="chooseServiceResponses" AutoOpen="false" runat="server" Modal="true" Width="500"
            Buttons="{'Confirm Service': function() { confirmServiceClick(); } }"></juice:Dialog>
        <juice:Dialog ID="juiceSelectConditionDialog" TargetControlID="selectConditionDialog" AutoOpen="false" runat="server" Modal="true"
            Buttons="{'Select Condition': function() { selectConditionClick(); } }"></juice:Dialog>
        <juice:Dialog ID="juiceChooseConditionOption" TargetControlID="chooseConditionOptions" AutoOpen="false" runat="server" Modal="true" Width="500"
            Buttons="{'Confirm Condition': function() { confirmConditionClick(); } }"></juice:Dialog>
        <juice:Dialog ID="juiceEnterChainName" TargetControlID="enterChainName" AutoOpen="false" runat="server" Modal="true" Width="500"
            Buttons="{'Confirm Chain Name': function() { confirmChainNameClick(); } }"></juice:Dialog>
        <p>
            <button id="addNodeDialogBtn" class="addNodeDialog" runat="server">Add a new service or condition</button>
            <juice:Button ID="Button1" TargetControlID="addNodeDialogBtn" runat="server" />
        </p>
        <p>
            <button id="saveChainBtn" class="saveChainBtn" runat="server">Save Chain</button>
            <juice:Button ID="Button3" TargetControlID="saveChainBtn" runat="server" />
        </p>
    </div>
    <script src="Scripts/D3/d3.v2.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#MainContent_saveChainBtn").hide();
            $("#selectableSIPResponse").selectable();
            $("#selectableConditionOptions").selectable();
            $("#MainContent_addNodeDialogBtn").click(addServiceOrCondition);
            $('#MainContent_saveChainBtn').click(askForNameClick);
        });
    </script>
    <script type="text/javascript">


        function addServiceOrCondition() {
            $("#MainContent_addNodeDialog").dialog("open");
            $("#MainContent_addNodeDialogBtn").remove();
            $("#MainContent_saveChainBtn").show();
            return false;
        }

        function addServiceClick() {
            $("#MainContent_addNodeDialog").dialog("close");
            $("#MainContent_selectServiceDialog").dialog("open");
        }

        function askForNameClick() {
            $("#MainContent_enterChainName").dialog("open");
            return false;
        }

        function confirmChainNameClick() {
            $("#MainContent_enterChainName").dialog("close");
            debugger;
            saveChain($('#chainNameText').val());
            return false;
        }

        function selectServiceClick() {
            $("#MainContent_selectServiceDialog").dialog("close");
            var service_guid = $("#MainContent_ddlstService").val();
            var name = $("#MainContent_ddlstService option:selected").text();
            addNodeByName(name, service_guid, "Service", service_guid);
            var current_responses = []
            addNewSipResponse(service_guid, current_responses);
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

        function addConditionClick() {
            $("#MainContent_addNodeDialog").dialog("close");
            $("#MainContent_selectConditionDialog").dialog("open");
        }

        function selectConditionClick() {
            $("#MainContent_selectConditionDialog").dialog("close");
            var condition_guid = $("#MainContent_ddlstCondition").val();
            var name = $("#MainContent_ddlstCondition option:selected").text();
            addNodeByName(name, condition_guid, "Condition", condition_guid);
            var current_options = []
            addNewConditionOption(condition_guid, current_options);
        }

        function confirmConditionClick() {
            $("#MainContent_chooseConditionOptions").dialog("close");
            $(".ui-selected").each(function () {
                debugger;
                var text = this.textContent;
                var value = this.getAttribute("value");
                addNodeByName(text, value, "ConditionValue", value);
            });
            $("#selectableConditionOptions").children().remove();
        }

        function addSelectItem(container, key, value) {
            var container = $(container);
            var html = '<li class="ui-widget-content" value="' + value + '">' + key + '</li>';
            container.append($(html));
        }


        function saveChain(name) {
            var my_guid = JSON.stringify(guid())
            seen = []
            var chain = JSON.stringify(root, function (key, val) {
                if (typeof val == "object") {
                    if (seen.indexOf(val) >= 0)
                        return undefined
                    seen.push(val)
                }
                return val
            })
            debugger;
            $.ajax({
                type: "POST",
                url: "Services.asmx/SaveChain",
                data: "{'GUID':" + my_guid + ",'Chain':'" + chain + "','Name':'" + JSON.stringify(name) + "'}",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) {
                    var msg = eval('(' + data + ')');
                    if (msg.hasOwnProperty('d'))
                        return msg.d;
                    else
                        return msg;
                },
                success: function (data) {
                    alert(data);
                },
                error: function (data) {
                    alert(data);
                }
            });
            return false;
        }

        function S4() {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        }

        function guid() {
            return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
        }

        function addNodeByName(name, value, type, global_guid) {
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
        h = 500,
        i = 0,
        duration = 500,
        root;

        var tree = d3.layout.tree()
            .size([w - 160, h]);

        var diagonal = d3.svg.diagonal();

        var vis = d3.select("div.main").append("svg:svg")
            .attr("width", w)
            .attr("height", h + 500)
          .append("svg:g")
            .attr("transform", "translate(40,40)");

        d3.select(self.frameElement).style("height", "1000px");

        update(root)

        function update(source) {

            // Compute the new tree layout.
            var nodes = tree.nodes(root).reverse();
            // Update the nodes…
            var node = vis.selectAll("g.node")
              .data(nodes, function (d) { return d.id || (d.id = ++i); });

            var nodeEnter = node.enter().append("svg:g")
                .attr("class", "node")
                .attr("transform", function (d) { return "translate(" + source.x0 + "," + source.y0 + ")"; });

            // Enter any new nodes at the parent's previous position.
            nodeEnter.append("svg:circle")
              .attr("r", 6)
              .style("fill", "lightsteelblue")
              .on("click", click);

            nodeEnter.append("svg:text")
                .attr("x", function (d) { return -((d.name.length / 2) * 7) })
                .attr("y", 30)
                .text(function (d) { return d.name; });

            // Transition nodes to their new position.
            nodeEnter.transition()
                .duration(duration)
                .attr("transform", function (d) { return "translate(" + d.x + "," + d.y + ")"; })
                .style("opacity", 1)
              .select("circle")
                .style("fill", "lightsteelblue");

            node.transition()
              .duration(duration)
              .attr("transform", function (d) { return "translate(" + d.x + "," + d.y + ")"; })
              .style("opacity", 1);


            node.exit().transition()
              .duration(duration)
              .attr("transform", function (d) { return "translate(" + source.x + "," + source.y + ")"; })
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
                    currentID = d.id;
                    var current_responses = [];
                    for (var i = 0; i < d.children.length; i++) {
                        current_responses.push(d.children[i].name);
                    }
                    addNewSipResponse(d.global_guid, current_responses);
                    break;
                case "Condition":
                    currentID = d.id;
                    var current_options = [];
                    for (var i = 0; i < d.children.length; i++) {
                        current_options.push(d.children[i].name);
                    }
                    addNewConditionOption(d.global_guid, current_options);
                    break;
                case "ServiceValue":
                    currentID = d.id;
                    addServiceOrCondition();
                    break;
                case "ConditionValue":
                    currentID = d.id;
                    addServiceOrCondition();
                    break;
                default: alert("Unknown Node Type - Cannot add more than one starting point");
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
                    for (var key in data) {
                        var found = $.inArray(key, current_responses) > -1;
                        if (!(found)) {
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

        function addNewConditionOption(condition_guid, current_options) {
            $.ajax({
                type: "POST",
                url: "Services.asmx/ListConditionOptions",
                data: "{'ConditionGUID':" + JSON.stringify(condition_guid) + "}",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) {
                    var msg = eval('(' + data + ')');
                    if (msg.hasOwnProperty('d'))
                        return msg.d;
                    else
                        return msg;
                },
                success: function (data) {
                    debugger;
                    for (var key in data) {
                        var found = $.inArray(data[key], current_options) > -1;
                        if (!(found)) {
                            addSelectItem("#selectableConditionOptions", data[key], key);
                        }
                    }

                },
                error: function (msg) {
                    alert(msg);
                }
            });
            $("#MainContent_chooseConditionOptions").dialog("open");
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
