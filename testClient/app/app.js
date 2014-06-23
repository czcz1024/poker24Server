//js file
var app = {
    apiBaseUrl:"http://192.168.1.103:1432/"
};

$(function () {

    var server = localStorage.getItem("server");
    if (server != null) {
        app.apiBaseUrl = server;
    } else {
        localStorage.setItem("server", app.apiBaseUrl);
    }

    if (typeof (init) == "function") {
        init();
    }
});