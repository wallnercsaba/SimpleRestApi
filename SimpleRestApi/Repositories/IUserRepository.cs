using SimpleRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleRestApi.Repositories
{
    public interface IUserRepository
    {
        Task Add(User model);
        Task Update(User model);
        Task Delete(string id);

        Task<User> Get(string id);
        Task<List<User>> GetAll();

        Task SaveChanges();
    }
}
