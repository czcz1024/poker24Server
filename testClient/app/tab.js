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
    var arr=will.map(function () {
        return $(this).attr("v");
    }).get();

    var last = app.lastHand.map(function (item) { return item.Value });
    var x = arr.join(",");
    if (allow(arr) && biger(last,arr)) {
        app.proxy.server.poke(app.tab, app.uid, x);
    }
}

///////////////////////////////////////////////////////////////
function allow(cards) {
    if (cards.length == 1) return true;

    var allsame = allSame(cards);
    if (allsame) return true;

    return isSeq(cards);
}

function allSame(cards) {
    var first = cards[0];
    var allsame = true;
    for (var i = 0; i < cards.length; i++) {
        if (cards[i] != first) {
            allsame = false;
            break;
        }
    }
    return allsame;
}

function isPair(cards) {
    return cards.length == 2 && allSame(cards);
}

function isSingle(cards) {
    return cards.length == 1;
}

function isBoom(cards) {
    return cards.length > 2 && allSame(cards);
}

function isSeq(cards) {
    if (cards.length < 3) return false;
    fix2(cards);
    fixA(cards);
    fixW(cards);
    var cntA = hasCount(cards, 15);
    var cntXw = hasCount(cards, 21);
    var cntDw = hasCount(cards, 22);
    if (cntA > 0 || cntXw || cntDw)return false;
    var sort = Sort(cards);
    //3-13==2-13
    // A==15 2==16
    // DW=22 XW==21
    var cha = getCha(sort);
    
    return allSame(cha);
}

function fix2(cards) {
    for (var i = 0; i < cards.length; i++) {
        if (cards[i] == 16) {
            cards[i] = 2;
        }
    }
}

function fixA(cards) {
    var cntA = hasCount(cards, 15);
    if (cntA < 1) return;
    var cnt2 = hasCount(cards, 2);
    var cntK = hasCount(cards, 13);
    if (cnt2 > 0) {
        for (var i = 0; i < cards.length; i++) {
            if (cards[i] == 15) {
                cards[i] = 1;
                break;
            }
        }
    }
    if (cntK > 0) {
        for (var i = 0; i < cards.length; i++) {
            if (cards[i] == 15) {
                cards[i] = 14;
                break;
            }
        }
    }
}

function fixW(cards) {

}

function hasCount(cards, num) {
    var cnt= 0;
    for (var i = 0; i < cards.length; i++) {
        if (cards[i] == num) {
            cnt++;
        }
    }
    return cnt;
}

function getCha(cards) {
    var cha = [];
    for (var i = 0; i < cards.length - 1; i++) {
        var c = cards[i + 1] - cards[i];
        cha.push(c);
    }
    return cha;
}

function Sort(cards) {
    return cards.sort(function (a, b) {
        return a - b;  
    });  ;
}

function biger(last, cards) {
    if (last == null) return true;

    //last boom
    if (isBoom(last)) {
        if (isPair(cards)) {
            if (cards[0] == 4) return true;
        }
        if (isBoom(cards)) {
            if (cards.length > last.length) return true;
            if (cards[0] > last[0]) return true;
        }
    }

    //last pair
    if (isPair(last)) {
        if (isBoom(cards)) return true;
        if (isPair(cards)) {
            if (cards[0] > last[0]) return true;
        }
    }
    //last single
    if (isSingle(last)) {
        if (isBoom(cards)) return true;
        if (isSingle(cards)) {
            if (cards[0] > last[0]) return true;
        }
    }
    //last seq
    if (isSeq(last)) {
        if (isBoom(cards)) return true;
        if (isSeq(cards)) {
            if (last.length == cards.length) {
                if (cards[0] > last[0]) return true;
            }
        }
    }

    return false;
}