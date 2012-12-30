using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataUserRoleCollection : List<DataUserRole> 
    {
        /// <summary>
        /// Hydrates a collection of UserRole. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of UserRole. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataUserRole obj = new DataUserRole();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of UserRole
        /// </summary>
        public static DataUserRoleCollection FetchAll() {
            Query q = DataUserRole.CreateQuery();
            DataUserRoleCollection itemCollection = new DataUserRoleCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of UserRole based on the supplied query
        /// </summary>
        public static DataUserRoleCollection FetchByQuery(Query q) {
            DataUserRoleCollection itemCollection = new DataUserRoleCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of UserRole based on the supplied query
        /// </summary>
        public static DataUserRoleCollection FetchByColumn(Column column, object value) {
            DataUserRoleCollection itemCollection = new DataUserRoleCollection();
            Query q = DataUserRole.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
