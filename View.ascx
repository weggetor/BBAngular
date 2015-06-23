<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Bitboxx.DNNModules.BBAngular.View" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>

<div id='bitboxx-item-<%=this.ModuleId%>' class="bitboxx-item" 
    ng-controller="ItemController as vm" 
    ng-init="vm.init(<%=this.ModuleId%>,'<%=base.ModuleContext.Configuration.DesktopModule.ModuleName%>',<%=Editable.ToString().ToLower() %>)">

    <a class="btn" ng-click="vm.showAdd()" ng-show="vm.EditMode" href="javascript:void(0);">Add Item</a>
    <div>
        <ul>
            <li class="card" ng-repeat="item in vm.Items">
                <h3>
                    <span>{{item.ItemName}}</span>
                </h3>
                <span>{{item.ItemDescription}}</span>
                <div class="btnlist">
                    <a class="btn" ng-click="vm.showEdit(item, $index)" ng-show="vm.EditMode" href="javascript:void(0);">Edit</a>
                    <a class="btn" ng-click="vm.deleteItem(item, $index)" ng-show="vm.EditMode" href="javascript:void(0);">Delete</a>
                </div>
            </li>
        </ul>
    </div>

	<script type="text/ng-template" id="pnlAddEditItem">
		<div ng-form="dnnItemForm" class="dnnForm ngdialog-message">
            <h3>{{AddEditTitle}}</h3>
            <fieldset>
                <legend></legend>
                <div class="dnnFormItem">
                    <label class="dnnLabel" for="txtItemName"><%= Localization.GetString("lblName", LocalResourceFile) %></label>
                    <input id="txtItemName" name="txtItemName" 
                           ng-model="vm.Item.ItemName" 
                           ng-minlength="5" 
                           placeholder="Enter name" 
                           class="input" 
                           type="text" required />
                    <span style="display: inline;" class="dnnFormMessage dnnFormError" 
                            ng-show="(dnnItemForm.txtItemName.$dirty || vm.invalidSubmitAttempt) && dnnItemForm.txtItemName.$error.required">
                        <%= Localization.GetString("reqItemName", LocalResourceFile) %>
                    </span>
                    <span style="display: inline;" class="dnnFormMessage dnnFormError" 
                            ng-show="(dnnItemForm.txtItemName.$dirty || vm.invalidSubmitAttempt) && dnnItemForm.txtItemName.$error.minlength">
                        <%= Localization.GetString("minItemName", LocalResourceFile) %>
                    </span>
                </div>
                <div class="dnnFormItem">
                    <label class="dnnLabel" for="description"><%= Localization.GetString("lblDescription", LocalResourceFile) %></label>
                    <textarea name="txtItemDescription" ng-model="vm.Item.ItemDescription" type="text" rows="3" title="Description" required />
                    <span style="display: inline;" class="dnnFormMessage dnnFormError" 
                            ng-show="(dnnItemForm.txtItemDescription.$dirty || vm.invalidSubmitAttempt) && dnnItemForm.txtItemDescription.$error.required">
                        <%= Localization.GetString("reqItemDescription", LocalResourceFile) %>
                    </span>
                </div>
                <div class="dnnFormItem">
                    <label class="dnnLabel" for="ddlAssignedUser"><%= Localization.GetString("lblAssignedUser", LocalResourceFile) %></label>
                    <select name="ddlAssignedUser" 
                        ng-model="vm.Item.AssignedUserId",
                        ng-options="u.id as u.text for u in vm.UserList" required>
                    </select>
                    <span style="display: inline;" class="dnnFormMessage dnnFormError"
                            ng-show="(dnnItemForm.ddlAssignedUser.$dirty || vm.invalidSubmitAttempt) && dnnItemForm.ddlAssignedUser.$error.required">
                        <%= Localization.GetString("reqAssignedUser", LocalResourceFile) %>
                    </span>
                </div>
            </fieldset>
		</div>
		<div class="ngdialog-buttons">
			<button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(1)">Cancel</button>
			<button type="submit" class="ngdialog-button ngdialog-button-primary" ng-click="vm.createUpdateItem(dnnItemForm)">Save</button>
		</div>
	</script>
</div>

<script type="text/javascript">
    bitboxx.UserList = <%=UserList%>;
    angular.element(document).ready(function () {
        var moduleContainer = document.getElementById('bitboxx-item-<%=this.ModuleId%>');
        angular.bootstrap(moduleContainer, ["bitboxx.ItemApp"]);
    });
</script>
