using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    //clean coding technique to create interface -so code become testable and have different implementation
    public interface ITokenHandler
    {
       Task<string> CreateToken(User user);
    }
}
