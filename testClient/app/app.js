//js file
var app = {
    //apiBaseUrl:"http://localhost:1234/"
	
    apiBaseUrl:"http://localhost:1432/"
	//apiBaseUrl:"http://221.204.241.69:99/"
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

function quit() {
    localStorage.clear();
    location.href = "login.html";
}