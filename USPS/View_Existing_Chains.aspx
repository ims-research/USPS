<%@ Page Title="View Existing Service Chains" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="View_Existing_Chains.aspx.cs" Inherits="USPS.View_Existing_Chains" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div class="centre">
        <div class="CSSTableGenerator">
            <table id="ChainListTable">
                <tbody>
                    <tr>
                        <td>Chain Name</td>
                        <td>Delete Chain</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="d3Tree"></div>
    </div>
    <script src="Scripts/jquery-1.8.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.9.0.js" type="text/javascript"></script>
    <script src="Scripts/D3/d3.v2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(window).load(function () {
            getServiceChains();
        });

        function addChain(data) {

            for (var key in data) {
                var container = $('#ChainListTable > tbody:last');
                debugger;
                container.append('<tr class=' + data[key].FirstBlockGUID + '><td><a class=displayChain guid="' + data[key].FirstBlockGUID + '" href="#">' + data[key].Name + '</a></td><td><button class=deleteChain guid="' + data[key].FirstBlockGUID + '">Delete This Chain</button></td></tr>');
            }
            $('.displayChain').click(displayChain);
            $('.deleteChain').click(deleteChain);
        }

        function deleteChain() {
            root = [];
            update(root);
            var firstGUID = $(event.target)[0].attributes.getNamedItem('guid').value;
            $("." + firstGUID).remove();
            
            $.ajax({
                type: "POST",
                url: "Services.asmx/DeleteExistingChain",
                data: "{'FirstBlockGUID':'" + firstGUID + "'}",
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
                error: function (msg) {
                    alert(msg);
                }
            });
            return false;
        }

        function displayChain(event) {
            debugger;
            var firstGUID = $(event.target)[0].attributes.getNamedItem('guid').value;
            getServiceFlow(firstGUID);
            return false;
        }

       
        function getServiceFlow(FirstBlockGUID) {
            var my_guid = JSON.stringify(FirstBlockGUID);
            $.ajax({
                type: "POST",
                url: "Services.asmx/GetExistingChain",
                data: "{'FirstBlockGUID':" + my_guid + "}",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) {
                    var msg = eval('(' + data + ')');
                    if (msg.hasOwnProperty('d'))
                        return msg.d;
                    else
                        return msg;
                },
                success: function (data) {
                    showChain(data)
                },
                error: function (msg) {
                    alert(msg);
                }
            });
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
                    addChain(data);
                },
                error: function (msg) {
                    alert(msg);
                }
            });
        }
    </script>
    <script type="text/javascript">
        var root = {
            "name": "Start", "type": "Root", "children": []
        };

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
        
        function showChain(data) {
            root = data;
            update(data);
        }

        </script>

</asp:Content>
