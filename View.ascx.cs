﻿/*
' Copyright (c) 2015  bitboxx solutions
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System.Collections;
using System.Linq;
using System.Resources;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Bitboxx.DNNModules.BBAngular
{
    [DNNtc.ModuleDependencies(DNNtc.ModuleDependency.CoreVersion, "07.00.00")]
    [DNNtc.PackageProperties("BBAngular_Module", 1, "BBAngular Module", "An AngularJS sample module", "BBAngular.png", "Torsten Weggen", "bitboxx solutions", "http://www.bitboxx.net", "info@bitboxx.net", false)]
    [DNNtc.ModuleProperties("BBAngular_Module", "BBAngular Module", 0)]
    [DNNtc.ModuleControlProperties("", "BBAngular", DNNtc.ControlType.View, "", false, false)]
    public partial class View : PortalModuleBase
    {
        protected string Resources
        {
            get
            {
                using (var rsxr = new ResXResourceReader(MapPath(LocalResourceFile + ".ascx.resx")))
                {
                    var res = rsxr.OfType<DictionaryEntry>()
                        .ToDictionary(
                            entry => entry.Key.ToString().Replace(".", "_"),
                            entry => LocalizeString(entry.Key.ToString()));
                    
                    return ClientAPI.GetSafeJSString(JsonConvert.SerializeObject(res));
                }
                
            }
        }

        protected string ModuleSettings
        {
            get
            {
                return ClientAPI.GetSafeJSString(JsonConvert.SerializeObject(Settings));
            }
        }


        protected string Users
        {
            get
            {
                var users = UserController.GetUsers(PortalId).Cast<UserInfo>()
                    .Select(u => new {text = u.Username, id = u.UserID});
                return  ClientAPI.GetSafeJSString(JsonConvert.SerializeObject(users));
            }
        }

        protected bool Editable { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DotNetNuke.Framework.ServicesFramework.Instance.RequestAjaxScriptSupport();
                DotNetNuke.Framework.ServicesFramework.Instance.RequestAjaxAntiForgerySupport();

                // Register angular library
                JavaScript.RequestRegistration("AngularJS");
                JavaScript.RequestRegistration("angular-route", new Version("01.02.28"));

                // Register ngProgress
                JavaScript.RequestRegistration("angular-ng-progress", new Version("01.00.07"));
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/Resources/libraries/angular-ng-progress/01_00_07/ngProgress.min.css", DotNetNuke.Web.Client.FileOrder.Css.DefaultPriority);

                // Register ngDialog
                JavaScript.RequestRegistration("angular-ng-dialog", new Version("00.05.01"));
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/Resources/libraries/angular-ng-dialog/00_05_01/ngDialog.min.css", DotNetNuke.Web.Client.FileOrder.Css.DefaultPriority);
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/Resources/libraries/angular-ng-dialog/00_05_01/ngDialog-theme-default.min.css", DotNetNuke.Web.Client.FileOrder.Css.DefaultPriority);

                // Register jqueryUI sortable angular directives
                JavaScript.RequestRegistration("angular-ui-sortable", new Version("00.13.04"));

                // Register module resources
                ClientResourceManager.RegisterScript(this.Page, ControlPath + "js/app.js", DotNetNuke.Web.Client.FileOrder.Js.DnnControls);
                ClientResourceManager.RegisterScript(this.Page, ControlPath + "js/itemService.js", DotNetNuke.Web.Client.FileOrder.Js.DefaultPriority);
                ClientResourceManager.RegisterScript(this.Page, ControlPath + "js/itemController.js", DotNetNuke.Web.Client.FileOrder.Js.DefaultPriority);

                
                Editable = (IsEditable && UserId > 0);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }
    }
}