// Write your JavaScript code.
$("tr[data-link]").click(function (e) {
    if (e.toElement.tagName == 'SPAN') {
        var path = $(e.toElement.parentElement).attr("formaction");
        if (path != undefined)
            window.location = path;
    } else
        if (e.toElement.tagName == 'BUTTON') {
            var path = $(e.toElement).attr("formaction");
            if (path != undefined)
                window.location = path;
        }
        else {
            window.location = this.dataset.link;
        }
});

function textValidate(min, max, textarea, charLeftlabel = $('.characterLeft'), btnSubmit = $('input.btnSubmit')) {
    var len = textarea.textLength;
    charLeftlabel.text('Must be at least ' + min + ' and at max ' + max + ' characters long.');
    charLeftlabel.css("color", "red");
    btnSubmit.prop("disabled", true);
    if ((min <= len) && (len <= max)) {
        var ch = max - len;
        $('.characterLeft').text(ch + ' characters left');
        $('.characterLeft').css('color', "");
        btnSubmit.prop("disabled", false);
    }
    else {
        charLeftlabel.text('Must be at least ' + min + ' and at max ' + max + ' characters long.');
        charLeftlabel.css("color", "red");
        btnSubmit.prop("disabled", true);
    }
};

function standby() {
    document.getElementsById('Pic').src = 'https://www.winbirri.com/wp-content/uploads/2017/11/product-image-placeholder.jpg'
}
function addToCart(pid) {
    var list = cookieList("product");
    list.add(pid);
    $.get("/Cart/CheckCookie", {}, function (data) {
        data.forEach(function (e, i, t) {
            list.remove(e);
        });
        var cartURI = document.URL.split("Products")[0].concat("Cart/Index");
        window.location = cartURI;
    }, "json");
}  