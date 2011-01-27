using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataRoleCategoryPermissionsCollection : List<DataRoleCategoryPermissions> 
    {
        /// <summary>
        /// Hydrates a collection of RoleCategoryPermissions. In this case, the Reader should not be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates a collection of RoleCategoryPermissions. In this case, the Reader should not be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            while (reader.Read()) {
                DataRoleCategoryPermissions obj = new DataRoleCategoryPermissions();
                obj.Load(reader);
                this.Add(obj);
            }

            if (close)
                reader.Close();
        }

        /// <summary>
        /// Returns a collection containing all of the instances of RoleCategoryPermissions
        /// </summary>
        public static DataRoleCategoryPermissionsCollection FetchAll() {
            Query q = DataRoleCategoryPermissions.CreateQuery();
            DataRoleCategoryPermissionsCollection itemCollection = new DataRoleCategoryPermissionsCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of RoleCategoryPermissions based on the supplied query
        /// </summary>
        public static DataRoleCategoryPermissionsCollection FetchByQuery(Query q) {
            DataRoleCategoryPermissionsCollection itemCollection = new DataRoleCategoryPermissionsCollection();
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }

        /// <summary>
        /// Returns a collection of RoleCategoryPermissions based on the supplied query
        /// </summary>
        public static DataRoleCategoryPermissionsCollection FetchByColumn(Column column, object value) {
            DataRoleCategoryPermissionsCollection itemCollection = new DataRoleCategoryPermissionsCollection();
            Query q = DataRoleCategoryPermissions.CreateQuery();
            q.AndWhere(column, value);
            itemCollection.LoadAndCloseReader(q.ExecuteReader());
            return itemCollection;
        }
    }
}
