using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using DataBuddy;
using Graffiti.Core;

namespace Graffiti.Data.DataBuddy
{
    [Serializable]
    public class DataUser : DataBuddyBase
    {
        private static readonly Table _Table = null;

        static DataUser() {
            _Table = new Table("graffiti_Users", "User");
            _Table.IsReadOnly = false;
            _Table.PrimaryKey = "Id";
            _Table.Columns.Add(new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true));
            _Table.Columns.Add(new Column("Name", DbType.String, typeof(System.String), "Name", false, false));
            _Table.Columns.Add(new Column("Email", DbType.String, typeof(System.String), "Email", false, false));
            _Table.Columns.Add(new Column("ProperName", DbType.String, typeof(System.String), "ProperName", true, false));
            _Table.Columns.Add(new Column("TimeZoneOffSet", DbType.Double, typeof(System.Double), "TimeZoneOffSet", false, false));
            _Table.Columns.Add(new Column("Bio", DbType.String, typeof(System.String), "Bio", true, false));
            _Table.Columns.Add(new Column("Avatar", DbType.String, typeof(System.String), "Avatar", true, false));
            _Table.Columns.Add(new Column("PublicEmail", DbType.String, typeof(System.String), "PublicEmail", true, false));
            _Table.Columns.Add(new Column("WebSite", DbType.String, typeof(System.String), "WebSite", true, false));
            _Table.Columns.Add(new Column("Password", DbType.String, typeof(System.String), "Password", false, false));
            _Table.Columns.Add(new Column("Password_Salt", DbType.String, typeof(System.String), "PasswordSalt", false, false));
            _Table.Columns.Add(new Column("PasswordFormat", DbType.Int32, typeof(System.Int32), "PasswordFormat", false, false));
            _Table.Columns.Add(new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false));
        }

        /// <summary>
        /// Fetches an instance of User based on a single column value. If more than one record is found, only the first will be used.
        /// </summary>
        public static DataUser FetchByColumn(Column column, object value) {
            Query q = new Query(_Table);
            q.AndWhere(column, value);
            return FetchByQuery(q);
        }

        public static DataUser FetchByQuery(Query q) {
            DataUser item = new DataUser();
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    item.LoadAndCloseReader(reader);
            }

            return item;
        }

        /// <summary>
        /// Creates an instance of Query for the type User
        /// </summary>
        public static Query CreateQuery() {
            return new Query(_Table);
        }

        public DataUser() { }
        /// <summary>
        /// Loads an instance of User for the supplied primary key value
        /// </summary>
        public DataUser(object keyValue) {
            Query q = new Query(_Table);
            q.AndWhere(Columns.Id, keyValue);
            using (IDataReader reader = q.ExecuteReader()) {
                if (reader.Read())
                    LoadAndCloseReader(reader);
            }
        }

        /// <summary>
        /// Hydrates an instance of User. In this case, the Reader should be in a read ready state. The reader will not be closed once it is done processing.
        /// </summary>
        public void Load(IDataReader reader) {
            Load(reader, false);
        }

        /// <summary>
        /// Hydrates an instance of User. In this case, the Reader should be in a read ready state. The reader will be closed once it is done processing.
        /// </summary>
        public void LoadAndCloseReader(IDataReader reader) {
            Load(reader, true);
        }

        private void Load(IDataReader reader, bool close) {
            Id = DataService.GetValue<System.Int32>(Columns.Id, reader);
            Name = DataService.GetValue<System.String>(Columns.Name, reader);
            Email = DataService.GetValue<System.String>(Columns.Email, reader);
            ProperName = DataService.GetValue<System.String>(Columns.ProperName, reader);
            TimeZoneOffSet = DataService.GetValue<System.Double>(Columns.TimeZoneOffSet, reader);
            Bio = DataService.GetValue<System.String>(Columns.Bio, reader);
            Avatar = DataService.GetValue<System.String>(Columns.Avatar, reader);
            PublicEmail = DataService.GetValue<System.String>(Columns.PublicEmail, reader);
            WebSite = DataService.GetValue<System.String>(Columns.WebSite, reader);
            Password = DataService.GetValue<System.String>(Columns.Password, reader);
            PasswordSalt = DataService.GetValue<System.String>(Columns.PasswordSalt, reader);
            PasswordFormat = DataService.GetValue<System.Int32>(Columns.PasswordFormat, reader);
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

        #region public System.String Email

        private System.String _Email;

        public System.String Email {
            get { return _Email; }
            set { MarkDirty(); _Email = value; }
        }

        #endregion

        #region public System.String ProperName

        private System.String _ProperName;

        public System.String ProperName {
            get { return _ProperName; }
            set { MarkDirty(); _ProperName = value; }
        }

        #endregion

        #region public System.Double TimeZoneOffSet

        private System.Double _TimeZoneOffSet;

        public System.Double TimeZoneOffSet {
            get { return _TimeZoneOffSet; }
            set { MarkDirty(); _TimeZoneOffSet = value; }
        }

        #endregion

        #region public System.String Bio

        private System.String _Bio;

        public System.String Bio {
            get { return _Bio; }
            set { MarkDirty(); _Bio = value; }
        }

        #endregion

        #region public System.String Avatar

        private System.String _Avatar;

        public System.String Avatar {
            get { return _Avatar; }
            set { MarkDirty(); _Avatar = value; }
        }

        #endregion

        #region public System.String PublicEmail

        private System.String _PublicEmail;

        public System.String PublicEmail {
            get { return _PublicEmail; }
            set { MarkDirty(); _PublicEmail = value; }
        }

        #endregion

        #region public System.String WebSite

        private System.String _WebSite;

        public System.String WebSite {
            get { return _WebSite; }
            set { MarkDirty(); _WebSite = value; }
        }

        #endregion

        #region public System.String Password

        private System.String _Password;

        public System.String Password {
            get { return _Password; }
            set { MarkDirty(); _Password = value; }
        }

        #endregion

        #region public System.String PasswordSalt

        private System.String _PasswordSalt;

        public System.String PasswordSalt {
            get { return _PasswordSalt; }
            set { MarkDirty(); _PasswordSalt = value; }
        }

        #endregion

        #region public System.Int32 PasswordFormat

        private System.Int32 _PasswordFormat;

        public System.Int32 PasswordFormat {
            get { return _PasswordFormat; }
            set { MarkDirty(); _PasswordFormat = value; }
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
        /// The table object which represents User
        /// </summary>
        public static Table Table { get { return _Table; } }

        /// <summary>
        /// The columns which represent User
        /// </summary>
        public static class Columns {
            public static readonly Column Id = new Column("Id", DbType.Int32, typeof(System.Int32), "Id", false, true);
            public static readonly Column Name = new Column("Name", DbType.String, typeof(System.String), "Name", false, false);
            public static readonly Column Email = new Column("Email", DbType.String, typeof(System.String), "Email", false, false);
            public static readonly Column ProperName = new Column("ProperName", DbType.String, typeof(System.String), "ProperName", true, false);
            public static readonly Column TimeZoneOffSet = new Column("TimeZoneOffSet", DbType.Double, typeof(System.Double), "TimeZoneOffSet", false, false);
            public static readonly Column Bio = new Column("Bio", DbType.String, typeof(System.String), "Bio", true, false);
            public static readonly Column Avatar = new Column("Avatar", DbType.String, typeof(System.String), "Avatar", true, false);
            public static readonly Column PublicEmail = new Column("PublicEmail", DbType.String, typeof(System.String), "PublicEmail", true, false);
            public static readonly Column WebSite = new Column("WebSite", DbType.String, typeof(System.String), "WebSite", true, false);
            public static readonly Column Password = new Column("Password", DbType.String, typeof(System.String), "Password", false, false);
            public static readonly Column PasswordSalt = new Column("Password_Salt", DbType.String, typeof(System.String), "PasswordSalt", false, false);
            public static readonly Column PasswordFormat = new Column("PasswordFormat", DbType.Int32, typeof(System.Int32), "PasswordFormat", false, false);
            public static readonly Column UniqueId = new Column("UniqueId", DbType.Guid, typeof(System.Guid), "UniqueId", false, false);
        }


        public static int Destroy(Column column, object value) {
            DataUser objectToDelete = FetchByColumn(column, value);
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

            Parameter pEmail = new Parameter("Email", null, DbType.String);
            pEmail.Length = 128;
            parameters.Add(pEmail);

            Parameter pProperName = new Parameter("ProperName", null, DbType.String);
            pProperName.Length = 255;
            parameters.Add(pProperName);

            parameters.Add(new Parameter("TimeZoneOffSet", null, DbType.Double));

            Parameter pBio = new Parameter("Bio", null, DbType.String);
            pBio.Length = 2000;
            parameters.Add(pBio);

            Parameter pAvatar = new Parameter("Avatar", null, DbType.String);
            pAvatar.Length = 255;
            parameters.Add(pAvatar);

            Parameter pPublicEmail = new Parameter("PublicEmail", null, DbType.String);
            pPublicEmail.Length = 255;
            parameters.Add(pPublicEmail);

            Parameter pWebSite = new Parameter("WebSite", null, DbType.String);
            pWebSite.Length = 255;
            parameters.Add(pWebSite);

            Parameter pPassword = new Parameter("Password", null, DbType.String);
            pPassword.Length = 128;
            parameters.Add(pPassword);

            Parameter pPassword_Salt = new Parameter("Password_Salt", null, DbType.String);
            pPassword_Salt.Length = 128;
            parameters.Add(pPassword_Salt);

            parameters.Add(new Parameter("PasswordFormat", null, DbType.Int32));

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

            Parameter pEmail = new Parameter("Email", Email, DbType.String);
            pEmail.Length = 128;
            parameters.Add(pEmail);

            Parameter pProperName = new Parameter("ProperName", ProperName, DbType.String);
            pProperName.Length = 255;
            parameters.Add(pProperName);

            parameters.Add(new Parameter("TimeZoneOffSet", TimeZoneOffSet, DbType.Double));

            Parameter pBio = new Parameter("Bio", Bio, DbType.String);
            pBio.Length = 2000;
            parameters.Add(pBio);

            Parameter pAvatar = new Parameter("Avatar", Avatar, DbType.String);
            pAvatar.Length = 255;
            parameters.Add(pAvatar);

            Parameter pPublicEmail = new Parameter("PublicEmail", PublicEmail, DbType.String);
            pPublicEmail.Length = 255;
            parameters.Add(pPublicEmail);

            Parameter pWebSite = new Parameter("WebSite", WebSite, DbType.String);
            pWebSite.Length = 255;
            parameters.Add(pWebSite);

            Parameter pPassword = new Parameter("Password", Password, DbType.String);
            pPassword.Length = 128;
            parameters.Add(pPassword);

            Parameter pPassword_Salt = new Parameter("Password_Salt", PasswordSalt, DbType.String);
            pPassword_Salt.Length = 128;
            parameters.Add(pPassword_Salt);

            parameters.Add(new Parameter("PasswordFormat", PasswordFormat, DbType.Int32));

            parameters.Add(new Parameter("UniqueId", UniqueId, DbType.Guid));

            return parameters;
        }


        private string[] _roles = null;
        public string[] Roles
        {
            get
            {
                if(_roles == null)
                {
                    DataUserRoleCollection urc = DataUserRoleCollection.FetchByColumn(DataUserRole.Columns.UserId, Id);
                    List<string> roles = new List<string>(urc.Count);
                    foreach(DataUserRole ur in urc)
                        roles.Add(ur.RoleName);

                    _roles = roles.ToArray();
                }

                return _roles;
            }
        }



        private string _loadedPassword = null;

        protected override void BeforeValidate()
        {
            if (UniqueId == Guid.Empty)
                UniqueId = Guid.NewGuid();
            
            if (string.IsNullOrEmpty(Password))
                throw new Exception("Invalid Password");

            if (_loadedPassword != Password)
            {
                PasswordFormat = 1;
                PasswordSalt = GenerateSalt();
                Password = EncodePassword(Password, PasswordSalt);
            }


            if (string.IsNullOrEmpty(ProperName))
                ProperName = Name;

        }



        //protected override void AfterUpdate() 
        //{
        //    base.AfterUpdate();

        //    ZCache.RemoveCache("Users");
        //}

        protected override void Loaded()
        {
            //use this to check for password updates
            _loadedPassword = Password;

            base.Loaded();
        }


        public override void Save() {
            Events.Instance().ExecuteBeforeInsertEvent(this);

            base.Save();

            Events.Instance().ExecuteAfterInsertEvent(this);
        }

        private static string GenerateSalt()
        {
            byte[] buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        internal static string EncodePassword(string pass, string salt)
        {
            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);

            return Convert.ToBase64String(HashAlgorithm.Create("MD5").ComputeHash(bAll));
        }
    }
}