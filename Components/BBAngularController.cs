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

using System.Data;
using System.Linq;
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
        public void ClearCache(int moduleId)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                try
                {
                    // Setup fictitious item to delete (just to clear the scope cache)
                    DeleteItem(-1,moduleId);
                }
                catch { } // ignore
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <returns>IEnumerable&lt;ItemInfo&gt;.</returns>
        public IEnumerable<ItemInfo> GetItems(int moduleId)
        {
            IEnumerable<ItemInfo> items;
            using (IDataContext ctx = DataContext.Instance())
            {
                string sql = "SELECT * FROM {databaseOwner}[{objectQualifier}BBAngular_Items] WHERE ModuleId = @0 ORDER BY Sort";
                return ctx.ExecuteQuery<ItemInfo>(CommandType.Text,sql,moduleId);
            }
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
        /// Creates a new item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public int NewItem(ItemInfo item)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<ItemInfo>();
                rep.Insert((ItemInfo)item);
                return item.ItemId;
            }
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

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="moduleId">The module identifier.</param>
        public void DeleteItem(int itemId, int moduleId)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                string sql = "DELETE FROM {databaseOwner}[{objectQualifier}BBAngular_Items] WHERE ModuleId = @0 AND ItemId = @1";
                ctx.Execute(CommandType.Text, sql, moduleId, itemId);
            }
        }

        public void SetItemOrder(int itemId, int sort)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                string sql = "UPDATE {databaseOwner}[{objectQualifier}BBAngular_Items] SET Sort = @1 WHERE ItemId = @0";
                ctx.Execute(CommandType.Text, sql, itemId,sort);
            }
        }
    }
}