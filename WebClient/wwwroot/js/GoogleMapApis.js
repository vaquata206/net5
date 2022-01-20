jQuery.loadScript = function (callback) {
    jQuery.ajax({
        url: 'https://maps.googleapis.com/maps/api/js?key=AIzaSyAQwqoWk5OgbemXQ-dkh1iwgKrki0rVsbo&callback=initMap&libraries=drawing&v=weekly',
        dataType: 'script',
        success: callback,
        async: true
    });
};

function initMap() {}

function createMapTypeControl(map) {
    const mapTypeControl = document.createElement("div");
    mapTypeControl.classList.add("row");
    mapTypeControl.style.marginLeft = "8px";

    var select = document.createElement("select");
    select.name = "mapTypes";
    select.id = "mapTypes";
    select.classList.add("form-control");
    select.style.borderRadius = "2px";
    select.style.boxShadow = "brgb(0 0 0 / 30%) 0px 1px 4px -1px";
    select.style.cursor = "pointer";
    select.style.marginTop = "8px";
    select.style.marginBottom = "8px";
    select.style.fontFamily = "Roboto,Arial,sans-serif";
    select.style.fontSize = "18px";
    select.style.paddingTop = "8px";
    select.style.paddingBottom = "8px";
    select.style.paddingLeft = "15px";
    select.style.paddingRight = "15px";

    select.options[0] = new Option('Bản đồ', 'roadmap');
    select.options[1] = new Option('Địa hình', 'terrain');
    select.options[2] = new Option('Vệ tinh', 'satellite');
    select.options[3] = new Option('Nhãn', 'hybrid');

    select.addEventListener("change", () => {
        map.setMapTypeId(document.getElementById("mapTypes").value);
    });

    mapTypeControl.appendChild(select);

    map.controls[google.maps.ControlPosition.TOP_LEFT].push(mapTypeControl);
}

function getDistance(start, end) {
    const origin = start;
    const final = end;
    const service = new google.maps.DistanceMatrixService();

    return new Promise((resolve, reject) => {
        service.getDistanceMatrix(
            {
                origins: [origin],
                destinations: [final],
                travelMode: 'DRIVING'
            }, (response, status) => {
                if (status === 'OK' && response.rows[0].elements[0].status !== "ZERO_RESULTS") {
                    resolve({ distance: response.rows[0].elements[0].distance.text });
                } else {
                    resolve(null);
                }
            }
        );
    });
}

function getAllDistanceToMultiplace(start, ends) {
    const promisedDistances = ends.map((end) => getDistance(start, end));
    return Promise.all(promisedDistances);
}

function getAllDistanceFromMultiplace(starts, end) {
    const promisedDistances = starts.map((start) => getDistance(start, end));
    return Promise.all(promisedDistances);
}

function getMapGG(lat, lng) {
    var map = new google.maps.Map(document.getElementById("map"), {
        center: new google.maps.LatLng(lat, lng),
        mapTypeId: 'roadmap',
        zoom: 10,
        mapTypeControlOptions: {
            position: google.maps.ControlPosition.BOTTOM_LEFT
        },
    });
    //createMapTypeControl(map);
    return map;
}

function getMapGGCustom(divMap, lat, lng) {
    var map = new google.maps.Map(divMap, {
        center: new google.maps.LatLng(lat, lng),
        mapTypeId: 'roadmap',
        zoom: 10,
    });
    //createMapTypeControl(map);
    return map;
}

function getInfoWindow(lat, lng) {
    return new google.maps.InfoWindow({
        content: "Chọn tọa độ trên bản đổ!",
        position: new google.maps.LatLng(lat, lng),
        maxWidth: 320,
        maxHeight: 400,
    });
}

function KhoiTaoDiemTrenGG(dsDiaDiem, map, InforObj, markers, maskMarkerName = '') {
    if (dsDiaDiem.length === 0) return;

    // Tạo markers.
    for (let i = 0; i < dsDiaDiem.length; i++) {
        const infowindow = new google.maps.InfoWindow({
            content: dsDiaDiem[i].htmlMarker,
            maxWidth: 320,
            maxHeight: 400,
        });

        if (dsDiaDiem[i].viDo && dsDiaDiem[i].kinhDo) {
            let marker;
            if (dsDiaDiem[i].ten === maskMarkerName) {
                marker = new google.maps.Marker({
                    position: new google.maps.LatLng(dsDiaDiem[i].viDo, dsDiaDiem[i].kinhDo),
                    icon: "/images/Chitiet.png",
                    title: dsDiaDiem[i].ten,
                    map: map,
                });
                closeOtherInfo(InforObj);
                infowindow.open(marker.get('map'), marker);
                InforObj[0] = infowindow;
            }
            else {
                marker = new google.maps.Marker({
                    position: new google.maps.LatLng(dsDiaDiem[i].viDo, dsDiaDiem[i].kinhDo),
                    icon: "/images/Chitiet.png",
                    title: dsDiaDiem[i].ten,
                    map: map,
                });
            }
            marker.addListener('click', function () {
                closeOtherInfo(InforObj);
                map.setCenter(marker.getPosition());
                map.setZoom(17);
                infowindow.open(marker.get('map'), marker);
                InforObj[0] = infowindow;
            });
            markers.push(marker);
        }
    }
}

function KhoiTaoDiemNoneData(dsDiaDiem, map, markers) {
    if (dsDiaDiem.length === 0) return;

    // Tạo markers.
    for (let i = 0; i < dsDiaDiem.length; i++) {
        if (dsDiaDiem[i].viDo && dsDiaDiem[i].kinhDo) {
            let marker;
            marker = new google.maps.Marker({
                position: new google.maps.LatLng(dsDiaDiem[i].viDo, dsDiaDiem[i].kinhDo),
                icon: "/images/Chitiet.png",
                title: dsDiaDiem[i].ten,
                ma: dsDiaDiem[i].ma,
                map: map,
            });
            var info = {
                id: dsDiaDiem[i].id, loai: dsDiaDiem[i].loai
            }
            marker.info = info;
            markers.push(marker);
        }
    }
}

function veDienTich(map) {
    const drawingManager = new google.maps.drawing.DrawingManager({
        drawingMode: google.maps.drawing.OverlayType.MARKER,
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [
                google.maps.drawing.OverlayType.CIRCLE,
                google.maps.drawing.OverlayType.POLYGON,
                google.maps.drawing.OverlayType.POLYLINE,
                google.maps.drawing.OverlayType.RECTANGLE,
            ],
        },
        markerOptions: {
            icon: "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png",
        },
        polylineOptions: {
            fillColor: "#ffff00",
            fillOpacity: 1,
            strokeWeight: 5,
            clickable: false,
            editable: true,
            zIndex: 1,
        },
        circleOptions: {
            fillColor: "#ffff00",
            fillOpacity: 1,
            strokeWeight: 5,
            clickable: false,
            editable: true,
            zIndex: 1,
        },
    });

    drawingManager.setMap(map);
}

function veDienTich2(map) {
    const drawingManager = new google.maps.drawing.DrawingManager({
        drawingMode: google.maps.drawing.OverlayType.MARKER,
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [
                google.maps.drawing.OverlayType.CIRCLE,
                google.maps.drawing.OverlayType.POLYGON,
                google.maps.drawing.OverlayType.POLYLINE,
                google.maps.drawing.OverlayType.RECTANGLE,
            ],
        },
        markerOptions: {
            icon: "https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png",
        },
        polylineOptions: {
            fillColor: "#ffff00",
            fillOpacity: 1,
            strokeWeight: 5,
            clickable: false,
            editable: true,
            zIndex: 1,
        },
        circleOptions: {
            fillColor: "#000000",
            fillOpacity: 1,
            strokeWeight: 5,
            clickable: false,
            editable: true,
            zIndex: 1,
        },
    });

    drawingManager.setMap(map);
}

function closeOtherInfo(InforObj) {
    if (InforObj.length > 0) {
        InforObj[0].set("marker", null);
        InforObj[0].close();
        InforObj.length = 0;
    }
}

function createTriggerClickGG(funcClick) {
    new google.maps.event.trigger(funcClick, 'click');
}

// Removes the markers from the map, but keeps them in the array.
function clearAllMarkers(markers) {
    setMapOnAll(null, markers);
    markers.length = 0;
}

// Sets the map on all markers in the array.
function setMapOnAll(map, markers) {
    for (let i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}