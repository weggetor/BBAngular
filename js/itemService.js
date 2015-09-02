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
        service.Reorder = Reorder;
        service.GetCategories = GetCategories;

        function GetAllItems() {
            return $http.get(urlBase + "list");
        };
        
        function UpdateItem(item) {
            return $http.post(urlBase + "edit",item);
        }

        function NewItem(item) {
            return $http.post(urlBase + "new", item );
        }
        
        function DeleteItem(item) {
            return $http.post(urlBase + "delete", item );
        }
        function Reorder(sortItems) {
            return $http.post(urlBase + "reorder", sortItems );
        }
        function GetCategories() {
            var req = {
                method: 'GET',
                url: "http://dnnfaq.local/Desktopmodules/faqs/API/service/GetCategories",
                headers: {
                    'TabId': undefined,
                    'ModuleId': undefined,
                    'RequestVerificationToken': undefined
                },
                params: { moduleId: 412, onlyUsedCategories: false }
            };
            return $http(req);
        }

        return service;
   }
})();