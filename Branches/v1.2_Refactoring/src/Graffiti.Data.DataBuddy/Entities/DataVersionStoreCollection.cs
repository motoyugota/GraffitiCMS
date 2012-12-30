using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataVersionStoreCollection : List<DataVersionStore> 
    {
        /// <summary>
        /// Hydrates a collection of VersionStore. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of VersionStore. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataVersionStore obj = new DataVersionStore();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of VersionStore
        /// </summary>
        public static DataVersionStoreCollection FetchAll() {
            Query q = DataVersionStore.CreateQuery();
            DataVersionStoreCollection itemCollection = new DataVersionStoreCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of VersionStore based on the supplied query
        /// </summary>
        public static DataVersionStoreCollection FetchByQuery(Query q) {
            DataVersionStoreCollection itemCollection = new DataVersionStoreCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of VersionStore based on the supplied query
        /// </summary>
        public static DataVersionStoreCollection FetchByColumn(Column column, object value) {
            DataVersionStoreCollection itemCollection = new DataVersionStoreCollection();
            Query q = DataVersionStore.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }        
    }
}
