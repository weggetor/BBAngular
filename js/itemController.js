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
        vm.Categories = [];

        vm.getAll = getAll;
        vm.createUpdateItem = createUpdateItem;
        vm.deleteItem = deleteItem;
        vm.showAdd = showAdd;
        vm.showEdit = showEdit;
        vm.reset = resetItem;
        vm.initUser = initUser;
        vm.changeUser = changeUser;
        vm.sortableOptions = { stop: sortStop, disabled: !vm.EditMode };
        vm.getCategories = getCategories;

        resetItem();
        vm.getCategories();
        vm.getAll();
 
        function makeTree(options) {
            var sorted, children, temp, cfi, e, i, id, o, pid, rfi, ri, thisid, _i, _j,_k, _len, _len1,_len2, _ref, _ref1, _ref2;
            id = options.id || "id";
            pid = options.parentid || "parentid";
            children = options.children || "children";
            ri = [];
            rfi = {};
            cfi = {};
            o = [];
            _ref = options.q;
            for (i = _i = 0, _len = _ref.length; _i < _len; i = ++_i) {
                e = _ref[i];
                rfi[e[id]] = i;
                if (cfi[e[pid]] == null) {
                    cfi[e[pid]] = [];
                }
                cfi[e[pid]].push(options.q[i][id]);
            }
            _ref1 = options.q;
            for (_j = 0, _len1 = _ref1.length; _j < _len1; _j++) {
                e = _ref1[_j];
                if (rfi[e[pid]] == null) {
                    ri.push(e[id]);
                }
            }
            while (ri.length) {
                thisid = ri.splice(0, 1);
                o.push(options.q[rfi[thisid]]);
                if (cfi[thisid] != null) {
                    ri = cfi[thisid].concat(ri);
                }
            }
            sorted = o;
            temp = {};
            o = [];
            _ref2 = sorted;
            for (_k = 0, _len2 = _ref2.length; _k < _len2; _k++) {
                e = _ref2[_k];
                e[children] = [];
                temp[e[id]] = e;
                if (temp[e[pid]] != null) {
                    temp[e[pid]][children].push(e);
                } else {
                    o.push(e);
                }
            }
            return o;
        };



        function getCategories() {
            ngProgress.color('green');
            ngProgress.start();
            itemService.GetCategories()
                .success(function (response) {
                    var options = { id: "FaqCategoryId", parentid: "FaqCategoryParentId", q: response };
                    vm.Categories = makeTree(options);
                    $log.info(vm.Categories);
                    ngProgress.complete();
                })
                .error(function (errData) {
                    $log.error('failure loading categories', errData);
                    ngProgress.complete();
                });
        };

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
