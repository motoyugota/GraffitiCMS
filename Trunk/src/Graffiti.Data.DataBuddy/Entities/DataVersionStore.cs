using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using DataBuddy;

namespace Graffiti.Data.DataBuddy
{
    /// <summary>
    /// Summary description for Tag
    /// </summary>
    [Serializable]
    public class DataVersionStore : DataBuddyBase
    {
        private static readonly Table _Table = null;

		static DataVersionStore ()
		{
			_Table = new Table("graffiti_VersionStore", "VersionStore");
			_Table.IsReadOnly = false;
			_Table.PrimaryKey = "Id";
			_Table.Columns.Add(new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true));
			_Table.Columns.Add(new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false));
			_Table.Columns.Add(new Column("Data", DbType.String, typeof(System.String), "Data", false, false));
			_Table.Columns.Add(new Column("Type", DbType.String, typeof(System.String), "Type", false, false));
			_Table.Columns.Add(new Column("Version", DbType.Int32, typeof(System.Int32), "Version", false, false));
			_Table.Columns.Add(new Column("CreatedBy", DbType.String, typeof(System.String), "CreatedBy", false, false));
			_Table.Columns.Add(new Column("CreatedOn", DbType.DateTime, typeof(System.DateTime), "CreatedOn", false, false));
			_Table.Columns.Add(new Column("Name", DbType.String, typeof(System.String), "Name", false, false));
			_Table.Columns.Add(new Column("ItemId", DbType.Int32, typeof(System.Int32), "ItemId", false, false));
			_Table.Columns.Add(new Column("Notes", DbType.String, typeof(System.String), "Notes", false, false));
		}

		/// <summary>
		/// Fetches an instance of VersionStore based on a single column value. If more than one record is found, only the first will be used.
		/// </summary>
		public static DataVersionStore FetchByColumn(Column column, object value)
		{
			Query q = new Query(_Table);
			q.AndWhere(column, value);
			return FetchByQuery(q);
		}

		public static DataVersionStore FetchByQuery(Query q)
		{
			DataVersionStore item = new DataVersionStore ();
			using(IDataReader reader = q.ExecuteReader())
			{
				if(reader.Read())
					item.LoadAndCloseReader(reader);
			}

			return item;
		}

		/// <summary>
		/// Creates an instance of Query for the type VersionStore
		/// </summary>
		public static Query CreateQuery()
		{
			return new Query(_Table);
		}

		public DataVersionStore (){}
		/// <summary>
		/// Loads an instance of VersionStore for the supplied primary key value
		/// </summary>
		public DataVersionStore (object keyValue)
		{
			Query q = new Query(_Table);
			q.AndWhere(Columns.Id, keyValue);
			using(IDataReader reader = q.ExecuteReader())
			{
				if(reader.Read())
					LoadAndCloseReader(reader);
			}
		}

		/// <summary>
		/// Hydrates an instance of VersionStore. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
		/// </summary>
		public void Load(IDataReader reader)
		{
			Load(reader, false);
		}

		/// <summary>
		/// Hydrates an instance of VersionStore. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
		/// </summary>
		public void LoadAndCloseReader(IDataReader reader)
		{
			Load(reader, true);
		}

		private void Load(IDataReader reader, bool close)
		{
			Id = DataService.GetValue<System.Int32>(Columns.Id, reader);
			UniqueId = DataService.GetValue<System.Guid>(Columns.UniqueId, reader);
			Data = DataService.GetValue<System.String>(Columns.Data, reader);
			Type = DataService.GetValue<System.String>(Columns.Type, reader);
			Version = DataService.GetValue<System.Int32>(Columns.Version, reader);
			CreatedBy = DataService.GetValue<System.String>(Columns.CreatedBy, reader);
			CreatedOn = DataService.GetValue<System.DateTime>(Columns.CreatedOn, reader);
			Name = DataService.GetValue<System.String>(Columns.Name, reader);
			ItemId = DataService.GetValue<System.Int32>(Columns.ItemId, reader);
			Notes = DataService.GetValue<System.String>(Columns.Notes, reader);
			Loaded();
			ResetStatus();

			if(close)
				reader.Close();
		}

		#region public System.Int32 Id

		private System.Int32 _Id;

		public System.Int32 Id
		{
			get{return _Id;}
			set{MarkDirty();_Id = value;}
		}

		#endregion

		#region public System.Guid UniqueId

		private System.Guid _UniqueId;

		public System.Guid UniqueId
		{
			get{return _UniqueId;}
			set{MarkDirty();_UniqueId = value;}
		}

		#endregion

		#region public System.String Data

		private System.String _Data;

		public System.String Data
		{
			get{return _Data;}
			set{MarkDirty();_Data = value;}
		}

		#endregion

		#region public System.String Type

		private System.String _Type;

		public System.String Type
		{
			get{return _Type;}
			set{MarkDirty();_Type = value;}
		}

		#endregion

		#region public System.Int32 Version

		private System.Int32 _Version;

		public System.Int32 Version
		{
			get{return _Version;}
			set{MarkDirty();_Version = value;}
		}

		#endregion

		#region public System.String CreatedBy

		private System.String _CreatedBy;

		public System.String CreatedBy
		{
			get{return _CreatedBy;}
			set{MarkDirty();_CreatedBy = value;}
		}

		#endregion

		#region public System.DateTime CreatedOn

		private System.DateTime _CreatedOn;

		public System.DateTime CreatedOn
		{
			get{return _CreatedOn;}
			set{MarkDirty();_CreatedOn = value;}
		}

		#endregion

		#region public System.String Name

		private System.String _Name;

		public System.String Name
		{
			get{return _Name;}
			set{MarkDirty();_Name = value;}
		}

		#endregion

		#region public System.Int32 ItemId

		private System.Int32 _ItemId;

		public System.Int32 ItemId
		{
			get{return _ItemId;}
			set{MarkDirty();_ItemId = value;}
		}

		#endregion

		#region public System.String Notes

		private System.String _Notes;

		public System.String Notes
		{
			get{return _Notes;}
			set{MarkDirty();_Notes = value;}
		}

		#endregion

		/// <summary>
		/// The table object which represents VersionStore
		/// </summary>
		public static Table Table { get{return _Table;}}

		/// <summary>
		/// The columns which represent VersionStore
		/// </summary>
		public static class Columns
		{
			public static readonly Column Id = new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true);
			public static readonly Column UniqueId = new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false);
			public static readonly Column Data = new Column("Data", DbType.String, typeof(System.String), "Data", false, false);
			public static readonly Column Type = new Column("Type", DbType.String, typeof(System.String), "Type", false, false);
			public static readonly Column Version = new Column("Version", DbType.Int32, typeof(System.Int32), "Version", false, false);
			public static readonly Column CreatedBy = new Column("CreatedBy", DbType.String, typeof(System.String), "CreatedBy", false, false);
			public static readonly Column CreatedOn = new Column("CreatedOn", DbType.DateTime, typeof(System.DateTime), "CreatedOn", false, false);
			public static readonly Column Name = new Column("Name", DbType.String, typeof(System.String), "Name", false, false);
			public static readonly Column ItemId = new Column("ItemId", DbType.Int32, typeof(System.Int32), "ItemId", false, false);
			public static readonly Column Notes = new Column("Notes", DbType.String, typeof(System.String), "Notes", false, false);
		}


		public static int Destroy(Column column, object value)
		{
			DataVersionStore objectToDelete = FetchByColumn(column, value);
			if(!objectToDelete.IsNew)
			{
				objectToDelete.BeforeRemove(true);
				int i = DataService.Destroy(Table,column,value);
				objectToDelete.AfterRemove(true);
				return i;
			}

			return 0;
		}


		public static int Destroy(object value)
		{
			return Destroy(Columns.Id, value);
		}
		protected override void SetPrimaryKey(int pkID)
		{
			Id = pkID;
		}

		protected override Table GetTable()
		{
			return Table;
		}

		public static Parameter FindParameter(List<Parameter> parameters, string name)
		{
			if( parameters == null )
				throw new ArgumentNullException("parameters");
			if( string.IsNullOrEmpty( name ) )
				throw new ArgumentNullException("name", "The value cannot be null or an empty string.");

			return parameters.Find( delegate(Parameter p) { return ( p.Name == name ); } );
		}

		public static Parameter FindParameter(string name)
		{
			if( string.IsNullOrEmpty( name ) )
				throw new ArgumentNullException("name", "The value cannot be null or an empty string.");

			return GenerateParameters().Find( delegate(Parameter p) { return ( p.Name == name ); } );
		}

		#region public static List<Parameter> GenerateParameters()

		public static List<Parameter> GenerateParameters()
		{
			List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

			parameters.Add( new Parameter( "Id", null, DbType.Int32 ) );

			parameters.Add( new Parameter( "UniqueId", null, DbType.Guid ) );

			Parameter pData = new Parameter( "Data", null, DbType.String );
			pData.Length = 2147483647;
			parameters.Add( pData );

			Parameter pType = new Parameter( "Type", null, DbType.String );
			pType.Length = 128;
			parameters.Add( pType );

			parameters.Add( new Parameter( "Version", null, DbType.Int32 ) );

			Parameter pCreatedBy = new Parameter( "CreatedBy", null, DbType.String );
			pCreatedBy.Length = 128;
			parameters.Add( pCreatedBy );

			parameters.Add( new Parameter( "CreatedOn", null, DbType.DateTime ) );

			Parameter pName = new Parameter( "Name", null, DbType.String );
			pName.Length = 255;
			parameters.Add( pName );

			parameters.Add( new Parameter( "ItemId", null, DbType.Int32 ) );

			Parameter pNotes = new Parameter( "Notes", null, DbType.String );
			pNotes.Length = 2000;
			parameters.Add( pNotes );

			return parameters;
		}

		#endregion

		protected override List<Parameter> GetParameters()
		{
			List<Parameter> parameters = new List<Parameter>(Table.Columns.Count);

			if(!IsNew)
			{
				parameters.Add( new Parameter("Id", Id, DbType.Int32) );
			}

			parameters.Add( new Parameter("UniqueId", UniqueId, DbType.Guid) );

			Parameter pData = new Parameter("Data", Data, DbType.String);
			pData.Length = 2147483647;
			parameters.Add( pData );

			Parameter pType = new Parameter("Type", Type, DbType.String);
			pType.Length = 128;
			parameters.Add( pType );

			parameters.Add( new Parameter("Version", Version, DbType.Int32) );

			Parameter pCreatedBy = new Parameter("CreatedBy", CreatedBy, DbType.String);
			pCreatedBy.Length = 128;
			parameters.Add( pCreatedBy );

			parameters.Add( new Parameter("CreatedOn", CreatedOn, DbType.DateTime) );

			Parameter pName = new Parameter("Name", Name, DbType.String);
			pName.Length = 255;
			parameters.Add( pName );

			parameters.Add( new Parameter("ItemId", ItemId, DbType.Int32) );

			Parameter pNotes = new Parameter("Notes", Notes, DbType.String);
			pNotes.Length = 2000;
			parameters.Add( pNotes );

			return parameters;
		}

        public static int VersionFile(FileInfo fi, string username, DateTime saveTime) 
        {
                DataVersionStore vs = new DataVersionStore();
                vs.Name = fi.FullName;
                vs.Version = CurrentVersion(fi) + 1;
                vs.Type = DataUtil.GetMapping(fi.FullName);
                vs.UniqueId = Guid.NewGuid();

                using (StreamReader sr = new StreamReader(fi.FullName)) {
                    vs.Data = sr.ReadToEnd();
                    sr.Close();
                }

                vs.Save(username, saveTime);

                return vs.Id;
            }

            public static int CurrentVersion(FileInfo fi) {
                Query q = CreateQuery();
                q.Top = "1";
                q.AndWhere(Columns.Name, fi.FullName);
                q.AndWhere(Columns.Type, DataUtil.GetMapping(fi.FullName));
                q.OrderByDesc(Columns.Version);

                DataVersionStore vs = FetchByQuery(q);

                return vs.Version;
            }

            public static DataVersionStoreCollection GetVersionHistory(string filePath) {
                return GetVersionHistory(filePath, true);
            }

            internal static DataVersionStoreCollection GetVersionHistory(string filePath, bool checkLicensed) {
                Query versionQuery = CreateQuery();
                versionQuery.AndWhere(Columns.Name, filePath);
                versionQuery.AndWhere(Columns.Type, DataUtil.GetMapping(filePath));
                versionQuery.OrderByAsc(Columns.Version);

                return DataVersionStoreCollection.FetchByQuery(versionQuery);
            }

            public static DataVersionStoreCollection GetVersionHistory(int postId) {
                Query versionQuery = CreateQuery();
                versionQuery.AndWhere(Columns.Type, "post/xml");
                versionQuery.AndWhere(Columns.ItemId, postId);
                versionQuery.OrderByDesc(Columns.Version);

                return DataVersionStoreCollection.FetchByQuery(versionQuery);
            }
    }



}