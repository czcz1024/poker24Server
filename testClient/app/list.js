//js file

function init() {
    var uid = localStorage.getItem("uid");
    if (uid == null) {
        location.href = "login.html";
        return;
    }
    fillListData();

    $("#create").click(function () {
        createTab();
    });
}

function fillListData() {
    var url = app.apiBaseUrl + "api/tabs/list";
    $.ajax(url, {
        crossDomain: true,
        type: "get",
        success: function (data) {
            $("#container").empty();
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    $("#container").append("<li onclick=\"gotab('" + data[i].Id + "')\">" + data[i].PlayerCount + "人</li>");
                }
            }
        }
    });
}

function createTab() {
    //alert("can't create");
    //return;
    var url = app.apiBaseUrl + "api/tabs/CreateTab";
    $.ajax(url, {
        crossDomain: true,
        type: "post",
        success: function (data) {
            gotab(data);
        }
    });
}

function gotab(id) {
    localStorage.setItem("tabid", id);
    location.href = "b.html";
}