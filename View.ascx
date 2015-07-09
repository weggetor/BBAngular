<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Bitboxx.DNNModules.BBAngular.View" %>
<div id="itemApp">
    <div ng-view></div>
</div>
<script>
    angular.element(document).ready(function () {

        function init(appName, moduleId, apiPath) {
            var sf = $.ServicesFramework(moduleId);
            var localAppName = appName + moduleId;
            var app = angular.module(localAppName, [appName])
                .constant("serviceRoot", sf.getServiceRoot(apiPath))
                .config(function($httpProvider) {
                    var httpHeaders = { "ModuleId": sf.getModuleId(), "TabId": sf.getTabId(), "RequestVerificationToken": sf.getAntiForgeryValue() };
                    angular.extend($httpProvider.defaults.headers.common, httpHeaders);
                });
            return app;
        };

        var app = init("itemApp", <%=ModuleId%>, "BBAngular");
        app.constant("Userlist", '<%=Users%>');
        app.constant("Resources", '<%=Resources%>');
        var moduleContainer = document.getElementById("itemApp");
        angular.bootstrap(moduleContainer, [app.name]);
    });
</script>