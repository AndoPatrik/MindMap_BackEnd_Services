using MindMap_General_Purpose_API.Controllers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MindMap_General_Purpose_API.Models
{
    public class User
    {
        public User()
        {
            //this.ConnectedWorkspaces = new List<ConnectedWorkspace>();
        }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonIgnoreIfNull]
        public string Email { get; set; }
        [BsonIgnoreIfNull]
        public string Password { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        public bool IsActive { get; set; }
        [BsonIgnoreIfNull]
        [BsonIgnoreIfDefault]
        //[BsonElement("ConnectedWorkspaces")]
        public List<ConnectedWorkspace> ConnectedWorkspaces { get; set; }
    }
}
