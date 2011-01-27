using System.Collections.Generic;
using System.Linq;
using Graffiti.Core;
using Graffiti.Core.Services;
using Graffiti.Data;

namespace Graffiti.Services 
{
    public class TagService : ITagService
    {
        private ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IList<Tag> FetchAllTags()
        {
            return _tagRepository.FetchAllTags().ToList();
        }

        public IList<TagWeight> FetchAllTagWeights()
        {
            return _tagRepository.FetchAllTagWeights().ToList();
        }

    }
}
