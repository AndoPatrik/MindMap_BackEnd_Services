using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MindMap_General_Purpose_API.Models
{
    public class Workspace
    {
        public Workspace()
        {
            ShareLink = Guid.NewGuid().ToString();
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public List<User> Users { get; set; }
        public User Creator { get; set; }
        [BsonElement("ShareLink")]
        public string ShareLink { get; }
        public object Content { get; set; } // type object needs to be specified later on as we have clear idea
    }
}
