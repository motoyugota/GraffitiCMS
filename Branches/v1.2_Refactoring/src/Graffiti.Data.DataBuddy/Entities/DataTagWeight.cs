using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataTagWeight : DataBuddyBase
    {

        private static readonly Table _Table = null;

        static DataTagWeight() {
            _Table = new Table("graffiti_TagWeights", "TagWeight");
            _Table.IsReadOnly = true;
            _Table.PrimaryKey = "";
            _Table.Columns.Add(new Column("Name", DbType.String, typeof(System.String), "Name", false, false));
            _Table.Columns.Add(new Column("WEIGHT", DbType.Int32, typeof(System.Int32), "Weight", false, false));
        }

        /// <summary>
        /// Fetches an instance of TagWeight based on a single column value. If more than one record is found, only the first will be used.
        /// </summary>
        public static DataTagWeight FetchByColumn(Column column, object value) {
            Query q = new Query(_Table);
            q.AndWhere(column, value);
            return FetchByQuery(q);
        }

        public static DataTagWeight FetchByQuery(Query q) {
            DataTagWeight item = new DataTagWeight();
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    item.LoadAndCloseReader(reader);
            }

            return item;
        }

        /// <summary>
        /// Creates an instance of Query for the type TagWeight
        /// </summary>
        public static Query CreateQuery() {
            return new Query(_Table);
        }

        public DataTagWeight() { }

        /// <summary>
        /// Hydrates an instance of TagWeight. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates an instance of TagWeight. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            Name = DataService.GetValue<System.String>(Columns.Name, reader);
            Weight = DataService.GetValue<System.Int32>(Columns.Weight, reader);
            Loaded();
            ResetStatus();

            if (close)
                reader.Close();
        }

        #region public System.String Name

        private System.String _Name;

        public System.String Name {
            get { return _Name; }
            set { MarkDirty(); _Name = value; }
        }

        #endregion

        #region public System.Int32 Weight

        private System.Int32 _Weight;

        public System.Int32 Weight {
            get { return _Weight; }
            set { MarkDirty(); _Weight = value; }
        }

        #endregion

        /// <summary>
        /// The table object which represents TagWeight
        /// </summary>
        public static Table Table { get { return _Table; } }

        /// <summary>
        /// The columns which represent TagWeight
        /// </summary>
        public static class Columns {
            public static readonly Column Name = new Column("Name", DbType.String, typeof(System.String), "Name", false, false);
            public static readonly Column Weight = new Column("WEIGHT", DbType.Int32, typeof(System.Int32), "Weight", false, false);
        }

        protected override void SetPrimaryKey(int pkID) {
            throw new Exception("This table is readonly does not have a settable PK");
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

            parameters.Add(new Parameter("Name", null, DbType.String));

            parameters.Add(new Parameter("WEIGHT", null, DbType.Int32));

            return parameters;
        }

        #endregion

        protected override List<Parameter> GetParameters() {
            throw new Exception("This table is readonly and cannot be updated");
        }

        public string VirtualUrl
        {
            get { return "~/tags/" + Name.ToLower() + "/"; }
        }

        public string Url
        {
            get { return VirtualPathUtility.ToAbsolute(VirtualUrl); }
        }


        private string _fontSize;

        public string FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        private int _fontFactor;

        public int FontFactor
        {
            get { return _fontFactor; }
            set { _fontFactor = value; }
        }
	
        public int Count
        {
            get { return Weight; }
        }
	
    }
}
