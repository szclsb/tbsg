using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface IEntityService<T> where T : IEntity
    {
        public Task Create(T t);
        public Task<T> Find(string id);
        public Task<IEnumerable<T>> FindMany();
        public Task Update(string id, T t);
        public Task Delete(string id);
    }
}