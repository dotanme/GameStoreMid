$(document).ready(function () { 
    //get addresses
    $.get("/ClientOrders/MostRecentOrders", {}, function (data) {
        data.forEach(function (x) {
            codeAddress(x);
        });
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
});

// Note: This example requires that you consent to location sharing when
// prompted by your browser. If you see the error "The Geolocation service
// failed.", it means you probably did not give permission for the browser to
// locate you.
var map, infoWindow;
var geocoder;


function initMap() {
    geocoder = new google.maps.Geocoder();
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 31.9730, lng: 34.7925 }, 
        zoom: 9
    });
    infoWindow = new google.maps.InfoWindow;

    // Try HTML5 geolocation.
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            map.setCenter(pos);
        }, function () {
            handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, infoWindow, map.getCenter());
    }
}

function codeAddress(address) {

    // Define address to center map to
    geocoder.geocode(address, function (results, status) {

        if (status == google.maps.GeocoderStatus.OK) {

            // Center map on location
            map.setCenter(results[0].geometry.location);

            // Add marker on location
            var marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location
            });

        } else {

            console.log("Geocode was not successful for the following reason: " + status);
        }
    });
}
function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(browserHasGeolocation ?
        'Error: The Geolocation service failed.' :
        'Error: Your browser doesn\'t support geolocation.');
    infoWindow.open(map);
}