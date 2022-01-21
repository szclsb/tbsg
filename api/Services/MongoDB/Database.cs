using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace api.Services.MongoDB
{
    public class Database
    {
        private readonly IMongoDatabase _database;
        
        public Database(IConfiguration config, ITbsgDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString
                .Replace("$user", config["database:user"])
                .Replace("$password", config["database:password"]));
            _database = client.GetDatabase(settings.DatabaseName);
        }
        
        public IMongoCollection<T> GetCollection<T>(string collectionName) =>
            _database.GetCollection<T>(collectionName);
    }
}