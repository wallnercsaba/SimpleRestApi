using Microsoft.EntityFrameworkCore;
using SimpleRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRestApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SimpleRestApiDbContext simpleRestApiDbContext;

        public UserRepository(SimpleRestApiDbContext simpleRestApiDbContext)
        {
            this.simpleRestApiDbContext = simpleRestApiDbContext;
        }

        public async Task Add(User user)
        {
            await simpleRestApiDbContext.Users.AddAsync(user);
        }

        public Task Delete(string id)
        {
            simpleRestApiDbContext.Users.Remove(simpleRestApiDbContext.Users.FirstOrDefault(user => user.Id == id));
            return Task.FromResult(0);
        }

        public async Task<User> Get(string id)
        {
            return await simpleRestApiDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<List<User>> GetAll()
        {
            return await simpleRestApiDbContext.Users.ToListAsync();
        }

        public async Task SaveChanges()
        {
            await simpleRestApiDbContext.SaveChangesAsync();
        }

        public Task Update(User user)
        {
            simpleRestApiDbContext.Users.Update(user);
            return Task.FromResult(0);
        }
    }
}
