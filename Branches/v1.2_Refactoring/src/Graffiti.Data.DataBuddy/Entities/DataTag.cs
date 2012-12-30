using System;
using System.Collections.Generic;
using System.Data;
using DataBuddy;
using Graffiti.Core;

namespace Graffiti.Data.DataBuddy
{
    /// <summary>
    /// Summary description for Tag
    /// </summary>
    [Serializable]
    public class DataTag : DataBuddyBase
    {

        private static readonly Table _Table = null;

        static DataTag() {
            _Table = new Table("graffiti_Tags", "Tag");
            _Table.IsReadOnly = false;
            _Table.PrimaryKey = "Id";
            _Table.Columns.Add(new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true));
            _Table.Columns.Add(new Column("Name", DbType.String, typeof(System.String), "Name", false, false));
            _Table.Columns.Add(new Column("PostId", DbType.Int32, typeof(System.Int32), "PostId", false, false));
        }

        /// <summary>
        /// Fetches an instance of Tag based on a single column value. If more than one record is found, only the first will be used.
        /// </summary>
        public static DataTag FetchByColumn(Column column, object value) {
            Query q = new Query(_Table);
            q.AndWhere(column, value);
            return FetchByQuery(q);
        }

        public static DataTag FetchByQuery(Query q) {
            DataTag item = new DataTag();
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    item.LoadAndCloseReader(reader);
            }

            return item;
        }

        /// <summary>
        /// Creates an instance of Query for the type Tag
        /// </summary>
        public static Query CreateQuery() {
            return new Query(_Table);
        }

        public DataTag() { }
        /// <summary>
        /// Loads an instance of Tag for the supplied primary key value
        /// </summary>
        public DataTag(object keyValue) {
            Query q = new Query(_Table);
            q.AndWhere(Columns.Id, keyValue);
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    LoadAndCloseReader(reader);
            }
        }

        /// <summary>
        /// Hydrates an instance of Tag. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates an instance of Tag. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            Id = DataService.GetValue<System.Int32>(Columns.Id, reader);
            Name = DataService.GetValue<System.String>(Columns.Name, reader);
            PostId = DataService.GetValue<System.Int32>(Columns.PostId, reader);
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

        #region public System.Int32 PostId

        private System.Int32 _PostId;

        public System.Int32 PostId {
            get { return _PostId; }
            set { MarkDirty(); _PostId = value; }
        }

        #endregion

        /// <summary>
        /// The table object which represents Tag
        /// </summary>
        public static Table Table { get { return _Table; } }

        /// <summary>
        /// The columns which represent Tag
        /// </summary>
        public static class Columns {
            public static readonly Column Id = new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true);
            public static readonly Column Name = new Column("Name", DbType.String, typeof(System.String), "Name", false, false);
            public static readonly Column PostId = new Column("PostId", DbType.Int32, typeof(System.Int32), "PostId", false, false);
        }


        public static int Destroy(Column column, object value) {
            DataTag objectToDelete = FetchByColumn(column, value);
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

            parameters.Add(new Parameter("PostId", null, DbType.Int32));

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

            parameters.Add(new Parameter("PostId", PostId, DbType.Int32));

            return parameters;
        }

        protected override void AfterCommit()
        {
            base.AfterCommit();
            
            WritePage(Name);
        }

        public static void WritePage(string name) 
        {
            PageTemplateToolboxContext templateContext = new PageTemplateToolboxContext();
            templateContext.Put("tag", name);
            templateContext.Put("MetaDescription", "Posts and articles tagged as " + name);
            templateContext.Put("MetaKeywords", name);


            PageWriter.Write("tag.view", "~/tags/" + name + "/" + Util.DEFAULT_PAGE, templateContext);
            PageWriter.Write("tagrss.view", "~/tags/" + name + "/feed/" + Util.DEFAULT_PAGE, templateContext);

        }
    }



}