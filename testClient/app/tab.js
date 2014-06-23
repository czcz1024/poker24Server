//js file
function init() {
    var tabid = localStorage.getItem("tabid");
    var uid = localStorage.getItem("uid");
    app.tab = tabid;
    app.uid = uid;
    listentab(tabid, uid);
}

function listentab(id, uid) {
    $.connection.hub.qs = { "tab": id, "uid": uid };

    app.proxy = $.connection.tabHub;
    $.connection.hub.error(function (error) {
        console.log('SignalR error: ' + error);
    });
    $.connection.hub.url = "http://192.168.1.103:1432/signalr";

    var client = $.connection.tabHub.client;
    setClientFunc(client);
    $.connection.hub.start();
}

function setClientFunc(client) {
    client.test = function (txt) {
        console.log("test " + txt);
    }

    client.refreshUsers = function (seats) {
        console.log(seats);
        refreshSeats(seats);
    }
}

function refreshSeats(seats) {
    $("#seats").empty();
    for (var i = 0; i < seats.length; i++) {
        var li = $("<li />");
        var seat = seats[i];
        if (seat.HasUser) {
            li.append(seat.UserName);
            if (seat.IsOK) {
                li.append("[ok]");
            } else {
                if (seat.UserId == app.uid) {
                    li.append("[<button onclick='setOk()'>ok</button>]");
                }
            }
        } else {
            li.append("empty");
        }
        $("#seats").append(li);
    }
}

function setOk() {
    app.proxy.server.ready(app.tab, app.uid);
}