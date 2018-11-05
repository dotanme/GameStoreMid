
$.get("/ProductTags/getJson10MostUsedTags", {}, function (data) {
    start(data, "#mostusedtags");
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
function start(data, id) {
    var margin = { top: 40, right: 20, bottom: 30, left: 40 },
        width = 960 - margin.left - margin.right,
        height = 500 - margin.top - margin.bottom;


    var x = d3.scale.ordinal()
        .rangeRoundBands([0, width], .1);

    var y = d3.scale.linear()
        .range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left");

    var tip = d3.tip()
        .attr('class', 'd3-tip')
        .offset([-10, 0])
        .html(function (d) {
            return "<strong>Games:</strong> <span style='color:red'>" + d.count + "</span>";
        })

    var svg = d3.select(id).append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    svg.call(tip);

    x.domain(data.map(function (d) { return d.name; }));
    y.domain([0, d3.max(data, function (d) { return d.count; })]);

    svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + height + ")")
        .call(xAxis).selectAll(".tick text")
        .call(wrap, x.rangeBand());

    var array = [];
    $("svg g.x.axis g.tick").each(function (i, e) { array.push(e.getBoundingClientRect().height); });
    var labelheight = Math.max(...array);
    $("div#mostusedtags svg").attr("height", height + (labelheight * 2)).css({
        "display": "block",
        "margin": "0 auto"
        
    });

    svg.append("g")
        .attr("class", "y axis")
        .call(yAxis)
        .append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 6)
        .attr("dy", ".71em")
        .style("text-anchor", "end")
        .text("Count");

    svg.selectAll(".bar")
        .data(data)
        .enter().append("rect")
        .attr("class", "bar")
        .attr("x", function (d) { return x(d.name); })
        .attr("width", x.rangeBand())
        .attr("y", function (d) { return y(d.count); })
        .attr("height", function (d) { return height - y(d.count); })
        .on('mouseover', tip.show)
        .on('mouseout', tip.hide);

};

function wrap(text, width) {
    text.each(function () {
        var text = d3.select(this),
            words = text.text().split(/\s+/).reverse(),
            word,
            line = [],
            lineNumber = 0,
            lineHeight = 1.1, // ems
            y = text.attr("y"),
            dy = parseFloat(text.attr("dy")),
            tspan = text.text(null).append("tspan").attr("x", 0).attr("y", y).attr("dy", dy + "em");
        while (word = words.pop()) {
            line.push(word);
            tspan.text(line.join(" "));
            if (tspan.node().getComputedTextLength() > width) {
                line.pop();
                tspan.text(line.join(" "));
                line = [word];
                tspan = text.append("tspan").attr("x", 0).attr("y", y).attr("dy", ++lineNumber * lineHeight + dy + "em").text(word);
            }
        }
    });
}