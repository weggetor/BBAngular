(function () {
    angular
        .module("itemApp")
        .controller("itemController", itemController);

    itemController.$inject = ["$scope", "$window", "$log", "ngDialog", "ngProgress", "itemService"];
    
    function itemController($scope, $window, $log, ngDialog, ngProgress, itemService) {

        var vm = this;
        vm.Items = [];
        vm.AddEditTitle = "";
        vm.EditIndex = -1;
        vm.UserList = app.UserList;
        vm.localize = app.Resources;
        vm.Item = {};

        vm.getAll = getAll;
        vm.createUpdateItem = createUpdateItem;
        vm.deleteItem = deleteItem;
        vm.showAdd = showAdd;
        vm.showEdit = showEdit;
        vm.reset = resetItem;
        vm.initUser = initUser;
        vm.changeUser = changeUser;

        resetItem();
        vm.getAll();

        function getAll() {
            ngProgress.color('red');
            ngProgress.start();
            itemService.GetAllItems()
                .then(function(response) {
                    vm.Items = response.data;
                    ngProgress.complete();
                })
                .catch(function(errData) {
                    $log.error('failure loading items', errData.data);
                    ngProgress.complete();
                });
        };

        function createUpdateItem(form) {
            $log.info("Save button clicked! Form is valid:", !form.$invalid);
            vm.invalidSubmitAttempt = false;
            if (form.$invalid) {
                vm.invalidSubmitAttempt = true;
                return;
            }

            if (vm.Item.ItemId > 0) {
                itemService.UpdateItem(angular.toJson(vm.Item))
                    .then(function(response) {
                        $log.info("Edit item sended:", vm.Item);
                        $log.info("Edititem received", response.data);
                        if (vm.EditIndex >= 0) {
                            vm.Items[vm.EditIndex] = vm.Item;
                        }
                    })
                    .catch(function(errData) {
                        $log.error('failure saving item', errData.data);
                    });
            } else {
                itemService.NewItem(angular.toJson(vm.Item))
                    .success(function (response) {
                        $log.info("New item sended:", vm.Item);
                        $log.info("New item received", response.data);
                        if (response.data.ItemId > 0) {
                            vm.Items.push(response.data);
                        }
                    })
                    .error(function (errData) {
                        $log.error('failure saving new item', errData.data);
                    });
            }
            ngDialog.close();
        };

        function deleteItem(item, idx) {
            if (confirm('Are you sure to delete "' + item.ItemName + '"?')) {
                itemService.DeleteItem(angular.toJson(vm.Item))
                    .success(function (response) {
                        vm.Items.splice(idx, 1);
                    })
                    .error(function (errData) {
                        $log.error('failure deleting item', errData.data);
                    });
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
})();
