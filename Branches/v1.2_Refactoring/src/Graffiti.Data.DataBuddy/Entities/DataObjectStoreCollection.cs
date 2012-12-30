using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataObjectStoreCollection : List<DataObjectStore> {
        /// <summary>
        /// Hydrates a collection of ObjectStore. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of ObjectStore. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataObjectStore obj = new DataObjectStore();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of ObjectStore
        /// </summary>
        public static DataObjectStoreCollection FetchAll() {
            Query q = DataObjectStore.CreateQuery();
            DataObjectStoreCollection itemCollection = new DataObjectStoreCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of ObjectStore based on the supplied query
        /// </summary>
        public static DataObjectStoreCollection FetchByQuery(Query q) {
            DataObjectStoreCollection itemCollection = new DataObjectStoreCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of ObjectStore based on the supplied query
        /// </summary>
        public static DataObjectStoreCollection FetchByColumn(Column column, object value) {
            DataObjectStoreCollection itemCollection = new DataObjectStoreCollection();
            Query q = DataObjectStore.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
