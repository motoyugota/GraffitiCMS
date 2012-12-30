using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataTagWeightCollection : List<DataTagWeight> {
        /// <summary>
        /// Hydrates a collection of TagWeight. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of TagWeight. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataTagWeight obj = new DataTagWeight();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of TagWeight
        /// </summary>
        public static DataTagWeightCollection FetchAll() {
            Query q = DataTagWeight.CreateQuery();
            DataTagWeightCollection itemCollection = new DataTagWeightCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of TagWeight based on the supplied query
        /// </summary>
        public static DataTagWeightCollection FetchByQuery(Query q) {
            DataTagWeightCollection itemCollection = new DataTagWeightCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of TagWeight based on the supplied query
        /// </summary>
        public static DataTagWeightCollection FetchByColumn(Column column, object value) {
            DataTagWeightCollection itemCollection = new DataTagWeightCollection();
            Query q = DataTagWeight.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
