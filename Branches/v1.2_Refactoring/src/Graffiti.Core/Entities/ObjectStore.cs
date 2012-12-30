using System;

namespace Graffiti.Core
{
    [Serializable]
    public class ObjectStore
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ContentType { get; set; }
        public int Version { get; set; }
        public Guid UniqueId { get; set; }
        public bool IsLoaded { get; set; }

        private bool _isNew = true;
        public bool IsNew {
            get { return _isNew; }
            set { _isNew = value; }
        }

        
        //protected override void BeforeValidate()
        //{
        //    base.BeforeValidate();

        //    if (IsNew && UniqueId == Guid.Empty)
        //        UniqueId = Guid.NewGuid();

        //    if (string.IsNullOrEmpty(Type))
        //        throw new Exception("ObjectStore.Type must be defined");

        //    if (string.IsNullOrEmpty("Name"))
        //        throw new Exception("ObjectStore.Name must be defined");

        //}
    }
}
