using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataTagCollection : List<DataTag> 
    {
        /// <summary>
        /// Hydrates a collection of Tag. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of Tag. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataTag obj = new DataTag();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of Tag
        /// </summary>
        public static DataTagCollection FetchAll() {
            Query q = DataTag.CreateQuery();
            DataTagCollection itemCollection = new DataTagCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of Tag based on the supplied query
        /// </summary>
        public static DataTagCollection FetchByQuery(Query q) {
            DataTagCollection itemCollection = new DataTagCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of Tag based on the supplied query
        /// </summary>
        public static DataTagCollection FetchByColumn(Column column, object value) {
            DataTagCollection itemCollection = new DataTagCollection();
            Query q = DataTag.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
