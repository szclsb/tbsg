using System.Threading.Tasks;
using MongoDB.Driver;
using api.Models;

namespace api.Services.MongoDB
{
    public class UserService : AbstractService<User>, IUserService
    {
        public UserService(Database database, ITbsgDatabaseSettings settings)
            : base(database, settings.UserCollectionName) { }

        public async Task<User> FindByUsername(string username) =>
            await Guarded(Collection.Find(user => user.Username == username).FirstOrDefaultAsync());
    }
}