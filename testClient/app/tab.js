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
    $.connection.hub.url = app.apiBaseUrl+"signalr";

    var client = $.connection.tabHub.client;
    setClientFunc(client);
    $.connection.hub.start();
}

function setClientFunc(client) {
    client.test = function (txt) {
        console.log("test " + txt);
    }

    client.refreshUsers = function (seats) {
        
        refreshSeats(seats);
    }

    client.refreshGame = function (game) {
        refreshGame(game);
    }
}

function refreshSeats(seats) {
    $("#seats").empty();
    for (var i = 0; i < seats.length; i++) {
        var li = $("<li />");
        li.attr("v", seats[i].UserId);
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

function refreshGame(game) {
    for (var i = 0; i < game.Seats.length; i++) {
        var seat = game.Seats[i];
        if (seat.UserId == app.uid) {
            refreshHand(seat.InHand);
        }
    }

    app.lastHand = game.LastHand;
    app.lastPusher = game.LastHandUser;
    console.log(game.LastHandUser);
    app.nowPusher = game.NowUser;

    refreshState();
}

function refreshState() {
    if (app.lastHand) {
        var con = $("#last");
        con.empty();
        for (var i = 0; i < app.lastHand.length; i++) {
            var li = $("<li />");
            li.append(app.lastHand[i].Text);
            con.append(li);
        }
    }

    $("#seats li").removeClass("nowTurn");
    $("#seats li[v=" + app.lastPusher + "]").addClass("nowTurn");
}

function refreshHand(hands) {
    var h = $("#hands");
    h.empty();
    for (var i = 0; i < hands.length; i++) {
        var li = $("<li />");
        li.click(function () {
            $(this).toggleClass("will");
        });
        li.attr("v", hands[i].Value);
        li.append(hands[i].Text);
        h.append(li);
    }
}

function checkPush() {
    var will = $("#hands .will");
    var x=will.map(function () {
        return $(this).attr("v");
    }).get().join(",");

    app.proxy.server.poke(app.tab,app.uid,x);
}