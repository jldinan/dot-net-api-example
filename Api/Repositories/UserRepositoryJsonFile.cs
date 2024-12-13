using Api.Interfaces;
using Api.Models;
using Api.Utilities;
using System.Text.Json;

namespace Api.Repositories
{
    public class UserRepositoryJsonFile: IUserRepository
    {
        private readonly string filePath = Path.Combine(AppContext.BaseDirectory, "users.json");
        private readonly ILogger logger;
        private readonly JsonFileHelper jsonFileHelper;

        public UserRepositoryJsonFile(ILogger<UserRepositoryJsonFile> logger, JsonFileHelper jsonFileHelper)
        {
            this.logger = logger;
            this.jsonFileHelper = jsonFileHelper;            
        }

        public async Task<List<User>> GetAll()
        {
            return await jsonFileHelper.GetObjectListFromJsonFile<User>(filePath);
        }

        public async Task<User?> GetById(int id)
        {
            var users = await GetAll();            
            return users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User> Create(User user)
        {
            var users = await GetAll();
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);

            await jsonFileHelper.SaveObjectListToJsonFile(users, filePath);
            return user;
        }

        public async Task<User?> Update(User user)
        {
            var users = await GetAll();
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);

            if (existingUser is not null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
            }

            await jsonFileHelper.SaveObjectListToJsonFile(users, filePath);
            return existingUser;
        }

        public async Task<bool> DeleteById(int id)
        {
            var users = await GetAll();
            var userToDelete = users.FirstOrDefault(u => u.Id == id);

            if (userToDelete is not null)
            {
                users.Remove(userToDelete);
                await jsonFileHelper.SaveObjectListToJsonFile(users, filePath);
                return true;
            }
            return false;
        }
    }
}
