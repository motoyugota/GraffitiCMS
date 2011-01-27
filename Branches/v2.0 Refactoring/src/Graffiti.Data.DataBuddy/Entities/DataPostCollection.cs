using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;
using Graffiti.Core;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataPostCollection : List<DataPost> 
    {
        /// <summary>
        /// Hydrates a collection of Post. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of Post. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataPost obj = new DataPost();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of Post
        /// </summary>
        public static DataPostCollection FetchAll() {
            Query q = DataPost.CreateQuery();
            DataPostCollection itemCollection = new DataPostCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of Post based on the supplied query
        /// </summary>
        public static DataPostCollection FetchByQuery(Query q) {
            DataPostCollection itemCollection = new DataPostCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of Post based on the supplied query
        /// </summary>
        public static DataPostCollection FetchByColumn(Column column, object value) {
            DataPostCollection itemCollection = new DataPostCollection();
            Query q = DataPost.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        public static Query DefaultQuery() {
            return DefaultQuery(SortOrderType.Descending);
        }

        /// <summary>
        /// Returns a query which applies all of the common filters
        /// </summary>
        /// <returns></returns>
        public static Query DefaultQuery(SortOrderType sot) {
            Query q = DataPost.CreateQuery();
            q.AndWhere(DataPost.Columns.IsPublished, true);
            q.AndWhere(DataPost.Columns.IsDeleted, false);

            //if (SiteSettings.Get().FilterUncategorizedPostsFromLists) //removed to restore uncat post visibility in control panel
                //q.AndWhere(DataPost.Columns.CategoryId, DataCategoryController.UnCategorizedId, Comparison.NotEquals);

            q.AndWhere(DataPost.Columns.Published, SiteSettings.CurrentUserTime, Comparison.LessOrEquals);

            switch (sot) {
                case SortOrderType.Ascending:
                    q.OrderByAsc(DataPost.Columns.Published);
                    break;

                case SortOrderType.Views:
                    q.OrderByDesc(DataPost.Columns.Views);
                    break;

                case SortOrderType.Custom:
                    q.OrderByAsc(DataPost.Columns.SortOrder);
                    break;

                case SortOrderType.Alphabetical:
                    q.OrderByAsc(DataPost.Columns.Title);
                    break;

                default:
                    q.OrderByDesc(DataPost.Columns.Published);
                    break;
            }


            return q;
        }

        public static Query HomeQueryOverride(int pageIndex, int pageSize) {
            Query q = DataPost.CreateQuery();
            q.AndWhere(DataPost.Columns.IsPublished, true);
            q.AndWhere(DataPost.Columns.IsDeleted, false);
            q.AndWhere(DataPost.Columns.IsHome, true);
            q.AndWhere(DataPost.Columns.Published, SiteSettings.CurrentUserTime, Comparison.LessOrEquals);

            q.PageSize = pageSize;
            q.PageIndex = pageIndex;

            q.OrderByAsc(DataPost.Columns.HomeSortOrder);

            return q;
        }

        /// <summary>
        /// Returns a query which applies all of the common filters and enables paging.
        /// </summary>
        /// <returns></returns>
        public static Query DefaultQuery(int pageIndex, int pageSize, SortOrderType sot) {
            Query q = DefaultQuery(sot);
            q.PageIndex = pageIndex;
            q.PageSize = pageSize;
            return q;

        }
    }
}
