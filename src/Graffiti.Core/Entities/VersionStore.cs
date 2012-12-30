using System;

namespace Graffiti.Core
{
    [Serializable]
    public class VersionStore 
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public string Data { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        public int ItemId { get; set; }
        public string Notes { get; set; }        
	}
}
