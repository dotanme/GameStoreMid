$(document).ready(totalIt());


function totalIt() {
    var curTotal = document.getElementsByName("selectedProducts");
    var orgCost = document.getElementsByName("originalCost");
    var total = 0;
    var subTotal = 0;
    var discount = 0;

    for (var i = 0; i < curTotal.length; i++) {
        total += parseFloat(curTotal[i].getAttribute("value"));
        subTotal += parseFloat(orgCost[i].getAttribute("value"));
    }
    subTotal = Math.abs(subTotal - total);
    discount = (subTotal / total)*100;

    document.getElementById("grandTotal").textContent = total.toFixed(2).toString() + "$";
    document.getElementById("disAmount").textContent = subTotal.toString() + "$";
    document.getElementById("disPrsnt").textContent = discount.toString() +"%";
};
