using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MindMap_General_Purpose_API.Models
{
    public class Workspace
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<User> Users { get; set; }
        public User Creator { get; set; }
        public object Content { get; set; } // type object needs to be specified later on as we have clear idea
    }
}
