using System;

namespace Graffiti.Core 
{
    [Serializable]
    public class RolePermissions
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool HasRead { get; set; }
        public bool HasEdit { get; set; }
        public bool HasPublish { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }        
    }
}
