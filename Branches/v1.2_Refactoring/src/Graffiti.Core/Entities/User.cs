using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Graffiti.Core
{
    [Serializable]
    public class User : IGraffitiUser, IMenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProperName { get; set; }
        public double TimeZoneOffSet { get; set; }
        public string Bio { get; set; }
        public string PublicEmail { get; set; }
        public string Avatar { get; set; }
        public string WebSite { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int PasswordFormat { get; set; }
        public Guid UniqueId { get; set; }
        public string[] Roles { get; set; }
        public bool IsLoaded { get; set; }

        private bool _isNew = true;
        public bool IsNew {
            get { return _isNew; }
            set { _isNew = value; }
        }

        #region IMenuItem Members

        public List<MenuItem> GetMenuItems() {
            List<MenuItem> menuItems = new List<MenuItem>();

            menuItems.Add(new MenuItem("Name", "$user.Name", "The username", "User"));
            menuItems.Add(new MenuItem("ProperName", "$user.ProperName", "A user\'s formal name. If null, it will be the Name", "User"));
            menuItems.Add(new MenuItem("Email", "$user.Email", "", "User"));
            menuItems.Add(new MenuItem("TimeZoneOffSet", "$user.TimeZoneOffSet", "", "User"));
            menuItems.Add(new MenuItem("WebSite", "$user.WebSite", "", "User"));
            menuItems.Add(new MenuItem("Bio", "$user.Bio", "", "User"));

            return menuItems;
        }

        #endregion

        #region Events
        //protected override void BeforeValidate()
        //{
        //    if (UniqueId == Guid.Empty)
        //        UniqueId = Guid.NewGuid();

        //    if (string.IsNullOrEmpty(Password))
        //        throw new Exception("Invalid Password");

        //    if (_loadedPassword != Password)
        //    {
        //        PasswordFormat = 1;
        //        PasswordSalt = GenerateSalt();
        //        Password = EncodePassword(Password, PasswordSalt);
        //    }


        //    if (string.IsNullOrEmpty(ProperName))
        //        ProperName = Name;

        //}
        #endregion

        #region old stuff
        //protected override void Loaded()
        //{
        //    //use this to check for password updates
        //    _loadedPassword = Password;

        //    base.Loaded();
        //}


        //public override void Save()
        //{
        //    Events.Instance().ExecuteUserBeforeUserUpdate(this);

        //    base.Save();

        //    Events.Instance().ExecuteAfterUserUpdated(this);
        //}
        #endregion

        #region Static Helpers

        private static string GenerateSalt() 
        {
            byte[] buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        public static string EncodePassword(string pass, string salt) 
        {
            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);

            return Convert.ToBase64String(HashAlgorithm.Create("MD5").ComputeHash(bAll));
        }

        #endregion

    }
}