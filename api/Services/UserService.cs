using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using server.Models;

namespace server.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        
        public UserService(IConfiguration config, ITbsgDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString
                .Replace("$user", config["database:user"])
                .Replace("$password", config["database:password"]));
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UserCollectionName);
        }

        public async Task Create(User user) {
            user.Id = null;
            await _users.InsertOneAsync(user);
        }

        public async Task<User> Get(string id) =>
            await _users.Find(book => book.Id == id).FirstOrDefaultAsync();
        
        public async Task<List<User>> Get() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task Update(string id, User user)
        {
            user.Id = null;
            await _users.ReplaceOneAsync(userDb => userDb.Id == id, user);
        }

        public async Task Remove(User user) =>
            await _users.DeleteOneAsync(userDb => userDb.Id == user.Id);

        public async Task Remove(string id) => 
            await _users.DeleteOneAsync(userDb => userDb.Id == id);
    }
}