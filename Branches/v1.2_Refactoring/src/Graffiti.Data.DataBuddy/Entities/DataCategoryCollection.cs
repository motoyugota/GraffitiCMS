using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataCategoryCollection : List<DataCategory> 
    {
        /// <summary>
        /// Hydrates a collection of Category. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of Category. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataCategory obj = new DataCategory();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of Category
        /// </summary>
        public static DataCategoryCollection FetchAll() {
            Query q = DataCategory.CreateQuery();
            DataCategoryCollection itemCollection = new DataCategoryCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of Category based on the supplied query
        /// </summary>
        public static DataCategoryCollection FetchByQuery(Query q) {
            DataCategoryCollection itemCollection = new DataCategoryCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of Category based on the supplied query
        /// </summary>
        public static DataCategoryCollection FetchByColumn(Column column, object value) {
            DataCategoryCollection itemCollection = new DataCategoryCollection();
            Query q = DataCategory.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
