using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public partial class DataLogCollection : List<DataLog> {
        /// <summary>
        /// Hydrates a collection of Log. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of Log. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataLog obj = new DataLog();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of Log
        /// </summary>
        public static DataLogCollection FetchAll() {
            Query q = DataLog.CreateQuery();
            DataLogCollection itemCollection = new DataLogCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of Log based on the supplied query
        /// </summary>
        public static DataLogCollection FetchByQuery(Query q) {
            DataLogCollection itemCollection = new DataLogCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of Log based on the supplied query
        /// </summary>
        public static DataLogCollection FetchByColumn(Column column, object value) {
            DataLogCollection itemCollection = new DataLogCollection();
            Query q = DataLog.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
