using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public partial class DataRolePermissionsCollection : List<DataRolePermissions> 
    {
        /// <summary>
        /// Hydrates a collection of RolePermissions. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of RolePermissions. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataRolePermissions obj = new DataRolePermissions();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of RolePermissions
        /// </summary>
        public static DataRolePermissionsCollection FetchAll() {
            Query q = DataRolePermissions.CreateQuery();
            DataRolePermissionsCollection itemCollection = new DataRolePermissionsCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of RolePermissions based on the supplied query
        /// </summary>
        public static DataRolePermissionsCollection FetchByQuery(Query q) {
            DataRolePermissionsCollection itemCollection = new DataRolePermissionsCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of RolePermissions based on the supplied query
        /// </summary>
        public static DataRolePermissionsCollection FetchByColumn(Column column, object value) {
            DataRolePermissionsCollection itemCollection = new DataRolePermissionsCollection();
            Query q = DataRolePermissions.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
