
var url = "http://localhost:56106";
var hamKordinatlar = null;
var indirgenmisKordinatlar = null;

var onceki_dikdortgen_ham = null;
var onceki_dikdortgen_indirgenmis = null;

$(document).ready(function() {

});


function initIndirgenmisMap(data) {

    var map = new google.maps.Map(document.getElementById('indirgenmisMap'), {
        zoom: 3,
        center: data[Math.floor( (data.length / 2) )]
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

    var drawingManagerIndirgenmis = new google.maps.drawing.DrawingManager({
        drawingMode: google.maps.drawing.OverlayType.RECTANGLE,
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: ['rectangle']
        },
        rectangleOptions: {
            strokeColor: '#38006b',
            strokeWeight: 3.5,
            fillColor: '#9c4dcc',
            fillOpacity: 0.6,
            editable: true,
            draggable: true
        }
    });
    drawingManagerIndirgenmis.setMap(map);

    paths.setMap(map);

    google.maps.event.addListener(drawingManagerIndirgenmis, 'rectanglecomplete', function (rectangle) {

        if (onceki_dikdortgen_indirgenmis != null) {
            onceki_dikdortgen_indirgenmis.setVisible(false);
        }
        onceki_dikdortgen_indirgenmis = rectangle;
        var ne = rectangle.getBounds().getNorthEast();
        var sw = rectangle.getBounds().getSouthWest();
        console.log(ne + "\n" + sw);
    });

    $("#divIndirgenmisSection").fadeIn(1000);

}

function initHamMap(uluru) {

    var map = new google.maps.Map(document.getElementById('hamMap'), {
        zoom: 3,
        center: uluru[ Math.floor( (uluru.length ) / 2)]
    });

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

    var drawingManagerHam = new google.maps.drawing.DrawingManager({
        drawingMode: google.maps.drawing.OverlayType.RECTANGLE,
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: ['rectangle']
        },
        rectangleOptions: {
            strokeColor: '#38006b',
            strokeWeight: 3.5,
            fillColor: '#9c4dcc',
            fillOpacity: 0.6,
            editable: true,
            draggable: true
        }   
    });
    drawingManagerHam.setMap(map);

    paths.setMap(map);

    google.maps.event.addListener(drawingManagerHam, 'rectanglecomplete', function (rectangle) {

        if (onceki_dikdortgen_ham != null) {
            onceki_dikdortgen_ham.setVisible(false);
        }
        onceki_dikdortgen_ham = rectangle;
        var ne = rectangle.getBounds().getNorthEast();
        var sw = rectangle.getBounds().getSouthWest();
        console.log(ne + "\n" + sw);
    });

    $("#divHamSection").fadeIn(1000);

}

function initIndirgenmisAramaMap(uluru) {

    var map = new google.maps.Map(document.getElementById('indirgenmisAramaMap'), {
        zoom: 3,
        center: uluru[Math.floor((uluru.length) / 2)]
    });

    for (var i = 0; i < uluru.length; i++) {
        var marker = new google.maps.Marker({
            position: uluru[i],
            map: map
        });
    }

    $("#indirgenmisAramaSonucu").fadeIn(1000);
}

$("#btnİndirgenmisVeriArama").click(function() {
    if (onceki_dikdortgen_indirgenmis == null) {
        alert("lütfen bir alan seçiniz");
        return;
    }
    var limit = onceki_dikdortgen_indirgenmis.getBounds().getNorthEast() +
        " " +
        onceki_dikdortgen_indirgenmis.getBounds().getSouthWest();
    $.ajax({
        method: "POST",
        url: url + "/home/AramaIndirgenmis",
        data: "= " + JSON.stringify({ 'kordinatlar': indirgenmisKordinatlar, 'limit': limit }),
        type: "json"
    }).done(function (response) {

        initIndirgenmisAramaMap(response);

    }).fail(function (response) {
        console.log("patladı");
    });
});

function initHamAramaMap(uluru) {

    var map = new google.maps.Map(document.getElementById('hamAramaMap'), {
        zoom: 3,
        center: uluru[Math.floor((uluru.length) / 2)]
    });

    for (var i = 0; i < uluru.length; i++) {
        var marker = new google.maps.Marker({
            position: uluru[i],
            map: map
        });
    }


    $("#hamAramaSonucu").fadeIn(1000);

}

$("#btnHamVeriArama").click(function() {
    if (onceki_dikdortgen_ham == null) {
        alert("lütfen bir alan seçiniz");
        return;
    }
    var limit = onceki_dikdortgen_ham.getBounds().getNorthEast() +
        " " +
        onceki_dikdortgen_ham.getBounds().getSouthWest();
    
    $.ajax({
        method: "POST",
        url: url + "/home/AramaHam",
        data: "= " + JSON.stringify({ 'kordinatlar': hamKordinatlar, 'limit': limit }),
        type: "json"
    }).done(function(response) {

        initHamAramaMap(response);

    }).fail(function(response) {
        console.log("patladı");
    });
});



$("#dosyaYukle").ajaxForm({
    beforeSend: function () {
        onceki_dikdortgen_indirgenmis = null;
        onceki_dikdortgen_ham = null;
        indirgenmisKordinatlar = null;
        hamKordinatlar = null;

        $("#fileUploadLoading").show();
    },
    complete: function (xhr) {
        $("#fileUploadLoading").hide();
        hamKordinatlar = JSON.parse(xhr.responseJSON);
        initHamMap(hamKordinatlar);
        getSimplifyedData(hamKordinatlar);
    }

});


function getSimplifyedData(coordinates) {
    $.ajax({
        method: "POST",
        url: url + "/home/DataSimplify",
        data: "=" + JSON.stringify({ coordinates,'tolerans':$("#textboxTolerans").val() }),
        type: "json"
    }).done(function (response) {
        indirgenmisKordinatlar = response;
        initIndirgenmisMap(response);
    }).fail(function() {
        console.log("patladı");
    });

}