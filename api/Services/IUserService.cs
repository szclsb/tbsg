using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface IUserService : IEntityService<User>
    {
        public Task<User> FindByUsername(string username);
    }
}