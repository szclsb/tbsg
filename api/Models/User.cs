using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace api.Models
{
    public class User : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("username")]
        public string Username { get; set; }
        
        [BsonElement("email")]
        public string Email { get; set; }
        
        [BsonElement("pwd")]
        [JsonIgnore]
        public string Password { get; set; }
        
        [BsonElement("roles")]
        public IEnumerable<string> Roles { get; set; }
    }
}