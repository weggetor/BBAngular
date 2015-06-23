angular.module("bitboxx.ItemApp")
    .factory("dnnServiceClient", ['$http', dnnServiceClient]);

function dnnServiceClient($http) {
    $self = this;

    return {
        init: function (moduleId, moduleName) {
            if ($.ServicesFramework) {
                var sf = $.ServicesFramework(moduleId);
                $self.ServiceRoot = sf.getServiceRoot(moduleName);
                $self.Headers = {
                    "ModuleId": moduleId,
                    "TabId": sf.getTabId(),
                    "RequestVerificationToken": sf.getAntiForgeryValue()
                };
            }
        },
        callGetService: function (method) {
            return $http({
                method: 'GET',
                url: $self.ServiceRoot + method,
                headers: $self.Headers
            });
        },
        callPostService: function (method, data) {
            return $http({
                method: 'POST',
                url: $self.ServiceRoot + method,
                headers: $self.Headers,
                data: data
            });
        }
    };
};