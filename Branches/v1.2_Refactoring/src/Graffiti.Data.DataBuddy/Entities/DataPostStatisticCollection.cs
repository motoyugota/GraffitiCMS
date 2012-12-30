using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataPostStatisticCollection : List<DataPostStatistic> 
    {
        /// <summary>
        /// Hydrates a collection of PostStatistic. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of PostStatistic. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataPostStatistic obj = new DataPostStatistic();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of PostStatistic
        /// </summary>
        public static DataPostStatisticCollection FetchAll() {
            Query q = DataPostStatistic.CreateQuery();
            DataPostStatisticCollection itemCollection = new DataPostStatisticCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of PostStatistic based on the supplied query
        /// </summary>
        public static DataPostStatisticCollection FetchByQuery(Query q) {
            DataPostStatisticCollection itemCollection = new DataPostStatisticCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of PostStatistic based on the supplied query
        /// </summary>
        public static DataPostStatisticCollection FetchByColumn(Column column, object value) {
            DataPostStatisticCollection itemCollection = new DataPostStatisticCollection();
            Query q = DataPostStatistic.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
