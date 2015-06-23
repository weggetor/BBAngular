/*
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

using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Bitboxx.DNNModules.BBAngular
{
    [DNNtc.ModuleDependencies(DNNtc.ModuleDependency.CoreVersion, "07.00.00")]
    [DNNtc.PackageProperties("BBAngular_Module", 1, "BBAngular Module", "An AngularJS sample module", "BBAngular.png", "Torsten Weggen", "bitboxx solutions", "http://www.bitboxx.net", "info@bitboxx.net", false)]
    [DNNtc.ModuleProperties("BBAngular_Module", "BBAngular Module", 0)]
    [DNNtc.ModuleControlProperties("", "BBAngular", DNNtc.ControlType.View, "", false, false)]
    public partial class View : PortalModuleBase
    {
        protected string UserList { get; set; }
        protected bool Editable { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DotNetNuke.Framework.ServicesFramework.Instance.RequestAjaxScriptSupport();
                DotNetNuke.Framework.ServicesFramework.Instance.RequestAjaxAntiForgerySupport();
                
                Editable = (IsEditable || UserId > 0);

                if (!Page.IsPostBack)
                {
                    //get a list of users to assign the user to the Object
                    populateUserLists();
                }
                
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Register angular library
            JavaScript.RequestRegistration("AngularJS");
            JavaScript.Register(this.Page);

            // Register module resources
            ClientResourceManager.RegisterScript(this.Page, ControlPath + "js/item-app.js", DotNetNuke.Web.Client.FileOrder.Js.DnnControls);
            ClientResourceManager.RegisterScript(this.Page, ControlPath + "js/dnnServiceClient.js", DotNetNuke.Web.Client.FileOrder.Js.DefaultPriority);
            ClientResourceManager.RegisterScript(this.Page, ControlPath + "js/itemController.js", DotNetNuke.Web.Client.FileOrder.Js.DefaultPriority);

            // Register ngProgress
            ClientResourceManager.RegisterScript(this.Page, ControlPath + "js/ngProgress/ngProgress.min.js", DotNetNuke.Web.Client.FileOrder.Js.DnnControls);
            ClientResourceManager.RegisterScript(this.Page, ControlPath + "js/ngProgress/ngProgress-directive.js", DotNetNuke.Web.Client.FileOrder.Js.DefaultPriority);
            ClientResourceManager.RegisterStyleSheet(this.Page, ControlPath + "js/ngProgress/ngProgress.css", DotNetNuke.Web.Client.FileOrder.Css.DefaultPriority);

            // Register ngDialog
            ClientResourceManager.RegisterScript(this.Page, ControlPath + "js/ngDialog/ngDialog.min.js", DotNetNuke.Web.Client.FileOrder.Js.DnnControls);
            ClientResourceManager.RegisterStyleSheet(this.Page, ControlPath + "js/ngDialog/ngDialog.min.css", DotNetNuke.Web.Client.FileOrder.Css.DefaultPriority);
            ClientResourceManager.RegisterStyleSheet(this.Page, ControlPath + "js/ngDialog/ngDialog-theme-default.min.css", DotNetNuke.Web.Client.FileOrder.Css.DefaultPriority);
        }

        private void populateUserLists()
        {
            var userList = new List<ListOption>();
            foreach (UserInfo u in UserController.GetUsers(PortalId))
            {
                userList.Add(new ListOption { text = u.Username, id = u.UserID });
            }
            UserList = new JavaScriptSerializer().Serialize(userList);
        }
    }

    public class ListOption
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}