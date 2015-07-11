(function () {
    angular
        .module("itemApp")
        .factory("itemService", itemService);

    itemService.$inject = ["$http", "serviceRoot"];
    
    function itemService($http, serviceRoot) {

        var urlBase = serviceRoot + "item/";
        var service = {};
        service.GetAllItems = GetAllItems;
        service.UpdateItem = UpdateItem;
        service.NewItem = NewItem;
        service.DeleteItem = DeleteItem;

        function GetAllItems() {
            return $http.get(urlBase + "list");
        };
        
        function UpdateItem(item) {
            return $http.post(urlBase + "edit", { params: { item: item } });
        }

        function NewItem(item) {
            return $http.post(urlBase + "new", { params: { item: item } });
        }
        
        function DeleteItem(item) {
            return $http.post(urlBase + "delete", { params: { item: item } });
        }

        return service;
   }
})();