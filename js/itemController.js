(function () {
    angular
        .module("itemApp")
        .controller("itemController", itemController);

    itemController.$inject = ["$scope", "$window", "$log", "ngDialog", "ngProgress", "itemService", "userlist", "resources", "settings", "editable","moduleId"];
    
    function itemController($scope, $window, $log, ngDialog, ngProgress, itemService, userlist, resources, settings, editable, moduleId) {

        var vm = this;
        vm.Items = [];
        vm.AddEditTitle = "";
        vm.EditIndex = -1;
        vm.UserList = JSON.parse(userlist);
        vm.localize = JSON.parse(resources);
        vm.settings = JSON.parse(settings);
        vm.EditMode = (editable.toLowerCase() === "true");
        vm.ModuleId = parseInt(moduleId);
        vm.Item = {};

        vm.getAll = getAll;
        vm.createUpdateItem = createUpdateItem;
        vm.deleteItem = deleteItem;
        vm.showAdd = showAdd;
        vm.showEdit = showEdit;
        vm.reset = resetItem;
        vm.initUser = initUser;
        vm.changeUser = changeUser;
        vm.sortableOptions = { stop: sortStop, disabled: !vm.EditMode  };

        resetItem();
        vm.getAll();
 
        function getAll() {
            ngProgress.color('red');
            ngProgress.start();
            itemService.GetAllItems()
                .success(function(response) {
                    vm.Items = response;
                    ngProgress.complete();
                })
                .error(function(errData) {
                    $log.error('failure loading items', errData);
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
                itemService.UpdateItem(vm.Item)
                    .success(function(response) {
                        $log.info("Edit item sended:", vm.Item);
                        $log.info("Edititem received", response);
                        if (vm.EditIndex >= 0) {
                            vm.Items[vm.EditIndex] = vm.Item;
                        }
                    })
                    .catch(function(errData) {
                        $log.error('failure saving item', errData);
                    });
            } else {
                itemService.NewItem(vm.Item)
                    .success(function (response) {
                        $log.info("New item sended:", vm.Item);
                        $log.info("New item received", response);
                        if (response.ItemId > 0) {
                            vm.Items.push(response);
                        }
                    })
                    .error(function (errData) {
                        $log.error('failure saving new item', errData);
                    });
            }
            ngDialog.close();
        };

        function deleteItem(item, idx) {
            if (confirm('Are you sure to delete "' + item.ItemName + '"?')) {
                itemService.DeleteItem(item)
                    .success(function (response) {
                        vm.Items.splice(idx, 1);
                    })
                    .error(function (errData) {
                        $log.error('failure deleting item', errData);
                    });
            }
        };

        function showAdd() {
            vm.reset();
            vm.AddEditTitle = "Add Item";
            ngDialog.open({
                template: '/desktopmodules/BBAngular/js/itemForm.html',
                className: 'ngdialog-theme-default',
                scope: $scope
            });
        };

        function showEdit(item, idx) {
            vm.Item = angular.copy(item);
            vm.EditIndex = idx;
            vm.AddEditTitle = "Edit Item: #" + item.ItemId;
            ngDialog.open({
                template: '/desktopmodules/BBAngular/js/itemForm.html',
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
        
        function sortStop(e, ui) {

            var sortItems = [];
            for (var index in vm.Items) {
                if (vm.Items[index].ItemId) {
                    var sortItem = { ItemId: vm.Items[index].ItemId, Sort: index };
                    vm.Items[index].Sort = index;
                    sortItems.push(sortItem);
                    // $log.info(vm.Items[index].ItemName + "(" +vm.Items[index].ItemId + ") = " + index);
                }
            }       
            itemService.Reorder(angular.toJson(sortItems))
                .catch(function(errData) {
                    $log.error('failure reordering items', errData.data);
                });
        }
    };
})();
