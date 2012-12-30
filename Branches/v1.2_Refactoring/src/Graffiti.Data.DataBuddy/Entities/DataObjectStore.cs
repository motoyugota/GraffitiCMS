using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataObjectStore : DataBuddyBase
    {

        private static readonly Table _Table = null;

        static DataObjectStore() {
            _Table = new Table("graffiti_ObjectStore", "ObjectStore");
            _Table.IsReadOnly = false;
            _Table.PrimaryKey = "Id";
            _Table.Columns.Add(new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true));
            _Table.Columns.Add(new Column("Name", DbType.String, typeof(System.String), "Name", false, false));
            _Table.Columns.Add(new Column("Data", DbType.String, typeof(System.String), "Data", false, false));
            _Table.Columns.Add(new Column("Type", DbType.String, typeof(System.String), "Type", false, false));
            _Table.Columns.Add(new Column("CreatedOn", DbType.DateTime, typeof(System.DateTime), "CreatedOn", false, false));
            _Table.Columns.Add(new Column("ModifiedOn", DbType.DateTime, typeof(System.DateTime), "ModifiedOn", false, false));
            _Table.Columns.Add(new Column("Content_Type", DbType.String, typeof(System.String), "ContentType", false, false));
            _Table.Columns.Add(new Column("Version", DbType.Int32, typeof(System.Int32), "Version", false, false));
            _Table.Columns.Add(new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false));
        }

        /// <summary>
        /// Fetches an instance of ObjectStore based on a single column value. If more than one record is found, only the first will be used.
        /// </summary>
        public static DataObjectStore FetchByColumn(Column column, object value) {
            Query q = new Query(_Table);
            q.AndWhere(column, value);
            return FetchByQuery(q);
        }

        public static DataObjectStore FetchByQuery(Query q) {
            DataObjectStore item = new DataObjectStore();
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    item.LoadAndCloseReader(reader);
            }

            return item;
        }

        /// <summary>
        /// Creates an instance of Query for the type ObjectStore
        /// </summary>
        public static Query CreateQuery() {
            return new Query(_Table);
        }

        public DataObjectStore() { }
        /// <summary>
        /// Loads an instance of ObjectStore for the supplied primary key value
        /// </summary>
        public DataObjectStore(object keyValue) {
            Query q = new Query(_Table);
            q.AndWhere(Columns.Id, keyValue);
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    LoadAndCloseReader(reader);
            }
        }

        /// <summary>
        /// Hydrates an instance of ObjectStore. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates an instance of ObjectStore. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            Id = DataService.GetValue<System.Int32>(Columns.Id, reader);
            Name = DataService.GetValue<System.String>(Columns.Name, reader);
            Data = DataService.GetValue<System.String>(Columns.Data, reader);
            Type = DataService.GetValue<System.String>(Columns.Type, reader);
            CreatedOn = DataService.GetValue<System.DateTime>(Columns.CreatedOn, reader);
            ModifiedOn = DataService.GetValue<System.DateTime>(Columns.ModifiedOn, reader);
            ContentType = DataService.GetValue<System.String>(Columns.ContentType, reader);
            Version = DataService.GetValue<System.Int32>(Columns.Version, reader);
            UniqueId = DataService.GetValue<System.Guid>(Columns.UniqueId, reader);
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

        #region public System.String Name

        private System.String _Name;

        public System.String Name {
            get { return _Name; }
            set { MarkDirty(); _Name = value; }
        }

        #endregion

        #region public System.String Data

        private System.String _Data;

        public System.String Data {
            get { return _Data; }
            set { MarkDirty(); _Data = value; }
        }

        #endregion

        #region public System.String Type

        private System.String _Type;

        public System.String Type {
            get { return _Type; }
            set { MarkDirty(); _Type = value; }
        }

        #endregion

        #region public System.DateTime CreatedOn

        private System.DateTime _CreatedOn;

        public System.DateTime CreatedOn {
            get { return _CreatedOn; }
            set { MarkDirty(); _CreatedOn = value; }
        }

        #endregion

        #region public System.DateTime ModifiedOn

        private System.DateTime _ModifiedOn;

        public System.DateTime ModifiedOn {
            get { return _ModifiedOn; }
            set { MarkDirty(); _ModifiedOn = value; }
        }

        #endregion

        #region public System.String ContentType

        private System.String _ContentType;

        public System.String ContentType {
            get { return _ContentType; }
            set { MarkDirty(); _ContentType = value; }
        }

        #endregion

        #region public System.Int32 Version

        private System.Int32 _Version;

        public System.Int32 Version {
            get { return _Version; }
            set { MarkDirty(); _Version = value; }
        }

        #endregion

        #region public System.Guid UniqueId

        private System.Guid _UniqueId;

        public System.Guid UniqueId {
            get { return _UniqueId; }
            set { MarkDirty(); _UniqueId = value; }
        }

        #endregion

        /// <summary>
        /// The table object which represents ObjectStore
        /// </summary>
        public static Table Table { get { return _Table; } }

        /// <summary>
        /// The columns which represent ObjectStore
        /// </summary>
        public static class Columns {
            public static readonly Column Id = new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true);
            public static readonly Column Name = new Column("Name", DbType.String, typeof(System.String), "Name", false, false);
            public static readonly Column Data = new Column("Data", DbType.String, typeof(System.String), "Data", false, false);
            public static readonly Column Type = new Column("Type", DbType.String, typeof(System.String), "Type", false, false);
            public static readonly Column CreatedOn = new Column("CreatedOn", DbType.DateTime, typeof(System.DateTime), "CreatedOn", false, false);
            public static readonly Column ModifiedOn = new Column("ModifiedOn", DbType.DateTime, typeof(System.DateTime), "ModifiedOn", false, false);
            public static readonly Column ContentType = new Column("Content_Type", DbType.String, typeof(System.String), "ContentType", false, false);
            public static readonly Column Version = new Column("Version", DbType.Int32, typeof(System.Int32), "Version", false, false);
            public static readonly Column UniqueId = new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false);
        }


        public static int Destroy(Column column, object value) {
            DataObjectStore objectToDelete = FetchByColumn(column, value);
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

            Parameter pName = new Parameter("Name", null, DbType.String);
            pName.Length = 128;
            parameters.Add(pName);

            Parameter pData = new Parameter("Data", null, DbType.String);
            pData.Length = 2147483647;
            parameters.Add(pData);

            Parameter pType = new Parameter("Type", null, DbType.String);
            pType.Length = 255;
            parameters.Add(pType);

            parameters.Add(new Parameter("CreatedOn", null, DbType.DateTime));

            parameters.Add(new Parameter("ModifiedOn", null, DbType.DateTime));

            Parameter pContent_Type = new Parameter("Content_Type", null, DbType.String);
            pContent_Type.Length = 128;
            parameters.Add(pContent_Type);

            parameters.Add(new Parameter("Version", null, DbType.Int32));

            parameters.Add(new Parameter("UniqueId", null, DbType.Guid));

            return parameters;
        }

        #endregion

        protected override List<Parameter> GetParameters() {
            List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

            if (!IsNew) {
                parameters.Add(new Parameter("Id", Id, DbType.Int32));
            }

            Parameter pName = new Parameter("Name", Name, DbType.String);
            pName.Length = 128;
            parameters.Add(pName);

            Parameter pData = new Parameter("Data", Data, DbType.String);
            pData.Length = 2147483647;
            parameters.Add(pData);

            Parameter pType = new Parameter("Type", Type, DbType.String);
            pType.Length = 255;
            parameters.Add(pType);

            parameters.Add(new Parameter("CreatedOn", CreatedOn, DbType.DateTime));

            parameters.Add(new Parameter("ModifiedOn", ModifiedOn, DbType.DateTime));

            Parameter pContent_Type = new Parameter("Content_Type", ContentType, DbType.String);
            pContent_Type.Length = 128;
            parameters.Add(pContent_Type);

            parameters.Add(new Parameter("Version", Version, DbType.Int32));

            parameters.Add(new Parameter("UniqueId", UniqueId, DbType.Guid));

            return parameters;
        }

        protected override void BeforeValidate()
        {
            base.BeforeValidate();

            if (IsNew && UniqueId == Guid.Empty)
                UniqueId = Guid.NewGuid();

            if (string.IsNullOrEmpty(Type))
                throw new Exception("ObjectStore.Type must be defined");

            if (string.IsNullOrEmpty("Name"))
                throw new Exception("ObjectStore.Name must be defined");

        }
    }
}
