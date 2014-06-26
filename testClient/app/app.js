//js file
var app = {
    //apiBaseUrl:"http://localhost:1234/"
    apiBaseUrl:"http://localhost:1432/"
};

$(function () {
    //document.domain = app.apiBaseUrl;
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