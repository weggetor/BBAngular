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

using System.Collections.Generic;
using System.Threading;
using Bitboxx.DNNModules.BBAngular.Components;
using Bitboxx.DNNModules.BBAngular.Models;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bitboxx.DNNModules.BBAngular.Services
{
    [SupportedModules(Constants.DESKTOPMODULE_NAME)]
    public class ItemController : DnnApiController
    {

        /// <summary>
        /// API that returns Hello world
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpGet]  //[baseURL]/item/test
        [ActionName("test")]
        [AllowAnonymous]
        public HttpResponseMessage HelloWorld()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello World!");
        }

        /// <summary>
        /// API that creates a new item in the item list
        /// </summary>
        /// <returns></returns>
        [HttpPost]  //[baseURL]/item/new
        [ValidateAntiForgeryToken]
        [ActionName("new")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage AddItem(RequestItem item)
        {
            try
            {
                ItemInfo newItem = new ItemInfo();
                newItem.ModuleId = item.ModuleId;
                newItem.CreatedByUserId = UserInfo.UserID;
                newItem.CreatedOnDate = DateTime.Now;
                newItem.LastModifiedByUserId = UserInfo.UserID;
                newItem.LastModifiedOnDate = DateTime.Now;
                newItem.AssignedUserId = item.AssignedUserId;
                newItem.ItemName = item.ItemName;
                newItem.ItemDescription = item.ItemDescription;
                int itemId = BBAngularController.Instance.NewItem(newItem);
                return Request.CreateResponse(HttpStatusCode.OK, newItem);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// API that deletes an item from the item list
        /// </summary>
        /// <returns></returns>
        [HttpPost]  //[baseURL]/item/delete
        [ValidateAntiForgeryToken]
        [ActionName("delete")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage DeleteItem(RequestItem delItem)
        {
            try
            {
                BBAngularController.Instance.DeleteItem(delItem.ItemId, delItem.ModuleId);
                return Request.CreateResponse(HttpStatusCode.OK, true.ToString());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        /// <summary>
        /// API that creates a new item in the item list
        /// </summary>
        /// <returns></returns>
        [HttpPost]  //[baseURL]/item/edit
        [ValidateAntiForgeryToken]
        [ActionName("edit")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage UpdateItem(ItemInfo item)
        {
            try
            {
                item.LastModifiedByUserId = UserInfo.UserID;
                item.LastModifiedOnDate = DateTime.Now;
                BBAngularController.Instance.UpdateItem(item);
                return Request.CreateResponse(HttpStatusCode.OK, true.ToString());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// API that returns an item list
        /// </summary>
        /// <returns></returns>
        [HttpPost,HttpGet]  //[baseURL]/item/list
        [ValidateAntiForgeryToken]
        [ActionName("list")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage GetModuleItems()
        {
            try
            {
                var itemList = BBAngularController.Instance.GetItems(ActiveModule.ModuleID);
                return Request.CreateResponse(HttpStatusCode.OK, itemList.ToList());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// API that returns a single item
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpGet]  //[baseURL]/item/byid
        [ValidateAntiForgeryToken]
        [ActionName("byid")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage GetItem(RequestById itemReq)
        {
            try
            {
                var item = BBAngularController.Instance.GetItem(itemReq.ItemId, ActiveModule.ModuleID);
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// API that reorders an item list
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpGet]  //[baseURL]/item/list
        [ValidateAntiForgeryToken]
        [ActionName("reorder")]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public HttpResponseMessage ReorderItems(List<RequestItemOrder> sortItems)
        {
            try
            {
                foreach (var itemorder in sortItems)
                {
                    BBAngularController.Instance.SetItemOrder(itemorder.ItemId, Convert.ToInt16(itemorder.Sort));
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }

    public class RequestItem
    {
        public int ItemId { get; set; }
        public int ModuleId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int AssignedUserId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedCreatedOnDate { get; set; }
    }

    public class RequestById
    {
        public int ItemId { get; set; }
    }


    public class RequestItemOrder
    {
        public int ItemId { get; set; }
        public string Sort { get; set; }
    }

}