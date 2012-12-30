using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataRolePermissions : DataBuddyBase 
    {
        private static readonly Table _Table = null;

        static DataRolePermissions() {
            _Table = new Table("graffiti_RolePermissions", "RolePermissions");
            _Table.IsReadOnly = false;
            _Table.PrimaryKey = "Id";
            _Table.Columns.Add(new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true));
            _Table.Columns.Add(new Column("RoleName", DbType.String, typeof(System.String), "RoleName", false, false));
            _Table.Columns.Add(new Column("HasRead", DbType.Boolean, typeof(System.Boolean), "HasRead", false, false));
            _Table.Columns.Add(new Column("HasEdit", DbType.Boolean, typeof(System.Boolean), "HasEdit", false, false));
            _Table.Columns.Add(new Column("HasPublish", DbType.Boolean, typeof(System.Boolean), "HasPublish", false, false));
            _Table.Columns.Add(new Column("CreatedBy", DbType.String, typeof(System.String), "CreatedBy", true, false));
            _Table.Columns.Add(new Column("ModifiedOn", DbType.DateTime, typeof(System.DateTime), "ModifiedOn", false, false));
            _Table.Columns.Add(new Column("ModifiedBy", DbType.String, typeof(System.String), "ModifiedBy", true, false));
        }

        /// <summary>
        /// Fetches an instance of RolePermissions based on a single column value. If more than one record is found, only the first will be used.
        /// </summary>
        public static DataRolePermissions FetchByColumn(Column column, object value) {
            Query q = new Query(_Table);
            q.AndWhere(column, value);
            return FetchByQuery(q);
        }

        public static DataRolePermissions FetchByQuery(Query q) {
            DataRolePermissions item = new DataRolePermissions();
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    item.LoadAndCloseReader(reader);
            }

            return item;
        }

        /// <summary>
        /// Creates an instance of Query for the type RolePermissions
        /// </summary>
        public static Query CreateQuery() {
            return new Query(_Table);
        }

        public DataRolePermissions() { }
        /// <summary>
        /// Loads an instance of RolePermissions for the supplied primary key value
        /// </summary>
        public DataRolePermissions(object keyValue) {
            Query q = new Query(_Table);
            q.AndWhere(Columns.Id, keyValue);
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    LoadAndCloseReader(reader);
            }
        }

        /// <summary>
        /// Hydrates an instance of RolePermissions. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates an instance of RolePermissions. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            Id = DataService.GetValue<System.Int32>(Columns.Id, reader);
            RoleName = DataService.GetValue<System.String>(Columns.RoleName, reader);
            HasRead = DataService.GetValue<System.Boolean>(Columns.HasRead, reader);
            HasEdit = DataService.GetValue<System.Boolean>(Columns.HasEdit, reader);
            HasPublish = DataService.GetValue<System.Boolean>(Columns.HasPublish, reader);
            CreatedBy = DataService.GetValue<System.String>(Columns.CreatedBy, reader);
            ModifiedOn = DataService.GetValue<System.DateTime>(Columns.ModifiedOn, reader);
            ModifiedBy = DataService.GetValue<System.String>(Columns.ModifiedBy, reader);
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

        #region public System.String RoleName

        private System.String _RoleName;

        public System.String RoleName {
            get { return _RoleName; }
            set { MarkDirty(); _RoleName = value; }
        }

        #endregion

        #region public System.Boolean HasRead

        private System.Boolean _HasRead;

        public System.Boolean HasRead {
            get { return _HasRead; }
            set { MarkDirty(); _HasRead = value; }
        }

        #endregion

        #region public System.Boolean HasEdit

        private System.Boolean _HasEdit;

        public System.Boolean HasEdit {
            get { return _HasEdit; }
            set { MarkDirty(); _HasEdit = value; }
        }

        #endregion

        #region public System.Boolean HasPublish

        private System.Boolean _HasPublish;

        public System.Boolean HasPublish {
            get { return _HasPublish; }
            set { MarkDirty(); _HasPublish = value; }
        }

        #endregion

        #region public System.String CreatedBy

        private System.String _CreatedBy;

        public System.String CreatedBy {
            get { return _CreatedBy; }
            set { MarkDirty(); _CreatedBy = value; }
        }

        #endregion

        #region public System.DateTime ModifiedOn

        private System.DateTime _ModifiedOn;

        public System.DateTime ModifiedOn {
            get { return _ModifiedOn; }
            set { MarkDirty(); _ModifiedOn = value; }
        }

        #endregion

        #region public System.String ModifiedBy

        private System.String _ModifiedBy;

        public System.String ModifiedBy {
            get { return _ModifiedBy; }
            set { MarkDirty(); _ModifiedBy = value; }
        }

        #endregion

        /// <summary>
        /// The table object which represents RolePermissions
        /// </summary>
        public static Table Table { get { return _Table; } }

        /// <summary>
        /// The columns which represent RolePermissions
        /// </summary>
        public static class Columns {
            public static readonly Column Id = new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true);
            public static readonly Column RoleName = new Column("RoleName", DbType.String, typeof(System.String), "RoleName", false, false);
            public static readonly Column HasRead = new Column("HasRead", DbType.Boolean, typeof(System.Boolean), "HasRead", false, false);
            public static readonly Column HasEdit = new Column("HasEdit", DbType.Boolean, typeof(System.Boolean), "HasEdit", false, false);
            public static readonly Column HasPublish = new Column("HasPublish", DbType.Boolean, typeof(System.Boolean), "HasPublish", false, false);
            public static readonly Column CreatedBy = new Column("CreatedBy", DbType.String, typeof(System.String), "CreatedBy", true, false);
            public static readonly Column ModifiedOn = new Column("ModifiedOn", DbType.DateTime, typeof(System.DateTime), "ModifiedOn", false, false);
            public static readonly Column ModifiedBy = new Column("ModifiedBy", DbType.String, typeof(System.String), "ModifiedBy", true, false);
        }


        public static int Destroy(Column column, object value) {
            DataRolePermissions objectToDelete = FetchByColumn(column, value);
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

            Parameter pRoleName = new Parameter("RoleName", null, DbType.String);
            pRoleName.Length = 128;
            parameters.Add(pRoleName);

            parameters.Add(new Parameter("HasRead", null, DbType.Boolean));

            parameters.Add(new Parameter("HasEdit", null, DbType.Boolean));

            parameters.Add(new Parameter("HasPublish", null, DbType.Boolean));

            Parameter pCreatedBy = new Parameter("CreatedBy", null, DbType.String);
            pCreatedBy.Length = 128;
            parameters.Add(pCreatedBy);

            parameters.Add(new Parameter("ModifiedOn", null, DbType.DateTime));

            Parameter pModifiedBy = new Parameter("ModifiedBy", null, DbType.String);
            pModifiedBy.Length = 128;
            parameters.Add(pModifiedBy);

            return parameters;
        }

        #endregion

        protected override List<Parameter> GetParameters() {
            List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

            if (!IsNew) {
                parameters.Add(new Parameter("Id", Id, DbType.Int32));
            }

            Parameter pRoleName = new Parameter("RoleName", RoleName, DbType.String);
            pRoleName.Length = 128;
            parameters.Add(pRoleName);

            parameters.Add(new Parameter("HasRead", HasRead, DbType.Boolean));

            parameters.Add(new Parameter("HasEdit", HasEdit, DbType.Boolean));

            parameters.Add(new Parameter("HasPublish", HasPublish, DbType.Boolean));

            Parameter pCreatedBy = new Parameter("CreatedBy", CreatedBy, DbType.String);
            pCreatedBy.Length = 128;
            parameters.Add(pCreatedBy);

            parameters.Add(new Parameter("ModifiedOn", ModifiedOn, DbType.DateTime));

            Parameter pModifiedBy = new Parameter("ModifiedBy", ModifiedBy, DbType.String);
            pModifiedBy.Length = 128;
            parameters.Add(pModifiedBy);

            return parameters;
        }
    }
}
