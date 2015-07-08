angular.module("itemApp")
    .controller("ItemController", ItemController);

function ItemController($scope, $window, $log, ngDialog, ngProgress, dnnServiceClient) {

    /*
    * ITEM CONTROLLER
    * Parameters like $window and $log are injected through the Dependency Injection of angular
    *
    * $window: A jQuery (lite)-wrapped reference to window
    * $log: Simple service for logging. Default implementation safely writes the message into the browser's console (if present).
    * ngDialog: Modals and popups provider for Angular.js applications
    * dnnServiceClient: Our custom dnnService framework client factory 
    */
    
    var vm = this;
    vm.Items = [];
    vm.AddEditTitle = "";
    vm.EditIndex = -1;
    vm.UserList = app.UserList;
    vm.localize = app.Resources;
    vm.Item = {};

    vm.init = init;
    vm.getAll = getAll;
    vm.createUpdateItem = createUpdateItem;
    vm.deleteItem = deleteItem;
    vm.showAdd = showAdd;
    vm.showEdit = showEdit;
    vm.reset = resetItem;
    vm.initUser = initUser;
    vm.changeUser = changeUser;

    resetItem();

    function init(moduleId, moduleName, editable) {
        vm.ModuleId = moduleId;
        vm.EditMode = editable;
        dnnServiceClient.init(moduleId, moduleName);
        vm.getAll();
    };

    function getAll() {
        ngProgress.color('red');
        ngProgress.start();
        var promiseResp = dnnServiceClient.callGetService("item/list");
        promiseResp.then(
            function (payload) {
                vm.Items = payload.data;
                ngProgress.complete();
            },
            function (errorPayload) {
                $log.error('failure loading items', errorPayload);
                ngProgress.complete();
            }
        );
    };

    function createUpdateItem(form) {
        $log.info("Save button clicked! Form is valid:", !form.$invalid);
        vm.invalidSubmitAttempt = false;
        if (form.$invalid) {
            vm.invalidSubmitAttempt = true;
            return;
        }
        var sMethod = null;

        if (vm.Item.ItemId > 0) {
            sMethod = "item/edit";
        } else {
            sMethod = "item/new";
        }

        var promiseResp = dnnServiceClient.callPostService(sMethod, angular.toJson(vm.Item));
        promiseResp.then(
            function (payload) {
                $log.info("New item sended:", vm.Item);
                $log.info("New item received", payload.data);
                if (sMethod === "item/new") {
                    if (payload.data.ItemId > 0) {
                        vm.Items.push(payload.data);
                    }
                } else {
                    if (vm.EditIndex >= 0)
                        vm.Items[vm.EditIndex] = vm.Item;
                }
            },
            function (errorPayload) {
                $log.error('failure saving item', errorPayload);
            }
        );
        ngDialog.close();
    };

    function deleteItem(item, idx) {
        if (confirm('Are you sure to delete "' + item.ItemName + '"?')) {
            var promiseResp = dnnServiceClient.callPostService("item/delete", angular.toJson(item));
            promiseResp.then(
                function (payload) {
                    vm.Items.splice(idx, 1);
                },
                function (errorPayload) {
                    $log.error('failure deleting item', errorPayload);
                }
            );
        }
    };

    //function showAdd() {
    //    vm.reset();
    //    vm.AddEditTitle = "Add Item";
    //    ngDialog.open({
    //        template: 'desktopmodules/BBAngular/js/itemForm.html',
    //        className: 'ngdialog-theme-default',
    //        scope: $scope
    //    });
    //};
    
    function showAdd() {
        vm.reset();
        vm.AddEditTitle = "Add Item";
        ngDialog.open({
            template: 'pnlAddEditItem',
            className: 'ngdialog-theme-default',
            scope: $scope
        });
    };

    function showEdit(item, idx) {
        vm.Item = angular.copy(item);
        vm.EditIndex = idx;
        vm.AddEditTitle = "Edit Item: #" + item.ItemId;
        ngDialog.open({
            template: 'pnlAddEditItem',
            className: 'ngdialog-theme-default',
            scope: $scope
        });
    };

    function resetItem() {
        vm.Item = {
            ItemId: 0,
            ModuleId: vm.ModuleId,
            ItemName: '',
            ItemDescription: '',
            AssignedUserId: ''
        };
    };

    function initUser() {
        vm.Item.AssignedUserSel = vm.Item.AssignedUserId.toString();
    };

    function changeUser() {
        vm.Item.AssignedUserId = parseInt(vm.Item.AssignedUserSel.id);
    };

};