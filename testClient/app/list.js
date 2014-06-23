//js file

function init() {
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
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    $("#container").append("<li onclick=\"gotab('"+data[i].Id+"')\">"+data[i].Id+"</li>");
                }
            }
        }
    });
}

function createTab() {
    alert("can't create");
    return;
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
    location.href = "tab.html";
}