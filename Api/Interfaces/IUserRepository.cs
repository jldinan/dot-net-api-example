using Api.Models;

namespace Api.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User?> GetById(int id);
        Task<User> Create(User user);
        Task<User?> Update(User user);
        Task<bool> DeleteById(int id);
    }
}
