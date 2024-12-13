using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class UserRepositoryEF: IUserRepository
    {
        private readonly ILogger logger;
        private readonly ApiDbContext context;

        public UserRepositoryEF(ILogger<UserRepositoryJsonFile> logger, ApiDbContext context)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<List<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetById(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User> Create(User user) 
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> Update(User user)
        {
            var existingUser = await context.Users.FindAsync(user.Id);

            if (existingUser is not null) 
            {             
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
            }

            await context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteById(int id)
        {
            var user = await context.Users.FindAsync(id);
            //C# 11 negate pattern using not
            if (user is not null) 
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
