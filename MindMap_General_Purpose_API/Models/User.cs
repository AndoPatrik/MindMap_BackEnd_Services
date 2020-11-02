using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MindMap_General_Purpose_API.Models
{
    public class User
    {
        public User()
        {

        }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
