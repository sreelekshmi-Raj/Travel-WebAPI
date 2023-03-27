using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string username, string password);
    }
}
