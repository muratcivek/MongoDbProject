using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Travel.WEB.Entities.Common;

namespace Travel.WEB.Entities
{
    public class Banner:BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
