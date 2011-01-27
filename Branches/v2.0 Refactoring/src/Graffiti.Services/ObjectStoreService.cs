using System;
using System.Collections.Generic;
using System.Linq;
using Graffiti.Core;
using Graffiti.Core.Services;
using Graffiti.Data;

namespace Graffiti.Services 
{
    public class ObjectStoreService : IObjectStoreService
    {
        private IObjectStoreRepository _objectStoreRepository;

        public ObjectStoreService(IObjectStoreRepository objectStoreRepository)
        {
            _objectStoreRepository = objectStoreRepository;
        }


        public ObjectStore FetchByName(string name)
        {
            return _objectStoreRepository.FetchObjectStoreByName(name);
        }

        public ObjectStore FetchByUniqueId(Guid uniqueId)
        {
            return _objectStoreRepository.FetchObjectStoreByUniqueId(uniqueId);
        }

        public IList<ObjectStore> FetchByNameAndContentType(string name, string contentType)
        {
            return FetchByContentType(contentType).Where(x => x.Name == name).ToList();
        }

        public IList<ObjectStore> FetchByContentType(string contentType)
        {
            return _objectStoreRepository.FetchObjectStoresByContentType(contentType).ToList();
        }

        public ObjectStore SaveObjectStore(ObjectStore store)
        {
            return _objectStoreRepository.SaveObjectStore(store);
        }

        public ObjectStore SaveObjectStore(ObjectStore store, string username)
        {
            return _objectStoreRepository.SaveObjectStore(store, username);
        }

        public void DestroyObjectStore(int id)
        {
            _objectStoreRepository.DestroyObjectStore(id);
        }
    }
}
