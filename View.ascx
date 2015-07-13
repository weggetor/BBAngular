<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Bitboxx.DNNModules.BBAngular.View" %>
<%@ Import Namespace="Bitboxx.DNNModules.BBAngular.Components" %>
<div id="itemApp<%=ModuleId%>">
    <div ng-view></div>
</div>
<script>
    angular.element(document).ready(function () {

        function init(appName, moduleId, apiPath) {
            var sf = $.ServicesFramework(moduleId);
            var localAppName = appName + moduleId;
            var application = angular.module(localAppName, [appName])
                .constant("serviceRoot", sf.getServiceRoot(apiPath))
                .config(function($httpProvider) {
                    var httpHeaders = { "ModuleId": sf.getModuleId(), "TabId": sf.getTabId(), "RequestVerificationToken": sf.getAntiForgeryValue() };
                    angular.extend($httpProvider.defaults.headers.common, httpHeaders);
                });
            return application;
        };

        var app = init("itemApp", <%=ModuleId%>, "BBAngular_Module");
        app.constant("userlist", '<%=Users%>');
        app.constant("resources", '<%=Resources%>');
        app.constant("editable", '<%=Editable%>');
        app.constant("moduleId", '<%=ModuleId%>');
        app.constant("settings", '<%=ModuleSettings%>');
        var moduleContainer = document.getElementById("itemApp<%=ModuleId%>");
        angular.bootstrap(moduleContainer, [app.name]);
    });
</script>