using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Exceptions;
using api.Models;
using MongoDB.Driver;

namespace api.Services.MongoDB
{
    public abstract class AbstractService<T> : IEntityService<T> where T : IEntity
    {
        protected readonly IMongoCollection<T> Collection;

        /// <summary>
        /// Converts MongoDB error and converts them into custom exceptions
        /// </summary>
        /// <param name="function">mongodb driver function</param>
        /// <typeparam name="TR">mongodb driver function return type</typeparam>
        /// <returns>mongodb driver function return value</returns>
        /// <exception cref="DatabaseException">General exception</exception>
        /// <exception cref="DoublicateException"></exception>
        
        protected async Task<TR> Guarded<TR>(Task<TR> function)
        {
            try
            {
                return await function;
            }
            catch (MongoWriteException e)
            {
                throw e.WriteError.Code switch
                {
                    11000 => new DoublicateException(),
                    _ => new DatabaseException()
                };
            }
        }

        protected AbstractService(Database database, string collectionName)
        {
            Collection = database.GetCollection<T>(collectionName);
        }

        public async Task Create(T t)
        {
            t.Id = null;
            // convert return type of mongodb insert into int to use the guarded wrapper
            await Guarded(Task.Run(async () =>
            {
                await Collection.InsertOneAsync(t);
                return 0;
            }));
        }

        public async Task<T> Find(string id) =>
            await Guarded(Collection.Find(tt => tt.Id == id).FirstOrDefaultAsync());

        public async Task<IEnumerable<T>> FindMany() => 
            await Guarded(Collection.Find(_ => true).ToListAsync());

        public async Task Update(string id, T t)
        {
            t.Id = null;
            await Guarded(Collection.ReplaceOneAsync(tt => tt.Id == id, t));
        }
        
        public async Task Delete(string id) =>
            await Guarded(Collection.DeleteOneAsync(tt => tt.Id == id));
    }
}