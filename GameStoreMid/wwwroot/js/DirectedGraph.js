$body = $("body");

$(document).on({
    ajaxStart: function () {
        console.log("start loading");
        $body.addClass("loading");

    },
    ajaxStop: function () {
        console.log("stop loading");
        $body.removeClass("loading");
    }
});

var svg = d3.select("svg"),
    width = +svg.attr("width"),
    height = +svg.attr("height");

var color = d3.scaleOrdinal(d3.schemeCategory20);

var simulation = d3.forceSimulation()
    .force("link", d3.forceLink().id(function (d) { return d.id; }))
    .force("charge", d3.forceManyBody())
    .force("center", d3.forceCenter(width / 2, height / 2));

$.get("/Apriori/GenerateSimilaritiesJson", {}, function (data) {
    console.log(data);
    start(data);
}, "json").done(function () {
}).fail(function (data, textStatus, xhr) {
    //This shows status code eg. 403
    console.log("error", data.status);
    console.log(data);
    console.log(xhr);
    console.log(textStatus);
    //This shows status message eg. Forbidden
    console.log("STATUS: " + xhr);
}).always(function () {
    //TO-DO after fail/done request.
    });

function start(graph) {
    

        var link = svg.append("g")
            .attr("class", "links")
            .selectAll("line")
            .data(graph.links)
            .enter().append("line")
            .attr("stroke-width", function (d) { return Math.sqrt(d.value); });

    var tip = d3.tip()
        .attr('class', 'd3-tip')
        .offset([-10, 0])
        .html(function (d) {
            return "<strong>Game: </strong> <span style='color:red'>" + d.id + "</span>";
        })

        svg.call(tip);
        var node = svg.append("g")
            .attr("class", "nodes")
            .selectAll("circle")
            .data(graph.nodes)
            .enter().append("circle")
            .attr("r", 5)
            .attr("fill", function (d) { return color(d.group); })
            .call(d3.drag()
                .on("start", dragstarted)
                .on("drag", dragged)
                .on("end", dragended)
        ).on('mouseover', tip.show)
            .on('mouseout', tip.hide)
           ;


     
        node.append("title")
            .text(function (d) { return d.id; });

        simulation
            .nodes(graph.nodes)
            .on("tick", ticked);

        simulation.force("link")
            .links(graph.links);

        simulation.alpha(1).restart();

        function ticked() {
            link
                .attr("x1", function (d) { return d.source.x; })
                .attr("y1", function (d) { return d.source.y; })
                .attr("x2", function (d) { return d.target.x; })
                .attr("y2", function (d) { return d.target.y; });

            node
                .attr("cx", function (d) { return d.x; })
                .attr("cy", function (d) { return d.y; });
        }
    }


function dragstarted(d) {
    if (!d3.event.active) simulation.alphaTarget(0.3).restart();
    d.fx = d.x;
    d.fy = d.y;
}

function dragged(d) {
    d.fx = d3.event.x;
    d.fy = d3.event.y;
}

function dragended(d) {
    if (!d3.event.active) simulation.alphaTarget(0);
    d.fx = null;
    d.fy = null;
}