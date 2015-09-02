(function () {
    // get the location of your javascript file during run time 
    // using jQuery by parsing the DOM for the 'src' attribute that referred it
    var jsFileLocation = $('script[src*="BBAngular/js/app"]').attr('src');  // the js file path
    jsFileLocation = jsFileLocation.replace('app.js', '');   // the js folder path
    if (jsFileLocation.indexOf('?') > -1) {
        jsFileLocation = jsFileLocation.substr(0, jsFileLocation.indexOf('?'));
    }
    angular
        .module("itemApp", ["ngRoute","ngDialog","ngProgress","ui.sortable"])
        .config(function ($routeProvider, $locationProvider) {
            $routeProvider
            .otherwise({
                templateUrl: jsFileLocation + "index.html",
                controller: "itemController",
                controllerAs: "vm"
            });
        });
})();