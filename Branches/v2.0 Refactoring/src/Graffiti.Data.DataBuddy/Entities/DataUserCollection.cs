using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataUserCollection : List<DataUser> 
    {
        /// <summary>
        /// Hydrates a collection of User. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of User. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataUser obj = new DataUser();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of User
        /// </summary>
        public static DataUserCollection FetchAll() {
            Query q = DataUser.CreateQuery();
            DataUserCollection itemCollection = new DataUserCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of User based on the supplied query
        /// </summary>
        public static DataUserCollection FetchByQuery(Query q) {
            DataUserCollection itemCollection = new DataUserCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of User based on the supplied query
        /// </summary>
        public static DataUserCollection FetchByColumn(Column column, object value) {
            DataUserCollection itemCollection = new DataUserCollection();
            Query q = DataUser.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
