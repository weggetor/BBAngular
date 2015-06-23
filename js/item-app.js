/* http://www.jerriepelser.com/blog/share-translations-between-aspnet-mvc-and-angularjs */

var bitboxx = bitboxx || {};
angular
    .module("bitboxx.ItemApp", ['ngDialog','ngProgress'])
    .value("bitboxx", bitboxx);

