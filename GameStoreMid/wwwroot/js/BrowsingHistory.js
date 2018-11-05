$(document).ready(function () {
    
    
    var pathArray = window.location.pathname.split('/');
    var productId = pathArray[pathArray.length - 1];

    
    $.ajax({
        type: "POST",
        url: "/BrowsingHistories/Create",
        
        data: { ProductID: productId },
        success: function (data) {
            
        },
        error: function (data) {
            
        }
                
    });
});