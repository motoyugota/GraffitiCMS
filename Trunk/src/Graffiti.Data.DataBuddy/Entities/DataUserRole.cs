using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy 
{
    [Serializable]
    public class DataUserRole : DataBuddyBase 
    {
        private static readonly Table _Table = null;

        static DataUserRole() {
            _Table = new Table("graffiti_UserRoles", "UserRole");
            _Table.IsReadOnly = false;
            _Table.PrimaryKey = "Id";
            _Table.Columns.Add(new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true));
            _Table.Columns.Add(new Column("UserId", DbType.Int32, typeof(System.Int32), "UserId", false, false));
            _Table.Columns.Add(new Column("RoleName", DbType.String, typeof(System.String), "RoleName", false, false));
        }

        /// <summary>
        /// Fetches an instance of UserRole based on a single column value. If more than one record is found, only the first will be used.
        /// </summary>
        public static DataUserRole FetchByColumn(Column column, object value) {
            Query q = new Query(_Table);
            q.AndWhere(column, value);
            return FetchByQuery(q);
        }

        public static DataUserRole FetchByQuery(Query q) {
            DataUserRole item = new DataUserRole();
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    item.LoadAndCloseReader(reader);
            }

            return item;
        }

        /// <summary>
        /// Creates an instance of Query for the type UserRole
        /// </summary>
        public static Query CreateQuery() {
            return new Query(_Table);
        }

        public DataUserRole() { }
        /// <summary>
        /// Loads an instance of UserRole for the supplied primary key value
        /// </summary>
        public DataUserRole(object keyValue) {
            Query q = new Query(_Table);
            q.AndWhere(Columns.Id, keyValue);
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    LoadAndCloseReader(reader);
            }
        }

        /// <summary>
        /// Hydrates an instance of UserRole. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates an instance of UserRole. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            Id = DataService.GetValue<System.Int32>(Columns.Id, reader);
            UserId = DataService.GetValue<System.Int32>(Columns.UserId, reader);
            RoleName = DataService.GetValue<System.String>(Columns.RoleName, reader);
            Loaded();
            ResetStatus();

            if (close)
                reader.Close();
        }

        #region public System.Int32 Id

        private System.Int32 _Id;

        public System.Int32 Id {
            get { return _Id; }
            set { MarkDirty(); _Id = value; }
        }

        #endregion

        #region public System.Int32 UserId

        private System.Int32 _UserId;

        public System.Int32 UserId {
            get { return _UserId; }
            set { MarkDirty(); _UserId = value; }
        }

        #endregion

        #region public System.String RoleName

        private System.String _RoleName;

        public System.String RoleName {
            get { return _RoleName; }
            set { MarkDirty(); _RoleName = value; }
        }

        #endregion

        /// <summary>
        /// The table object which represents UserRole
        /// </summary>
        public static Table Table { get { return _Table; } }

        /// <summary>
        /// The columns which represent UserRole
        /// </summary>
        public static class Columns {
            public static readonly Column Id = new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true);
            public static readonly Column UserId = new Column("UserId", DbType.Int32, typeof(System.Int32), "UserId", false, false);
            public static readonly Column RoleName = new Column("RoleName", DbType.String, typeof(System.String), "RoleName", false, false);
        }


        public static int Destroy(Column column, object value) {
            DataUserRole objectToDelete = FetchByColumn(column, value);
            if (!objectToDelete.IsNew) {
                objectToDelete.BeforeRemove(true);
                int i = DataService.Destroy(Table, column, value);
                objectToDelete.AfterRemove(true);
                return i;
            }

            return 0;
        }


        public static int Destroy(object value) {
            return Destroy(Columns.Id, value);
        }
        protected override void SetPrimaryKey(int pkID) {
            Id = pkID;
        }

        protected override Table GetTable() {
            return Table;
        }

        public static Parameter FindParameter(List<Parameter> parameters, string name) {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The value cannot be null or an empty string.");

            return parameters.Find(delegate(Parameter p) { return (p.Name == name); });
        }

        public static Parameter FindParameter(string name) {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "The value cannot be null or an empty string.");

            return GenerateParameters().Find(delegate(Parameter p) { return (p.Name == name); });
        }

        #region public static List<Parameter> GenerateParameters()

        public static List<Parameter> GenerateParameters() {
            List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

            parameters.Add(new Parameter("Id", null, DbType.Int32));

            parameters.Add(new Parameter("UserId", null, DbType.Int32));

            Parameter pRoleName = new Parameter("RoleName", null, DbType.String);
            pRoleName.Length = 128;
            parameters.Add(pRoleName);

            return parameters;
        }

        #endregion

        protected override List<Parameter> GetParameters() {
            List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

            if (!IsNew) {
                parameters.Add(new Parameter("Id", Id, DbType.Int32));
            }

            parameters.Add(new Parameter("UserId", UserId, DbType.Int32));

            Parameter pRoleName = new Parameter("RoleName", RoleName, DbType.String);
            pRoleName.Length = 128;
            parameters.Add(pRoleName);

            return parameters;
        }
    }
}
