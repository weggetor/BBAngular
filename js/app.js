(function () {
    var moduleFolder = "/DesktopModules/BBAngular/";
    angular
        .module("itemApp", ["ngRoute","ngDialog","ngProgress","ui.sortable"])
        .config(function ($routeProvider, $locationProvider) {
            $routeProvider
            .otherwise({
                templateUrl: moduleFolder + "js/index.html",
                controller: "itemController",
                controllerAs: "vm"
            });
        });
})();