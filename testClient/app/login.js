//js file
function init() {

    var uid = localStorage.getItem("uid");
    if (uid != null) {
        goTabList();
    }

    $("#login").click(function () {
        var uname = $("#loginName").val();
        var pass = $("#loginPass").val();
        checkLogin(uname, pass);
    });

    $("#reg").click(function () {
        var uname = $("#regName").val();
        var pass = $("#regPass").val();

        if (checkReg(uname, pass)) {
            goReg(uname, pass);
        }
    });
}

function checkLogin(uname, pass) {
    var url = app.apiBaseUrl + "api/user/login";
    $.ajax(url, {
        type: "post",
        crossDomain: true,
        data: {
            Username: uname,
            Password:pass
        },
        success: function (data) {
            if (data == "00000000-0000-0000-0000-000000000000") {
                alert("错误");
            } else {
                localStorage.setItem("uid", data);
                goTabList();
            }
        }
    });
}

function checkReg(uname, pass) {
    return uname != "" && pass != "";
}

function goReg(uname, pass) {
    var url = app.apiBaseUrl + "api/user/register";
    $.ajax(url, {
        type: "post",
        crossDomain:true,
        data: {
            UserName: uname,
            Password: pass,
            NickName:uname
        },
        success: function (data) {
            if (data == "00000000-0000-0000-0000-000000000000") {
                alert("尚未开放");
            } else {
                localStorage.setItem("uid", data);
                //goTabList();
            }
        }
    });
}

function goTabList() {
    location.href = "list.html";
}