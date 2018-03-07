
$(document).ready(function() {
    $.ajax({
        method: "GET",
        url: "http://localhost:56106/home/tarih"

    }).done(function(response) {
        console.log(response);
    });
});


function initIndirgenmisMap(data) {

    var map = new google.maps.Map(document.getElementById('indirgenmisMap'), {
        zoom: 13,
        center: data[0]
    });

    for (var i = 0; i < data.length; i++) {
        var marker = new google.maps.Marker({
            position: data[i],
            map: map
        });
    }

    var paths = new google.maps.Polyline({
        path: data,
        geodesic: true,
        strokeColor: '#FF0000',
        strokeOpacity: 1.0,
        strokeWeight: 2
    });

    paths.setMap(map);
}

function initHamMap(uluru) {

    var map = new google.maps.Map(document.getElementById('hamMap'), {
        zoom: 13,
        center: uluru[0]
    });
    console.log(uluru.length);
    for (var i = 0; i < uluru.length; i++) {
        var marker = new google.maps.Marker({
            position: uluru[i],
            map: map
        });
    }

    var paths = new google.maps.Polyline({
        path: uluru,
        geodesic: true,
        strokeColor: '#FF0000',
        strokeOpacity: 1.0,
        strokeWeight: 2
    });

    paths.setMap(map);

}

$("#dosyaYukle").ajaxForm({
    beforeSend: function () {
        $("#fileUploadLoading").show();
    },
    complete: function (xhr) {
        console.log(xhr);
        console.log(xhr.responseJSON);
        $("#fileUploadLoading").hide();
        initHamMap(JSON.parse(xhr.responseJSON));
        getSimplifyedData(JSON.parse(xhr.responseJSON));
    }

});


function getSimplifyedData(coordinates) {
    $.ajax({
        method: "POST",
        url: "http://localhost:56106/home/DataSimplify",
        data: "=" + JSON.stringify(coordinates),
        type: "json"
    }).done(function (response) {
        initIndirgenmisMap(response);
    }).fail(function() {
        console.log("sıçtı");
    });

}