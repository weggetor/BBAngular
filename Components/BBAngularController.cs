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

using Bitboxx.DNNModules.BBAngular.Models;
using DotNetNuke.Data;
using System;
using System.Collections.Generic;

/// <summary>
/// The Components namespace.
/// </summary>
namespace Bitboxx.DNNModules.BBAngular.Components
{
    /// <summary>
    /// Class BBAngularController.
    /// </summary>
    public class BBAngularController
    {
        /// <summary>
        /// The _instance
        /// </summary>
        private static BBAngularController _instance;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static BBAngularController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BBAngularController();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        /// <param name="moduleid">The moduleid.</param>
        public void ClearCache(int moduleid)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                try
                {
                    // Setup fictitious item to delete (just to clear the scope cache)
                    var item = new ItemInfo { ItemId = -1, ModuleId = moduleid };
                    DeleteItem(item);
                }
                catch { } // ignore
            }
        }

        /// <summary>
        /// Creates the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public int CreateItem(ItemInfo item)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                rep.Insert((ItemInfo)item);
                return item.ItemId;
            }
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="moduleId">The module identifier.</param>
        public void DeleteItem(int itemId, int moduleId)
        {
            var item = GetItem(itemId, moduleId);
            DeleteItem(item);
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void DeleteItem(ItemInfo item)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                rep.Delete((ItemInfo)item);
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <returns>IEnumerable&lt;ItemInfo&gt;.</returns>
        public IEnumerable<ItemInfo> GetItems(int moduleId)
        {
            IEnumerable<ItemInfo> item;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                item = rep.Get(moduleId);
            }
            return item;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="moduleId">The module identifier.</param>
        /// <returns>ItemInfo.</returns>
        public ItemInfo GetItem(int itemId, int moduleId)
        {
            ItemInfo item;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                item = rep.GetById(itemId, moduleId);
            }
            return item;
        }

        /// <summary>
        /// Gets the item by identifier list.
        /// </summary>
        /// <param name="idlist">The idlist.</param>
        /// <param name="moduleId">The module identifier.</param>
        /// <returns>IEnumerable&lt;ItemInfo&gt;.</returns>
        public IEnumerable<ItemInfo> GetItemByIdList(int[] idlist, int moduleId)
        {
            IEnumerable<ItemInfo> item;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                item = rep.Find(String.Format("WHERE ModuleId = @0 AND ItemId IN ( {0} )", String.Join(",", idlist)), moduleId);
            }
            return item;
        }

        /// <summary>
        /// Gets the name of the item by.
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="itemname">The itemname.</param>
        /// <returns>IEnumerable&lt;ItemInfo&gt;.</returns>
        public IEnumerable<ItemInfo> GetItemByName(int moduleId, string itemname)
        {
            IEnumerable<ItemInfo> item;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                item = rep.Find("WHERE ModuleId = @0 AND ItemName LIKE @1", moduleId, itemname);
            }
            return item;
        }

        /// <summary>
        /// Gets all items by date.
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="beginDate">The begin date.</param>
        /// <returns>IEnumerable&lt;ItemInfo&gt;.</returns>
        public IEnumerable<ItemInfo> GetAllItemsByDate(int moduleId, DateTime beginDate)
        {
            IEnumerable<ItemInfo> item;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                item = rep.Find("WHERE ModuleId=@0 AND LastModifiedOnDate >= @1", moduleId, beginDate);
            }
            return item;
        }

        /// <summary>
        /// Updates the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void UpdateItem(ItemInfo item)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                rep.Update((ItemInfo)item);
            }
        }
    }
}