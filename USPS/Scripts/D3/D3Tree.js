
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
            .attr("cy", function (d) { return d.y; });
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

    vis.selectAll("circle.node")
      .data(nodes)
    .enter().insert("svg:circle", "circle.cursor")
      .attr("class", "node")
      .attr("cx", function (d) { return d.x; })
      .attr("cy", function (d) { return d.y; })
      .attr("r", 5)
      .call(force.drag);

    force.start();
}