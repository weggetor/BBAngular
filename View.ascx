<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Bitboxx.DNNModules.BBAngular.View" %>
<base href="<%=Request.ApplicationPath %>" />

<div id='bitboxx-item-<%=this.ModuleId%>' class="bitboxx-item" 
    ng-controller="ItemController as vm" 
    ng-init="vm.init(<%=this.ModuleId%>,'<%=base.ModuleContext.Configuration.DesktopModule.ModuleName%>',<%=Editable.ToString().ToLower() %>)">
    <div class="btn-add card" ng-show="vm.EditMode">
        <a class="btn" ng-click="vm.showAdd()"  href="javascript:void(0);">{{vm.localize.AddItem_Text}}</a>
    </div>
        <div>
        <ul>
            <li class="card" ng-repeat="item in vm.Items">
                <h3>
                    <span>{{item.ItemName}}</span>
                </h3>
                <span>{{item.ItemDescription}}</span>
                <div class="btnlist">
                    <a class="btn btn-edit" ng-click="vm.showEdit(item, $index)" ng-show="vm.EditMode" href="javascript:void(0);">{{vm.localize.EditItem_Text}}</a>
                    <a class="btn btn-delete" ng-click="vm.deleteItem(item, $index)" ng-show="vm.EditMode" href="javascript:void(0);">{{vm.localize.DeleteItem_Text}}</a>
                </div>
            </li>
        </ul>
    </div>

    <script type="text/ng-template" id="pnlAddEditItem">
		<div ng-form="dnnItemForm" class="dnnForm ngdialog-message">
            <h3>{{vm.AddEditTitle}}</h3>
            <fieldset>
                <legend></legend>
                <div class="dnnFormItem">
                    <label class="dnnLabel" for="txtItemName">{{vm.localize.lblName_Text}}</label>
                    <input id="txtItemName" name="txtItemName" 
                           ng-model="vm.Item.ItemName" 
                           ng-minlength="5" 
                           placeholder="Enter name" 
                           class="input" 
                           type="text" required />
                    <span style="display: inline;" class="dnnFormMessage dnnFormError" 
                            ng-show="(dnnItemForm.txtItemName.$dirty || vm.invalidSubmitAttempt) && dnnItemForm.txtItemName.$error.required">
                        {{vm.localize.reqItemName_Text}}
                    </span>
                    <span style="display: inline;" class="dnnFormMessage dnnFormError" 
                            ng-show="(dnnItemForm.txtItemName.$dirty || vm.invalidSubmitAttempt) && dnnItemForm.txtItemName.$error.minlength">
                        {{vm.localize.minItemName_Text}}
                    </span>
                </div>
                <div class="dnnFormItem">
                    <label class="dnnLabel" for="description">{{vm.localize.lblDescription_Text}}</label>
                    <textarea name="txtItemDescription" ng-model="vm.Item.ItemDescription" type="text" rows="3" title="Description" required />
                    <span style="display: inline;" class="dnnFormMessage dnnFormError" 
                            ng-show="(dnnItemForm.txtItemDescription.$dirty || vm.invalidSubmitAttempt) && dnnItemForm.txtItemDescription.$error.required">
                        {{vm.localize.reqItemDescription_Text}}
                    </span>
                </div>
                <div class="dnnFormItem">
                    <label class="dnnLabel" for="ddlAssignedUser">{{vm.localize.lblAssignedUser_Text}}</label>
                    <select name="ddlAssignedUser" 
                        ng-model="vm.Item.AssignedUserId",
                        ng-options="u.id as u.text for u in vm.UserList" required>
                    </select>
                    <span style="display: inline;" class="dnnFormMessage dnnFormError"
                            ng-show="(dnnItemForm.ddlAssignedUser.$dirty || vm.invalidSubmitAttempt) && dnnItemForm.ddlAssignedUser.$error.required">
                        {{vm.localize.reqAssignedUser_Text}}
                    </span>
                </div>
            </fieldset>
		</div>
		<div class="ngdialog-buttons">
			<button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(1)">{{vm.localize.CancelEdit_Text}}</button>
			<button type="submit" class="ngdialog-button ngdialog-button-primary" ng-click="vm.createUpdateItem(dnnItemForm)">{{vm.localize.SaveEdit_Text}}</button>
		</div>
	</script>
</div>


<script type="text/javascript">
    var app;
    app.UserList = <%=Users%>;
    app.Resources = <%=Resources%>;
    angular.element(document).ready(function () {
        var moduleContainer = document.getElementById('bitboxx-item-<%=this.ModuleId%>');
        angular.bootstrap(moduleContainer, ["itemApp"]);
    });
</script>
