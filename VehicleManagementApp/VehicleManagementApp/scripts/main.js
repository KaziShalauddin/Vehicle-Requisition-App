require.config({
    // relative url from where modules will load
    baseUrl: "Scripts/",
    paths: {
        "jquery": "jquery-2.2.4"
    }
});

require(["app/app"], function (app) {
    app.init();
});