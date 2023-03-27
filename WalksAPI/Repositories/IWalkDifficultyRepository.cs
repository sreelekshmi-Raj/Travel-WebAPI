using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public interface IWalkDifficultyRepository
    {
        //Get All Walk Difficulties
        Task<IEnumerable<WalkDifficulty>> GetAllAsync();
        //Get Single Walk difficulty
        Task<WalkDifficulty> GetAsync(Guid id);
        //Create new walk difficulty
        Task<WalkDifficulty> CreateAsync(WalkDifficulty walkDifficulty);

        //update walk difficulty
        Task<WalkDifficulty> UpdateAsync(Guid id,WalkDifficulty newDifficulty);
        //Delete walk difficulty
        Task<WalkDifficulty> DeleteAsync(Guid id);
    }
}
